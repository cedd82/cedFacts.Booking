using System;

using FluentNHibernate.Mapping;

namespace FACTS.GenericBooking.Repository.Ingres.Entities
{
    public class Quote
    {
        public virtual int QuoteNo { get; set; }
        public virtual string QuoteStatusCode { get; set; }
        public virtual DateTime StatusEffectDate { get; set; }
        public virtual int CasualCustomerIndicator { get; set; }
        public virtual string CasualCustomerName { get; set; }
        public virtual string CasualCustomerStreet { get; set; }
        public virtual string CasualCustomerSuburb { get; set; }
        public virtual string CasualCustomerState { get; set; }
        public virtual string ContactTitle { get; set; }
        public virtual string ContactFirstName { get; set; }
        public virtual string ContactSurname { get; set; }
        public virtual string ContactStdNo { get; set; }
        public virtual string ContactPhoneNo { get; set; }
        public virtual string ContactFaxNo { get; set; }
        public virtual int MktCode { get; set; }
        public virtual int AccCusNo { get; set; }
        public virtual string PickupSuburb { get; set; }
        public virtual string PickupState { get; set; }
        public virtual string PickupLocationCode { get; set; }
        public virtual string DeliverySuburb { get; set; }
        public virtual string DeliveryState { get; set; }
        public virtual string DeliveryLocationCode { get; set; }
        public virtual int VehCount { get; set; }
        public virtual decimal TotQuoteCharge { get; set; }
        public virtual string OperatorId { get; set; }
        public virtual DateTime QuoteTms { get; set; }
        public virtual string RateGroupCode { get; set; }
        public virtual decimal GstCharge { get; set; }
        public virtual string ContactMobileNo { get; set; }
        public virtual int PrintInd { get; set; }
        public virtual int TransitDays { get; set; }
        public virtual string EmailId { get; set; }
        public virtual int WebInd { get; set; }
        public virtual string WebLogin { get; set; }
        public virtual string ContactEmailId { get; set; }
        public virtual string PickupStreet { get; set; }
        public virtual string DeliveryStreet { get; set; }
    }

    public class QuoteTypeMap : ClassMap<Quote>
    {
        public QuoteTypeMap()
        {
            Table("quote");
            Id(x => x.QuoteNo, "quote_no")
                .Access.Property()
                .Unique()
                .Not.Nullable()
                .GeneratedBy.Sequence("seq_quote_no");
            //.GeneratedBy.SequenceIdentity("seq_quote_no");

            //Map(x => x.QuoteNo, "quote_no").Not.Nullable();
            Map(x => x.QuoteStatusCode, "quote_status_code").Length(3).Not.Nullable();
            Map(x => x.StatusEffectDate, "status_effect_date").Not.Nullable();
            Map(x => x.CasualCustomerIndicator, "cas_cus_ind").Not.Nullable();
            Map(x => x.CasualCustomerName, "cas_cus_name").Length(30).Not.Nullable();
            Map(x => x.CasualCustomerStreet, "cas_cus_street").Length(30).Not.Nullable();
            Map(x => x.CasualCustomerSuburb, "cas_cus_suburb").Length(20).Not.Nullable();
            Map(x => x.CasualCustomerState, "cas_cus_state").Length(3).Not.Nullable();
            Map(x => x.ContactTitle, "cont_title").Length(6).Not.Nullable();
            Map(x => x.ContactFirstName, "cont_first_name").Length(30).Not.Nullable();
            Map(x => x.ContactSurname, "cont_surname").Length(30).Not.Nullable();
            Map(x => x.ContactStdNo, "cont_std_no").Length(4).Not.Nullable();
            Map(x => x.ContactPhoneNo, "cont_phone_no").Length(8).Not.Nullable();
            Map(x => x.ContactFaxNo, "cont_fax_no").Length(8).Not.Nullable();
            Map(x => x.MktCode, "mkt_code").Not.Nullable();
            Map(x => x.AccCusNo, "acc_cus_no").Not.Nullable();
            Map(x => x.PickupSuburb, "pkup_suburb").Length(20).Not.Nullable();
            Map(x => x.PickupState, "pkup_state").Length(3).Not.Nullable();
            Map(x => x.PickupLocationCode, "pkup_location_code").Length(3).Not.Nullable();
            Map(x => x.DeliverySuburb, "dlvr_suburb").Length(20).Not.Nullable();
            Map(x => x.DeliveryState, "dlvr_state").Length(3).Not.Nullable();
            Map(x => x.DeliveryLocationCode, "dlvr_location_code").Length(3).Not.Nullable();
            Map(x => x.VehCount, "veh_count").Not.Nullable();
            Map(x => x.TotQuoteCharge, "tot_quote_chrg").Not.Nullable();
            Map(x => x.OperatorId, "operator_id").Length(8).Not.Nullable();
            Map(x => x.QuoteTms, "quote_tms").Not.Nullable();
            Map(x => x.RateGroupCode, "rate_group_code").Length(3).Not.Nullable();
            Map(x => x.GstCharge, "gst_chrg").Not.Nullable();
            Map(x => x.ContactMobileNo, "cont_mobile_no").Length(15).Not.Nullable();
            Map(x => x.PrintInd, "print_ind").Not.Nullable();
            Map(x => x.TransitDays, "transit_days").Not.Nullable();
            Map(x => x.EmailId, "email_id").Length(50).Not.Nullable();
            Map(x => x.WebInd, "web_ind").Not.Nullable();
            Map(x => x.WebLogin, "web_login").Length(20).Not.Nullable();
            Map(x => x.ContactEmailId, "cont_email_id").Length(70).Not.Nullable();
            Map(x => x.PickupStreet, "pkup_street").Length(30).Not.Nullable();
            Map(x => x.DeliveryStreet, "dlvr_street").Length(30).Not.Nullable();

        }
    }
}

