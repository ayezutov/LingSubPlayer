namespace LingSubPlayer.Common.Subtitles.Data
{
    public class FormattedText
    {
        private readonly string value;

        public FormattedText(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }
    }
}