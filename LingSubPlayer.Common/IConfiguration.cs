namespace LingSubPlayer.Common
{
    public interface IConfiguration
    {
        string VlcPath { get; }
        string UpdateUrl { get; }
        string ApplicationPackageName { get; }
    }
}