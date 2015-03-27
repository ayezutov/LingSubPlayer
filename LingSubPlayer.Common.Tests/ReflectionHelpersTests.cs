using System;
using System.Linq.Expressions;
using NUnit.Framework;

namespace LingSubPlayer.Common.Tests
{
    public class TestBaseClass : TestBaseBaseClass<int>
    {
    }

    public class TestBaseBaseClass<T> : TestBaseBaseBaseClass<T>
    {
    }    
    
    public class TestBaseBaseBaseClass<T>
    {
    }

    public interface ITestInterface<T> : ITestBaseInterface<T>
    {
    }
    
    public interface ITestBaseInterface<T>
    {
    }

    public class TestClass : TestBaseClass, ITestInterface<int>
    {
        public string TestProperty { get; set; }

        public int TestIntProperty { get; set; }

        public string TestMethod()
        {
            return null;
        }
    }

    [TestFixture]
    public class ReflectionHelpersTests
    {
        [Test]
        public void CanGetPropertyName()
        {
            Expression<Func<TestClass, string>> expr = tc => tc.TestProperty;

            var propertyName = expr.GetPropertyOrMethodName();

            Assert.That(propertyName, Is.EqualTo("TestProperty"));
        }

        [Test]
        public void CanGetMethodName()
        {
            Expression<Func<TestClass, string>> expr = tc => tc.TestMethod();

            var propertyName = expr.GetPropertyOrMethodName();

            Assert.That(propertyName, Is.EqualTo("TestMethod"));
        }

        [Test]
        public void ThrowsExceptionForComplexExpression()
        {
            Expression<Func<TestClass, string>> expr = tc => tc.TestMethod() + tc.TestProperty;

            Assert.Throws<NotSupportedException>(() => { expr.GetPropertyOrMethodName(); });
        }

        [Test]
        public void ConvertStatementsAreUnwrapped()
        {
            Expression<Func<TestClass, object>> expr = tc => tc.TestIntProperty;

            var propertyName = expr.GetPropertyOrMethodName();

            Assert.That(propertyName, Is.EqualTo("TestIntProperty"));
        }


        [TestCase(typeof(int), typeof(object), true)]
        [TestCase(typeof(TestClass), typeof(TestBaseClass), true)]
        
        [TestCase(typeof(TestBaseClass), typeof(TestClass), false)]

        [TestCase(typeof(TestClass), typeof(TestBaseBaseClass<double>), false)]
        [TestCase(typeof(TestClass), typeof(TestBaseBaseClass<int>), true)]
        [TestCase(typeof(TestClass), typeof(TestBaseBaseClass<>), true)]
        
        
        [TestCase(typeof(TestClass), typeof(ITestInterface<double>), false)]
        [TestCase(typeof(TestClass), typeof(ITestInterface<int>), true)]
        [TestCase(typeof(TestClass), typeof(ITestInterface<>), true)]
        
        [TestCase(typeof(TestClass), typeof(ITestBaseInterface<double>), false)]
        [TestCase(typeof(TestClass), typeof(ITestBaseInterface<int>), true)]
        [TestCase(typeof(TestClass), typeof(ITestBaseInterface<>), true)]

        [TestCase(typeof(TestBaseClass), typeof(ITestInterface<int>), false)]
        [TestCase(typeof(TestBaseClass), typeof(ITestInterface<>), false)]
        
        [TestCase(typeof(ITestInterface<int>), typeof(ITestBaseInterface<double>), false)]
        [TestCase(typeof(ITestInterface<int>), typeof(ITestBaseInterface<int>), true)]
        [TestCase(typeof(ITestInterface<int>), typeof(ITestBaseInterface<>), true)]
        [TestCase(typeof(ITestInterface<>), typeof(ITestBaseInterface<int>), false)]
        [TestCase(typeof(ITestInterface<>), typeof(ITestBaseInterface<>), true)]

        [TestCase(typeof(TestBaseBaseClass<int>), typeof(TestBaseBaseBaseClass<double>), false)]
        [TestCase(typeof(TestBaseBaseClass<int>), typeof(TestBaseBaseBaseClass<int>), true)]
        [TestCase(typeof(TestBaseBaseClass<int>), typeof(TestBaseBaseBaseClass<>), true)]
        [TestCase(typeof(TestBaseBaseClass<>), typeof(TestBaseBaseBaseClass<int>), false)]
        [TestCase(typeof(TestBaseBaseClass<>), typeof(TestBaseBaseBaseClass<>), true)]

        [TestCase(typeof(TestClass), typeof(TestClass), true)]
        [TestCase(typeof(TestBaseBaseClass<>), typeof(TestBaseBaseClass<>), true)]
        [TestCase(typeof(TestBaseBaseClass<int>), typeof(TestBaseBaseClass<int>), true)]
        [TestCase(typeof(ITestInterface<>), typeof(ITestInterface<>), true)]
        [TestCase(typeof(ITestInterface<int>), typeof(ITestInterface<int>), true)]
        
        public void IsAssignableToTests(Type initial, Type type, bool expected)
        {
            var result = initial.IsAssignableTo(type);

            Assert.That(result, Is.EqualTo(expected), string.Format("{0} is assignable to {1}", initial.FullName, type.FullName));
        }
    }
}
