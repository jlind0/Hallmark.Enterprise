using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HallData.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HallData.Security;

namespace HallData.Tests.HallData.Utilities
{
	[TestClass]
	public class ValidationExtensionsTests
	{
		[TestMethod]
		public void Validate_success()
		{
			SecurityUser user = new SecurityUser { UserName = "Some Value" };

			ValidationExtensions.Validate(user);
		}

		[TestMethod]
		public void Validate_error()
		{
			SecurityUser user = new SecurityUser { UserName = null };

			try
			{
				ValidationExtensions.Validate(user);
				Assert.Fail();
			}
			catch (ValidationException ve)
			{
				Assert.AreEqual("The UserName field is required.", ve.Message);
			}
		}

		[TestMethod]
		public void TryValidate_ValidationResults_No_Errors()
		{
			SecurityUser user = new SecurityUser { UserName = "Some Value" };
			List<ValidationResult> validationResultList = new List<ValidationResult>();

			ValidationExtensions.TryValidate(user, ref validationResultList);

			Assert.IsTrue(validationResultList.Count == 0);
		}

		[TestMethod]
		public void TryValidate_ValidationResults_Errors()
		{
			SecurityUser user = new SecurityUser { UserName = null };
			List<ValidationResult> validationResultList = new List<ValidationResult>();

			ValidationExtensions.TryValidate(user, ref validationResultList);

			Assert.IsTrue(validationResultList.Count == 1);
		}

		[TestMethod]
		public void TryValidate_Results_No_Errors()
		{
			SecurityUser user = new SecurityUser { UserName = "Some Value" };
			ValidationExtensions.TryValidate(user);
		}

		[TestMethod]
		public void TryValidate_Results_Errors()
		{
			SecurityUser user = new SecurityUser { UserName = null };

			ValidationExtensions.TryValidate(user);
		}
	}
}
