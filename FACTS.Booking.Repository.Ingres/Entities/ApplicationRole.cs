using FluentNHibernate.Mapping;

namespace FACTS.GenericBooking.Repository.Ingres.Entities
{
    public class ApplicationRole
    {
        public virtual int RoleId { get; set; }
        public virtual string Description { get; set; }
        public virtual int RoleLevel { get; set; }
        public virtual string BusinessUnitType { get; set; }
        public virtual int DeletedIndicator { get; set; }
    }

    public class ApplicationRoleMap : ClassMap<ApplicationRole>
    {
        public ApplicationRoleMap()
        {
            Table("appln_role");
            Id(x => x.RoleId, "role_id")
                .Access.Property()
                .Unique()
                .Not.Nullable()
                .GeneratedBy.Assigned();

            Map(x => x.Description, "description").Length(25).Not.Nullable();
            Map(x => x.RoleLevel, "role_level").Not.Nullable();
            Map(x => x.BusinessUnitType, "bus_unit_type").Length(3).Not.Nullable();
            Map(x => x.DeletedIndicator, "log_del_ind").Not.Nullable();
        }
    }
}
//CREATE TABLE dba.appln_role (
//role_id INTEGER NOT NULL,
//description VARCHAR(25) NOT NULL,
//role_level INTEGER NOT NULL,
//bus_unit_type CHAR(3) NOT NULL,
//log_del_ind INTEGER NOT NULL,
//CONSTRAINT "APPLN_ROLE_PK" PRIMARY KEY (role_id)
//);

