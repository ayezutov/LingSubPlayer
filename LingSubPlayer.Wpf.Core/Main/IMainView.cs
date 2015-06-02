using System.Threading.Tasks;
using LingSubPlayer.Wpf.Core.Controllers;
using LingSubPlayer.Wpf.Core.ViewModel;

namespace LingSubPlayer.Wpf.Core.MVC.Main
{
    public interface IMainView : IView<MainController>, IUpdatesAvailableView
    {
        void PlayFile(string videoFileName, CurrentSubtitleView subtitles);
        
        Task ShowDialog<TController>(IView<TController> control);
    }
}