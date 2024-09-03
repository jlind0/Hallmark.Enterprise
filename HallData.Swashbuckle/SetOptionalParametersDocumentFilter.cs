using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace HallData.Swashbuckle
{
    public class SetOptionalParametersDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            foreach(var path in swaggerDoc.paths)
            {
                if (path.Value.delete != null)
                    SetOptionalParameters(path.Key, path.Value.delete);
                if (path.Value.get != null)
                    SetOptionalParameters(path.Key, path.Value.get);
                if (path.Value.head != null)
                    SetOptionalParameters(path.Key, path.Value.head);
                if (path.Value.options != null)
                    SetOptionalParameters(path.Key, path.Value.options);
                if (path.Value.patch != null)
                    SetOptionalParameters(path.Key, path.Value.patch);
                if (path.Value.post != null)
                    SetOptionalParameters(path.Key, path.Value.post);
                if (path.Value.put != null)
                    SetOptionalParameters(path.Key, path.Value.put);
            }
        }
        private void SetOptionalParameters(string path, Operation operation)
        {
            if (operation.parameters != null)
            {
                foreach (var parameter in operation.parameters)
                {
                    if (parameter.@in == "query" || (parameter.@in == "path" && !path.Contains("{" + parameter.name + "}")))
                        parameter.required = false;
                }
            }
        }
    }
}
