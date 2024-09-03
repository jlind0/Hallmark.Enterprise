using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace HallData.Validation
{
	public static class ValidationExtensions
	{
		public static void Validate(this object obj)
		{
			if (obj == null)
				throw new GlobalizedValidationException("VALIDATION_NULL");
			ValidationContext context = new ValidationContext(obj);
			Validator.ValidateObject(obj, context);
		}

		public static bool TryValidate(this object obj, ref List<ValidationResult> results)
		{
			if (obj == null)
				throw new GlobalizedValidationException("VALIDATION_NULL");
			ValidationContext context = new ValidationContext(obj);
			if(results == null)
				results = new List<ValidationResult>();
			return Validator.TryValidateObject(obj, context, results);
		}

		public static bool TryValidate(this object obj)
		{
			List<ValidationResult> results = null;
			return obj.TryValidate(ref results);
		}
	}
}
