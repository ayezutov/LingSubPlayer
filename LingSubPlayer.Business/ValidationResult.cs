namespace LingSubPlayer.Business
{
    public class ValidationResult
    {
        public ValidationResult(bool isValid, string[] messages)
        {
            IsValid = isValid;
            Messages = messages;
        }

        public bool IsValid { get; private set; }

        public string[] Messages { get; private set; }
    }
}