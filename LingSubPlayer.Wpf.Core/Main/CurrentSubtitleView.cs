using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using LingSubPlayer.Common.Subtitles.Data;
using LingSubPlayer.Wpf.Core.Annotations;

namespace LingSubPlayer.Wpf.Core.Main
{
    public class CurrentSubtitleView : DependencyObject, INotifyPropertyChanged
    {
        public static DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof (TimeSpan),
            typeof (CurrentSubtitleView), new PropertyMetadata(PositionChanged));

        public static readonly DependencyPropertyKey TextProperty = DependencyProperty.RegisterReadOnly(
            "Text", typeof (FormattedText), typeof (CurrentSubtitleView), new PropertyMetadata());

        public static readonly DependencyPropertyKey TranslatedTextProperty = DependencyProperty.RegisterReadOnly(
            "TranslatedText", typeof (FormattedText), typeof (CurrentSubtitleView), new PropertyMetadata());

        private readonly VideoSubtitleCollection collection;
        private readonly VideoSubtitleCollection translatedCollection;

        public CurrentSubtitleView(VideoSubtitleCollection collection, VideoSubtitleCollection translatedCollection = null)
        {
            this.collection = collection;
            this.translatedCollection = translatedCollection;
        }

        private void UpdateCurrentTextIfRequired(TimeSpan value)
        {
            if (collection != null)
            {
                SetValue(TextProperty, collection.GetTextFromPosition(value));
            }
            
            if (translatedCollection != null)
            {
                SetValue(TranslatedTextProperty, translatedCollection.GetTextFromPosition(value));
            }
        }

        public TimeSpan Position
        {
            get { return (TimeSpan)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public FormattedText Text
        {
            get { return (FormattedText) GetValue(TextProperty.DependencyProperty); }
        }

        public FormattedText TranslatedText
        {
            get { return (FormattedText)GetValue(TranslatedTextProperty.DependencyProperty); }
        }

        private static void PositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CurrentSubtitleView;

            if (instance != null)
            {
                instance.UpdateCurrentTextIfRequired((TimeSpan) e.NewValue);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}