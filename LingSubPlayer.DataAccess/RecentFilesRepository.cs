using System.Collections.Generic;
using System.Linq;
using DBreeze;
using DBreeze.DataTypes;
using DBreeze.Utils;
using LingSubPlayer.Common.Data;

namespace LingSubPlayer.DataAccess
{
    public class RecentFilesRepository : IRecentFilesRepository
    {
        private const string TableName = "RecentFiles";
        private readonly IRepositorySettings settings;

        public RecentFilesRepository(IRepositorySettings settings)
        {
            this.settings = settings;
        }

        public IList<SessionData> GetRecentSessions()
        {
            using (var engine = new DBreezeEngine(settings.ConnectionData))
            {
                using (var trans = engine.GetTransaction())
                {
                    return trans.SelectForward<int, DbMJSON<SessionData>>(TableName).Select(r => r.Value.Get).ToList();
                }
                
            }
        }

        public void SaveRecentSessions(IList<SessionData> files)
        {
            using (var engine = new DBreezeEngine(settings.ConnectionData))
            {
                using (var trans = engine.GetTransaction())
                {
                    trans.RemoveAllKeys(TableName, true);

                    int i = 0;
                    foreach (var file in files)
                    {
                        trans.Insert(TableName, i++, new DbMJSON<SessionData>(file));
                    }

                    trans.Commit();
                }
                
            }
        }
    }
}