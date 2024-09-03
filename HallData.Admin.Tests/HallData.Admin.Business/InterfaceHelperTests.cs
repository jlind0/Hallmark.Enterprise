using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HallData.Admin.Business;
using System.Collections.Generic;
using HallData.Admin.ApplicationViews;
using System.Linq;

namespace HallData.Admin.Tests.HallData.Admin.Business
{

    [TestClass]
    public class InterfaceHelperTests
    {
        

        [TestMethod]
        public void GetComposedAttributes_ZeroAttribute_EmptyResult()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            var results = attributes.GetComposedAttributes();

            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void GetComposedAttributes_OneAttribute_EmptyResult()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult statusTypeInterface = new InterfaceResult() { InterfaceId = 1 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                DisplayName = "Status Type Name",
                IsCollection = false,
                IsKey = false,
                Name = "Name",
                InterfaceAttributeId = 1,
                Type = statusTypeInterface
            };

            attributes.Add(iar);

            var results = attributes.GetComposedAttributes();

            Assert.AreEqual(0, results.Count());
        }


        [TestMethod]
        public void GetComposedAttributes_DiffAttributeNamesSameType_EmptyResult()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult statusTypeInterface = new InterfaceResult() { InterfaceId = 1 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                DisplayName = "Status Type Name",
                IsCollection = false,
                IsKey = false,
                Name = "Name",
                InterfaceAttributeId = 1,
                Type = statusTypeInterface
            };

            attributes.Add(iar);

            InterfaceAttributeResult iar2 = new InterfaceAttributeResult()
            {
                DisplayName = "Status Type Id",
                IsCollection = false,
                IsKey = true,
                Name = "StatusTypeId",
                InterfaceAttributeId = 2,
                Type = statusTypeInterface
            };

            attributes.Add(iar2);

            var results = attributes.GetComposedAttributes();

            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void GetComposedAttributes_DiffAttributesDiffType_ReturnsBothNameAttributes()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult statusTypeInterface = new InterfaceResult() { InterfaceId = 1 };

            InterfaceResult deliveryMethodTypeInterface = new InterfaceResult() { InterfaceId = 2 };

            InterfaceAttributeResult statusTypeAttr1 = new InterfaceAttributeResult()
            {
                DisplayName = "Status Type Name",
                IsCollection = false,
                IsKey = false,
                Name = "Name",
                InterfaceAttributeId = 1,
                Type = statusTypeInterface
            };

            InterfaceAttributeResult statusTypeAttr2 = new InterfaceAttributeResult()
            {
                DisplayName = "Status Type Id",
                IsCollection = false,
                IsKey = true,
                Name = "StatusTypeId",
                InterfaceAttributeId = 2,
                Type = statusTypeInterface
            };

            attributes.Add(statusTypeAttr1);

            attributes.Add(statusTypeAttr2);

            InterfaceAttributeResult deliveryMethodTypeAttr1 = new InterfaceAttributeResult()
            {
                DisplayName = "Delivery Method Type Name",
                IsCollection = false,
                IsKey = false,
                Name = "Name",
                InterfaceAttributeId = 3,
                Type = deliveryMethodTypeInterface
            };

            InterfaceAttributeResult deliveryMethodTypeAttr2 = new InterfaceAttributeResult()
            {
                DisplayName = "Delivery Method Id",
                IsCollection = false,
                IsKey = true,
                Name = "DeliveryMethodId",
                InterfaceAttributeId = 4,
                Type = deliveryMethodTypeInterface
            };

            attributes.Add(deliveryMethodTypeAttr1);
            attributes.Add(deliveryMethodTypeAttr2);

            var results = attributes.GetComposedAttributes();

