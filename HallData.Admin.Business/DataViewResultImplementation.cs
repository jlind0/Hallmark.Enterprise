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
    public class ReadOnlyDataViewResultImplementation : ReadOnlyBusinessRepositoryProxy<IReadOnlyDataViewResultRepository, DataViewResultResult>, IReadOnlyDataViewResultImplementation
    {
        public ReadOnlyDataViewResultImplementation(IReadOnlyDataViewResultRepository repository, ISecurityImplementation security) : base(repository, security) { }

        public Task<QueryResults<DataViewResultResult>> GetChildren(int dataViewResultId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetChildren(dataViewResultId, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByDataView(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataView(dataViewId, orderIndex, RecursionLevel.None, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByDataViewWithUpwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataView(dataViewId, orderIndex, RecursionLevel.Up, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByDataViewWithDownwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataView(dataViewId, orderIndex, RecursionLevel.Down, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByDataViewWithBiDirectionalRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataView(dataViewId, orderIndex, RecursionLevel.BiDirectional, viewName, userId, filter, sort, page, token), token);
        }


        public Task<QueryResults<DataViewResultResult>> GetByCollectionAttribute(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByCollectionAttribute(collectionInterfaceAttributeId, RecursionLevel.None, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByCollectionAttributeWithUpwardRecursion(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByCollectionAttribute(collectionInterfaceAttributeId, RecursionLevel.Up, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByCollectionAttributeWithDownwardRecursion(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByCollectionAttribute(collectionInterfaceAttributeId, RecursionLevel.Down, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByCollectionAttributeWithBiDirectionalRecursion(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByCollectionAttribute(collectionInterfaceAttributeId, RecursionLevel.BiDirectional, viewName, userId, filter, sort, page, token), token);
        }
    }
    public class DataViewResultImplementation : DeletableBusinessRepositoryProxyWithBase<IDataViewResultRepository, int, DataViewResultResult, DataViewResultForAdd, DataViewResultForUpdate>, IDataViewResultImplementation
    {
        protected IReadOnlyDataViewResultImplementation ReadOnly { get; private set; }
        protected IDataViewColumnPassThroughImplementation Columns { get; private set; }
        protected IReadOnlyDataViewImplementation DataView { get; private set; }
        protected IInterfaceImplementationBase Interface { get; private set; }
        protected IReadOnlyInterfaceImplementation ReadOnlyInterface { get; private set; }
        protected IReadOnlyInterfaceAttributeImplementation Attribute { get; private set; }
        protected virtual bool IsPassThrough { get { return false; } }
        public DataViewResultImplementation(IDataViewResultRepository repository, ISecurityImplementation security, IReadOnlyDataViewResultImplementation readOnly, 
            IDataViewColumnPassThroughImplementation columns, IReadOnlyDataViewImplementation dataView, IInterfaceImplementationBase interfaceBase, IReadOnlyInterfaceImplementation readOnlyInterface,
            IReadOnlyInterfaceAttributeImplementation attribute)
            : base(repository, security)
        {
            this.ReadOnly = readOnly;
            this.Columns = columns;
            this.DataView = dataView;
            this.Interface = interfaceBase;
            this.ReadOnlyInterface = readOnlyInterface;
            this.Attribute = attribute;
        }

        public Task<QueryResults<DataViewColumnResult>> GetColumns(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Columns.GetByDataViewResult(dataViewResultId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetColumnsWithUpwardRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Columns.GetByDataViewResultWithUpwardRecursion(dataViewResultId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetColumnsWithDownwardRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Columns.GetByDataViewResultWithDownwardRecursion(dataViewResultId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetColumnsWithBiDirectionalRecursion(int dataViewResultId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Columns.GetByDataViewResultWithBiDirectionalRecursion(dataViewResultId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetChildren(int dataViewResultId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetChildren(dataViewResultId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByDataView(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataView(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByDataViewWithUpwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataViewWithUpwardRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByDataViewWithDownwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataViewWithDownwardRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByDataViewWithBiDirectionalRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataViewWithBiDirectionalRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }
        protected override Task<ChangeStatusResult> ChangeStatus(int id, string statusTypeName, bool force, Guid? userId, CancellationToken token)
        {
            throw new NotImplementedException();
        }
        public override Task<QueryResult<DataViewResultResult>> Get(int id, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.Get(id, token);
        }
        public override Task<QueryResults<DataViewResultResult>> GetMany(string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.GetMany(viewName, filter, sort, page, token);
        }
        public override Task<QueryResult<JObject>> GetView(int id, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.GetView(id, token);
        }
        public override Task<QueryResults<JObject>> GetManyView(string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.GetManyView(viewName, filter, sort, page, token);
        }
        protected async override Task Add(DataViewResultForAdd view, Guid? userId, CancellationToken token)
        {
            if(!this.IsPassThrough)
            {
                view.Validate();
                var dv = await this.DataView.Get(view.DataView.DataViewId.Value, token);
                if (dv == null)
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_ADD_DATAVIEW_NOTFOUND");
                var results = (await this.GetByDataViewWithBiDirectionalRecursion(view.DataViewResultId.Value, view.ResultIndex.Value, token: token)).Results.ToList();
                if (results.Any(r => r.ResultIndex == view.ResultIndex && r.DataView.DataViewId == view.DataView.DataViewId))
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_ADD_DUPLICATE");
                results.Add(view.CreateRelatedInstance<DataViewResultResult>());
                if (!await this.DoDataViewResultsCompose(results, token))
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_ADD_COMPOSITION_INVALID");
                if(view.DataViewColumns != null && view.DataViewColumns.Count > 0)
                {
                    foreach (var col in view.DataViewColumns)
                    {
                        if (col.DataViewColumnPaths != null && col.DataViewColumnPaths.Select(v => v.PathOrderIndex.Value).Distinct().Count() != col.DataViewColumnPaths.Count)
                            throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_ADD_COLUMNS_DATAVIEWCOLUMNPATH_DUPLICATE");
                    }
                    var columns = (await this.GetColumnsWithBiDirectionalRecursion(view.DataView.DataViewId.Value, token: token)).Results.ToList();
                    if(columns.Count > 0)
                    {
                        columns.AddRange(view.DataViewColumns.Select(dc => dc.CreateRelatedInstance<DataViewColumnResult>()));
                        if (!await this.Columns.DoColumnsCompose(columns, token))
                            throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_ADD_COLUMNS_COMPOSITION_INVALID");
                    }
                }
            }
            await base.Add(view, userId, token);
            if(view.DataViewColumns != null)
            {
                foreach(var col in view.DataViewColumns)
                {
                    var c = col.CreateRelatedInstance<DataViewColumnForAdd>();
                    c.DataViewResult = new DataViewResultKey();
                    c.DataViewResult.DataViewResultId = view.DataViewResultId;
                    await this.Columns.Add(c, token);
                }
            }
        }
        protected async override Task Update(DataViewResultForUpdate view, Guid? userId, CancellationToken token)
        {
            if(!this.IsPassThrough)
            {
                view.Validate();
                var dr = await this.Get(view.DataViewResultId.Value, token);
                if (dr == null)
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_UPDATE_NOTEXIST");
                var results = (await this.GetByDataViewWithBiDirectionalRecursion(dr.Result.DataViewResultId.Value, view.ResultIndex.Value, token: token)).Results.ToList();
                if (results.Any(d => d.DataViewResultId != view.DataViewResultId && d.ResultIndex == view.ResultIndex && d.DataView.DataViewId == dr.Result.DataView.DataViewId))
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_UPDATE_DUPLICATE");
                var r = view.CreateRelatedInstance<DataViewResultResult>();
                r.DataView = new DataViewResult();
                r.DataView.DataViewId = dr.Result.DataView.DataViewId;
                results.RemoveAll(d => d.DataViewResultId == view.DataViewResultId);
                results.Add(r);
                if (!await this.DoDataViewResultsCompose(results, token))
                    throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_UPDATE_COMPOSITION_INVALID");
                if(view.DataViewColumns != null)
                {
                    foreach(var col in view.DataViewColumns.Update)
                    {
                        var oCol = await this.Columns.Get(col.DataViewColumnId.Value, token);
                        if (oCol == null)
                            throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_UPDATE_COLUMNS_UPDATE_NOTFOUND");
                        if (col.DataViewColumnPaths != null)
                        {
                            List<DataViewColumnPathResult> paths = new List<DataViewColumnPathResult>();
                            if (oCol.Result.DataViewColumnPaths != null)
                                paths.AddRange(oCol.Result.DataViewColumnPaths);
                            paths.RemoveAll(p => col.DataViewColumnPaths.Update.Any(pp => p.PathOrderIndex == pp.PathOrderIndex));
                            paths.RemoveAll(p => col.DataViewColumnPaths.HardDelete.Union(col.DataViewColumnPaths.SoftDelete).Any(pp => p.PathOrderIndex == pp.PathOrderIndex));
                            paths.AddRange(col.DataViewColumnPaths.Add.Union(col.DataViewColumnPaths.Update).Select(p => p.CreateRelatedInstance<DataViewColumnPathResult>()));
                            if(!await this.Columns.AreColumnPathsValid(paths, col.InterfaceAttribute.InterfaceAttributeId.Value, token))
                                throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_UPDATE_COLUMNS_UPDATE_PATH_INVALID");
                        }
                    }
                    foreach(var col in view.DataViewColumns.Add.Where(c => c.DataViewColumnPaths != null && c.DataViewColumnPaths.Count > 0))
                    {
                        if (!await this.Columns.AreColumnPathsValid(col.DataViewColumnPaths.Select(p => p.CreateRelatedInstance<DataViewColumnPathResult>()), col.InterfaceAttribute.InterfaceAttributeId.Value, token))
                            throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_UPDATE_COLUMNS_ADD_PATH_INVALID");
                    }
                    var columns = (await this.GetColumnsWithBiDirectionalRecursion(view.DataViewResultId.Value, token: token)).Results.ToList();
                    columns.RemoveAll(c => view.DataViewColumns.Update.Any(cc => c.DataViewColumnId == cc.DataViewColumnId));
                    columns.RemoveAll(c => view.DataViewColumns.HardDelete.Union(view.DataViewColumns.SoftDelete).Any(cc => c.DataViewColumnId == cc.DataViewColumnId));
                    columns.AddRange(view.DataViewColumns.Update.Select(c => c.CreateRelatedInstance<DataViewColumnResult>()));
                    columns.AddRange(view.DataViewColumns.Add.Select(c => c.CreateRelatedInstance<DataViewColumnResult>()));
                    if (!await this.Columns.DoColumnsCompose(columns, token))
                        throw new GlobalizedValidationException("ADMIN_DATAVIEWRESULT_UPDATE_COLUMNS_COMPOSITION_INVALID");
                    
                    
                }

            }
            await base.Update(view, userId, token);
            if(view.DataViewColumns != null)
            {
                foreach(var col in view.DataViewColumns.Add)
                {
                    var c = col.CreateRelatedInstance<DataViewColumnForAdd>();
                    c.DataViewResult = new DataViewResultKey();
                    c.DataViewResult.DataViewResultId = view.DataViewResultId;
                    await this.Columns.Add(c, token);
                }
                foreach(var col in view.DataViewColumns.Update)
                {
                    await this.Columns.Update(col, token);
                }
                foreach(var col in view.DataViewColumns.HardDelete)
                {
                    await this.Columns.DeleteHard(col.DataViewColumnId.Value, token);
                }
                foreach(var col in view.DataViewColumns.SoftDelete)
                {
                    await this.Columns.DeleteSoft(col.DataViewColumnId.Value, token);
                }
            }
        }


        public async Task<bool> DoDataViewResultsCompose(IEnumerable<DataViewResultResult> results, CancellationToken token = default(CancellationToken))
        {

            foreach (var composed in DataViewResultHelper.GetComposedResults(results))
            {
                var interfaces = composed.Select(c => c.Interface.InterfaceId.Value).Distinct();
                if (!await this.ReadOnlyInterface.AreInterfacesCommon(interfaces, token) || !await this.Interface.CanInterfacesRelate(interfaces, token))
                    return false;
                var collections = composed.Select(c => c.CollectionInterfaceAttribute != null ? c.CollectionInterfaceAttribute.InterfaceAttributeId : null as int?).Distinct();
                if (collections.Count() > 1)
                {
                    if (collections.Any(c => c != null) && collections.Any(c => c == null))
                        return false;
                    if (collections.All(c => c != null))
                    {
                        List<InterfaceAttributeResult> collectionAttrs = new List<InterfaceAttributeResult>();
                        foreach (var col in collections)
                        {
                            var attr = await this.Attribute.Get(col.Value, token);
                            if (attr == null)
                                return false;
                            collectionAttrs.Add(attr.Result);
                        }
                        if (collectionAttrs.GroupBy(g => g.Name.ToLowerInvariant()).Count() != 1 || !await this.ReadOnlyInterface.AreInterfacesCommon(collectionAttrs.Select(c => c.Interface.InterfaceId.Value), token))
                            return false;
                    }
                }
            }
            return true;
        }


        public Task<QueryResults<DataViewResultResult>> GetByCollectionAttribute(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByCollectionAttribute(collectionInterfaceAttributeId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByCollectionAttributeWithUpwardRecursion(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByCollectionAttributeWithUpwardRecursion(collectionInterfaceAttributeId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByCollectionAttributeWithDownwardRecursion(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByCollectionAttributeWithDownwardRecursion(collectionInterfaceAttributeId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetByCollectionAttributeWithBiDirectionalRecursion(int collectionInterfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByCollectionAttributeWithBiDirectionalRecursion(collectionInterfaceAttributeId, viewName, filter, sort, page, token);
        }
    }
    public class DataViewResultPassThroughImplementation : DataViewResultImplementation, IDataViewResultPassThroughImplementation
    {
        public DataViewResultPassThroughImplementation(IDataViewResultRepository repository, ISecurityImplementation security, IReadOnlyDataViewResultImplementation readOnly, 
            IDataViewColumnPassThroughImplementation columns, IReadOnlyDataViewImplementation dataView, IInterfaceImplementationBase interfaceBase, IReadOnlyInterfaceImplementation readOnlyInterface,
            IReadOnlyInterfaceAttributeImplementation attribute)
            :base(repository, security, readOnly, columns, dataView, interfaceBase, readOnlyInterface, attribute)
        {

        }
        protected override bool IsPassThrough
        {
            get
            {
                return true;
            }
        }
    }
}
