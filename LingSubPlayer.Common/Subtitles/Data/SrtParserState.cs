namespace LingSubPlayer.Common.Subtitles.Data
{
    internal enum SrtParserState
    {
        NotStarted,
        BlockBeginningLine,
        TimeAndPositionLine,
        TextLine,
        SeparatorLine,
        Finished
    }
}