using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LingSubPlayer.Common.Data;
using LingSubPlayer.Infrastructure;
using LingSubPlayer.Presentation.OpenFileDialog;
using LingSubPlayer.Wpf.Core.Controls;
using LingSubPlayer.Wpf.Core.MVC;
using LingSubPlayer.Wpf.Core.Validators;

namespace LingSubPlayer
{
    /// <summary>
    /// Interaction logic for OpenFileDialog.xaml
    /// </summary>
    public partial class OpenFileDialog : IOpenFileDialogView, IModalDialog
    {
        public static readonly DependencyProperty SessionDataProperty = DependencyProperty.Register(
            "SessionData", typeof (SessionData), typeof (OpenFileDialog), new PropertyMetadata(default(SessionData)));

        public OpenFileDialogController Controller { get; set; }

        public SessionData SessionData
        {
            get { return (SessionData) GetValue(SessionDataProperty); }
            set { SetValue(SessionDataProperty, value); }
        }

        public OpenFileDialog()
        {
            InitializeComponent();
        }

        protected override void OnShown(ExtendedUserControl sender)
        {
            Controller.Start();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Controller.CancelTriggered();
        }

        private void OpenClicked(object sender, RoutedEventArgs e)
        {
            if (!Validator.IsValid(VisualTreeHelper.GetParent(this)))
            {
                e.Handled = true;
                return;
            }

            Controller.OpenTriggered();
        }
    }
}
