using System.Collections.Generic;
using LingSubPlayer.Common.Data;

namespace LingSubPlayer.Presentation.ViewModels
{
    public class RecentFilesView
    {
        public RecentFilesView()
        {
            RecentFiles = new List<SessionData>();
        }

        public IList<SessionData> RecentFiles { get; set; }
    }
}