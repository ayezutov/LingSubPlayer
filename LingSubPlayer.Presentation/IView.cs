namespace LingSubPlayer.Wpf.Core.Controllers
{
    public interface IView<TController>
    {
        TController Controller { get; set; }
    }
}