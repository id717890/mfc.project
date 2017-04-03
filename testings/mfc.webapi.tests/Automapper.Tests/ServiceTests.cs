using mfc.domain.entities;
using mfc.webapi.Models;
using NUnit.Framework;

namespace mfc.webapi.tests.Automapper.Tests
{
    [TestFixture]
    [Category(TestCategories.AutomapperTest)]
    public class ServiceTests : AutomapperBaseTests<Service, ServiceModel>
    {

        [TestCase(1, "Service caption 1", null)]
        [TestCase(2, "Service caption 2", null)]
        [TestCase(4, "Service caption 3", null)]
        public void ServiceToModel(long serviceId, string serviceCaption, Organization organization)
        {
            var service = new Service
            {
                Id = serviceId,
                Caption = serviceCaption,
                Organization = organization
            };
            AssertEntity(service);
        }


        [TestCase(1, "Service caption", null)]
        public void ModelToService(long serviceId, string serviceCaption, OrganizationModel organization)
        {
            var service = new ServiceModel
            {
                Id = serviceId,
                Caption = serviceCaption,
                Organization = organization
            };
            AssertModel(service);
        }

        protected override void AssertEntityToModel(Service entity, ServiceModel model)
        {
            Assert.AreEqual(entity.Id, model.Id);
            Assert.AreEqual(entity.Caption, model.Caption);
            Assert.AreEqual(entity.Organization, model.Organization);
        }

        protected override void AssertModelToEntity(ServiceModel model, Service entity)
        {
            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Caption, entity.Caption);
            Assert.AreEqual(model.Organization, entity.Organization);
        }
    }
}
