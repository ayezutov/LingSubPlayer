using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace LingSubPlayer.Wpf.Core.Controls
{
    public class ExtendedUserControl : UserControl
    {
        public ExtendedUserControl()
        {
            //base.IsVisibleChanged += OnIsVisibleChanged;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!this.Equals(sender))
            {
                return;
            }

            var wasVisible = (bool)dependencyPropertyChangedEventArgs.OldValue;
            var isVisible = (bool)dependencyPropertyChangedEventArgs.NewValue;
            if (!wasVisible && isVisible)
            {
                OnShown(sender as ExtendedUserControl);
            }
        }

        protected virtual void OnShown(ExtendedUserControl sender)
        {
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == "IsVisible")
            {
                OnIsVisibleChanged(this, e);
            }
        }
    }
}