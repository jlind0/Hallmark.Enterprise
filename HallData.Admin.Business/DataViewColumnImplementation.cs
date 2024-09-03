using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.ApplicationViews;
using HallData.Business;
using HallData.Security;
using System.Threading;
using HallData.Admin.Data;
using Newtonsoft.Json.Linq;
using HallData.Validation;
using HallData.Exceptions;
using HallData.Utilities;

namespace HallData.Admin.Business
{
    public class ReadOnlyDataViewColumnImplementation : ReadOnlyBusinessRepositoryProxy<IReadOnlyDataViewColumnRepository, DataViewColumnResult>, IReadOnlyDataViewColumnImplementation
    {
        public ReadOnlyDataViewColumnImplementation(IReadOnlyDataViewColumnRepository repository, ISecurityImplementation security) : base(repository, security) { }

        public Task<QueryResults<DataViewColumnResult>> GetByDataView(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataView(dataViewId, orderIndex, RecursionLevel.None, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewResult(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataViewResult(dataViewResultId, RecursionLevel.None, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewWithUpwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataView(dataViewId, orderIndex, RecursionLevel.Up, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewResultWithUpwardRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataViewResult(dataViewResultId, RecursionLevel.Up, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewWithDownwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataView(dataViewId, orderIndex, RecursionLevel.Down, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewResultWithDownwardRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataViewResult(dataViewResultId, RecursionLevel.Down, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewWithBiDirectionalRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataView(dataViewId, orderIndex, RecursionLevel.BiDirectional, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewResultWithBiDirectionalRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataViewResult(dataViewResultId, RecursionLevel.BiDirectional, viewName, userId, filter, sort, page, token), token);
        }


        public Task<QueryResults<DataViewColumnResult>> GetChildren(int dataViewColumnId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByParent(dataViewColumnId, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByInterfaceAttribute(int interfaceAttributeId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByInterfaceAttribute(interfaceAttributeId, viewName, userId, filter, sort, page), token);
        }
    }
    public class DataViewColumnImplementation : DeletableBusinessRepositoryProxyWithBase<IDataViewColumnRepository, int, DataViewColumnResult, DataViewColumnForAdd, DataViewColumnForUpdate>, IDataViewColumnImplementation
    {
        protected IReadOnlyDataViewColumnImplementation ReadOnly { get; private set; }
        protected virtual bool IsPassThrough { get { return false; } }
        protected IInterfaceImplementationBase Interface { get; private set; }
        protected IReadOnlyInterfaceAttributeImplementation Attribute { get; private set; }
        protected IReadOnlyInterfaceImplementation ReadOnlyInterface { get; private set; }
        protected IDataViewColumnPassThroughImplementation PassThrough { get; set; }
        public DataViewColumnImplementation(IDataViewColumnRepository repository, ISecurityImplementation security, IReadOnlyDataViewColumnImplementation readOnly, IInterfaceImplementationBase interfaceImpl,
            IReadOnlyInterfaceImplementation readOnlyInterface, IReadOnlyInterfaceAttributeImplementation attribute, IDataViewColumnPassThroughImplementation passThrough)
            : this(repository, security, readOnly, interfaceImpl, readOnlyInterface, attribute) 
        {
            this.PassThrough = passThrough;
        }
        protected DataViewColumnImplementation(IDataViewColumnRepository repository, ISecurityImplementation security, IReadOnlyDataViewColumnImplementation readOnly, IInterfaceImplementationBase interfaceImpl,
            IReadOnlyInterfaceImplementation readOnlyInterface, IReadOnlyInterfaceAttributeImplementation attribute)
            : base(repository, security)
        {
            this.ReadOnly = readOnly;
            this.Interface = interfaceImpl;
            this.Attribute = attribute;
            this.ReadOnlyInterface = readOnlyInterface;
        }
        public override Task<QueryResult<DataViewColumnResult>> Get(int id, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.Get(id, token);
        }
        public override Task<QueryResults<DataViewColumnResult>> GetMany(string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.GetMany(viewName, filter, sort, page, token);
        }
        public override Task<QueryResults<JObject>> GetManyView(string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.GetManyView(viewName, filter, sort, page, token);
        }
        public override Task<QueryResult<JObject>> GetView(int id, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.GetView(id, token);
        }

        public async Task<QueryResult<DataViewColumnResult>> AddPath(DataViewColumnPathForAddUpdate path, CancellationToken token = default(CancellationToken))
        {
            if(!this.IsPassThrough)
            {
                path.Validate();
                var column = await this.Get(path.DataViewColumnId.Value, token);
                if (column == null)
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWCOLUMNPATH_ADD_NOTFOUND");
                if (column.Result.DataViewColumnPaths != null && !column.Result.DataViewColumnPaths.All(r => r.PathOrderIndex == path.PathOrderIndex))
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWCOLUMNPATH_ADD_DUPLICATE");
            }
            return await ExecuteAction(userId => this.Repository.AddPath(path, userId, token), userId => this.Get(path.DataViewColumnId.Value, token), token);
        }

        public async Task<QueryResult<DataViewColumnResult>> UpdatePath(DataViewColumnPathForAddUpdate path, CancellationToken token = default(CancellationToken))
        {
            if(!this.IsPassThrough)
            {
                path.Validate();
                var column = await this.Get(path.DataViewColumnId.Value, token);
                if (column == null || column.Result.DataViewColumnPaths == null || !column.Result.DataViewColumnPaths.Any(c => c.PathOrderIndex == path.PathOrderIndex))
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWCOLUMNPATH_UPDATE_NOTFOUND");
            }
            return await ExecuteAction(userId => this.Repository.UpdatePath(path, userId, token), userId => this.Get(path.DataViewColumnId.Value, token), token);
        }

        public Task<QueryResult<DataViewColumnResult>> RemovePath(int dataViewColumnId, int? orderIndex = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteAction(userId => this.Repository.RemovePath(dataViewColumnId, orderIndex, userId, token), userId => this.Get(dataViewColumnId, token), token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataView(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataView(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewResult(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataViewResult(dataViewResultId, viewName, filter, sort, page, token);
        }

        protected override async Task Delete(int id, Guid? userId, bool isHard, CancellationToken token)
        {
            await this.RemovePath(id, token: token);
            await base.Delete(id, userId, isHard, token);
        }
        protected override async Task Add(DataViewColumnForAdd view, Guid? userId, CancellationToken token)
        {
            if(!this.IsPassThrough)
            {
                view.Validate();
                if(view.DataViewColumnPaths != null && !await this.AreColumnPathsValid(view.DataViewColumnPaths.Select(dp => dp.CreateRelatedInstance<DataViewColumnPathResult>()), view.InterfaceAttribute.InterfaceAttributeId.Value, token))
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWCOLUMNPATH_ADD_DUPLICATE");
                var columnsResult = await this.GetByDataViewResultWithBiDirectionalRecursion(view.DataViewResult.DataViewResultId.Value, token: token);
                List<DataViewColumnResult> columns = new List<DataViewColumnResult>(columnsResult.Results);
                columns.Add(view.CreateRelatedInstance<DataViewColumnResult>());
                if (!await this.DoColumnsCompose(columns, token))
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWCOLUMN_ADD_INVALID_COMPOSITION");
            }
            await base.Add(view, userId, token);
            if(view.DataViewColumnPaths != null)
            {
                foreach (var path in view.DataViewColumnPaths)
                {
                    var uPath = path.CreateRelatedInstance<DataViewColumnPathForAddUpdate>();
                    uPath.DataViewColumnId = view.DataViewColumnId;
                    await PassThrough.AddPath(uPath, token);
                }
            }
        }
        protected override async Task Update(DataViewColumnForUpdate view, Guid? userId, CancellationToken token)
        {
            if (!this.IsPassThrough)
            {
                view.Validate();
                var oldColumn = await this.Get(view.DataViewColumnId.Value, token);
                if (oldColumn == null)
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWCOLUMN_UPDATE_NOTFOUND");
                if (view.DataViewColumnPaths != null)
                {
                    List<DataViewColumnPathKey> paths = new List<DataViewColumnPathKey>();
                    if (oldColumn.Result.DataViewColumnPaths != null)
                        paths.AddRange(oldColumn.Result.DataViewColumnPaths);
                    paths.RemoveAll(p => view.DataViewColumnPaths.HardDelete.Any(u => u.PathOrderIndex == p.PathOrderIndex) 
                        || view.DataViewColumnPaths.SoftDelete.Any(u => u.PathOrderIndex == p.PathOrderIndex));
                    if (!view.DataViewColumnPaths.Update.All(u => paths.Any(p => p.PathOrderIndex == u.PathOrderIndex)))
                        throw new GlobalizedValidationException("ADMIN_DATAVIEWCOLUMNPATH_UPDATE_NOTFOUND");
                    paths.AddRange(view.DataViewColumnPaths.Add);
                    if (!await this.AreColumnPathsValid(paths.Select(p => p.CreateRelatedInstance<DataViewColumnPathResult>()), view.InterfaceAttribute.InterfaceAttributeId.Value, token))
                        throw new GlobalizedValidationException("ADMIN_DATAVIEWCOLUMNPATH_ADD_DUPLICATE");
                }
                var columnsResult = await this.GetByDataViewResultWithBiDirectionalRecursion(oldColumn.Result.DataViewResult.DataViewResultId.Value, token: token);
                List<DataViewColumnResult> columns = new List<DataViewColumnResult>(columnsResult.Results);
                columns.RemoveAll(c => c.DataViewColumnId == view.DataViewColumnId);
                columns.Add(view.CreateRelatedInstance<DataViewColumnResult>());
                if (!await this.DoColumnsCompose(columns, token))
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWCOLUMN_UPDATE_INVALID_COMPOSITION");
            }
            await base.Update(view, userId, token);
            if(view.DataViewColumnPaths != null)
            {
                foreach (var path in view.DataViewColumnPaths.HardDelete.Union(view.DataViewColumnPaths.SoftDelete))
                    await PassThrough.RemovePath(view.DataViewColumnId.Value, path.DataViewColumnId.Value);
                foreach(var path in view.DataViewColumnPaths.Add)
                {
                    var p = path.CreateRelatedInstance<DataViewColumnPathForAddUpdate>();
                    p.DataViewColumnId = view.DataViewColumnId;
                    await PassThrough.AddPath(p, token);
                }
                foreach(var path in view.DataViewColumnPaths.Update)
                {
                    var p = path.CreateRelatedInstance<DataViewColumnPathForAddUpdate>();
                    p.DataViewColumnId = view.DataViewColumnId;
                    await PassThrough.UpdatePath(p, token);
                }
            }
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewWithUpwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataViewWithUpwardRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewResultWithUpwardRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataViewResultWithUpwardRecursion(dataViewResultId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewWithDownwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataViewWithDownwardRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewResultWithDownwardRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataViewResultWithDownwardRecursion(dataViewResultId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewWithBiDirectionalRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataViewWithBiDirectionalRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByDataViewResultWithBiDirectionalRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataViewResultWithBiDirectionalRecursion(dataViewResultId, viewName, filter, sort, page, token);
        }


        public async Task<bool> DoColumnsCompose(IEnumerable<DataViewColumnResult> columns, CancellationToken token = default(CancellationToken))
        {
            IEnumerable<IGrouping<DataViewColumnHelper.ColumnCompositionKey, DataViewColumnResult>> attributeComposed;
            if (!DataViewColumnHelper.DoColumnsCompose(columns, out attributeComposed))
                return false;
            foreach(var attributeGroup in attributeComposed)
            {
                List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();
                foreach (var attr in attributeGroup)
                    attributes.Add((await this.Attribute.Get(attr.InterfaceAttribute.InterfaceAttributeId.Value, token)).Result);
                if (attributes.Select(a => a.Name).Distinct().Count() != 1)
                    return false;
                if (!await this.ReadOnlyInterface.AreInterfacesCommon(attributes.Select(a => a.Interface.InterfaceId.Value).Distinct(), token))
                    return false;
                
            }
            return true;
        }


        public Task<bool> AreColumnPathsValid(IEnumerable<DataViewColumnPathResult> paths, int interfaceAttributeId, CancellationToken token = default(CancellationToken))
        {
            //TODO: verify column paths step
            return Task.FromResult(paths.Select(v => v.PathOrderIndex.Value).Distinct().Count() == paths.Count());
        }


        public Task<QueryResults<DataViewColumnResult>> GetChildren(int dataViewColumnId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetChildren(dataViewColumnId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetByInterfaceAttribute(int interfaceAttributeId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByInterfaceAttribute(interfaceAttributeId, viewName, filter, sort, page, token);
        }
    }
    public class DataViewColumnPassThroughImplementation : DataViewColumnImplementation, IDataViewColumnPassThroughImplementation
    {
        protected override bool IsPassThrough
        {
            get
            {
                return true;
            }
        }
        public DataViewColumnPassThroughImplementation(IDataViewColumnRepository repository, ISecurityImplementation security, IReadOnlyDataViewColumnImplementation readOnly, IInterfaceImplementationBase interfaceImpl,
            IReadOnlyInterfaceImplementation readOnlyInterface, IReadOnlyInterfaceAttributeImplementation attribute)
            : base(repository, security, readOnly, interfaceImpl, readOnlyInterface, attribute) 
        {
            this.PassThrough = this;
        }
    }
}
