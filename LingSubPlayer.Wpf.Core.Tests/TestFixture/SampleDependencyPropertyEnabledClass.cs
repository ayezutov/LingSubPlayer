using System;
using System.Windows;
using LingSubPlayer.Common.Subtitles.Data;

namespace LingSubPlayer.Wpf.Core.Tests.TestFixture
{
    public class SampleDependencyPropertyEnabledClass: DependencyObject
    {
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof (FormattedText),
            typeof (SampleDependencyPropertyEnabledClass), new PropertyMetadata(TextChangedCallback));

        private static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as SampleDependencyPropertyEnabledClass;

            if (instance != null)
            {
                instance.TextChangedCount++;

                if (instance.TextChanged != null)
                {
                    instance.TextChanged(instance, e);
                }
            }
        }

        public FormattedText Text
        {
            get { return (FormattedText) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public int TextChangedCount { get; private set; }
        public event EventHandler<DependencyPropertyChangedEventArgs> TextChanged;
    }
}