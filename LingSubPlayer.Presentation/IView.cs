namespace LingSubPlayer.Presentation
{
    public interface IView<TController>
    {
        TController Controller { get; set; }
    }
}