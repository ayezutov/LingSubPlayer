using System.Threading.Tasks;
using LingSubPlayer.Presentation;
using LingSubPlayer.Presentation.ApplicationUpdate;

namespace LingSubPlayer.Wpf.Core.Main
{
    public interface IMainView : IView<MainController>, IUpdatesAvailableView
    {
        void PlayFile(string videoFileName, CurrentSubtitleView subtitles);
        
        Task ShowDialog<TController>(IView<TController> control);
    }
}