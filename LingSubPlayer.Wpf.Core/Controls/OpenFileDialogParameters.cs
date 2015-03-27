namespace LingSubPlayer.Wpf.Core.Controls
{
    public class OpenFileDialogParameters
    {
        public bool CheckFileExists { get; set; }

        public string Filter { get; set; }

        public string Title { get; set; }

        public string InitialDirectory { get; set; }
    }
}