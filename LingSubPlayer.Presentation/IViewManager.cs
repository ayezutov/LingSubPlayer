using System.Threading.Tasks;
using LingSubPlayer.Wpf.Core.Controllers;

namespace LingSubPlayer.Wpf.Core.MVC
{
    public interface IViewManager
    {
        Task Show<TController>(IView<TController> view);

        void Hide<TController>(IView<TController> view);

        Task ShowMessage(string message);
    }
}