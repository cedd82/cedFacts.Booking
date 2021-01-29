using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using FACTS.GenericBooking.Common.ExtensionMethods;
using FACTS.GenericBooking.Common.Helpers;
using FACTS.GenericBooking.Common.Models.Api;
using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Common.Models.Domain.Messages;
using FACTS.GenericBooking.Domain.Mappers;
using FACTS.GenericBooking.Domain.Models.Auth;
using FACTS.GenericBooking.Domain.Services.Interfaces;
using FACTS.GenericBooking.Repository.Ingres.Entities;
using FACTS.GenericBooking.Repository.Ingres.SqlQueries;

using NHibernate;
using NHibernate.Transform;

using NLog;

namespace FACTS.GenericBooking.Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICachedIngresEntitiesService _cachedIngresEntitiesService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly IJwtHelperService _jwtHelperService;
        private readonly ISession _nhibernateSession;
        
        public AuthenticationService(IMapper mapper, 
                                     ISession nhibernateSession,
                                     IJwtHelperService jwtHelperService,
                                     ICachedIngresEntitiesService cachedIngresEntitiesService)
        {
            _mapper            = mapper;
            _nhibernateSession = nhibernateSession;
            _jwtHelperService  = jwtHelperService;
            _cachedIngresEntitiesService = cachedIngresEntitiesService;
        }

        public async Task<Result<UserLoginDto>> AuthenticateUserAsync(AuthenticateUserDto authenticateUser, HttpRequestDeviceDetails deviceDetails)
        {
            using ITransaction transaction = _nhibernateSession.BeginTransaction(IsolationLevel.ReadUncommitted);
            IQuery query = _nhibernateSession.CreateSQLQuery(AuthQueries.GetUserLogin)
                .SetParameter("operatorId", authenticateUser.Username)
                .SetResultTransformer(new AliasToBeanResultTransformer(typeof(UserLoginDetailsDto)));
            UserLoginDetailsDto userLogin = await query.UniqueResultAsync<UserLoginDetailsDto>();

            if (userLogin == null)
                return new Result<UserLoginDto>(ErrorMessages.UsernameOrPasswordIncorrect);
            if (userLogin.IsEnabled == 0)
                return new Result<UserLoginDto>(ErrorMessages.UserIsDisabled);

            if (PasswordHelper.VerifyPasswordHash(authenticateUser.Password, userLogin.PasswordHash, userLogin.PasswordSalt) && userLogin.IsLocked == 0)
            {
                Logger.Info( $"user with {userLogin.OperatorId} logged in");
                string jwtToken = _jwtHelperService.BuildEncryptedJwtTokenAsJwe(userLogin);
                UserLoginDto userLoginDto = _mapper.Map<UserLoginDetailsDto, UserLoginDto>(userLogin);
                userLoginDto.AccessToken = jwtToken;
                await _nhibernateSession.CreateSQLQuery(@"update operator_password 
                                                          set update_tms = date('now'), 
                                                              bad_login_count = 0
                                                          where operator_id = :operatorId")
                    .SetParameter("operatorId", authenticateUser.Username)
                    .ExecuteUpdateAsync();
                await transaction.CommitAsync();
                await AddLoginAuditAsync(authenticateUser.Username, deviceDetails);
                return new Result<UserLoginDto>(userLoginDto);
            }
            
            int badLoginCount = userLogin.BadLoginCount + 1;
            if (badLoginCount >= 5)
                userLogin.IsLocked = 1;

            await _nhibernateSession.CreateSQLQuery(@"update operator_password 
                    set update_tms = date('now'), 
                        bad_login_count = :badLoginCount,
                        lock_ind = :isLocked
                where operator_id = :operatorId")
                .SetInt32("badLoginCount", badLoginCount)
                .SetInt32("isLocked", userLogin.IsLocked)
                .SetParameter("operatorId", authenticateUser.Username)
                .ExecuteUpdateAsync();
            await transaction.CommitAsync();
            await AddLoginAuditAsync(authenticateUser.Username, deviceDetails);
            
            if (badLoginCount < 5 && userLogin.IsLocked == 0)
                return new Result<UserLoginDto>(ErrorMessages.UsernameOrPasswordIncorrect);

            string auditNote = $"User:{authenticateUser.Username} locked out due to failed password attempts:{badLoginCount}";
            await AddOperatorAuditNoteAsync(authenticateUser.Username, auditNote);
            Logger.Info($"User:{authenticateUser.Username} locked out due to failed password attempts:{badLoginCount}");
            return new Result<UserLoginDto>(ErrorMessages.UserIsLockedOut);
        }
        
        // creates external user for now
        public async Task<Result> CreateUserAsync(CreateUserDto model)
        {
            int existingUser = await _nhibernateSession.CreateSQLQuery("select 1 from operator where operator_id = :operatorId")
                .SetParameter("operatorId", model.Username)
                .UniqueResultAsync<short>();
            if (existingUser == 1)
                return Result.Fail(ErrorMessages.UserAlreadyExists);

            string operatorTypeCode = "EXN";
            IList<ApplicationRole> applicationRoles = await _cachedIngresEntitiesService.GetApplicationRolesCachedAsync();
            ApplicationRole role = applicationRoles.First(x => x.BusinessUnitType.Trim() == "CCC" && x.Description == "EXTERNAL USER");
            
            PasswordHelper.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
            Operator userOperator = OperatorFromCreateUserMap.Map(model, operatorTypeCode);
            
            using ITransaction transaction = _nhibernateSession.BeginTransaction(IsolationLevel.ReadUncommitted);
            await _nhibernateSession.SaveAsync(userOperator);
            
            await _nhibernateSession.CreateSQLQuery(
                  @"INSERT INTO operator_bus_unit (operator_id, bus_unit_code, group_id, role_id, is_enabled, create_tms)
                    values (:operatorId, 'CCC', 'OPERATOR' , :roleId, 1, date('now'))")
                .SetParameter("operatorId", model.Username)
                .SetParameter("roleId", role.RoleId)
                .ExecuteUpdateAsync();

            await _nhibernateSession.CreateSQLQuery(
               @"INSERT INTO operator_password (operator_id, bus_unit_code, password_hash, password_salt, create_tms) 
                 values ( :operatorId, 'CCC', :passwordHash, :passwordSalt, date('now'))")
                .SetParameter("operatorId", model.Username)
                .SetParameter("passwordHash", passwordHash)
                .SetParameter("passwordSalt", passwordSalt)
                .ExecuteUpdateAsync();

            string notes = $"New user: {model.Username} created and assigned the CCC business Unit";
            await AddOperatorAuditNoteAsync(model.Username, notes);
            await transaction.CommitAsync();
            return Result.Ok();
        }

        public async Task<Result> UpdatePasswordAsync(UpdatePasswordDto model)
        {
            UserLoginDetailsDto userLogin = await _nhibernateSession.CreateSQLQuery(AuthQueries.GetUserLogin)
                .SetParameter("operatorId", model.Username)
                .SetResultTransformer(new AliasToBeanResultTransformer(typeof(UserLoginDetailsDto)))
                .UniqueResultAsync<UserLoginDetailsDto>();

            if (userLogin == null)
            {
                Logger.Log(LogLevel.Error, $"UpdatePassword failed: User not found with username:{model.Username}");
                return Result.Fail(ErrorMessages.ErrUserNotFound);
            }

            bool verifyResult = PasswordHelper.VerifyPasswordHash(model.OldPassword, userLogin.PasswordHash, userLogin.PasswordSalt);

            if (model.OldPassword.Equals(model.NewPassword, StringComparison.InvariantCulture))
                return Result.Fail(ErrorMessages.ErrNewPasswordIsSameAsOld);

            if (verifyResult == false)
                return Result.Fail(ErrorMessages.InvalidPasswordOnUpdatePassword);

            PasswordHelper.CreatePasswordHash(model.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

            int rows = await _nhibernateSession.CreateSQLQuery(@"UPDATE operator_password 
                                        set password_hash = :passwordHash,
                                        password_salt = :passwordSalt,
                                        update_tms = date('now')
                                        where operator_id = :operatorId")
                .SetParameter("passwordHash", passwordHash)
                .SetParameter("passwordSalt", passwordSalt)
                .SetParameter("operatorId", userLogin.OperatorId)
                .ExecuteUpdateAsync();
            if (rows == 0)
                return Result.Fail(ErrorMessages.ErrUserChangePassword);

            string auditNote = $"Updated password of User: {userLogin.OperatorId}";
            await AddOperatorAuditNoteAsync(userLogin.OperatorId, auditNote);
            Logger.Info(auditNote);
            return Result.Ok();
        }

        private async Task AddLoginAuditAsync(string username, HttpRequestDeviceDetails deviceDetails)
        {
            string notes = deviceDetails.IpAddress == "::1" ? "Run at localhost" : string.Empty;
            await _nhibernateSession.CreateSQLQuery("insert into operator_bus_login_audit ( operator_id, bus_unit_code, login_tms ) values ( :operatorId, 'CCC', date('now') )")
                .SetParameter("operatorId", username)
                .ExecuteUpdateAsync();

            await _nhibernateSession.CreateSQLQuery(
                 @"update operator_password 
                        set last_login_date = date('now'), 
                        bad_login_count = 0 
                    where operator_id = :operatorId 
                    and bus_unit_code = 'CCC'")
                .SetParameter("operatorId", username)
                .ExecuteUpdateAsync();

            await _nhibernateSession.CreateSQLQuery(@"insert into operator_login_attempts_audit (operator_id, appln_code, create_tms, ip_address, browser,notes) 
                    values (:operatorId,'CCC',date('now'),:ipAddress,:browser,:notes)")
                .SetParameter("operatorId", username)
                .SetParameter("ipAddress", deviceDetails.IpAddress.TruncateTrim(20))
                .SetParameter("browser", deviceDetails.Browser.TruncateTrim(20))
                .SetParameter("notes", notes)
                .ExecuteUpdateAsync().ConfigureAwait(false);
        }

        private Task AddOperatorAuditNoteAsync(string operatorId, string notes)
        {
            string auditNoteSql = @"INSERT INTO operator_audit_note (operator_id, create_by, notes, create_tms ) values ( :operatorId, :createBy, :notes, date('now') )";
            return _nhibernateSession.CreateSQLQuery(auditNoteSql)
                .SetParameter("operatorId", operatorId)
                .SetParameter("createBy", "SYSTEM")
                .SetParameter("notes", notes)
                .ExecuteUpdateAsync();
        }
    }
}