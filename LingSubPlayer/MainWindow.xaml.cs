using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac;
using LingSubPlayer.Common.Data;
using LingSubPlayer.Dialogs;
using LingSubPlayer.Wpf.Core.Controllers;
using LingSubPlayer.Wpf.Core.Controls;
using LingSubPlayer.Wpf.Core.MVC.Main;
using LingSubPlayer.Wpf.Core.ViewModel;
using Vlc.DotNet.Core.Medias;
using Binding = System.Windows.Data.Binding;
using Control = System.Windows.Controls.Control;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace LingSubPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        private readonly ILifetimeScope container;

        // Using property injection, as circular references are 
        // not supported via constructor injection by Autofac
        public MainController Controller { get; set; }

        public MainWindow(ILifetimeScope container)
        {
            this.container = container;
            InitializeComponent();
            
            Closed += WindowClosed;
            Loaded += WindowLoaded;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            Controller.OnBeforeExit();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Controller.OnLoad();
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

        public void PlayFile(string videoFileName, CurrentSubtitleView subtitles)
        {
            DataContext = subtitles;

            var binding = new Binding { Source = VlcControl, Path = new PropertyPath("Time"), Mode = BindingMode.OneWay };
            BindingOperations.SetBinding(subtitles, CurrentSubtitleView.PositionProperty, binding);

            VlcControl.Media = new PathMedia(videoFileName);
            VlcControl.Play();
        }

        public async Task ShowDialog<TController>(IView<TController> control)
        {
            await ModalWindowContainer.ShowControlAsWindow(control as Control, new WindowParameters
            {
                WindowStyle = WindowStyle.None,
                Style = ChildWindowStyle
            });
        }

        public void ShowUpdatesAvailable(AvailableUpdatesInformation availableUpdatesInformation)
        {
            Dispatcher.Invoke(() =>
            {
                UpdatesAvailableDialog.Visibility = Visibility.Visible;
                UpdatesAvailableDialog.DataContext = availableUpdatesInformation;
            });
        }

        private Style ChildWindowStyle
        {
            get { return FindResource("ChildWindowStyle") as Style; }
        }

        private void ToSubtitleNextBlockBeginningExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ToSubtitlePreviousBlockBeginningExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
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
