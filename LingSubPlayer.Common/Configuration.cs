using System.Configuration;

namespace LingSubPlayer.Common
{
    public class Configuration : IConfiguration
    {
        public string VlcPath {
            get { return ConfigurationManager.AppSettings["vlc.location"]; }
        }
    }
}