using System.Configuration;

namespace LingSubPlayer.Common
{
    public class Configuration : IConfiguration
    {
        public string VlcPath {
            get { return ConfigurationManager.AppSettings["vlc.location"]; }
        }

        public string UpdateUrl {
            get { return ConfigurationManager.AppSettings["update.url"]; }
        }

        public string ApplicationPackageName
        {
            get { return "LingSubPlayer"; } 
        }
    }
}