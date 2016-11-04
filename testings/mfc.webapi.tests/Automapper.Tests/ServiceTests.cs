using mfc.domain.entities;
using mfc.webapi.Models;
using NUnit.Framework;

namespace mfc.webapi.tests.Automapper.Tests
{
    public class ServiceTests : AutomapperBaseTests<Service, ServiceInfo>
    {

        [TestCase(1, "Service caption 1", 1, "Organization caption 3")]
        [TestCase(2, "Service caption 2", 3, "Organization caption 1")]
        [TestCase(4, "Service caption 3", 2, "Organization caption 2")]
        public void ServiceToModel(long serviceId, string serviceCaption, long organizationId, string organizationCaption)
        {
            var service = new Service
            {
                Id = serviceId,
                Caption = serviceCaption,
                Organization = new Organization()
                {
                    Id = organizationId,
                    Caption = organizationCaption,
                }
            };
            AssertEntity(service);
        }


        [TestCase(1, "Service caption", 1, "Organization caption")]
        public void ModelToService(long serviceId, string serviceCaption, long organizationId, string organizationCaption)
        {
            var service = new ServiceInfo
            {
                Id = serviceId,
                Caption = serviceCaption,
                OrganizationId = organizationId,
                Organization = organizationCaption,
            };
            AssertModel(service);
        }

        protected override void AssertEntityToModel(Service entity, ServiceInfo model)
        {
            Assert.AreEqual(entity.Id, model.Id);
            Assert.AreEqual(entity.Caption, model.Caption);
            Assert.AreEqual(entity.Organization.Id, model.OrganizationId);
            Assert.AreEqual(entity.Organization.Caption, model.Organization);
        }

        protected override void AssertModelToEntity(ServiceInfo model, Service entity)
        {
            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Caption, entity.Caption);
            Assert.AreEqual(model.OrganizationId, entity.Organization.Id);
            Assert.AreEqual(model.Organization, entity.Organization.Caption);
        }
    }
}
