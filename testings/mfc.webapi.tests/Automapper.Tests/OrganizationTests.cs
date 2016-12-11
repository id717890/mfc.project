using mfc.domain.entities;
using mfc.webapi.Models;
using NUnit.Framework;

namespace mfc.webapi.tests.Automapper.Tests
{
    [TestFixture]
    [Category(TestCategories.AutomapperTest)]
    public class OrganizationTests : AutomapperBaseTests<Organization, OrganizationInfo>
    {

        [TestCase(1, "Organization caption", "Full organization caption", 1, "type 1")]
        [TestCase(2, "Organization caption", "Full organization caption", 2, "type 2")]
        [TestCase(3, "Organization caption", "Full organization caption", 1, "type 1")]
        [TestCase(4, "Organization caption", "Full organization caption", 3, "type 3")]
        public void OrganizationToModel(long id, string caption, string fullcaption, long typeId, string typeCaption)
        {
            var entity = new Organization
            {
                Id = id,
                Caption = caption,
                FullCaption = fullcaption,
                Type = new OrganizationType
                {
                    Id = typeId,
                    Caption = typeCaption
                }
            };
            AssertEntity(entity);
        }

        [TestCase(1, "Organization caption", "Full organization caption", 1, "type 1")]
        [TestCase(2, "Organization caption", "Full organization caption", 2, "type 2")]
        [TestCase(3, "Organization caption", "Full organization caption", 1, "type 1")]
        [TestCase(4, "Organization caption", "Full organization caption", 3, "type 3")]
        public void ModelToOrganization(long id, string caption, string fullcaption, long typeId, string typeCaption)
        {
            var model = new OrganizationInfo
            {
                Id = id,
                Caption = caption,
                FullCaption = fullcaption,
                OrganizationType = new OrganizationTypeInfo
                {
                    Id = typeId,
                    Caption = typeCaption
                }
            };
            AssertModel(model);
        }


        protected override void AssertEntityToModel(Organization entity, OrganizationInfo model)
        {
            Assert.AreEqual(entity.Id, model.Id);
            Assert.AreEqual(entity.Caption, model.Caption);
            Assert.AreEqual(entity.FullCaption, model.FullCaption);
            Assert.AreEqual(entity.Type.Id, model.OrganizationType.Id);
            Assert.AreEqual(entity.Type.Caption, model.OrganizationType.Caption);
        }

        protected override void AssertModelToEntity(OrganizationInfo model, Organization entity)
        {
            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Caption, entity.Caption);
            Assert.AreEqual(model.FullCaption, entity.FullCaption);
            Assert.AreEqual(model.OrganizationType.Id, entity.Type.Id);
            Assert.AreEqual(model.OrganizationType.Caption, entity.Type.Caption);
        }
    }
}
