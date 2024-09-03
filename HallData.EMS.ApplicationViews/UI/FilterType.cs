using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews.UI
{
    public class FilterTypeKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? FilterTypeId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.FilterTypeId ?? 0;
            }
            set
            {
                this.FilterTypeId = value;
            }
        }
    }
    public class FilterType<TTemplate, TFilterOperationOption> : FilterTypeKey
        where TTemplate: TemplateKey
        where TFilterOperationOption: FilterOperationOptionKey
    {
        public FilterType()
        {
            this.FilterOperationOptions = new HashSet<TFilterOperationOption>();
        }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public string Name { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public string DataType { get; set; }
        [ChildView]
        public TTemplate Template { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildViewCollection("ui.filtertypeoptionstabletype")]
        public ICollection<TFilterOperationOption> FilterOperationOptions { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool IsDefault { get; set; }
    }
    public class FilterType : FilterType<Template, FilterOperationOption> { }
    public class FilterTypeWithColumns<TTemplate, TFilterOperationOption, TDataViewColumn> : FilterType<TTemplate, TFilterOperationOption>
        where TTemplate : TemplateKey
        where TFilterOperationOption : FilterOperationOptionKey
        where TDataViewColumn: DataViewColumnKey
    {
        public FilterTypeWithColumns()
        {
            this.AvailableColumns = new HashSet<TDataViewColumn>();
        }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildViewCollection("ui.dataviewcolumnstabletype")]
        public ICollection<TDataViewColumn> AvailableColumns { get; set; }
    }
    public class FilterTypeBase : FilterTypeWithColumns<TemplateKey, FilterOperationOptionBase, DataViewColumnKey> { }
    public class FilterTypeWithColumns : FilterTypeWithColumns<Template, FilterOperationOption, DataViewColumn> { }
}
