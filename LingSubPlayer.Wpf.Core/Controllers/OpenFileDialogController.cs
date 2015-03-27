namespace LingSubPlayer.Wpf.Core.Controllers
{
    public class OpenFileDialogController
    {
        public IOpenFileDialogView View { get; private set; }

        public OpenFileDialogController(IOpenFileDialogView view)
        {
            this.View = view;
        }
    }
}