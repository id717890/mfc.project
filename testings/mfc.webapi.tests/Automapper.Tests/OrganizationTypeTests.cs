﻿using mfc.domain.entities;
using NUnit.Framework;
using AutoMapper;
using mfc.webapi.Models;
using System;

namespace mfc.webapi.tests.Automapper.Tests
{
    public class OrganizationTypeTests : AutomapperBaseTests<OrganizationType, OrganizationTypeInfo>
    {


        [TestCase(1, "Caption")]
        [TestCase(2, "Caption")]
        [TestCase(3, "Caption_1")]
        [TestCase(4, "Caption_4")]
        public void OrganizationTypeToModel(long id, string caption)
        {
            var entity = new OrganizationType();
            entity.Id = id;
            entity.Caption = caption;
            entity.IsDeleted = false;
            AssertEntity(entity);
        }


        [TestCase(1, "Caption")]
        [TestCase(2, "Caption")]
        [TestCase(3, "Caption_1")]
        [TestCase(4, "Caption_4")]
        public void ModelToOrganizationType(long id, string caption)
        {
            var model = new OrganizationTypeInfo();
            model.Id = id;
            model.Caption = caption;
            AssertModel(model);
        }

        protected override void AssertEntityToModel(OrganizationType entity, OrganizationTypeInfo model)
        {
            Assert.AreEqual(entity.Id, model.Id);
            Assert.AreEqual(entity.Caption, model.Caption);
        }

        protected override void AssertModelToEntity(OrganizationTypeInfo entity, OrganizationType model)
        {
            Assert.AreEqual(model.Id, entity.Id);
            Assert.AreEqual(model.Caption, entity.Caption);
        }
    }
}
