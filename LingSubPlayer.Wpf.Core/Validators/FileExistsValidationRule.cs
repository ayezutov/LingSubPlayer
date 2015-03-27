using System.Globalization;
using System.Windows.Controls;
using SystemWrapper.IO;

namespace LingSubPlayer.Wpf.Core.Validators
{
    public class FileExistsValidationRule : ValidationRule
    {
        private readonly IFileWrap file;

        public FileExistsValidationRule() : this(new FileWrap())
        {
        }

        public FileExistsValidationRule(IFileWrap file)
        {
            this.file = file;
        }

        public bool ValidateEmptyValue { get; set; }

        public string MessageDoesNotExist { get; set; }

        public string MessageEmptyValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (ValidateEmptyValue && string.IsNullOrEmpty(value as string))
            {
                return new ValidationResult(false, MessageEmptyValue ?? "The value must be a path to an existing file and cannot be left empty");
            }

            if (file.Exists(value as string))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, MessageDoesNotExist ?? "The value must be a path to an existing file");
        }
    }
}