using System.Windows.Input;

namespace LingSubPlayer.Wpf.Core
{
    public class Commands
    {
        public static RoutedUICommand ToSubtitleNextBlockBeginning = new RoutedUICommand("Navigate to the beginning of the next subtitle block", "ToSubtitleNextBlockBeginning", typeof(Commands));

        public static RoutedUICommand ToSubtitlePreviousBlockBeginning = new RoutedUICommand("Navigate to the beginning of the previous subtitle block", "ToSubtitlePreviousBlockBeginning", typeof(Commands));

        public static RoutedUICommand ToSubtitleCurrentBlockBeginning = new RoutedUICommand("Navigate to the beginning of the current subtitle block", "ToSubtitleCurrentBlockBeginning", typeof(Commands));
    }
}
