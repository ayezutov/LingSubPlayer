using System;
using System.IO;
using System.Threading.Tasks;
using LingSubPlayer.Common;
using LingSubPlayer.Common.Subtitles;
using LingSubPlayer.Wpf.Core.ViewModel;
using Vlc.DotNet.Core;

namespace LingSubPlayer.Wpf.Core.Controllers
{
    public class MainController
    {
        private readonly ISubtitlesParser parser;
        private readonly ApplicationUpdateController updateController;
        private Task updateTask;
        public IMainView<MainController> View { get; private set; }

        public MainController(IMainView<MainController> view, 
            ISubtitlesParser parser, 
            ApplicationUpdateController updateController)
        {
            this.parser = parser;
            this.updateController = updateController;
            View = view;
        }

        public async void OnLoad()
        {
            var info = new AvailableUpdatesInformation();
            updateTask = Task.Run(() => updateController.CheckForUpdatesAndDownload(View, info));
            var session = await View.ShowOpenVideoFileDialog();

//            SessionData session = null; new SessionData()
//            {
//                VideoFileName = @"d:\temp\LingSubPlayer\data\cartoon.flv",
//                SubtitlesOriginalFileName = @"d:\temp\LingSubPlayer\data\ENG.srt",
//                SubtitlesTranslatedFileName = @"d:\temp\LingSubPlayer\data\RUS.srt"
//            };

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
        }
    }
}