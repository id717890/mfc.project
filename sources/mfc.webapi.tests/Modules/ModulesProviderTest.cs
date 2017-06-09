using mfc.webapi.Helpers;
using NUnit.Framework;
using System;
using System.Linq;

namespace mfc.webapi.tests.Modules
{
    [TestFixture]
    [Category(TestCategories.ActualTest)]
    public class ModulesProviderTest
    {
        [Test]
        public void Create_Provider_ArgumentException_When_Passed_Wrong_Config()
        {
            Assert.Throws<ArgumentNullException>(() => { new ModulesProvider(null); });
            Assert.Throws<ArgumentException>(() => { new ModulesProvider(new MockModulesConfiguration("")); });
        }

        [Test]
        public void Modules_Count_Equals_3()
        {
            var modulesProvider = new ModulesProvider(new MockModulesConfiguration());
            Assert.AreEqual(3, modulesProvider.GetRoot().Count());
        }

        [Test]
        public void Modules_All_Count_Equals_4()
        {
            var modulesProvider = new ModulesProvider(new MockModulesConfiguration());
            Assert.AreEqual(3, modulesProvider.GetRoot().Count());
        }

        [Test]
        public void Opeation_Count_Equals_3()
        {
            var modulesProvider = new ModulesProvider(new MockModulesConfiguration());
            var modules = modulesProvider.GetRoot().ToList();
            Assert.AreEqual(3, modules[0].Modules.ToList()[0].Operations.Count());
        }

        [TestCase(0, "admin", "Администрирование")]
        [TestCase(1, "actions", "Приемы")]
        [TestCase(2, "files", "Дела")]
        public void Module_Code_And_Name_Equals_Expected(int index, string code, string name)
        {
            var modulesProvider = new ModulesProvider(new MockModulesConfiguration());
            var module = modulesProvider.GetRoot().ToList()[index];
            Assert.AreEqual(code, module.Code);
            Assert.AreEqual(name, module.Name);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        public void Child_Module_Count_Equals_Expected(int index, int count)
        {
            var modulesProvider = new ModulesProvider(new MockModulesConfiguration());
            var module = modulesProvider.GetRoot().ToList()[index];
            Assert.AreEqual(count, module.Modules.Count());
        }

        [TestCase(0, "add", "Добавить")]
        [TestCase(1, "delete", "Удалить")]
        [TestCase(2, "update", "Изменить")]
        public void Operation_Code_And_Name_Equals_Expected(int index, string code, string name)
        {
            var modulesProvider = new ModulesProvider(new MockModulesConfiguration());
            var operation = modulesProvider.GetRoot().ToList()[0].Modules.ToList()[0].Operations.ToList()[index];
            Assert.AreEqual(code, operation.Code);
            Assert.AreEqual(name, operation.Name);
        }
    }
}
