using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HallData.Exceptions;
using HallData.Translation;

namespace HallData.Validation
{
    public class GlobalizedValidationResult : ValidationResult, IHasErrorCode
    {
        public string ErrorCode { get; private set; }
        public GlobalizedValidationResult(ITranslationService translation, ValidationResult result, string errorCode = null)
            : base(result)
        {
            this.ErrorCode = errorCode;
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                if (errorCode == null)
                    this.ErrorMessage = translation.Translate(this.ErrorMessage);
                else
                    this.ErrorMessage = translation.GetErrorMessage(errorCode);
            }
        }
    }

    public static class ValidationResultFactory
    {
        private static Func<ValidationResult, string, ValidationResult> Factory { get; set; }
        public static void Initialize(Func<ValidationResult, string, ValidationResult> factory)
        {
            Factory = factory;
        }
        public static ValidationResult Create(ValidationResult result, string errorCode = null)
        {
            return Factory(result, errorCode);
        }
    }
}
