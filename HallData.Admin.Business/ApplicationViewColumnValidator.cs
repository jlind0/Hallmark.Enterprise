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
    public class ApplicationViewColumnValidator
    {
        protected IReadOnlyTemplateImplementation Template { get; private set; }
        public ApplicationViewColumnValidator(IReadOnlyTemplateImplementation template)
        {
            this.Template = template;
            this.TemplateTemplateTypeMapping = new ConcurrentDictionary<int, TemplateTypeKey>();
        }
        private ConcurrentDictionary<int, TemplateTypeKey> TemplateTemplateTypeMapping { get; set; }
        protected async Task<TemplateTypeKey> GetTemplateType(int templateId, CancellationToken token = default(CancellationToken))
        {
            TemplateTypeKey templateType;
            if(!this.TemplateTemplateTypeMapping.TryGetValue(templateId, out templateType))
            {
                var template = await this.Template.Get(templateId, token);
                if (template == null)
                    throw new GlobalizedValidationException("ADMIN_TEMPLATE_NOTFOUND");
                templateType = template.Result.TemplateType;
                this.TemplateTemplateTypeMapping.TryAdd(templateId, templateType);
            }
            return templateType;
        }
        public async Task Validate(ApplicationViewColumnResult column, CancellationToken token = default(CancellationToken))
        {
            if(column.DefaultSpec != null)
            {
                if ((column.DefaultSpec.CanFilter ?? false) && (column.DefaultSpec.Filter == null || column.DefaultSpec.Filter.FilterType == null || column.DefaultSpec.Filter.FilterType.FilterTypeId == null))
                    throw new GlobalizedValidationException("ADMIN_APPLICATIONVIEWCOLUMN_FILTER_INVALID");
                if (column.DefaultSpec.CellTemplate != null && column.DefaultSpec.CellTemplate.TemplateId != null &&
                    ((TemplateTypes)(await this.GetTemplateType(column.DefaultSpec.CellTemplate.TemplateId.Value, token)).TemplateTypeId.Value) != TemplateTypes.GridCell)
                    throw new GlobalizedValidationException("ADMIN_APPLICATIONVIEWCOLUMN_CELLTEMPLATE_TEMPLATETYPE_INVALID");
                if (column.DefaultSpec.ColumnTemplate != null && column.DefaultSpec.ColumnTemplate.TemplateId != null &&
                    ((TemplateTypes)(await this.GetTemplateType(column.DefaultSpec.ColumnTemplate.TemplateId.Value, token)).TemplateTypeId.Value) != TemplateTypes.GridColumn)
                    throw new GlobalizedValidationException("ADMIN_APPLICATIONVIEWCOLUMN_COLUMNTEMPLATE_TEMPLATETYPE_INVALID");
                if (column.DefaultSpec.HeaderTemplate != null && column.DefaultSpec.HeaderTemplate.TemplateId != null &&
                    ((TemplateTypes)(await this.GetTemplateType(column.DefaultSpec.HeaderTemplate.TemplateId.Value, token)).TemplateTypeId.Value) != TemplateTypes.GridHeader)
                    throw new GlobalizedValidationException("ADMIN_APPLICATIONVIEWCOLUMN_HEADERTEMPLATE_TEMPLATETYPE_INVALID");
                if (column.DefaultSpec.Filter != null && column.DefaultSpec.HeaderTemplate != null && column.DefaultSpec.HeaderTemplate.TemplateId != null &&
                    ((TemplateTypes)(await this.GetTemplateType(column.DefaultSpec.HeaderTemplate.TemplateId.Value, token)).TemplateTypeId.Value) != TemplateTypes.GridFilter)
                    throw new GlobalizedValidationException("ADMIN_APPLICATIONVIEWCOLUMN_FILTERTEMPLATE_TEMPLATETYPE_INVALID");
            }
        }
    }
}
