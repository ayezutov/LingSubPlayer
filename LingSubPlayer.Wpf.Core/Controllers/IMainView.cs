using System.Threading.Tasks;
using LingSubPlayer.Common.Data;
using LingSubPlayer.Wpf.Core.ViewModel;

namespace LingSubPlayer.Wpf.Core.Controllers
{
    public interface IMainView<TController> : IView<TController> 
    {
        Task<SessionData> ShowOpenVideoFileDialog();

        void PlayFile(string videoFileName, CurrentSubtitleView subtitles);
    }
}