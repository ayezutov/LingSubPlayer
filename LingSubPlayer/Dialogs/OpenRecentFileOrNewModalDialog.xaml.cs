using System.Windows.Input;
using LingSubPlayer.Common.Data;
using LingSubPlayer.Wpf.Core.Controllers;
using LingSubPlayer.Wpf.Core.Controls;
using LingSubPlayer.Wpf.Core.ViewModel;

namespace LingSubPlayer.Dialogs
{
    /// <summary>
    /// Interaction logic for OpenRecentFileOrNewModalDialog.xaml
    /// </summary>
    public partial class OpenRecentFileOrNewModalDialog : ExtendedUserControl, IOpenNewOrRecentView, IModalDialog
    {
        public OpenNewOrRecentController Controller { get; set; }

        public OpenRecentFileOrNewModalDialog()
        {
            InitializeComponent();
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter == null)
            {
                Controller.Open();
            }
            else
            {
                Controller.Open(e.Parameter as SessionData);
            }
        }

        protected override void OnShown(ExtendedUserControl sender)
        {
            Controller.OnStart();
        }

        public RecentFilesView ViewModel
        {
            get { return DataContext as RecentFilesView; }
            set { DataContext = value; }
        }
    }
}
