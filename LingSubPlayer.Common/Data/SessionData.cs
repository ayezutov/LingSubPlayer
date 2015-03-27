using System.ComponentModel;
using System.Runtime.CompilerServices;
using LingSubPlayer.Wpf.Core.Annotations;

namespace LingSubPlayer.Common.Data
{
    public class SessionData : INotifyPropertyChanged
    {
        private string videoFileName;
        private string subtitlesOriginalFileName;
        private string subtitlesTranslatedFileName;

        public string VideoFileName
        {
            get { return videoFileName; }
            set
            {
                if (value == videoFileName) return;
                videoFileName = value;
                OnPropertyChanged();
            }
        }

        public string SubtitlesOriginalFileName
        {
            get { return subtitlesOriginalFileName; }
            set
            {
                if (value == subtitlesOriginalFileName) return;
                subtitlesOriginalFileName = value;
                OnPropertyChanged();
            }
        }

        public string SubtitlesTranslatedFileName
        {
            get { return subtitlesTranslatedFileName; }
            set
            {
                if (value == subtitlesTranslatedFileName) return;
                subtitlesTranslatedFileName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}