using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews.UI
{
    public class TemplateKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? TemplateId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.TemplateId ?? 0;
            }
            set
            {
                this.TemplateId = value;
            }
        }
    }
    public class Template<TTemplateType> : TemplateKey
        where TTemplateType: TemplateTypeKey
    {
        
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired]
        public string Name { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired]
        public string Code { get; set; }
        [ChildView]
        [GlobalizedRequired]
        public TTemplateType TemplateType { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool IsDefault { get; set; }
        
    }
    public class TemplateBase<TTemplateType, TChildTemplate, TDataViewColumn> : Template<TTemplateType>
        where TTemplateType: TemplateTypeKey
        where TChildTemplate: TemplateKey
        where TDataViewColumn: DataViewColumnKey
    {
        public TemplateBase()
        {
            this.ChildTemplates = new HashSet<TChildTemplate>();
            this.DataViewColumns = new HashSet<TDataViewColumn>();
        }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildViewCollection("ui.templatestabletype")]
        public ICollection<TChildTemplate> ChildTemplates { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildViewCollection("ui.dataviewcolumnstabletype")]
        public ICollection<TDataViewColumn> DataViewColumns { get; set; }
    }
    public class TemplateBase : TemplateBase<TemplateTypeKey, TemplateKey, DataViewColumnKey> { }
    public class Template : Template<TemplateType> { }
    public class TemplateResult : TemplateBase<TemplateType, Template, DataViewColumn> { }
}
