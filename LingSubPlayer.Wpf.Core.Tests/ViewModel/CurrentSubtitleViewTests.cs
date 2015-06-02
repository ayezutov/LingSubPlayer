using System;
using System.Windows;
using System.Windows.Data;
using LingSubPlayer.Common.Subtitles.Data;
using LingSubPlayer.Wpf.Core.Main;
using LingSubPlayer.Wpf.Core.Tests.Fixtures;
using NUnit.Framework;

namespace LingSubPlayer.Wpf.Core.Tests.ViewModel
{
    [TestFixture]
    public class CurrentSubtitleViewTests
    {
        private class SUT
        {
            public CurrentSubtitleView View { get; set; }
            public SampleDependencyPropertyEnabledClass Target { get; set; }
        }

        [Test]
        public void VerifyThatValueIsChangedWhenRequestingPosition()
        {
            var sut = GetSystemUnderTest(CurrentSubtitleView.TextProperty);

            sut.View.Position = TimeSpan.FromSeconds(1);

            Assert.That(sut.Target.Text.ToString(), Is.EqualTo("A"));
        }

        [Test]
        public void VerifyThatValueIsChangedWhenRequestingPositionFromDifferentTimeSlots()
        {
            var sut = GetSystemUnderTest(CurrentSubtitleView.TextProperty);

            sut.View.Position = TimeSpan.FromSeconds(1);
            sut.View.Position = TimeSpan.FromSeconds(3.5);

            Assert.That(sut.Target.Text.ToString(), Is.EqualTo("B"));
        }

        [Test]
        public void VerifyThatValueIsNullForNoSlot()
        {
            var sut = GetSystemUnderTest(CurrentSubtitleView.TextProperty);

            sut.View.Position = TimeSpan.FromSeconds(2.5);

            Assert.That(sut.Target.Text, Is.Null);
        }

        [Test]
        public void VerifyThatForTheSameTimeSlotTextIsChangedOnlyOnce()
        {
            var sut = GetSystemUnderTest(CurrentSubtitleView.TextProperty);

            sut.View.Position = TimeSpan.FromSeconds(1);
            sut.View.Position = TimeSpan.FromSeconds(1.2);
            sut.View.Position = TimeSpan.FromSeconds(1.4);
            sut.View.Position = TimeSpan.FromSeconds(1.6);

            Assert.That(sut.Target.TextChangedCount, Is.EqualTo(1), "Text changed incorrect number of times");
        }

        [Test]
        public void VerifyThatTranslatedValueIsChangedWhenRequestingPosition()
        {
            var sut = GetSystemUnderTest(CurrentSubtitleView.TranslatedTextProperty);

            sut.View.Position = TimeSpan.FromSeconds(1);

            Assert.That(sut.Target.Text.ToString(), Is.EqualTo("Б"));
        }

        [Test]
        public void VerifyThatTranslatedValueIsChangedWhenRequestingPositionFromDifferentTimeSlots()
        {
            var sut = GetSystemUnderTest(CurrentSubtitleView.TranslatedTextProperty);

            sut.View.Position = TimeSpan.FromSeconds(1);
            sut.View.Position = TimeSpan.FromSeconds(3.5);

            Assert.That(sut.Target.Text.ToString(), Is.EqualTo("Ж"));
        }

        [Test]
        public void VerifyThatTranslatedValueIsNullForNoSlot()
        {
            var sut = GetSystemUnderTest(CurrentSubtitleView.TranslatedTextProperty);

            sut.View.Position = TimeSpan.FromSeconds(2.5);

            Assert.That(sut.Target.Text, Is.Null);
        }

        [Test]
        public void VerifyThatForTheSameTimeSlotTranslatedTextIsChangedOnlyOnce()
        {
            var sut = GetSystemUnderTest(CurrentSubtitleView.TranslatedTextProperty);

            sut.View.Position = TimeSpan.FromSeconds(1);
            sut.View.Position = TimeSpan.FromSeconds(1.2);
            sut.View.Position = TimeSpan.FromSeconds(1.4);
            sut.View.Position = TimeSpan.FromSeconds(1.6);

            Assert.That(sut.Target.TextChangedCount, Is.EqualTo(1), "TranslatedText changed incorrect number of times");
        }

        private SUT GetSystemUnderTest(DependencyPropertyKey propertyToBind)
        {
            var sut = new SUT()
            {
                View = new CurrentSubtitleView(GetSubtitles(), GetTranslatedSubtitles())
            };
            var binding = new Binding
            {
                Source = sut.View,
                Path = new PropertyPath(propertyToBind.DependencyProperty),
                Mode = BindingMode.OneWay
            };
            sut.Target = new SampleDependencyPropertyEnabledClass();
            BindingOperations.SetBinding(sut.Target, SampleDependencyPropertyEnabledClass.TextProperty, binding);
            return sut;
        }

        private VideoSubtitleCollection GetSubtitles()
        {
            return new VideoSubtitleCollection(new []
            {
                new VideoSubtitlesRecord
                {
                    StartTime = TimeSpan.Parse("00:00:01.000"),
                    EndTime = TimeSpan.Parse("00:00:02.000"),
                    Value = new FormattedText("A")
                },
                new VideoSubtitlesRecord
                {
                    StartTime = TimeSpan.Parse("00:00:03.001"),
                    EndTime = TimeSpan.Parse("00:00:04.000"),
                    Value = new FormattedText("B")
                },
            });
        }

        private VideoSubtitleCollection GetTranslatedSubtitles()
        {
            return new VideoSubtitleCollection(new []
            {
                new VideoSubtitlesRecord
                {
                    StartTime = TimeSpan.Parse("00:00:01.000"),
                    EndTime = TimeSpan.Parse("00:00:02.000"),
                    Value = new FormattedText("Б")
                },
                new VideoSubtitlesRecord
                {
                    StartTime = TimeSpan.Parse("00:00:03.001"),
                    EndTime = TimeSpan.Parse("00:00:04.000"),
                    Value = new FormattedText("Ж")
                },
            });
        }
    }
}