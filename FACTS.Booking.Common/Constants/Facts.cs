using System.Collections.Generic;

namespace FACTS.GenericBooking.Common.Constants
{
    public static class AddressConsts
    {
        public const string Pick = "Pickup";
        public const string Deliver = "Delivery";
        public static IReadOnlyList<string> States { get; } = new List<string>
        {
            "NSW", "QLD", "SA", "TAS", "VIC", "WA", "ACT", "NT"
        };
    }

    public static class CustomClaims
    {
        public const string OperatorId = "OperatorId";
    }

    public static class AuthRoles
    {
        public const string ExternalUser = "EXN";
    }

    public static class LocationType
    {
        public const string Depot = "DEP";
        public const string Customer = "CUS";
    }

    public static class ServiceType
    {
        public const string Standard = "STD";
        public const string Express = "PEX";
        public const string PremiumEnclosed = "CPM";
        //public const string COVERED = "COV";
    }

    public static class ServiceTypeDescription
    {
        public const string Standard = "STANDARD";
        public const string Express = "EXPRESS";
        public const string PremiumEnclosed = "PREMIUMENCLOSED";
        //public const string Compound = "RAPT";
    }

    public static class QuoteConsts
    {
        public const string SpotSpecial = "SPT";
        public const string CasualCustomerAccountNumber = "PP9900";
    }

    public static class RateGroupCode
    {
        public const string AghGroup = "AHG";
        public const string Casual = "CAS";
        public const string Contract = "CON";
        public const string Dealer = "DLR";
        public const string TollTransaction = "ERE";
        public const string Grace = "GRC";
        public const string Removalist = "MRM";
        public const string OnlineBroker = "MVG";
        public const string NationwideTowing = "NWT";
        public const string OlbMovingAgain = "OLB";
        public const string MajorRemovalist = "REM";
        public const string Tmca = "TMC";
        public const string Test = "TRE";
    }
}