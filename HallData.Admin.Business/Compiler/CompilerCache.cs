using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.Business;
using HallData.Admin.ApplicationViews;
using System.Threading;
using System.Collections.Concurrent;
using HallData.ApplicationViews;

namespace HallData.Admin.Compiler
{
    public class CompilerCache
    {
        protected IReadOnlyDataViewColumnImplementation Column { get; private set; }
        protected IReadOnlyInterfaceAttributeImplementation Attribute { get; private set; }
        protected IReadOnlyDataViewResultImplementation Result { get; private set; }
        private ConcurrentDictionary<int, InterfaceAttributeResult> Attributes { get; set; }
        private ConcurrentDictionary<int, DataViewColumnResult> Columns { get; set; }
        private ConcurrentDictionary<int, DataViewResultResult> Results { get; set; }
        private ConcurrentDictionary<int, DataViewColumnResult[]> KeyColumns { get; set; }
        public CompilerCache(IReadOnlyDataViewColumnImplementation column, IReadOnlyInterfaceAttributeImplementation attribute, IReadOnlyDataViewResultImplementation result)
        {
            this.Column = column;
            this.Attribute = attribute;
            this.Result = result;
            this.Attributes = new ConcurrentDictionary<int, InterfaceAttributeResult>();
            this.Columns = new ConcurrentDictionary<int, DataViewColumnResult>();
            this.Results = new ConcurrentDictionary<int, DataViewResultResult>();
            this.KeyColumns = new ConcurrentDictionary<int, DataViewColumnResult[]>();
        }
        public async Task<InterfaceAttributeResult> GetAttribute(int interfaceAttributeId, CancellationToken token)
        {
            InterfaceAttributeResult attribute;
            if (!this.Attributes.TryGetValue(interfaceAttributeId, out attribute))
            {
                attribute = (await this.Attribute.Get(interfaceAttributeId, token)).Result;
                this.Attributes.TryAdd(interfaceAttributeId, attribute);
            }
            return attribute;
        }

        public async Task<DataViewColumnResult> GetColumn(int dataViewColumnId, CancellationToken token)
        {
            DataViewColumnResult column;
            if (!this.Columns.TryGetValue(dataViewColumnId, out column))
            {
                column = (await this.Column.Get(dataViewColumnId, token)).Result;
                this.Columns.TryAdd(dataViewColumnId, column);
            }
            return column;
        }
        public async Task<IEnumerable<DataViewColumnResult>> GetKeyColumns(int dataViewResultId, CancellationToken token)
        {
            DataViewColumnResult[] columns;
            if (!this.KeyColumns.TryGetValue(dataViewResultId, out columns))
            {
                columns = (await this.Column.GetByDataViewResultWithDownwardRecursion(dataViewResultId, 
                    filter: FilterContext<DataViewColumnResult>.CreateContext().Like(c => c.ResultName, "%#"), token: token)).Results.ToArray();
                this.KeyColumns.TryAdd(dataViewResultId, columns);
            }
            return columns;
        }
        public async Task<DataViewResultResult> GetResult(int dataViewResultId, CancellationToken token)
        {
            DataViewResultResult result;
            if (!this.Results.TryGetValue(dataViewResultId, out result))
            {
                result = (await this.Result.Get(dataViewResultId, token)).Result;
                this.Results.TryAdd(dataViewResultId, result);
            }
            return result;
        }
    }
}
