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
    public class ApplicationViewKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? ApplicationViewId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.ApplicationViewId ?? 0;
            }
            set
            {
                this.ApplicationViewId = value;
            }
        }
    }
    public class ApplicationViewName : ApplicationViewKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired]
        public string Name { get; set; }
    }
    public class ApplicationView<TApplicationViewColumn, TTemplate> : ApplicationViewName
        where TApplicationViewColumn: ApplicationViewColumnKey
        where TTemplate: TemplateKey
    {
        public ApplicationView()
        {
            this.Columns = new HashSet<TApplicationViewColumn>();
            this.PageOptions = new HashSet<PageOptionHolder>();
        }
        
        public ICollection<TApplicationViewColumn> Columns { get; set; }
        [ChildView]
        public TTemplate GridTemplate { get; set; }
        [ChildView]
        public TTemplate PagerTemplate { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? HasSearchCriteria { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? SearchCriteriaDelay { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? InitialPage { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? InitialPageSize { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? PageDisplayCount { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? HasPaging { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? CanSaveSettings { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? SortSingleColumnMode { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? CanPickColumns { get; set; }
        public bool? CanReorderColumns { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildViewCollection("ui.pageoptionstabletype")]
        public ICollection<PageOptionHolder> PageOptions { get; set; }
    }
    public class ApplicationView<TApplicationViewColumn, TTemplate, TDataView> : ApplicationView<TApplicationViewColumn, TTemplate>
        where TApplicationViewColumn : ApplicationViewColumnKey
        where TTemplate : TemplateKey
        where TDataView: DataViewKey
    {
        [ChildView]
        [GlobalizedRequired]
        public TDataView DataView { get; set; }
    }
    public class ApplicationViewBase : ApplicationView<ApplicationViewColumnBase, TemplateKey, DataViewKey> { }
    public class ApplicationViewForParty : ApplicationView<ApplicationViewColumnForParty, TemplateKey> { }
    public class ApplicationViewResult : ApplicationView<ApplicationViewColumnResult, Template, DataViewName> { }

}
