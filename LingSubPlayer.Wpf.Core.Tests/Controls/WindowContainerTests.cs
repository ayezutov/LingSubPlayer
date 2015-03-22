using System.Reflection;
using NUnit.Framework;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;

namespace LingSubPlayer.Wpf.Core.Tests.Controls
{
    [TestFixture]
    [RequiresSTA]
    public class WindowContainerTests
    {
        [Test]
        public void EnsureThatWindowControlHasIsModalChangedEvent()
        {
            var e = typeof (ChildWindow).GetEvent("IsModalChanged", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(e, Is.Not.Null);
        }

        [Test]
        public void EnsureThatXceedWindowContainerHasIsModalShownField()
        {
            var f = typeof (WindowContainer).GetField("_isModalBackgroundApplied",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(f, Is.Not.Null);
        }
    }
}