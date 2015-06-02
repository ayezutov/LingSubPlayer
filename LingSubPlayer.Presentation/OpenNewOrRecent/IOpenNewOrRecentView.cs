using LingSubPlayer.Wpf.Core.ViewModel;

namespace LingSubPlayer.Wpf.Core.Controllers
{
    public interface IOpenNewOrRecentView: IView<OpenNewOrRecentController>
    {
        RecentFilesView ViewModel { get; set; }
    }
}