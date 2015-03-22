using System;
using System.Reflection;
using System.Windows;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;

namespace LingSubPlayer.Wpf.Core.Controls
{
    public class WindowContainer : Xceed.Wpf.Toolkit.Primitives.WindowContainer
    {
        public static readonly DependencyProperty IsModalChildVisibleProperty = DependencyProperty.Register(
            "IsModalChildVisible", typeof (bool), typeof (WindowContainer), new PropertyMetadata(default(bool)));

        public WindowContainer()
        {
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            foreach (WindowControl windowControl in Children)
            {
                if (windowControl is Xceed.Wpf.Toolkit.MessageBox && windowControl.Left == 0.0 && windowControl.Top == 0.0 || windowControl is ChildWindow && ((ChildWindow)windowControl).WindowStartupLocation == Xceed.Wpf.Toolkit.WindowStartupLocation.Center)
                {
                    CenterChild(windowControl);
                }
            }
        }

        private void CenterChild(WindowControl windowControl)
        {
            if (windowControl.ActualWidth == 0.0 || windowControl.ActualHeight == 0.0)
                return;
            windowControl.Left = (ActualWidth - windowControl.ActualWidth) / 2.0;
            windowControl.Left += windowControl.Margin.Left - windowControl.Margin.Right;
            windowControl.Top = (ActualHeight - windowControl.ActualHeight) / 2.0;
            windowControl.Top += windowControl.Margin.Top - windowControl.Margin.Bottom;
        }

        public bool IsModalChildVisible
        {
            get { return (bool) GetValue(IsModalChildVisibleProperty); }
            set { SetValue(IsModalChildVisibleProperty, value); }
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            if (visualRemoved != null)
            {
                ProcessVisualChildRemoved(visualRemoved);
            }

            if (visualAdded != null)
            {
                ProcessVisualChildAdded(visualAdded);
            }
        }

        private void ProcessVisualChildAdded(DependencyObject visualAdded)
        {
            WindowControl windowControl = (WindowControl) visualAdded;
            windowControl.IsVisibleChanged += Child_IsVisibleChanged;
            var childWindow = windowControl as ChildWindow;
            if (childWindow != null)
            {
                var e = typeof (ChildWindow).GetEvent("IsModalChanged", BindingFlags.Instance | BindingFlags.NonPublic);
                if (e != null)
                {
                    var add = e.AddMethod;
                    add.Invoke(childWindow, new object[]{ new EventHandler<EventArgs>(Child_IsModalChanged) } );
                }
            }
        }

        private void ProcessVisualChildRemoved(DependencyObject visualRemoved)
        {
            WindowControl windowControl = (WindowControl) visualRemoved;

            windowControl.IsVisibleChanged -= Child_IsVisibleChanged;
            var childWindow = windowControl as ChildWindow;
            if (childWindow != null)
            {
                var e = typeof (ChildWindow).GetEvent("IsModalChanged", BindingFlags.Instance | BindingFlags.NonPublic);
                if (e != null)
                {
                    var remove = e.RemoveMethod;
                    remove.Invoke(childWindow, new object[]{ new EventHandler<EventArgs>(Child_IsModalChanged) });
                }
            }
        }

        private void Child_IsModalChanged(object sender, EventArgs e)
        {
            UpdateIsModalChildVisible();
        }

        private void Child_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateIsModalChildVisible();
        }

        private void UpdateIsModalChildVisible()
        {
            var f = typeof(Xceed.Wpf.Toolkit.Primitives.WindowContainer).GetField("_isModalBackgroundApplied", BindingFlags.Instance | BindingFlags.NonPublic);
            if (f != null)
            {
                IsModalChildVisible = (bool) f.GetValue(this);
            }
        }
    }
}