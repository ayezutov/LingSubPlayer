using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using LingSubPlayer.Properties;
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
            vlcControl.Media = new PathMedia(@"..\..\..\data\cartoon.flv");
            vlcControl.Play();
        }

        

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            VlcContext.CloseAll();
        }

        private void TogglePlayPauseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (vlcControl.IsPaused)
            {
                vlcControl.Play();
            }

            if (vlcControl.IsPlaying)
            {
                vlcControl.Pause();
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
    }
}
