using mfc.domain.entities;
using mfc.webapi.Models;
using NUnit.Framework;

namespace mfc.webapi.tests.Automapper.Tests
{
    [TestFixture]
    [Category(TestCategories.AutomapperTest)]
    public class UserTests : AutomapperBaseTests<User, UserInfo>
    {
        [TestCase(1, "account", "name", true, true, true)]
        [TestCase(1, "account", "name", false, true, true)]
        [TestCase(1, "account", "name", true, false, true)]
        [TestCase(1, "account", "name", true, true, false)]
        [TestCase(1, "account", "name", false, false, true)]
        [TestCase(1, "account", "name", false, true, false)]
        [TestCase(1, "account", "name", true, false, false)]
        [TestCase(1, "account", "name", false, false, false)]
        [TestCase(2, "account", "name", true, true, true)]
        [TestCase(2, "account", "name", false, true, true)]
        [TestCase(2, "account", "name", true, false, true)]
        [TestCase(2, "account", "name", true, true, false)]
        [TestCase(2, "account", "name", false, false, true)]
        [TestCase(2, "account", "name", false, true, false)]
        [TestCase(2, "account", "name", true, false, false)]
        [TestCase(2, "account", "name", false, false, false)]
        public void UserToModel(long id, string account, string name, bool isAdmin, bool isController, bool isExpert)
        {
            var user = new User();
            user.Id = id;
            user.IsAdmin = isAdmin;
            user.IsController = isController;
            user.IsExpert = isExpert;
            user.Name = name;
            user.Account = account;
            AssertEntity(user);
        }

        [TestCase(1, "account", "name", true, true, true)]
        [TestCase(1, "account", "name", false, true, true)]
        [TestCase(1, "account", "name", true, false, true)]
        [TestCase(1, "account", "name", true, true, false)]
        [TestCase(1, "account", "name", false, false, true)]
        [TestCase(1, "account", "name", false, true, false)]
        [TestCase(1, "account", "name", true, false, false)]
        [TestCase(1, "account", "name", false, false, false)]
        [TestCase(2, "account", "name", true, true, true)]
        [TestCase(2, "account", "name", false, true, true)]
        [TestCase(2, "account", "name", true, false, true)]
        [TestCase(2, "account", "name", true, true, false)]
        [TestCase(2, "account", "name", false, false, true)]
        [TestCase(2, "account", "name", false, true, false)]
        [TestCase(2, "account", "name", true, false, false)]
        [TestCase(2, "account", "name", false, false, false)]
        public void ModelToUser(long id, string account, string name, bool isAdmin, bool isController, bool isExpert)
        {
            var user = new UserInfo();
            user.Id = id;
            user.IsAdmin = isAdmin;
            user.IsController = isController;
            user.IsExpert = isExpert;
            user.Name = name;
            user.Account = account;
            AssertModel(user);
        }

        protected override void AssertEntityToModel(User entity, UserInfo model)
        {
            Assert.AreEqual(entity.Id, model.Id);
            Assert.AreEqual(entity.IsAdmin, model.IsAdmin);
            Assert.AreEqual(entity.IsController, model.IsController);
            Assert.AreEqual(entity.IsExpert, model.IsExpert);
            Assert.AreEqual(entity.Name, model.Name);
            Assert.AreEqual(entity.Account, model.Account);
        }

        protected override void AssertModelToEntity(UserInfo model, User entity)
        {
            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.IsAdmin, entity.IsAdmin);
            Assert.AreEqual(model.IsController, entity.IsController);
            Assert.AreEqual(model.IsExpert, entity.IsExpert);
            Assert.AreEqual(model.Name, entity.Name);
            Assert.AreEqual(model.Account, entity.Account);
        }
    }
}
