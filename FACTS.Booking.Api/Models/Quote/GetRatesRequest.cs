namespace FACTS.GenericBooking.Api.Models.Quote
{
    public class GetRatesRequest
    {
        /// <summary>
        /// Customer account number
        /// </summary>
        /// <example>PP9900</example>
        public string AccountNumber { get; set; }
        /// <summary>
        /// Filters the array of rates returned; if null all service types will be enumerated
        /// </summary>
        /// <example>standard</example>
        public string ServiceType { get; set; }
        /// <summary>
        /// Filters the array of rates returned; if null all pickupTypes will be enumerated
        /// </summary>
        /// <example>depot</example>
        public string PickupType { get; set; }
        /// <summary>
        /// Australian suburb where the vehicle is to be picked up
        /// </summary>
        /// <example>
        /// north sydney
        /// </example>
        public string PickupSuburb { get; set; }
        /// <summary>
        /// Postcode of the pickup suburb chosen
        /// </summary>
        /// <example>2060</example>
        public string PickupPostcode { get; set; }
        /// <summary>
        /// State of the pickup suburb
        /// </summary>
        /// <example>NSW</example>
        public string PickupState { get; set; }
        /// <summary>
        /// filters the array of rates returned; if null all deliveryTypes will be enumerated
        /// </summary>
        /// <example>customer</example>
        public string DeliveryType { get; set; }
        /// <summary>
        /// Australian suburb where the vehicle is to be delivered
        /// </summary>
        /// <example>cairns</example>
        public string DeliverySuburb { get; set; }
        /// <summary>
        /// Postcode of the delivery suburb chosen
        /// </summary>
        /// <example>4870</example>
        public string DeliveryPostcode { get; set; }
        /// <summary>
        /// State of the delivery suburb
        /// </summary>
        /// <example>QLD</example>
        public string DeliveryState { get; set; }
        /// <summary>
        /// indicate if vehicle is drivable 
        /// </summary>
        /// <example>true</example>
        public bool? IsDriveable { get; set; }
        /// <summary>
        /// indicate if vehicle is modified 
        /// </summary>
        /// <example>false</example>
        public bool? IsModified { get; set; }
        /// <summary>indicate if vehicle is damaged</summary>
        /// <example>false</example>
        public bool? IsDamaged { get; set; }
        /// <summary>make of the vehicle</summary>
        /// <example>ford</example>
        public string VehicleMake { get; set; }
        /// <summary>
        /// model of the vehicle make
        /// </summary>
        /// <example>falcon</example>
        public string VehicleModel { get; set; }
        /// <summary>
        /// type of vehicle make
        /// </summary>
        /// <example>sedan</example>
        public string VehicleType { get; set; }
        /// <summary>
        /// vehicle value
        /// </summary>
        /// <example>12000</example>
        public int? VehicleValue { get; set; }
    }
}