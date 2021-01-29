using System;

using FACTS.GenericBooking.Common.ExtensionMethods;
using FACTS.GenericBooking.Domain.Models.Auth;
using FACTS.GenericBooking.Repository.Ingres.Entities;

namespace FACTS.GenericBooking.Domain.Mappers
{
    public static class OperatorFromCreateUserMap
    {
        public static Operator Map(CreateUserDto model, string operatorTypeCode)
        {
            Operator map = new Operator
            {
                EmailAddress              = model.EmailAddress,
                OperatorName              = $"{model.FirstName} {model.LastName}".TruncateTrim(30),
                DepotAbbreviation         = "",
                ClosedDate                = new DateTime(1970, 1, 1, 0, 0, 0),
                CreateTms                 = DateTime.Now,
                DoNotClose                = "",
                DoNotLockInd              = 0,
                ElectronicBookingAlertInd = 0,
                ExitDate                  = new DateTime(1970, 1, 1, 0, 0, 0),
                GroupId                   = "OPERATOR",
                InternalUser              = "",
                LoginTms                  = new DateTime(1970, 1, 1, 0, 0, 0),
                MobileNo                  = model.MobileNumber,
                OperatorId                = model.Username,
                OperatorTypeCode          = operatorTypeCode,
                OrigOperatorId            = "",
                TpgAbbreviation           = "",
                UpdateTms                 = DateTime.Now,
                WebPassword               = "",
                WebSuperUser              = 0,
                WhAbbreviation            = "",
            };
            return map;
        }
    }
}