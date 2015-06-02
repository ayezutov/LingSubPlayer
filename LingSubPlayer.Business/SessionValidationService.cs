using System.Collections.Generic;
using System.Linq;
using LingSubPlayer.Common;
using LingSubPlayer.Common.Data;

namespace LingSubPlayer.Business
{
    public class SessionValidationService
    {
        private readonly IFileSystem fileSystem;

        public SessionValidationService(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public ValidationResult Validate(SessionData sessionData)
        {
            if (sessionData == null)
            {
                return new ValidationResult(false, new []{"Session data cannot be null"});
            }

            IList<string> messages = new List<string>();
            ValidateFileName(messages, sessionData.VideoFileName, "Video file");
            ValidateFileName(messages, sessionData.SubtitlesOriginalFileName, "Original subtitles file");
            ValidateFileName(messages, sessionData.SubtitlesTranslatedFileName, "Translated subtitles file");
            return new ValidationResult(messages.Count == 0, messages.Count == 0 ? null : messages.ToArray());
        }

        private void ValidateFileName(IList<string> messages, string fileName, string fieldName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                messages.Add(fieldName +
                             " name should not be empty");
            }
            else if (!fileSystem.FileExists(fileName))
            {
                messages.Add(fieldName +
                             " should exist on disk");
            }
        }
    }
}