using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using HallData.EMS.ApplicationViews.UI.Enums;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews.UI
{
    public class FilterKey : IHasKey
    {
        [ChildKeyOperationParameter]
        [UpdateOperationParameter]
        public int? FilterId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.FilterId ?? 0;
            }
            set
            {
                this.FilterId = value;
            }
        }
    }
    public class Filter<TFilterType, TFilterColumn, TFilterOperationOption, TTemplate> : FilterKey
        where TFilterType: FilterTypeKey
        where TFilterColumn: ApplicationViewColumnKey
        where TFilterOperationOption: FilterOperationOptionKey
        where TTemplate: TemplateKey
    {
        public Filter()
        {
            FilterOperationOptions = new HashSet<TFilterOperationOption>();
        }
        [ChildView]
        [GlobalizedRequired]
        public TFilterType FilterType { get; set; }
        [ChildView]
        public TFilterColumn FilterColumn { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildViewCollection("ui.filtertypeoptionstabletype")]
        public ICollection<TFilterOperationOption> FilterOperationOptions { get; set; }
        [UpdateOperationParameter]
        [AddOperationParameter]
        public int? DelayFilter { get; set; }
        [ChildView]
        public TTemplate Template { get; set; }
        
    }
    public class FilterBase : Filter<FilterTypeKey, ApplicationViewColumnKey, FilterOperationOptionBase, TemplateKey>
    {
    }
    public class Filter : Filter<FilterType, ApplicationViewColumnResult, FilterOperationOption, Template> { }
}
