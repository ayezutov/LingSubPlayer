using System.Collections.Generic;
using LingSubPlayer.Common.Data;

namespace LingSubPlayer.DataAccess
{
    public interface IRecentFilesRepository
    {
        IList<SessionData> GetRecentSessions();

        void SaveRecentSessions(IList<SessionData> files);
    }
}