using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HallData.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HallData.Tests.HallData.Utilities
{
    // HELPER CLASSES

    public class TestObjectWithNameAndValue1
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class TestObjectWithValue
    {
        public int Value { get; set; }
    }

    public class TestObjectWithValueAndName2
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public interface ITestInterface
    {
        string Name { get; set; }
    }

    [CustomAttribute("A")]
    [CustomAttribute("B")]
    public class TestInterfaceMultipleAttributes { }

    [CustomAttribute("A")]
    public class TestInterface : ITestInterface
    {
        private string name;

        [CustomAttribute("A")]
        public string Name
        {
            get { return name; }
            set { Name = value; }
        }

        private int age;

        [CustomAttribute("A")]
        [CustomAttribute("B")]
        public int Age
        {
            get { return age; }
            set { Age = value; }
        }

        [CustomAttribute("A")]
        public void DoSomething(string test) { }

        [CustomAttribute("A")]
        [CustomAttribute("B")]
        public void DoSomethingMultipleAttributes(string test) { }

        public void DoSomethingParameterAttribute([PAttribute("A")]string test) { }

        public void DoSomethingParameterAttributes([PAttribute("A")][PAttribute("B")]string test) { }

        public static void StaticMethod() { }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class CustomAttribute : Attribute
    {
        public string letter;

        public CustomAttribute(string letter)
        {
            this.letter = letter;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public class PAttribute : Attribute
    {
        public string letter;

        public PAttribute(string letter)
        {
            this.letter = letter;
        }

    }

    public interface ITestInterface<T> { }

    public interface IResult { }

    public interface ITestInterfaceDerived : ITestInterface { }

    public interface ITestInterfaceDerived<T> : ITestInterface<T> { }

    public class TestWithGenericInterface : ITestInterface<string> { }

    // TESTS

    [TestClass]
    public class ReflectionExtensionsTests
    {
        [TestMethod]
        public void CreateRelatedInstanceT_IsEqual()
        {
            TestObjectWithNameAndValue1 obj1 = new TestObjectWithNameAndValue1() { Name = "Test", Value = 3 };
            TestObjectWithValueAndName2 obj2 = obj1.CreateRelatedInstance<TestObjectWithValueAndName2>();

            Assert.AreEqual(obj2.GetType(), typeof(TestObjectWithValueAndName2), "Instance types should have been equal.");
        }

        [TestMethod]
        public void CreateRelatedInstance_IsEqual_Properties_AreEqual()
        {
            TestObjectWithNameAndValue1 obj1 = new TestObjectWithNameAndValue1() { Name = "Test", Value = 3 };

            dynamic obj2 = obj1.CreateRelatedInstance(typeof(TestObjectWithValueAndName2));

            Assert.AreEqual(typeof(TestObjectWithValueAndName2), obj2.GetType(), "Instance types should have been equal.");
            Assert.AreEqual(obj1.Name, obj2.Name, "Property Name should have been equal");
            Assert.AreEqual(obj1.Value, obj2.Value, "Property Value should have been euqal");
        }

        [TestMethod]
        public void ApplyProperties_TargetProperty_IsEqual()
        {
            TestObjectWithNameAndValue1 obj1 = new TestObjectWithNameAndValue1() { Name = "Test" };
            TestObjectWithValue obj2 = new TestObjectWithValue() { Value = 1 };
            obj1.ApplyProperties(obj2);

            Assert.AreEqual(1, obj1.Value, "Property Value value should have been applied to the object.");
        }

        [TestMethod]
        public void GetCollectionValueTypes_IsEqual()
        {
            Type type = typeof(IList<TestObjectWithValue>);
            Type result = type.GetCollectionValueType();

            Assert.AreEqual(typeof(TestObjectWithValue), result, "Collection Type should have been equal to specified type.");
        }

        [TestMethod]
        public void GetGenericInterface_InterfaceAndTTYpe_IsEqual()
        {
            Type type = typeof(TestWithGenericInterface);
            Type result = type.GetGenericInterface(typeof(ITestInterface<>));
            Assert.AreEqual(typeof(ITestInterface<string>), result, "Implemented interface and/or generic type not equal.");
        }

        [TestMethod]
        public void GetInterfacesCached_InterfacePresent_NotDuplicated()
        {
            Type type = typeof(ITestInterfaceDerived);
            Type[] tFirstRun = type.GetInterfacesCached();
            Type[] tSecondRun = type.GetInterfacesCached();

            Assert.AreEqual(typeof(ITestInterface), tSecondRun[0], "Inherited/Implemented interface is not equal.");
            Assert.IsTrue(tSecondRun.Length == 1, "Duplicate interfaces added for derived interface.");
        }

        [TestMethod]
        public void GetGenericInterfaceCached_InterfaceAndTTYpe_IsEqual()
        {
            Type type = typeof(TestWithGenericInterface);
            Type result = type.GetGenericInterfaceCached(typeof(ITestInterface<>));
            Assert.AreEqual(typeof(ITestInterface<string>), result, "Implemented interface and/or generic type not equal.");
        }

        [TestMethod]
        public void GetPropertiesCached_AreEqual_To_CorrectProperties()
        {
            Type type = typeof(TestInterface);
            PropertyInfo[] properties = type.GetPropertiesCached(BindingFlags.Public | BindingFlags.Instance);

            PropertyInfo property1 = type.GetProperty("Name");
            PropertyInfo property2 = type.GetProperty("Age");

            Assert.IsTrue(properties.Length == 2, "Incorrect number of properties returned");
            Assert.AreEqual(property1, properties[0]);
            Assert.AreEqual(property2, properties[1]);
        }

        [TestMethod]
        public void GetMethodsCached_AreEqual_To_CorrectMethods()
        {
            Type type = typeof(TestInterface);
            MethodInfo[] methods = type.GetMethodsCached(BindingFlags.Public | BindingFlags.Instance);

            MethodInfo method = type.GetMethod("DoSomething");

            Assert.AreEqual(method, methods[4]);
        }

        [TestMethod]
        public void GetParametersCached_AreEqual()
        {
            Type type = typeof(TestInterface);
            MethodInfo method = type.GetMethod("DoSomething");

            ParameterInfo[] parameters = method.GetParametersCached();

            Assert.AreEqual("test", parameters[0].Name);
            Assert.AreEqual(typeof(string), parameters[0].ParameterType);
        }

        // ATTRIBUTES
        // ----------

        // class

        [TestMethod]
        public void GetCustomAttributeCached_Class_IsEqual()
        {
            Type type = typeof(TestInterface);

            CustomAttribute attribute = type.GetCustomAttributeCached<CustomAttribute>();

            Assert.AreEqual("A", attribute.letter);
        }

        [TestMethod]
        public void GetCustomAttributesCached_Class_AreEqual()
        {
            Type type = typeof(TestInterfaceMultipleAttributes);

            IEnumerable<CustomAttribute> attributes = type.GetCustomAttributesCached<CustomAttribute>();
            Assert.IsTrue(attributes.Where(x => x.letter == "A").Count() == 1);
            Assert.IsTrue(attributes.Where(x => x.letter == "B").Count() == 1);
        }

        // parameter

        [TestMethod]
        public void GetCustomAttributeCached_Parameter_IsEqual()
        {
            Type type = typeof(TestInterface);
            MethodInfo method = type.GetMethod("DoSomethingParameterAttribute");
            ParameterInfo[] parameters = method.GetParameters();

            PAttribute attribute = parameters[0].GetCustomAttributeCached<PAttribute>();

            Assert.AreEqual("A", attribute.letter);
        }

        [TestMethod]
        public void GetCustomAttributeCached_Parameters_AreEqual()
        {
            Type type = typeof(TestInterface);
            MethodInfo method = type.GetMethod("DoSomethingParameterAttributes");
            ParameterInfo[] parameters = method.GetParameters();

            IEnumerable<PAttribute> attributes = parameters[0].GetCustomAttributesCached<PAttribute>();

            // cant guarantee the order of attributes being returned
            Assert.IsTrue(attributes.First().letter == "A" || attributes.First().letter == "B");
            Assert.IsTrue(attributes.Last().letter == "A" || attributes.Last().letter == "B");
        }

        // property

        [TestMethod]
        public void GetCustomAttributeCached_Property_IsEqual()
        {
            Type type = typeof(TestInterface);
            PropertyInfo[] properties = type.GetProperties();

            CustomAttribute attribute = properties[0].GetCustomAttributeCached<CustomAttribute>();

            Assert.AreEqual("A", attribute.letter);
        }

        [TestMethod]
        public void GetCustomAttributesCached_Property_AreEqual()
        {
            Type type = typeof(TestInterface);
            PropertyInfo[] properties = type.GetProperties();

            IEnumerable<CustomAttribute> attributes = properties[1].GetCustomAttributesCached<CustomAttribute>();

            // cant guarantee the order of attributes being returned
            Assert.IsTrue(attributes.First().letter == "A" || attributes.First().letter == "B");
            Assert.IsTrue(attributes.Last().letter == "A" || attributes.Last().letter == "B");
        }

        // method

        [TestMethod]
        public void GetCustomAttributeCached_Method_IsEqual()
        {
            Type type = typeof(TestInterface);
            MethodInfo method = type.GetMethod("DoSomething");

            CustomAttribute attribute = method.GetCustomAttributeCached<CustomAttribute>();

            Assert.AreEqual("A", attribute.letter);
        }

        [TestMethod]
        public void GetCustomAttributesCached_Method_AreEqual()
        {
            Type type = typeof(TestInterface);
            MethodInfo method = type.GetMethod("DoSomethingMultipleAttributes");

            IEnumerable<CustomAttribute> attributes = method.GetCustomAttributesCached<CustomAttribute>();

            Assert.AreEqual("B", attributes.First().letter);
            Assert.AreEqual("A", attributes.Last().letter);
        }

        // OTHER
        // -----

        [TestMethod]
        public void GetPropertyCached()
        {
            Type type = typeof(TestInterface);

            PropertyInfo property = type.GetPropertyCached("Name");

            Assert.AreEqual(typeof(string), property.PropertyType);
        }

        [TestMethod]
        public void HasInterface()
        {
            Type type = typeof(TestInterface);

            bool hasInterface = type.HasInterface(typeof(ITestInterface));

            Assert.IsTrue(hasInterface);
        }
    }
}
