using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
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
            VlcControl.Play();

            //Task.Delay(200).ContinueWith((t) => { VlcControl.AudioProperties.IsMute = true; });
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
