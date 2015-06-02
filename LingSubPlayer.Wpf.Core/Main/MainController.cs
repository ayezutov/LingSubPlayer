using System;
using System.IO;
using System.Threading.Tasks;
using LingSubPlayer.Common;
using LingSubPlayer.Common.Data;
using LingSubPlayer.Common.Subtitles;
using LingSubPlayer.Presentation;
using LingSubPlayer.Presentation.ApplicationUpdate;
using LingSubPlayer.Presentation.OpenNewOrRecent;
using Vlc.DotNet.Core;

namespace LingSubPlayer.Wpf.Core.Main
{
    public class MainController
    {
        private readonly IViewManager viewManager;
        private readonly ISubtitlesParser parser;
        private readonly ApplicationUpdateController updateController;
        private readonly OpenNewOrRecentController openNewOrRecentController;
        private Task updateTask;
        public IMainView View { get; private set; }

        public MainController(IMainView view,
            IViewManager viewManager,
            ISubtitlesParser parser, 
            ApplicationUpdateController updateController,
            OpenNewOrRecentController openNewOrRecentController)
        {
            this.viewManager = viewManager;
            this.parser = parser;
            this.updateController = updateController;
            this.openNewOrRecentController = openNewOrRecentController;
            View = view;
        }

        public async void OnLoad()
        {
            var info = new AvailableUpdatesInformation();
            updateTask = Task.Run(() => updateController.CheckForUpdatesAndDownload(View, info));

            await viewManager.Show(openNewOrRecentController.View);
            SessionData session = openNewOrRecentController.SessionData;

            if (session != null)
            {
                using (var subtitlesOriginalStream = File.OpenRead(session.SubtitlesOriginalFileName))
                {
                    using (var subtitlesTranslatedStream = File.OpenRead(session.SubtitlesTranslatedFileName))
                    {
                        var subtitles = new CurrentSubtitleView(parser.Parse(subtitlesOriginalStream), parser.Parse(subtitlesTranslatedStream));
                        View.PlayFile(session.VideoFileName, subtitles);
                    }
                }
            }
        }


        public static void InitializeApplication(Configuration configuration)
        {
            var vlcBaseDirectory = new DirectoryInfo(Path.Combine(configuration.VlcPath, Environment.Is64BitProcess ? "x64" : "x86"));

            VlcContext.LibVlcDllsPath = vlcBaseDirectory.FullName;
            VlcContext.LibVlcPluginsPath = Path.Combine(vlcBaseDirectory.FullName, "plugins");
            VlcContext.StartupOptions.ScreenSaverEnabled = false;
            VlcContext.Initialize();
        }

        public void OnBeforeExit()
        {
            VlcContext.CloseAll();
            updateTask.Wait();
            Environment.Exit(0);
        }
    }
}