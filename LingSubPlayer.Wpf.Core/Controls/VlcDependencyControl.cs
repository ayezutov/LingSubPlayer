using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using LingSubPlayer.Common;
using LingSubPlayer.Wpf.Core.Annotations;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures.LibVlc.Media;
using Vlc.DotNet.Core.Interops.Signatures.LibVlc.MediaListPlayer;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;

namespace LingSubPlayer.Wpf.Core.Controls
{
    public class VlcDependencyControl : FrameworkElement, IDisposable, INotifyPropertyChanged
    {
        public static readonly DependencyProperty VideoSourceProperty = VlcControl.VideoSourceProperty.AddOwner(typeof (VlcDependencyControl));
        public static readonly DependencyProperty VideoBrushProperty = VlcControl.VideoBrushProperty.AddOwner(typeof(VlcDependencyControl));
    
        private readonly VlcControl vlcControl;

        public VlcDependencyControl()
        {
            vlcControl = new VlcControl();
            AddBinding(vlcControl, VlcControl.VideoSourceProperty, this, VideoSourceProperty);
            AddBinding(vlcControl, VlcControl.VideoBrushProperty, this, VideoBrushProperty);

            BindToEventsForChangeNotifications();
        }

        private void BindToEventsForChangeNotifications()
        {
            vlcControl.TimeChanged += BindPropertyChanged("TimeChanged", x => x.Time);
            vlcControl.LengthChanged += BindPropertyChanged<long, TimeSpan>("LengthChanged", x => x.Duration);
            vlcControl.PositionChanged += BindPropertyChanged("PositionChanged", x => x.Position);
            
            vlcControl.PausableChanged += BindPropertyChanged<int, bool>("PausableChanged", x => x.IsPausable);
            vlcControl.SeekableChanged += BindPropertyChanged<int, bool>("SeekableChanged", x => x.IsSeekable);

            vlcControl.Paused += BindPropertyChanged<EventArgs, object>("Paused", x => x.IsPlaying, x => x.IsPaused, x => x.State);
            vlcControl.Playing += BindPropertyChanged<EventArgs, object>("Playing", x => x.IsPlaying, x => x.IsPaused, x => x.State);
            vlcControl.Stopped += BindPropertyChanged<EventArgs, object>("Stopped", x => x.IsPlaying, x => x.IsPaused, x => x.State);
            vlcControl.EndReached += BindPropertyChanged<EventArgs, object>("EndReached", x => x.IsPlaying, x => x.IsPaused, x => x.State);
            vlcControl.Buffering += BindPropertyChanged<float, object>("Buffering", x => x.State);
        }

        private readonly Dictionary<string, Delegate> allEvents = new Dictionary<string, Delegate>();

        private VlcEventHandler<VlcControl, T> BindPropertyChanged<T>(string eventName, params Expression<Func<VlcControl, T>>[] func)
        {
            return BindPropertyChanged<T, T>(eventName, func);
        }

        private VlcEventHandler<VlcControl, TDelegateType> BindPropertyChanged<TDelegateType, TPropertyType>(string eventName, params Expression<Func<VlcControl, TPropertyType>>[] func)
        {
            if (allEvents.ContainsKey(eventName))
            {
                throw new NotSupportedException("You can register property changed event only once.");
            }

            var propertyNames = func.Select(f => f.GetPropertyOrMethodName()).ToArray();

            VlcEventHandler<VlcControl, TDelegateType> handler = (c, d) =>
            {
                foreach (var propertyName in propertyNames)
                {
                    OnPropertyChanged(propertyName);
                }
            };

            allEvents[eventName] = handler;

            return handler;
        }

        private void UndindAllEventsFromChangeNotifications()
        {
            foreach (KeyValuePair<string, Delegate> keyValuePair in allEvents)
            {
                var eventInfo = GetType().GetEvent(keyValuePair.Key);

                if (eventInfo != null)
                {
                    eventInfo.RemoveEventHandler(this, keyValuePair.Value);
                }
            }
        }

        private void AddBinding(DependencyObject d1, DependencyProperty d1Property, DependencyObject d2, DependencyProperty d2Property)
        {
            var binding = new Binding {Source = d1, Path = new PropertyPath(d1Property), Mode = BindingMode.TwoWay};
            BindingOperations.SetBinding(d2, d2Property, binding);
        }

        #region Delegating original VlcControl properties

        public void NextFrame()
        {
            vlcControl.NextFrame();
        }

        public void Play()
        {
            vlcControl.Play();
        }

        public void Play(MediaBase media)
        {
            vlcControl.Play(media);
        }

        public void Stop()
        {
            vlcControl.Stop();
        }

        public void Next()
        {
            vlcControl.Next();
        }

        public void Previous()
        {
            vlcControl.Previous();
        }

        public void Pause()
        {
            vlcControl.Pause();
        }

        public void TakeSnapshot(string filePath, uint width, uint height)
        {
            vlcControl.TakeSnapshot(filePath, width, height);
        }

        public ImageSource VideoSource
        {
            get { return vlcControl.VideoSource; }
            set { vlcControl.VideoSource = value; }
        }

        public ImageBrush VideoBrush
        {
            get { return vlcControl.VideoBrush; }
            set { vlcControl.VideoBrush = value; }
        }

        public float FPS
        {
            get { return vlcControl.FPS; }
        }

