using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Autofac;
using LingSubPlayer.Common.Data;
using LingSubPlayer.Wpf.Core.Controllers;
using LingSubPlayer.Wpf.Core.Controls;
using LingSubPlayer.Wpf.Core.ViewModel;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;

namespace LingSubPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView<MainController>
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

        public async Task<SessionData> ShowOpenVideoFileDialog()
        {
            var controller = container.Resolve<OpenFileDialogController>();
            var openFileDialog = controller.View as OpenFileDialog;
            openFileDialog.SessionData = new SessionData();
            openFileDialog.CanCancel = false;
            await ModalWindowContainer.ShowControlAsWindow(openFileDialog, new WindowParameters
            {
                WindowStyle = WindowStyle.None,
                Style = ChildWindowStyle,
                Width = 500
            });
            return openFileDialog.SessionData;
        }

        public void PlayFile(string videoFileName, CurrentSubtitleView subtitles)
        {
            DataContext = subtitles;

            var binding = new Binding { Source = VlcControl, Path = new PropertyPath("Time"), Mode = BindingMode.OneWay };
            BindingOperations.SetBinding(subtitles, CurrentSubtitleView.PositionProperty, binding);

            VlcControl.Media = new PathMedia(videoFileName);
            VlcControl.Play();
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
