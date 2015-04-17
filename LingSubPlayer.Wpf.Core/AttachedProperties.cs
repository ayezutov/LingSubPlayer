using System.Diagnostics;
using System.Windows;

namespace LingSubPlayer.Wpf.Core
{
    public class AttachedProperties
    {
        public static readonly DependencyProperty CanPauseProperty = DependencyProperty.RegisterAttached(
            "CanPause", typeof (bool), typeof (AttachedProperties), new PropertyMetadata(default(bool)));

        public static void SetCanPause(DependencyObject element, bool value)
        {
            Debug.Write(string.Format("Can Pause: {0}", value));
            element.SetValue(CanPauseProperty, value);
        }

        public static bool GetCanPause(DependencyObject element)
        {
            return (bool) element.GetValue(CanPauseProperty);
        } 
    }
}