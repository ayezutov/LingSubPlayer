namespace LingSubPlayer.Common.Logging
{
    public interface ILog
    {
        IInternalLog Write { get; }
    }
}