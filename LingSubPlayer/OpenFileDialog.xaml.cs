using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LingSubPlayer.Common.Data;
using LingSubPlayer.Wpf.Core.Controllers;
using LingSubPlayer.Wpf.Core.Controls;
using LingSubPlayer.Wpf.Core.Tests.Validators;

namespace LingSubPlayer
{
    /// <summary>
    /// Interaction logic for OpenFileDialog.xaml
    /// </summary>
    public partial class OpenFileDialog : UserControl, IOpenFileDialogView
    {
        public static readonly DependencyProperty SessionDataProperty = DependencyProperty.Register(
            "SessionData", typeof (SessionData), typeof (OpenFileDialog), new PropertyMetadata(default(SessionData)));

        public static readonly DependencyProperty CanCancelProperty = DependencyProperty.Register(
            "CanCancel", typeof (bool), typeof (OpenFileDialog), new PropertyMetadata(true));

        public bool CanCancel
        {
            get { return (bool) GetValue(CanCancelProperty); }
            set { SetValue(CanCancelProperty, value); }
        }

        public SessionData SessionData
        {
            get { return (SessionData) GetValue(SessionDataProperty); }
            set { SetValue(SessionDataProperty, value); }
        }

        public OpenFileDialog()
        {
            InitializeComponent();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            WindowContainer.CloseCurrentWindow(this, false);
        }

        public OpenFileDialogController Controller { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Validator.IsValid(VisualTreeHelper.GetParent(this)))
            {
                e.Handled = true;
                return;
            }

            WindowContainer.CloseCurrentWindow(this);
        }
    }
}
