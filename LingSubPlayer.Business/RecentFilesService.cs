using System.Collections.Generic;
using LingSubPlayer.Common.Data;
using LingSubPlayer.DataAccess;

namespace LingSubPlayer.Business
{
    public class RecentFilesService
    {
        private readonly IRecentFilesRepository repo;

        public RecentFilesService(IRecentFilesRepository repo)
        {
            this.repo = repo;
        }

        public IList<SessionData> GetRecentSessions()
        {
            return repo.GetRecentSessions();
        }

        public void AddToRecentSessions(SessionData sessionData)
        {
            var existingSessions = repo.GetRecentSessions() ?? new List<SessionData>();

            AddSessionToList(sessionData, existingSessions);

            repo.SaveRecentSessions(existingSessions);
        }

        private void AddSessionToList(SessionData sessionData, IList<SessionData> existingSessions)
        {
            if (existingSessions.Contains(sessionData))
            {
                existingSessions.Remove(sessionData);
            }

            while (existingSessions.Count >= 5)
            {
                existingSessions.RemoveAt(existingSessions.Count - 1);
            }

            existingSessions.Insert(0, sessionData);
        }

        public void RemoveFromRecentSessions(SessionData sessionData)
        {
            var existingSessions = repo.GetRecentSessions() ?? new List<SessionData>();

            if (existingSessions.Contains(sessionData))
            {
                existingSessions.Remove(sessionData);
            }

            repo.SaveRecentSessions(existingSessions);
        }
    }
}