            Assert.AreEqual(2, results.Count());
        }

        [TestMethod]
        public void GetComposedAttributes_DiffAttributesSameNameSameType_ReturnsBoth()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult statusTypeInterface = new InterfaceResult() { InterfaceId = 1 };

            InterfaceAttributeResult statusTypeAttr1 = new InterfaceAttributeResult()
            {
                DisplayName = "Status Type Name",
                IsCollection = false,
                IsKey = false,
                Name = "Name",
                InterfaceAttributeId = 1,
                Type = statusTypeInterface
            };

            InterfaceAttributeResult statusTypeAttr2 = new InterfaceAttributeResult()
            {
                DisplayName = "Status Type Id",
                IsCollection = false,
                IsKey = true,
                Name = "Name",
                InterfaceAttributeId = 2,
                Type = statusTypeInterface
            };

            attributes.Add(statusTypeAttr1);

            attributes.Add(statusTypeAttr2);
            var results = attributes.GetComposedAttributes();

            Assert.AreEqual(2, results.Count());
        }

        [TestMethod]
        public void GetComposedAttributes_SameAttributesSameName_EmptyResult()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult statusTypeInterface = new InterfaceResult() { InterfaceId = 1 };

            InterfaceAttributeResult statusTypeAttr1 = new InterfaceAttributeResult()
            {
                DisplayName = "Status Type Name",
                IsCollection = false,
                IsKey = false,
                Name = "Name",
                InterfaceAttributeId = 1,
                Type = statusTypeInterface
            };

            InterfaceAttributeResult statusTypeAttr2 = new InterfaceAttributeResult()
            {
                DisplayName = "Status Type Name",
                IsCollection = false,
                IsKey = false,
                Name = "Name",
                InterfaceAttributeId = 1,
                Type = statusTypeInterface
            };

            attributes.Add(statusTypeAttr1);

            attributes.Add(statusTypeAttr2);
            var results = attributes.GetComposedAttributes();

            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void GetMismatchedComposedAttributes_SingleAttribute_ReturnEmpty()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult type = new InterfaceResult() { InterfaceId = 1 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type
            };

            attributes.Add(iar);

            var results = attributes.GetMismatchedComposedAttributes();

            Assert.AreEqual(0, results.Count());

        }

        [TestMethod]
        public void GetMismatchedComposedAttributes_SameAttribute_ReturnEmpty()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult type = new InterfaceResult() { InterfaceId = 1 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type
            };

            attributes.Add(iar);

            InterfaceAttributeResult iar2 = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type
            };

            attributes.Add(iar2);

            var results = attributes.GetMismatchedComposedAttributes();

            Assert.AreEqual(0, results.Count());

        }

        [TestMethod]
        public void GetMismatchedComposedAttributes_DiffType_ReturnBothAttributes()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult type1 = new InterfaceResult() { InterfaceId = 1 };

            InterfaceResult type2 = new InterfaceResult() { InterfaceId = 2 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type1
            };

            attributes.Add(iar);

            InterfaceAttributeResult iar2 = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type2
            };

            attributes.Add(iar2);

            var results = attributes.GetMismatchedComposedAttributes();

            Assert.AreEqual(2, results.Count());

        }

        [TestMethod]
        public void GetMismatchedComposedAttributes_DiffIsKey_ReturnEmpty()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult type1 = new InterfaceResult() { InterfaceId = 1 };

            InterfaceResult type2 = new InterfaceResult() { InterfaceId = 2 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type1
            };

            attributes.Add(iar);

            InterfaceAttributeResult iar2 = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = true,
                IsCollection = false,
                Type = type1
            };

            attributes.Add(iar2);

            var results = attributes.GetMismatchedComposedAttributes();

            Assert.AreEqual(0, results.Count());

        }

        [TestMethod]
        public void GetMismatchedComposedAttributes_DiffIsCollection_ReturnEmpty()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult type1 = new InterfaceResult() { InterfaceId = 1 };

            InterfaceResult type2 = new InterfaceResult() { InterfaceId = 2 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type1
            };

            attributes.Add(iar);

            InterfaceAttributeResult iar2 = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = true,
                Type = type1
            };

            attributes.Add(iar2);

            var results = attributes.GetMismatchedComposedAttributes();

            Assert.AreEqual(0, results.Count());

        }

        [TestMethod]
        public void GetTypeMismatchedComposedAttributes_SingleAttribute_ReturnEmpty()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult type = new InterfaceResult() { InterfaceId = 1 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type
            };

            attributes.Add(iar);



            var results = attributes.GetTypeMismatchedComposedAttributes();

            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void GetTypeMismatchedComposedAttributes_SameAttribute_ReturnEmpty()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult type = new InterfaceResult() { InterfaceId = 1 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type
            };

            attributes.Add(iar);

            InterfaceAttributeResult iar2 = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type
            };

            attributes.Add(iar2);

            var results = attributes.GetTypeMismatchedComposedAttributes();

            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void GetTypeMismatchedComposedAttributes_DiffType_ReturnEmpty()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult type1 = new InterfaceResult() { InterfaceId = 1 };

            InterfaceResult type2 = new InterfaceResult() { InterfaceId = 2 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type1
            };

            attributes.Add(iar);

            InterfaceAttributeResult iar2 = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type2
            };

            attributes.Add(iar2);

            var results = attributes.GetTypeMismatchedComposedAttributes();

            Assert.AreEqual(0, results.Count());

        }

        [TestMethod]
        public void GetTypeMismatchedComposedAttributes_DiffIsKey_ReturnBothAttributes()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult type1 = new InterfaceResult() { InterfaceId = 1 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type1
            };

            attributes.Add(iar);

            InterfaceAttributeResult iar2 = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = true,
                IsCollection = false,
                Type = type1
            };

            attributes.Add(iar2);

            var results = attributes.GetTypeMismatchedComposedAttributes();

            Assert.AreEqual(2, results.Count());

        }

        [TestMethod]
        public void GetTypeMismatchedComposedAttributes_DiffIsCollection_ReturnBothAttributes()
        {
            List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();

            InterfaceResult type1 = new InterfaceResult() { InterfaceId = 1 };

            InterfaceAttributeResult iar = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = false,
                Type = type1
            };

            attributes.Add(iar);

            InterfaceAttributeResult iar2 = new InterfaceAttributeResult()
            {
                Name = "Name",
                IsKey = false,
                IsCollection = true,
                Type = type1
            };

            attributes.Add(iar2);

            var results = attributes.GetTypeMismatchedComposedAttributes();

            Assert.AreEqual(2, results.Count());

        }

    }
}
