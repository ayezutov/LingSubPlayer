using System.Threading.Tasks;

namespace LingSubPlayer.Presentation
{
    public interface IViewManager
    {
        Task Show<TController>(IView<TController> view);

        void Hide<TController>(IView<TController> view);

        Task ShowMessage(string message);
    }
}