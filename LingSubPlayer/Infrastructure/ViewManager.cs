using System;
using System.Threading.Tasks;
using System.Windows;
using LingSubPlayer.Wpf.Core.Controllers;
using LingSubPlayer.Wpf.Core.Controls;
using LingSubPlayer.Wpf.Core.MVC;
using LingSubPlayer.Wpf.Core.MVC.Main;

namespace LingSubPlayer.Infrastructure
{
    public class ViewManager: IViewManager
    {
        private readonly IMainView mainView;

        public ViewManager(IMainView main)
        {
            this.mainView = main;
        }

        public async Task Show<TController>(IView<TController> view)
        {
            var dialog = view as IModalDialog;

            if (dialog != null)
            {
                await mainView.ShowDialog(view);
                return;
            }

            throw new NotSupportedException("This type of view is not supported by ViewManager");
        }

        public void Hide<TController>(IView<TController> view)
        {
            WindowContainer.CloseCurrentWindow(view as DependencyObject);
        }

        public Task ShowMessage(string message)
        {
            MessageBox.Show(message);

            return Task.Delay(1);
        }
    }
}