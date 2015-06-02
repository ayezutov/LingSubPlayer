using LingSubPlayer.Common.Data;

namespace LingSubPlayer.Presentation.OpenFileDialog
{
    public interface IOpenFileDialogView : IView<OpenFileDialogController>{
        SessionData SessionData { get; set; }
    }
}