        public bool WillPlay
        {
            get { return vlcControl.WillPlay; }
        }

        public bool IsSeekable
        {
            get { return vlcControl.IsSeekable; }
        }

        public bool IsPausable
        {
            get { return vlcControl.IsPausable; }
        }

        public bool IsPlaying
        {
            get { return vlcControl.IsPlaying; }
        }

        public bool IsPaused
        {
            get { return vlcControl.IsPaused; }
        }

        public float Position
        {
            get { return vlcControl.Position; }
            set { vlcControl.Position = value; }
        }

        public TimeSpan Time
        {
            get { return vlcControl.Time; }
            set { vlcControl.Time = value; }
        }

        public float Rate
        {
            get { return vlcControl.Rate; }
            set { vlcControl.Rate = value; }
        }

        public MediaBase Media
        {
            get { return vlcControl.Media; }
            set { vlcControl.Media = value; }
        }

        public States State
        {
            get { return vlcControl.State; }
        }

        public TimeSpan Duration
        {
            get { return vlcControl.Duration; }
        }

        public VlcAudioProperties AudioProperties
        {
            get { return vlcControl.AudioProperties; }
        }

        public VlcVideoProperties VideoProperties
        {
            get { return vlcControl.VideoProperties; }
        }

        public VlcLogProperties LogProperties
        {
            get { return vlcControl.LogProperties; }
        }

        public VlcAudioOutputDevices AudioOutputDevices
        {
            get { return vlcControl.AudioOutputDevices; }
        }

        public VlcMediaListPlayer Medias
        {
            get { return vlcControl.Medias; }
        }

        public PlaybackModes PlaybackMode
        {
            set { vlcControl.PlaybackMode = value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> Backward
        {
            add { vlcControl.Backward += value; }
            remove { vlcControl.Backward -= value; }
        }

        public event VlcEventHandler<VlcControl, float> Buffering
        {
            add { vlcControl.Buffering += value; }
            remove { vlcControl.Buffering -= value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> EncounteredError
        {
            add { vlcControl.EncounteredError += value; }
            remove { vlcControl.EncounteredError -= value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> EndReached
        {
            add { vlcControl.EndReached += value; }
            remove { vlcControl.EndReached -= value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> Forward
        {
            add { vlcControl.Forward += value; }
            remove { vlcControl.Forward -= value; }
        }

        public event VlcEventHandler<VlcControl, long> LengthChanged
        {
            add { vlcControl.LengthChanged += value; }
            remove { vlcControl.LengthChanged -= value; }
        }

        public event VlcEventHandler<VlcControl, int> PausableChanged
        {
            add { vlcControl.PausableChanged += value; }
            remove { vlcControl.PausableChanged -= value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> Paused
        {
            add { vlcControl.Paused += value; }
            remove { vlcControl.Paused -= value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> Playing
        {
            add { vlcControl.Playing += value; }
            remove { vlcControl.Playing -= value; }
        }

        public event VlcEventHandler<VlcControl, float> PositionChanged
        {
            add { vlcControl.PositionChanged += value; }
            remove { vlcControl.PositionChanged -= value; }
        }

        public event VlcEventHandler<VlcControl, int> SeekableChanged
        {
            add { vlcControl.SeekableChanged += value; }
            remove { vlcControl.SeekableChanged -= value; }
        }

        public event VlcEventHandler<VlcControl, string> SnapshotTaken
        {
            add { vlcControl.SnapshotTaken += value; }
            remove { vlcControl.SnapshotTaken -= value; }
        }

        public event VlcEventHandler<VlcControl, EventArgs> Stopped
        {
            add { vlcControl.Stopped += value; }
            remove { vlcControl.Stopped -= value; }
        }

        public event VlcEventHandler<VlcControl, TimeSpan> TimeChanged
        {
            add { vlcControl.TimeChanged += value; }
            remove { vlcControl.TimeChanged -= value; }
        }

        public event VlcEventHandler<VlcControl, long> TitleChanged
        {
            add { vlcControl.TitleChanged += value; }
            remove { vlcControl.TitleChanged -= value; }
        }

        public event VlcEventHandler<VlcControl, int> VideoOutChanged
        {
            add { vlcControl.VideoOutChanged += value; }
            remove { vlcControl.VideoOutChanged -= value; }
        }

        public static readonly DependencyPropertyKey CanPlayProperty = DependencyProperty.RegisterReadOnly(
            "CanPlay", typeof (bool), typeof (VlcDependencyControl), new PropertyMetadata(default(bool)));

        public bool CanPlay
        {
            get { return (bool) GetValue(CanPlayProperty.DependencyProperty); }
        }

        public static readonly DependencyPropertyKey CanPauseProperty = DependencyProperty.RegisterReadOnly(
            "CanPause", typeof (bool), typeof (VlcDependencyControl), new PropertyMetadata(default(bool)));

        public bool CanPause
        {
            get { return (bool) GetValue(CanPauseProperty.DependencyProperty); }
        }
        #endregion


        public void Dispose()
        {
            UndindAllEventsFromChangeNotifications();
            vlcControl.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<TResult>(Expression<Func<VlcDependencyControl, TResult>> propertyAccess)
        {
            OnPropertyChanged(propertyAccess.GetPropertyOrMethodName());
        }
    }
}
