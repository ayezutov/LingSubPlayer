using System;
using System.Windows;
using System.Windows.Controls;
using LingSubPlayer.Common.Logging;
using LingSubPlayer.Wpf.Core.Controllers;
using NLog;

namespace LingSubPlayer.Dialogs
{
    /// <summary>
    /// Interaction logic for UpdatesAvailableDialog.xaml
    /// </summary>
    public partial class UpdatesAvailableDialog : UserControl
    {
        public UpdatesAvailableDialog()
        {
            InitializeComponent();
        }
         
        private void RestartApplication(object sender, RoutedEventArgs e)
        {
            try
            {
                Squirrel.UpdateManager.RestartApp();
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error("Error while restarting application", ex);
                Visibility = Visibility.Collapsed;
            }
        }
    }
}
