using LingSubPlayer.Common.Data;

namespace LingSubPlayer.Presentation.OpenFileDialog
{
    public class OpenFileDialogController
    {
        public IOpenFileDialogView View { get; private set; }
        public IViewManager ViewManager { get; set; }
        public SessionData Session { get; set; }

        public OpenFileDialogController(IOpenFileDialogView view, IViewManager viewManager)
        {
            this.View = view;
            ViewManager = viewManager;
        }

        public void OpenTriggered()
        {
            CloseView(View.SessionData);
        }

        public void CancelTriggered()
        {
            CloseView(null);
        }

        private void CloseView(SessionData data)
        {
            Session = data;
            ViewManager.Hide(View);
        }

        public void Start()
        {
            View.SessionData = new SessionData()
            {
                VideoFileName = "",
                SubtitlesOriginalFileName = "",
                SubtitlesTranslatedFileName = ""
            };
        }
    }
}