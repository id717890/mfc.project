using FluentNHibernate.Mapping;
using mfc.domain.entities;

namespace mfc.dal.mapping {
    public class CustomerTypeMap : ClassMap<CustomerType> {
        public CustomerTypeMap() {
            Table("CustomerTypes");
            Id(e => e.Id).GeneratedBy.Assigned();
            Map(e => e.Caption);
            Map(e => e.IsDeleted, "is_deleted");
        }
    }
}