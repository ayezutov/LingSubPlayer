using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using LingSubPlayer.Common.Subtitles.Data;
using LingSubPlayer.Wpf.Core.ViewModel;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;

namespace LingSubPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Closing += WindowClosing;
            Loaded += WindowLoaded;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            VlcControl.Media = new PathMedia(@"..\..\..\data\cartoon.flv");
            VlcControl.Stop();
            //            VlcControl.Play();


            var subtitleView = new CurrentSubtitleView(new VideoSubtitleCollection(new[]
            {
                new VideoSubtitlesRecord
                {
                    StartTime = TimeSpan.Parse("00:00:01.000"),
                    EndTime = TimeSpan.Parse("00:00:04.000"),
                    Value = new FormattedText("Text from first to fourth second")
                },
                new VideoSubtitlesRecord
                {
                    StartTime = TimeSpan.Parse("00:00:06.001"),
                    EndTime = TimeSpan.Parse("00:00:10.000"),
                    Value = new FormattedText("Text from sixth to tenth second")
                }
            }),
            new VideoSubtitleCollection(new[]
            {
                new VideoSubtitlesRecord
                {
                    StartTime = TimeSpan.Parse("00:00:01.000"),
                    EndTime = TimeSpan.Parse("00:00:04.000"),
                    Value = new FormattedText("Текст с первой по четвёртую секунду")
                },
                new VideoSubtitlesRecord
                {
                    StartTime = TimeSpan.Parse("00:00:06.001"),
                    EndTime = TimeSpan.Parse("00:00:10.000"),
                    Value = new FormattedText("Текст с шестой по десятую секунду")
                }
            }));

            this.DataContext = subtitleView;

            var binding = new Binding {Source = VlcControl, Path = new PropertyPath("Time"), Mode = BindingMode.OneWay};
            BindingOperations.SetBinding(subtitleView, CurrentSubtitleView.PositionProperty, binding);
            abc.Show();
            SomeChildWindow.Show();
            SomeOtherChildWindow.Show();
            //def.Visibility = Visibility.Visible;
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            VlcContext.CloseAll();
        }

        private void TogglePlayPauseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (VlcControl.IsPaused)
            {
                VlcControl.Play();
            }

            if (VlcControl.IsPlaying)
            {
                VlcControl.Pause();
            }
        }

        private void ToSubtitleNextBlockBeginningExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ToSubtitlePreviousBlockBeginningExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void TextBlockMouseDown(object sender, MouseButtonEventArgs e)
        {
            var run = e.OriginalSource as Run;
            if (run != null)
            {
                MessageBox.Show(run.Text);
            }
        }
    }
}
