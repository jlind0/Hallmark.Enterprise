using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using HallData.Web.Controllers;
using HallData.Business;
using System.Reflection;
using HallData.ApplicationViews;
using System.ComponentModel.DataAnnotations;
using HallData.Utilities;

namespace HallData.Swashbuckle
{
    public class DescriptionAttributeOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            ServiceMethodHttpActionDescriptor actionDescriptor = apiDescription.ActionDescriptor as ServiceMethodHttpActionDescriptor;
            if(actionDescriptor != null)
            {
                var operationDescription = actionDescriptor.BusinessMethod.GetCustomAttributeCached<DescriptionAttribute>(true) ?? actionDescriptor.InterfaceMethod.GetCustomAttributeCached<DescriptionAttribute>(true);
                if (operationDescription != null)
                    operation.description = operationDescription.Description;
                var resultType = actionDescriptor.BusinessMethod.GetCustomAttributeCached<ServiceMethodResultAttribute>(true) ?? actionDescriptor.InterfaceMethod.GetCustomAttributeCached<ServiceMethodResultAttribute>(true);
                if(resultType != null)
                {
                    foreach(var response in operation.responses)
                    {
                        response.Value.schema = schemaRegistry.GetOrRegister(resultType.ResultType);
                    }
                }
                if(actionDescriptor.ServiceMethod.AcceptSessionHeader)
                {
                    if (operation.parameters == null)
                        operation.parameters = new List<Parameter>();
                    operation.parameters.Add(new Parameter() { @in = "header", name = "session.id", type = "string", required = actionDescriptor.ServiceMethod.RequireSessionHeader });
                }
                foreach(var parameter in actionDescriptor.GetParameters().Select(p => p).Cast<ServiceMethodParameterDescriptor>())
                {
                    var parmDescription = parameter.Parameter.GetCustomAttributeCached<DescriptionAttribute>(true) ?? parameter.InterfaceParameter.GetCustomAttributeCached<DescriptionAttribute>(true);
                    var jsonEncoded = parameter.Parameter.GetCustomAttributeCached<JsonEncode>(true) ?? parameter.InterfaceParameter.GetCustomAttributeCached<JsonEncode>(true);
                    var required = parameter.Parameter.GetCustomAttributeCached<RequiredAttribute>(true) ?? parameter.InterfaceParameter.GetCustomAttributeCached<RequiredAttribute>(true);

                    if (jsonEncoded != null)
                    {
                        var schema = schemaRegistry.GetOrRegister(parameter.ParameterType);
                        if (parameter.ParameterType == typeof(FilterContext) || parameter.ParameterType.IsSubclassOf(typeof(FilterContext)))
                        {
                            operation.parameters.Remove(operation.parameters.SingleOrDefault(p => p.name == "filters"));
                            operation.parameters.Remove(operation.parameters.SingleOrDefault(p => p.name == "searchcriteria"));

                        }
                        else if (parameter.ParameterType == typeof(SortContext) || parameter.ParameterType.IsSubclassOf(typeof(SortContext)))
                            operation.parameters.Remove(operation.parameters.SingleOrDefault(p => p.name == "sorts"));
                        else if (parameter.ParameterType == typeof(PageDescriptor))
                        {
                            operation.parameters.Remove(operation.parameters.SingleOrDefault(p => p.name == "pagesize"));
                            operation.parameters.Remove(operation.parameters.SingleOrDefault(p => p.name == "currentpage"));
                        }
                        operation.parameters.Remove(operation.parameters.SingleOrDefault(p => p.name == parameter.ParameterName));
                        operation.parameters.Add(new Parameter()
                        {
                            name = parameter.ParameterName,
                            @in = "query",
                            schema = schema,
                            required = required != null
                        });
                    }
                    else
                    {
                        var parm = operation.parameters.SingleOrDefault(p => p.name == parameter.ParameterName);
                        if (parmDescription != null && parm != null)
                        {
                            parm.description = parmDescription.Description;
                            if (required != null)
                                parm.required = true;
                        }
                    }
                }
            }
        }
    }
}
