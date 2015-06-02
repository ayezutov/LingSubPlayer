using LingSubPlayer.Presentation.ViewModels;

namespace LingSubPlayer.Presentation.OpenNewOrRecent
{
    public interface IOpenNewOrRecentView: IView<OpenNewOrRecentController>
    {
        RecentFilesView ViewModel { get; set; }
    }
}