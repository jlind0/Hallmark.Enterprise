using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using Newtonsoft.Json;
using HallData.Validation;

namespace HallData.EMS.ApplicationViews.UI
{
    public class DataViewColumnKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? DataViewColumnId { get; set; }
        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.DataViewColumnId ?? 0;
            }
            set
            {
                this.DataViewColumnId = value;
            }
        }
    }
    public class DataViewColumnDisplayName : DataViewColumnKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired]
        public string DisplayName { get; set; }
    }
    public class DataViewColumnResultName : DataViewColumnDisplayName
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired]
        public string ResultName { get; set; }
    }
    public class DataViewColumn<TDataView> : DataViewColumnResultName
        where TDataView: DataViewKey
    {
        [ChildView]
        [GlobalizedRequired]
        public TDataView DataView { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int ResultSetIndex { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public bool IsRequired { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [GlobalizedRequired]
        public string Name { get; set; }
        
    }
    public class DataViewColumnBase : DataViewColumn<DataViewKey> { }
    public class DataViewColumn : DataViewColumn<DataViewName> { }

}
