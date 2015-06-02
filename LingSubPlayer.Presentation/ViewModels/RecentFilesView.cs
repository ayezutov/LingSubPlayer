using System.Collections.Generic;
using LingSubPlayer.Common.Data;

namespace LingSubPlayer.Wpf.Core.ViewModel
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