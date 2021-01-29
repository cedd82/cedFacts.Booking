using System;

using FluentNHibernate.Mapping;

namespace FACTS.GenericBooking.Repository.Ingres.Entities
{
    public class OperatorPassword
    {
        public virtual int BadLoginCount { get; set; }
        public virtual string BusUnitCode { get; set; }
        public virtual DateTime CreateTms { get; set; }
        public virtual DateTime LastLoginDate { get; set; }
        public virtual int LockInd { get; set; }
        public virtual string OperatorId { get; set; }
        public virtual byte[] PasswordHash { get; set; }
        public virtual byte[] PasswordSalt { get; set; }
        public virtual DateTime UpdateTms { get; set; }
    }

    public class OperatorPasswordMap : ClassMap<OperatorPassword>
    {
        public OperatorPasswordMap()
        {
            Table("operator_password");
            Id(x => x.OperatorId, "operator_id")
                .Access.Property()
                .Length(8).Not.Nullable()
                .GeneratedBy.Assigned();
            Map(x => x.BadLoginCount, "bad_login_count").Not.Nullable();
            Map(x => x.BusUnitCode, "bus_unit_code").Length(3).Not.Nullable();
            Map(x => x.CreateTms, "create_tms").Not.Nullable();
            Map(x => x.LastLoginDate, "last_login_date").Not.Nullable();
            Map(x => x.LockInd, "lock_ind").Not.Nullable();
            //Map(x => x.OperatorId, "operator_id").Length(8).Not.Nullable();
            Map(x => x.PasswordHash, "password_hash").Length(64).Not.Nullable();
            Map(x => x.PasswordSalt, "password_salt").Length(128).Not.Nullable();
            Map(x => x.UpdateTms, "update_tms").Not.Nullable();
        }
    }
}

//CREATE TABLE dba.operator_password (
//operator_id CHAR(8) NOT NULL,
//bus_unit_code CHAR(3) NOT NULL,
//password_hash BYTE VARYING(64) NOT NULL,
//password_salt BYTE VARYING(128) NOT NULL,
//create_tms INGRESDATE NOT NULL,
//update_tms INGRESDATE NOT NULL,
//lock_ind INTEGER NOT NULL,
//bad_login_count INTEGER NOT NULL,
//last_login_date INGRESDATE NOT NULL,
//CONSTRAINT "OPERATOR_PASSWORD_PK" PRIMARY KEY (operator_id,bus_unit_code)
//);