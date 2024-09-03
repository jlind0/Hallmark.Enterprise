using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.Business;
using HallData.Admin.ApplicationViews;
using HallData.Validation;
using HallData.Exceptions;
using System.Collections.Concurrent;
using System.Threading;

namespace HallData.Admin.Business
{
    public class ApplicationViewValidator
    {
        protected IReadOnlyTemplateImplementation Template { get; private set; }
        public ApplicationViewValidator(IReadOnlyTemplateImplementation template)
        {
            this.Template = template;
            this.TemplateTemplateTypeMapping = new ConcurrentDictionary<int, TemplateTypeKey>();
        }
        private ConcurrentDictionary<int, TemplateTypeKey> TemplateTemplateTypeMapping { get; set; }
        protected async Task<TemplateTypeKey> GetTemplateType(int templateId, CancellationToken token = default(CancellationToken))
        {
            TemplateTypeKey templateType;
            if (!this.TemplateTemplateTypeMapping.TryGetValue(templateId, out templateType))
            {
                var template = await this.Template.Get(templateId, token);
                if (template == null)
                    throw new GlobalizedValidationException("ADMIN_TEMPLATE_NOTFOUND");
                templateType = template.Result.TemplateType;
                this.TemplateTemplateTypeMapping.TryAdd(templateId, templateType);
            }
            return templateType;
        }
        public async Task Validate(ApplicationViewResult view, CancellationToken token = default(CancellationToken))
        {
            if (view.DefaultSpec != null)
            {
                if (view.DefaultSpec.GridTemplate != null && view.DefaultSpec.GridTemplate.TemplateId != null && 
                    ((TemplateTypes)(await this.GetTemplateType(view.DefaultSpec.GridTemplate.TemplateId.Value, token)).TemplateTypeId.Value) != TemplateTypes.Grid)
                    throw new GlobalizedValidationException("ADMIN_APPLICATIONVIEW_GRIDTEMPLATE_TEMPLATETYPE_INVALID");
                if (view.DefaultSpec.PagerTemplate != null && view.DefaultSpec.PagerTemplate.TemplateId != null &&
                   ((TemplateTypes)(await this.GetTemplateType(view.DefaultSpec.PagerTemplate.TemplateId.Value, token)).TemplateTypeId.Value) != TemplateTypes.GridPager)
                    throw new GlobalizedValidationException("ADMIN_APPLICATIONVIEW_GRIDTEMPLATE_TEMPLATETYPE_INVALID");
            }
        }
    }
}
