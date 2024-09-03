using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;

namespace HallData.Exceptions
{
    public interface IHasErrorCode
    {
        string ErrorCode { get; }
    }
    public class GlobalizedException : Exception, IHasErrorCode
    {
        public string ErrorCode { get; private set; }
        public GlobalizedException(string errorCode, Exception innerException = null)
            : base(errorCode, innerException)
        {
            this.ErrorCode = errorCode;
        }
    }
    public class GlobalizedAuthorizationException : GlobalizedException
    {
        public GlobalizedAuthorizationException(string errorCode) : base(errorCode) { }
    }
    public class GlobalizedAuthenticationException : AuthenticationException, IHasErrorCode
    {
        public string ErrorCode { get; private set; }
        public const string DefaultErrorCode = "USER_NOTAUTH";
        public GlobalizedAuthenticationException(string errorCode = DefaultErrorCode) : base(errorCode)
        {
            this.ErrorCode = errorCode;
        }
    }
    public class GlobalizedValidationException : ValidationException, IHasErrorCode
    {
        public string ErrorCode{get; private set;}
        public GlobalizedValidationException(ValidationResult result, ValidationAttribute attribute, object value, string errorCode = null)
            : base(result, attribute, value)
        {
            this.ErrorCode = errorCode;
        }
        public GlobalizedValidationException(string errorCode, ValidationAttribute attribute, object value)
            : base(errorCode, attribute, value)
        {
            this.ErrorCode = errorCode;
        }
        public GlobalizedValidationException(string errorCode, Exception innerException = null)
            : base(errorCode, innerException)
        {
            this.ErrorCode = errorCode;
        }
    }

}