//CREATE TABLE dba.quote (
//quote_no INTEGER NOT NULL,
//quote_status_code CHAR(3) NOT NULL,
//status_effect_date INGRESDATE NOT NULL,
//CasualCustomer_ind INTEGER NOT NULL,
//CasualCustomer_name VARCHAR(30) NOT NULL,
//CasualCustomer_street VARCHAR(30) NOT NULL,
//CasualCustomer_suburb VARCHAR(20) NOT NULL,
//CasualCustomer_state CHAR(3) NOT NULL,
//Contact_title CHAR(6) NOT NULL,
//Contact_first_name VARCHAR(30) NOT NULL,
//Contact_surname VARCHAR(30) NOT NULL,
//Contact_std_no CHAR(4) NOT NULL,
//Contact_phone_no CHAR(8) NOT NULL,
//Contact_fax_no CHAR(8) NOT NULL,
//mkt_code INTEGER NOT NULL,
//acc_cus_no INTEGER NOT NULL,
//Pickup_suburb VARCHAR(20) NOT NULL,
//Pickup_state CHAR(3) NOT NULL,
//Pickup_location_code CHAR(3) NOT NULL,
//Delivery_suburb VARCHAR(20) NOT NULL,
//Delivery_state CHAR(3) NOT NULL,
//Delivery_location_code CHAR(3) NOT NULL,
//veh_count INTEGER NOT NULL,
//tot_quote_chrg MONEY NOT NULL,
//operator_id CHAR(8) NOT NULL,
//quote_tms INGRESDATE NOT NULL,
//rate_group_code CHAR(3) NOT NULL,
//gst_chrg MONEY NOT NULL,
//Contact_mobile_no VARCHAR(15) NOT NULL,
//print_ind INTEGER NOT NULL,
//transit_days INTEGER NOT NULL,
//email_id VARCHAR(50) NOT NULL,
//web_ind INTEGER NOT NULL,
//web_login VARCHAR(20) NOT NULL,
//Contact_email_id VARCHAR(70) NOT NULL,
//Pickup_street VARCHAR(30) NOT NULL,
//Delivery_street VARCHAR(30) NOT NULL,
//CONSTRAINT "QUOTE_PK" PRIMARY KEY (quote_no)
//);
//CREATE INDEX quote_1 ON dba.quote (quote_tms);