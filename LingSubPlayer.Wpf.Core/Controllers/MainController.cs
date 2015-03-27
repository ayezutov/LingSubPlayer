using System;
using System.IO;
using LingSubPlayer.Common.Subtitles;
using LingSubPlayer.Wpf.Core.ViewModel;
using Vlc.DotNet.Core;

namespace LingSubPlayer.Wpf.Core.Controllers
{
    public class MainController
    {
        private readonly ISubtitlesParser parser;
        public IMainView<MainController> View { get; private set; }

        public MainController(IMainView<MainController> view, ISubtitlesParser parser)
        {
            this.parser = parser;
            View = view;
        }

        public async void OnLoad()
        {
            var session = await View.ShowOpenVideoFileDialog();

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

        public static void InitializeApplication()
        {
            var vlcBaseDirectory = new DirectoryInfo(Path.Combine(@"..\..\..\vlc", Environment.Is64BitProcess ? "x64" : "x86"));

            VlcContext.LibVlcDllsPath = vlcBaseDirectory.FullName;
            VlcContext.LibVlcPluginsPath = Path.Combine(vlcBaseDirectory.FullName, "plugins");
            VlcContext.StartupOptions.ScreenSaverEnabled = false;
            VlcContext.Initialize();
        }
    }
}