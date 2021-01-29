using FluentNHibernate.Mapping;

namespace FACTS.GenericBooking.Repository.Ingres.Entities
{
    public class AccountCustomerBusinessUnit
    {
        public virtual int AccountCustomerNo { get; set; }
        public virtual string BusinessUnitCode { get; set; }
        
        protected bool Equals(AccountCustomerBusinessUnit other)
        {
            if (other == null)
            {
                return false;
            }

            return AccountCustomerNo == other.AccountCustomerNo && BusinessUnitCode.Equals(other.BusinessUnitCode);
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (AccountCustomerBusinessUnit))
            {
                return false;
            }
            return Equals((AccountCustomerBusinessUnit) obj);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = AccountCustomerNo.GetHashCode();
                hashCode = (hashCode*397) ^ BusinessUnitCode.GetHashCode();
                return hashCode;
            }
        }
    }

    public class AccountCustomerBusinessUnitMap : ClassMap<AccountCustomerBusinessUnit> {
        public AccountCustomerBusinessUnitMap() {
            Table("acc_cus_bus_unit");
            CompositeId()
                .KeyProperty(x => x.AccountCustomerNo, keyPropertyAction: k =>
                {
                    k.ColumnName("acc_cus_no");
                    k.Access.Property();
                })
                .KeyProperty(x => x.BusinessUnitCode, keyPropertyAction: k =>
                {
                    k.ColumnName("bus_unit_code");
                    k.Access.Property();
                    k.Length(3);
                });
            //Id(x => x.AccountCustomerNo, "acc_cus_no");
            //Map(x => x.BusinessUnitCode, "bus_unit_code").Length(3).Not.Nullable();
            //Schema("dba");
        }
    }
}
