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
    public interface IApplicationViewColumnKey: IHasKey
    {
        int? ApplicationViewColumnId { get; set; }
    }
    public class ApplicationViewColumnKey : IApplicationViewColumnKey
    {
        [ChildKeyOperationParameter]
        [UpdateOperationParameter]
        public int? ApplicationViewColumnId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.ApplicationViewColumnId ?? 0;
            }
            set
            {
                this.ApplicationViewColumnId = value;
            }
        }
    }
    public interface IApplicationViewColumn : IApplicationViewColumnKey
    {
        ApplicationViewName ApplicationView { get; set; }
    }
    
    public class ApplicationViewColumn : ApplicationViewColumnKey, IApplicationViewColumn
    {
        [ChildView]
        [GlobalizedRequired]
        public ApplicationViewName ApplicationView { get; set; }
    }
    public interface IApplicationViewColumnWithData<TDataViewColumn> : IApplicationViewColumn
        where TDataViewColumn : DataViewColumnKey
    {
        TDataViewColumn DataViewColumn { get; set; }
    }
    public class ApplicationViewColumnWithData<TDataViewColumn> : ApplicationViewColumn, IApplicationViewColumnWithData<TDataViewColumn>
        where TDataViewColumn : DataViewColumnKey
    {
        [ChildView]
        [GlobalizedRequired]
        public TDataViewColumn DataViewColumn { get; set; }
    }
    public class ApplicationViewColumn<TTemplate, TFilter> : ApplicationViewColumn
        where TTemplate : TemplateKey
        where TFilter : FilterKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? IsVisisble { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? IsIncludedInResult { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? DisplayOrderIndex { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? CanSort { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? InitialSortDirectionId { get; set; }
        [JsonIgnore]
        public SortDirectionOptions? InitialSortDirectionOption
        {
            get { return this.InitialSortDirectionId == null ? null as SortDirectionOptions? : (SortDirectionOptions)this.InitialSortDirectionId.Value; }
        }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool? CanFilter { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public string HeaderText { get; set; }
        [ChildView]
        public TTemplate ColumnTemplate { get; set; }
        [ChildView]
        public TTemplate CellTemplate { get; set; }
        [ChildView]
        public TTemplate HeaderTemplate { get; set; }
        [ChildView]
        public TFilter Filter { get; set; }
    }
    public class ApplicationViewColumn<TTemplate, TFilter, TDataViewColumn> : ApplicationViewColumn<TTemplate, TFilter>, IApplicationViewColumnWithData<TDataViewColumn>
        where TTemplate: TemplateKey
        where TFilter: FilterKey
        where TDataViewColumn: DataViewColumnKey
    {
        [ChildView]
        [GlobalizedRequired]
        public TDataViewColumn DataViewColumn { get; set; }
    }
    public class ApplicationViewColumnBase<TTemplate, TFilter, TDataViewColumn> : ApplicationViewColumn<TTemplate, TFilter, TDataViewColumn>
        where TDataViewColumn: DataViewColumnKey
        where TTemplate: TemplateKey
        where TFilter: FilterKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool CanBeAddedToUI { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int? ConfigureOrderIndex { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public string RouteData { get; set; }
    }
    public class ApplicationViewColumnForParty : ApplicationViewColumn<TemplateKey, FilterBase> { }
    public class ApplicationViewColumnBase : ApplicationViewColumnBase<TemplateKey, FilterBase, DataViewColumnKey> { }
    public class ApplicationViewColumnResult : ApplicationViewColumnBase<Template, Filter, DataViewColumnResultName> { }
    public class ApplicationViewColumnWithData : ApplicationViewColumnWithData<DataViewColumnDisplayName> { }
}
