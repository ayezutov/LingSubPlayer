using LingSubPlayer.Common.Data;

namespace LingSubPlayer.Presentation.ApplicationUpdate
{
    public interface IUpdatesAvailableView
    {
        void ShowUpdatesAvailable(AvailableUpdatesInformation availableUpdatesInformation);
    }
}