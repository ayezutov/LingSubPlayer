using LingSubPlayer.Business;
using LingSubPlayer.Common.Data;
using LingSubPlayer.Presentation.OpenFileDialog;
using LingSubPlayer.Presentation.ViewModels;

namespace LingSubPlayer.Presentation.OpenNewOrRecent
{
    public class OpenNewOrRecentController
    {
        private readonly IViewManager viewManager;
        private readonly OpenFileDialogController openFileDialogController;
        private readonly RecentFilesService service;
        private readonly SessionValidationService sessionValidator;

        public OpenNewOrRecentController(IOpenNewOrRecentView view,
            IViewManager viewManager,
            OpenFileDialogController openFileDialogController,
            RecentFilesService service,
            SessionValidationService sessionValidator)
        {
            this.viewManager = viewManager;
            this.openFileDialogController = openFileDialogController;
            this.service = service;
            this.sessionValidator = sessionValidator;
            View = view;
        }

        public IOpenNewOrRecentView View { get; set; }
        
        public SessionData SessionData { get; set; }

        public async void Open()
        {
            await viewManager.Show(openFileDialogController.View);

            var session = openFileDialogController.Session;

            if (session != null)
            {
                Open(session);
            }

        }

        public void OnStart()
        {
            View.ViewModel = new RecentFilesView()
            {
                RecentFiles = service.GetRecentSessions()
            };
        }

        public async void Open(SessionData sessionData)
        {
            var result = sessionValidator.Validate(sessionData);

            if (!result.IsValid)
            {
                await
                    viewManager.ShowMessage(string.Format("Cannot open saved session:\r\n\t{0}",
                        string.Join("\r\n\t", result.Messages)));

                service.RemoveFromRecentSessions(sessionData);
                OnStart();
                return;
            }

            SessionData = sessionData;
            service.AddToRecentSessions(SessionData);
            viewManager.Hide(View);
        }
    }
}