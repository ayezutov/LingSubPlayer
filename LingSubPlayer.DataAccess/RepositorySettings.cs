namespace LingSubPlayer.DataAccess
{
    public class RepositorySettings : IRepositorySettings
    {
        public RepositorySettings(string connectionData)
        {
            ConnectionData = connectionData;
        }

        public string ConnectionData { get; private set; }
    }
}