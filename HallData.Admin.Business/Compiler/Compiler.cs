using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.Admin.Business;
using System.Threading;
using System.Collections.Concurrent;

namespace HallData.Admin.Compiler
{
    public interface ICompiler : IDisposable
    {
        Task CompileByDataViewColumn(int dataViewColumnId);
        Task CompileByInterfaceAttribute(int interfaceAttributeId);
        Task CompileByDataViewResult(int dataViewResultId);
    }
    public static class CompilerFactory
    {
        private static Func<ICompiler> Factory { get; set; }
        public static void Initialize(Func<ICompiler> factory)
        {
            Factory = factory;
        }
        public static ICompiler Create()
        {
            return Factory();
        }
    }
    public class Compiler : ICompiler
    {
        protected IDataViewColumnPassThroughImplementation Column { get; private set; }
        protected IReadOnlyInterfaceAttributeImplementation Attribute { get; private set; }
        protected IReadOnlyDataViewResultImplementation Result { get; private set; }
        private ConcurrentDictionary<int, bool> UpdatedColumns { get; set; }
        protected ConcurrentStack<DataViewColumnResult> ColumnStack { get; set; }
        protected CompilerCache Cache { get; private set; }
        public Compiler(IDataViewColumnPassThroughImplementation column, IReadOnlyInterfaceAttributeImplementation attribute, IReadOnlyDataViewResultImplementation result)
        {
            this.Column = column;
            this.Attribute = attribute;
            this.Result = result;
            this.UpdatedColumns = new ConcurrentDictionary<int, bool>();
            this.ColumnStack = new ConcurrentStack<DataViewColumnResult>();
            this.Cache = new CompilerCache(column, attribute, result);
        }
        
        protected bool IsColumnUpdated(int dataViewColumnId)
        {
            return this.UpdatedColumns.ContainsKey(dataViewColumnId);
        }

        public Task CompileByDataViewColumn(int dataViewColumnId)
        {
            throw new NotImplementedException();
        }

        public Task CompileByInterfaceAttribute(int interfaceAttributeId)
        {
            throw new NotImplementedException();
        }

        public Task CompileByDataViewResult(int dataViewResultId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
