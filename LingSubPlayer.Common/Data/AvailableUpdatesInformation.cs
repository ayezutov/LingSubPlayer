using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LingSubPlayer.Wpf.Core.Annotations;

namespace LingSubPlayer.Common.Data
{
    public class AvailableUpdatesInformation : INotifyPropertyChanged
    {
        public enum UpdateState
        {
            Unknown,
            UpdatesAvailable,
            Downloading,
            Applying,
            RestartRequired,
            UpToDate
        }

        private int downloadPercentage;
        private Version currentVersion;
        private Version futureVersion;
        private UpdateState state;
        private int applicationPercentage;

        public UpdateState State
        {
            get { return state; }
            set
            {
                if (value == state) return;
                state = value;
                OnPropertyChanged();
                OnPropertyChanged("IsApplying");
                OnPropertyChanged("IsDownloading");
                OnPropertyChanged("IsReadyForRestart");
            }
        }

        public Version FutureVersion
        {
            get { return futureVersion; }
            set
            {
                if (Equals(value, futureVersion)) return;
                futureVersion = value;
                OnPropertyChanged();
            }
        }

        public Version CurrentVersion
        {
            get { return currentVersion; }
            set
            {
                if (Equals(value, currentVersion)) return;
                currentVersion = value;
                OnPropertyChanged();
            }
        }

        public int DownloadPercentage
        {
            get { return downloadPercentage; }
            set
            {
                if (value == downloadPercentage) return;
                downloadPercentage = value;
                OnPropertyChanged();
            }
        }

        public bool IsDownloading
        {
            get { return state == UpdateState.Downloading; }
        }

        public bool IsApplying
        {
            get { return state == UpdateState.Applying; }
        }

        public bool IsReadyForRestart
        {
            get { return state == UpdateState.RestartRequired; }
        }

        public int ApplicationPercentage
        {
            get { return applicationPercentage; }
            set
            {
                if (value == applicationPercentage) return;
                applicationPercentage = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}