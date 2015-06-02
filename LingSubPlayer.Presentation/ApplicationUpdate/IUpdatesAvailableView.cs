using LingSubPlayer.Common.Data;

namespace LingSubPlayer.Wpf.Core.Controllers
{
    public interface IUpdatesAvailableView
    {
        void ShowUpdatesAvailable(AvailableUpdatesInformation availableUpdatesInformation);
    }
}