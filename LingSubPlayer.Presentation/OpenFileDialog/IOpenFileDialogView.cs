using LingSubPlayer.Common.Data;

namespace LingSubPlayer.Wpf.Core.Controllers
{
    public interface IOpenFileDialogView : IView<OpenFileDialogController>{
        SessionData SessionData { get; set; }
    }
}