using System;

using FluentNHibernate.Mapping;

namespace FACTS.GenericBooking.Repository.Ingres.Entities
{
    public class Operator
    {
        public virtual string OperatorId { get; set; }
        public virtual string GroupId { get; set; }
        public virtual string OperatorName { get; set; }
        public virtual string DepotAbbreviation { get; set; }
        public virtual string WhAbbreviation { get; set; }
        public virtual int ElectronicBookingAlertInd { get; set; }
        public virtual string MobileNo { get; set; }
        public virtual string TpgAbbreviation { get; set; }
        public virtual DateTime UpdateTms { get; set; }
        public virtual DateTime CreateTms { get; set; }
        public virtual DateTime ExitDate { get; set; }
        public virtual string OrigOperatorId { get; set; }
        public virtual DateTime ClosedDate { get; set; }
        public virtual DateTime LoginTms { get; set; }
        public virtual string DoNotClose { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string WebPassword { get; set; }
        public virtual int WebSuperUser { get; set; }
        public virtual string InternalUser { get; set; }
        public virtual string OperatorTypeCode { get; set; }
        public virtual int DoNotLockInd { get; set; }
    }

    public class OperatorMap : ClassMap<Operator>
    {
        public OperatorMap()
        {
            Table("operator");
            Id(x => x.OperatorId, "operator_id")
                .Access.Property()
                .Length(8).Not.Nullable()
                .GeneratedBy.Assigned();
            Map(x => x.GroupId, "group_id").Length(20).Not.Nullable();
            Map(x => x.OperatorName, "operator_name").Length(30).Not.Nullable();
            Map(x => x.DepotAbbreviation, "depot_abrv").Length(3).Not.Nullable();
            Map(x => x.WhAbbreviation, "wh_abrv").Length(3).Not.Nullable();
            Map(x => x.ElectronicBookingAlertInd, "elec_bkg_alert_ind").Not.Nullable();
            Map(x => x.MobileNo, "mobile_no").Length(10).Not.Nullable();
            Map(x => x.TpgAbbreviation, "tpg_abrv").Length(3).Not.Nullable();
            Map(x => x.UpdateTms, "update_tms").Not.Nullable();
            Map(x => x.CreateTms, "create_tms").Not.Nullable();
            Map(x => x.ExitDate, "exit_date").Not.Nullable();
            Map(x => x.OrigOperatorId, "orig_operator_id").Length(8).Not.Nullable();
            Map(x => x.ClosedDate, "closed_date").Not.Nullable();
            Map(x => x.LoginTms, "login_tms").Not.Nullable();
            Map(x => x.DoNotClose, "do_not_close").Length(1).Not.Nullable();
            Map(x => x.EmailAddress, "email_addr").Length(50).Not.Nullable();
            Map(x => x.WebPassword, "web_password").Length(15).Not.Nullable();
            Map(x => x.WebSuperUser, "web_super_user").Not.Nullable();
            Map(x => x.InternalUser, "internal_user").Length(1).Not.Nullable();
            Map(x => x.OperatorTypeCode, "operator_type_code").Length(3).Not.Nullable();
            Map(x => x.DoNotLockInd, "do_not_lock_ind").Not.Nullable();
        }
    }
}

//CREATE TABLE dba.operator (
//operator_id CHAR(8) NOT NULL,
//group_id VARCHAR(20) NOT NULL,
//operator_name VARCHAR(30) NOT NULL,
//depot_abrv CHAR(3) NOT NULL,
//wh_abrv CHAR(3) NOT NULL,
//elec_bkg_alert_ind INTEGER NOT NULL,
//mobile_no CHAR(10) NOT NULL,
//tpg_abrv CHAR(3) NOT NULL,
//update_tms INGRESDATE NOT NULL,
//create_tms INGRESDATE NOT NULL,
//exit_date INGRESDATE NOT NULL,
//orig_operator_id CHAR(8) NOT NULL,
//closed_date INGRESDATE NOT NULL,
//login_tms INGRESDATE NOT NULL,
//do_not_close CHAR(1) NOT NULL,
//email_addr VARCHAR(50) NOT NULL,
//web_password VARCHAR(15) NOT NULL,
//web_super_user INTEGER NOT NULL,
//internal_user CHAR(1) NOT NULL,
//operator_type_code CHAR(3) NOT NULL,
//do_not_lock_ind INTEGER NOT NULL,
//CONSTRAINT "OPERATOR_PK" PRIMARY KEY (operator_name)
//);
//CREATE UNIQUE INDEX operator_1 ON dba.operator (operator_name);