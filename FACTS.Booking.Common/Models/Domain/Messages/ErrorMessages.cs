namespace FACTS.GenericBooking.Common.Models.Domain.Messages
{
    public static class ErrorMessages
    {
        public static readonly string ApiException = "A0000 Unhandled error";
        public static readonly ApiMessage UnableToSendEmail = new ApiMessage("A0003 Unable To Send Email.");
        public static ApiMessage CustomerAccountDetailsNotFound(string accountNumber) => new ApiMessage($"A0050 account details not found for account: {accountNumber}");
        public static ApiMessage DeliveryLocationNotFound(string suburb, string postcode, string state) 
            => new ApiMessage($"A0051 delivery address is invalid for combination for postcode:{postcode} suburb:{suburb} state:{state}");
        public static ApiMessage PickLocationNotFound(string suburb, string postcode, string state) 
            => new ApiMessage($"A0053 pickup address is invalid for combination for postcode:{postcode} suburb:{suburb} state:{state}");
        public static readonly string UnableToCreateQuote = "A0054 Unable to create quote. Contact CEVA directly for a quote";
        public static ApiMessage VehicleTypeNotFound(string make, string model, string type) => 
            new ApiMessage($"A0055 Vehicle not found make:{make} model:{model} type:{type}");

        public static readonly ApiMessage TransportChargesCouldNotBeDetermined = new ApiMessage("A0056 Transportation charges can not be calculated");
        public static readonly ApiMessage TransportChargesNotValid = new ApiMessage("A0057 Transportation charges are not valid");
        public static readonly ApiMessage RateGroupTypeNotFound = new ApiMessage("A0058 Unable to calculate insurance excess rate");
        public static readonly ApiMessage UnableToGetRatesForRateGroupAndServiceType = new ApiMessage("A0262 Unable to get vehicle rate. Contact CEVA directly for a rate.");

        // auth
        public static readonly ApiMessage UsernameOrPasswordIncorrect = new ApiMessage("A0200 Username or password is incorrect.");
        public static readonly ApiMessage UserIsDisabled = new ApiMessage("A0201 User is disabled in the system.");
        public static readonly ApiMessage UserIsLockedOut = new ApiMessage("A0202 User login is locked");
        public static readonly ApiMessage UserNotFound = new ApiMessage("A0203 User not found.");
        public static readonly ApiMessage InvalidUserTypeCodeOrDoesNotExist = new ApiMessage("A0214 User code not valid.");
        public static readonly ApiMessage ErrUserPassword = new ApiMessage("A0215 Invalid Password");
        public static readonly ApiMessage ErrUserMissingPassword = new ApiMessage("A0216 Password is missing.");
        public static readonly ApiMessage ErrUserPasswordLength = new ApiMessage("A0217 Minimum length of password is ");
        public static readonly ApiMessage ErrUserPasswordLengthRange = new ApiMessage("A0217 Passwords need be of length of at least ");
        public static readonly ApiMessage ErrUserPasswordHash = new ApiMessage("A0217 Invalid length of password hash(64 bytes expected).");
        public static readonly ApiMessage ErrUserPasswordSalt = new ApiMessage("A0218 Invalid length of password salt(128 bytes expected).");
        public static readonly ApiMessage ErrUserPermission = new ApiMessage("A0219 Could not get user permission.");
        public static readonly ApiMessage ErrUserNotFound = new ApiMessage("A0220 User not found");
        public static readonly ApiMessage ErrUserResetPassword = new ApiMessage("A0221 Reset password failed");
        public static readonly ApiMessage ErrUserMissingUsercode = new ApiMessage("A0222 User Code is missing");
        public static readonly ApiMessage UserAlreadyExists = new ApiMessage("A0223 User already exists.");
        public static readonly ApiMessage ErrUserCurrentPassword = new ApiMessage("A0225 Current password is missing");
        public static readonly ApiMessage ErrUserModifyUser = new ApiMessage("A0226 Updating user profile failed");
        public static readonly ApiMessage ErrUserEmptyCountryAdmin = new ApiMessage("A0227 Country Admin must be set to true.");
        public static readonly ApiMessage ErrUserMissingUserTypeCode = new ApiMessage("A0228 User Type Code is missing ");
        public static readonly ApiMessage ErrUserNotFoundUserType = new ApiMessage("A0229 User Type Code not found.");
        public static readonly ApiMessage ErrUserMissingEmail = new ApiMessage("A0230 Email address is missing");
        public static readonly ApiMessage ErrUserNotFoundMenuUser = new ApiMessage("A0231 Menu is not setup for this user role");
        public static readonly ApiMessage ErrUserFromUserCode = new ApiMessage("A0232 From Usercode is missing");
        public static readonly ApiMessage ErrUserToUserCode = new ApiMessage("A0233 To Usercode is missing");
        public static readonly ApiMessage ErrUserConfirmPassword = new ApiMessage("A0234 Confirm password is missing");
        public static readonly ApiMessage ErrUserMatchPassword = new ApiMessage("A0235 Password and Confirm Password do not match");
        public static readonly ApiMessage ErrUserUserName = new ApiMessage("A0236 User Name is missing");
        public static readonly ApiMessage ErrUserChangePassword = new ApiMessage("A0237 Change password failed");
        public static readonly ApiMessage ErrUserNewPassword = new ApiMessage("A0238 New Password is missing");
        public static readonly ApiMessage ErrInvalidCulture = new ApiMessage("A0239 Invalid culture");
        public static readonly ApiMessage InvalidPasswordOnUpdatePassword = new ApiMessage("A0240 Password is incorrect.");
        public static readonly ApiMessage ErrNewPasswordIsSameAsOld = new ApiMessage("A0241 New Password must be different.");
        public static readonly ApiMessage ErrNewPasswordUppercaseLetter = new ApiMessage("A0241 Password requires at least one upper case character");
        public static readonly ApiMessage ErrNewPasswordLowercaseLetter = new ApiMessage("A0241 Password requires at least one lower case character");
        public static readonly ApiMessage ErrNewPasswordDigit = new ApiMessage("A0241 Password requires at least one digit");
        public static readonly ApiMessage ErrNewPasswordSpecialCharacter = new ApiMessage("A0241 Password requires at least one special character");
        public static readonly ApiMessage ErrNewPasswordHasInvalidSpecialCharacter = new ApiMessage("A0241 Password can only have alphanumeric characters.");
        public static readonly ApiMessage AccountNotFound = new ApiMessage("A0260 Account not found.");
        public static readonly ApiMessage AccountDetailsNotFound = new ApiMessage("A0261 AccountDetails not found.");
    }
}