using System;
using System.Linq.Expressions;
using NUnit.Framework;

namespace LingSubPlayer.Common.Tests
{
    [TestFixture]
    public class ExpressionHelperTests
    {
        public class TestClass
        {
            public string TestProperty { get; set; }

            public string TestMethod()
            {
                return null;
            }
        }

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
    }
}
