using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Metadata;
using Newtonsoft.Json;
using System.Net;
using System.Web.Http;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Http.Controllers;
using System.Reflection;

namespace HallData.Web.ModelBinding
{
	public class JsonParameterModelBinder : IModelBinder
	{

		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

			if (result == null)
				return false;

			string json = result.RawValue as string;

			if (json == null)
			{
				bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Wrong value type");
				return false;
			}
			try
			{
				//json = WebUtility.UrlDecode(json);
				bindingContext.Model = JsonConvert.DeserializeObject(json, bindingContext.ModelType);
				var validationResults = new List<ValidationResult>();
				var vc = new ValidationContext(bindingContext.Model);
				if (!Validator.TryValidateObject(bindingContext.Model, vc, validationResults))
				{
					foreach (var fail in validationResults)
					{
						bindingContext.ModelState.AddModelError(bindingContext.ModelName, fail.ErrorMessage);
					}
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
				return false;
			}
		}
	}
}
