using System.Windows;

namespace LingSubPlayer.Wpf.Core.Controls
{
    public class WindowParameters
    {
        public WindowParameters()
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
        }

        public WindowStyle WindowStyle { get; set; }

        public Style Style { get; set; }

        public double? Width { get; set; }

        public double? Height { get; set; }
    }
}