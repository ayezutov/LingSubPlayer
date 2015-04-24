using System;
using System.Threading;
using LingSubPlayer.Wpf.Core.Converters;
using NUnit.Framework;

namespace LingSubPlayer.Wpf.Core.Tests.Converters
{
    [TestFixture]
    public class PercentageToAngleConverterTests
    {
        private PercentageToAngleConverter converter;

        [SetUp]
        public void Init()
        {
            converter = new PercentageToAngleConverter();
        }

        [TestCase(null, 0)]
        [TestCase(0, 0)]
        [TestCase(50, 180)]
        [TestCase(100, 360)]
        [TestCase(150, 540)]
        public void ConvertValidValue(object percentage, double expectedAngle)
        {
            var result = (double)converter.Convert(percentage, typeof(double), null, Thread.CurrentThread.CurrentCulture);

            Assert.That(result, Is.EqualTo(expectedAngle).Within(0.01));
        }
    }
}