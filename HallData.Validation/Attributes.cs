using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HallData.Exceptions;

namespace HallData.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GlobalizedRequiredAttribute : RequiredAttribute, IHasErrorCode
    {
        public string ErrorCode { get; private set; }
        public GlobalizedRequiredAttribute(string errorCode = null)
            : base()
        {
            this.ErrorCode = errorCode;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = base.IsValid(value, validationContext);
            if (isValid != null)
                return ValidationResultFactory.Create(isValid, this.ErrorCode);
            return isValid;
        }
    }
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GlobalizedDataTypeAttribute : DataTypeAttribute, IHasErrorCode
    {
        public string ErrorCode { get; private set; }
        public GlobalizedDataTypeAttribute(DataType dataType, string errorCode = null)
            : base(dataType)
        {
            this.ErrorCode = errorCode;
        }
        public GlobalizedDataTypeAttribute(string customDataType, string errorCode = null) :base(customDataType)
        {
            this.ErrorCode = errorCode;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = base.IsValid(value, validationContext);
            if (isValid != null)
                return ValidationResultFactory.Create(isValid, this.ErrorCode);
            return isValid;
        }
    }

    [AttributeUsageAttribute(AttributeTargets.Property, AllowMultiple = false)]
    public class GlobalizedCompareAttribute : CompareAttribute , IHasErrorCode
    {
        public string ErrorCode { get; private set; }
        public GlobalizedCompareAttribute(string otherProperty, string errorCode = null)
            : base(otherProperty)
        {
            this.ErrorCode = errorCode;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = base.IsValid(value, validationContext);
            if (isValid != null)
                return ValidationResultFactory.Create(isValid, this.ErrorCode);
            return isValid;
        }
    }

    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GlobalizedStringLengthAttribute : StringLengthAttribute, IHasErrorCode
    {
        public string ErrorCode { get; private set; }
        public GlobalizedStringLengthAttribute(int maximumLength, string errorCode = null)
            : base(maximumLength)
        {
            this.ErrorCode = errorCode;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = base.IsValid(value, validationContext);
            if (isValid != null)
                return ValidationResultFactory.Create(isValid, this.ErrorCode);
            return isValid;
        }
    }

    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GlobalizedRegularExpressionAttribute : RegularExpressionAttribute, IHasErrorCode
    {
        public string ErrorCode { get; private set; }
        public GlobalizedRegularExpressionAttribute(string pattern, string errorCode = null)
            : base(pattern)
        {
            this.ErrorCode = errorCode;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = base.IsValid(value, validationContext);
            if (isValid != null)
                return ValidationResultFactory.Create(isValid, this.ErrorCode);
            return isValid;
        }
    }

    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GlobalizedMinLengthAttribute : MinLengthAttribute, IHasErrorCode
    {
        public string ErrorCode { get; private set; }
        public GlobalizedMinLengthAttribute(int length, string errorCode = null)
            : base(length)
        {
            this.ErrorCode = errorCode;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = base.IsValid(value, validationContext);
            if (isValid != null)
                return ValidationResultFactory.Create(isValid, this.ErrorCode);
            return isValid;
        }
    }

    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GlobalizedMaxLengthAttribute : MaxLengthAttribute, IHasErrorCode
    {
        public string ErrorCode { get; private set; }
        public GlobalizedMaxLengthAttribute(string errorCode = null)
            : base()
        {
            this.ErrorCode = errorCode;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = base.IsValid(value, validationContext);
            if (isValid != null)
                return ValidationResultFactory.Create(isValid, this.ErrorCode);
            return isValid;
        }
    }

    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GlobalizedRangeAttribute : RangeAttribute, IHasErrorCode
    {
        public string ErrorCode { get; private set; }
        public GlobalizedRangeAttribute(double minimum, double maximum, string errorCode = null)
            : base(minimum, maximum)
        {
            this.ErrorCode = errorCode;
        }
        public GlobalizedRangeAttribute(int minimum, int maximum, string errorCode = null)
            : base(minimum, maximum)
        {
            this.ErrorCode = errorCode;
        }
        public GlobalizedRangeAttribute(Type type, string minimum, string maximum, string errorCode = null)
            : base(type, minimum, maximum)
        {
            this.ErrorCode = errorCode;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = base.IsValid(value, validationContext);
            if (isValid != null)
                return ValidationResultFactory.Create(isValid, this.ErrorCode);
            return isValid;
        }
    }

   
    
}
