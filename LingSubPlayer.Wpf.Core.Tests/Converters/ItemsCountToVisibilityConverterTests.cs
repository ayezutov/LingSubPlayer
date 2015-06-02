using System.Collections.Generic;
using System.Windows;
using LingSubPlayer.Wpf.Core.Converters;
using NUnit.Framework;
namespace LingSubPlayer.Wpf.Core.Tests.Converters
{
    [TestFixture]
    public class ItemsCountToVisibilityConverterTests
    {
        [TestCaseSource("Data")]
        public void Convert(object list, Visibility expected)
        {
            var result = new ItemsCountToVisibilityConverter().Convert(list, typeof (Visibility), null, null);

            Assert.That(result, Is.EqualTo(expected));
        }

        public IEnumerable<TestCaseData> Data
        {
            get
            {
                return new TestCaseData[]
                {
                    new TestCaseData(null, Visibility.Collapsed), 
                    new TestCaseData(new object[0], Visibility.Collapsed), 
                    new TestCaseData(new object[]{"x"}, Visibility.Visible), 
                    new TestCaseData(new List<string>(), Visibility.Collapsed), 
                    new TestCaseData(new List<string>(){"X"}, Visibility.Visible), 
                };
            }
        }
    }
}