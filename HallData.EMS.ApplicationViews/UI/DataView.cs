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
    public class DataViewKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int? DataViewId { get; set; }

        [JsonIgnore]
        public int Key
        {
            get
            {
                return this.DataViewId ?? 0;
            }
            set
            {
                this.DataViewId = value;
            }
        }
    }
    public class DataViewName : DataViewKey
    {

        public virtual string Name { get; set; }
    }
    public class DataView<TDataViewColumn> : DataViewName
        where TDataViewColumn: DataViewColumnKey
    {
        public DataView()
        {
            this.Columns = new HashSet<TDataViewColumn>();
        }
        [UpdateOperationParameter]
        [AddOperationParameter]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }
        public ICollection<TDataViewColumn> Columns { get; set; }
    }
    public class DataViewBase : DataView<DataViewColumnKey> { }
    public class DataView : DataView<DataViewColumnBase> { }
}
