using System;
using System.Globalization;
using LingSubPlayer.Wpf.Core.Converters;
using NUnit.Framework;

namespace LingSubPlayer.Wpf.Core.Tests.Converters
{
    [TestFixture]
    public class TimeSpanToDoubleConverterTests
    {
        [Test]
        public void ConvertFromTimeSpan()
        {
            var timeSpan = TimeSpan.FromMilliseconds(2435);

            var result = new TimeSpanToDoubleConverter().Convert(timeSpan, typeof(double), null, CultureInfo.CurrentCulture);

            Assert.That(result, Is.EqualTo(2435).Within(0.000001));
        }

        [Test]
        public void ConvertFromTimeSpanIntoFractionalValue()
        {
            var timeSpan = TimeSpan.FromTicks(2435);

            var result = new TimeSpanToDoubleConverter().Convert(timeSpan, typeof(double), null, CultureInfo.CurrentCulture);

            Assert.That(result, Is.EqualTo(0.2435).Within(0.000001));
        }

        [Test]
        public void ConvertBackValueIntoTimeSpan()
        {
            var value = 2435.00;

            var result = new TimeSpanToDoubleConverter().ConvertBack(value, typeof(double), null, CultureInfo.CurrentCulture);

            Assert.That(result, Is.EqualTo(TimeSpan.FromMilliseconds(2435)));
        }
    }
}
