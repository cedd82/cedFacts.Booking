using FluentNHibernate.Mapping;

namespace FACTS.GenericBooking.Repository.Ingres.Entities
{
    public class MakeType
    {
        public virtual string Description { get; set; }
        public virtual int IsLogDelete { get; set; }
        public virtual int IsWeb { get; set; }
        public virtual int MakeCode { get; set; }
        public virtual int ScanPrefixLength { get; set; }
        public virtual int ScanSuffixLength { get; set; }
        public virtual int ScanVinLength { get; set; }
    }

    public class MakeTypeMap : ClassMap<MakeType>
    {
        public MakeTypeMap()
        {
            Table("make_type");
            Id(x => x.MakeCode, "make_code")
                .Access.Property()
                .Unique()
                .Not.Nullable()
                .GeneratedBy.Assigned();

            Map(x => x.Description, "description").Length(20).Not.Nullable();
            Map(x => x.IsLogDelete, "log_del_ind").Not.Nullable();
            Map(x => x.IsWeb, "web_ind").Not.Nullable();
            Map(x => x.ScanPrefixLength, "scan_prefix_length").Not.Nullable();
            Map(x => x.ScanSuffixLength, "scan_suffix_length").Not.Nullable();
            Map(x => x.ScanVinLength, "scan_vin_length").Not.Nullable();
        }
    }
}

//CREATE TABLE dba.make_type (
//	make_code INTEGER NOT NULL,
//	description VARCHAR(20) NOT NULL,
//	log_del_ind INTEGER NOT NULL,
//	web_ind INTEGER NOT NULL,
//	scan_prefix_length INTEGER NOT NULL,
//	scan_suffix_length INTEGER NOT NULL,
//	scan_vin_length INTEGER NOT NULL,
//	CONSTRAINT MAKE_TYPE_PK PRIMARY KEY (make_code)
//);