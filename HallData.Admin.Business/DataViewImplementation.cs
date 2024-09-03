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
    public class ReadOnlyDataViewImplementation : ReadOnlyBusinessRepositoryProxy<IReadOnlyDataViewRepository, DataViewResult>, IReadOnlyDataViewImplementation
    {
        public ReadOnlyDataViewImplementation(IReadOnlyDataViewRepository repository, ISecurityImplementation security) : base(repository, security) { }


        public Task<QueryResult<DataViewResult>> GetByName(string name, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByName(name, userId, token), token);
        }

        public async Task<bool> DoesNameExist(string name, CancellationToken token = default(CancellationToken))
        {
            return (await this.GetByName(name, token)) != null;
        }

        public Task<bool> AreDataViewsCommon(int dataViewId1, int dataViewId2, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.AreDataViewsCommon(dataViewId1, dataViewId2, userId, token), token);
        }

        public async Task<bool> AreDataViewsCommon(IEnumerable<int> dataViewIds, CancellationToken token = default(CancellationToken))
        {
           List<Tuple<int, int>> procesedTypes = new List<Tuple<int, int>>();
           foreach (var type1 in dataViewIds)
            {
                foreach (var type2 in dataViewIds.Where(t => t != type1))
                {
                    if (procesedTypes.Contains(new Tuple<int, int>(type1, type2)) || procesedTypes.Contains(new Tuple<int, int>(type2, type1)))
                        continue;
                    procesedTypes.Add(new Tuple<int, int>(type1, type2));
                    if (!await this.AreDataViewsCommon(type1, type2))
                        return false;
                }
            }
            return true;
        }
    }
    public class DataViewImplementation : DeletableBusinessRepositoryProxyWithBase<IDataViewRepository, int, DataViewResult, DataViewForAdd, DataViewForUpdate>, IDataViewImplementation
    {
        protected IReadOnlyDataViewImplementation ReadOnly { get; private set; }
        protected IDataViewResultPassThroughImplementation Result { get; private set; }
        protected IDataViewColumnPassThroughImplementation Column { get; private set; }
        protected IReadOnlyApplicationViewImplementation ApplicationView { get; private set; }
        protected IDataViewPassThroughImplementation PassThrough { get; set; }
        protected virtual bool IsPassThrough { get { return false; } }
        public DataViewImplementation(IDataViewRepository repository, ISecurityImplementation security, IReadOnlyDataViewImplementation readOnly, IDataViewResultPassThroughImplementation result, 
            IDataViewColumnPassThroughImplementation column, IReadOnlyApplicationViewImplementation applicationView, IDataViewPassThroughImplementation passThrough = null)
            :base(repository, security)
        {
            this.ReadOnly = readOnly;
            this.Result = result;
            this.Column = column;
            this.ApplicationView = applicationView;
            this.PassThrough = passThrough;
        }

        public async Task<QueryResult<DataViewResult>> RelateDataViews(int dataViewId, int relatedDataViewId, CancellationToken token = default(CancellationToken))
        {
            if (!this.IsPassThrough && !await this.CanDataViewsRelate(dataViewId, relatedDataViewId, token))
                throw new GlobalizedValidationException("ADMIN_DATAVIEW_RELATE_INVALID");
            return await ExecuteAction(userId => this.Repository.RelateDataViews(dataViewId, relatedDataViewId, userId, token), userId => this.Get(dataViewId, token), token);
        }

        public Task<QueryResult<DataViewResult>> UnRelateDataViews(int dataViewId, int relatedDataViewId, CancellationToken token = default(CancellationToken))
        {
            return ExecuteAction(userId => this.Repository.UnRelateDataViews(dataViewId, relatedDataViewId, userId, token), userId => this.Get(dataViewId, token), token);
        }

        public Task<QueryResults<DataViewResultResult>> GetDataViewResults(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Result.GetByDataView(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetDataViewResultsWithDownwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Result.GetByDataViewWithDownwardRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetDataViewResultsWithUpwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Result.GetByDataViewWithUpwardRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetDataViewResultsWithBiDirectionalRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Result.GetByDataViewWithBiDirectionalRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetColumns(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Column.GetByDataView(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetColumnsWithDownwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Column.GetByDataViewWithDownwardRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetColumnsWithUpwardRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Column.GetByDataViewWithUpwardRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewColumnResult>> GetColumnsWithBiDirectionalRecursion(int dataViewId, int? orderIndex = null, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Column.GetByDataViewWithBiDirectionalRecursion(dataViewId, orderIndex, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<ApplicationViewResult>> GetApplicationViews(int dataViewId, string viewName = null, FilterContext<ApplicationViewResult> filter = null, SortContext<ApplicationViewResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ApplicationView.GetByDataView(dataViewId, viewName, filter, sort, page, token);
        }

        public Task<QueryResult<DataViewResult>> GetByName(string name, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByName(name, token);
        }

        public Task<bool> DoesNameExist(string name, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.DoesNameExist(name, token);
        }

        public Task<bool> AreDataViewsCommon(int dataViewId1, int dataViewId2, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.AreDataViewsCommon(dataViewId1, dataViewId2, token);
        }
        public Task<bool> AreDataViewsCommon(IEnumerable<int> dataViewIds, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.AreDataViewsCommon(dataViewIds, token);
        }

        public Task<bool> CanDataViewsRelate(int dataViewId, int relatedDataViewId, CancellationToken token = default(CancellationToken))
        {
            return this.CanDataViewsRelate(new int[] { dataViewId, relatedDataViewId }, token);
        }
        public override Task<QueryResult<DataViewResult>> Get(int id, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.Get(id, token);
        }
        public override Task<QueryResults<DataViewResult>> GetMany(string viewName = null, FilterContext<DataViewResult> filter = null, SortContext<DataViewResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
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
        protected override Task<ChangeStatusResult> ChangeStatus(int id, string statusTypeName, bool force, Guid? userId, CancellationToken token)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> CanDataViewsRelate(IEnumerable<int> dataViewIds, CancellationToken token = default(CancellationToken))
        {
            if (dataViewIds.Distinct().Count() != dataViewIds.Count())
                return false;
            List<DataViewResultResult> results = new List<DataViewResultResult>();
            foreach (var id in dataViewIds)
                results.AddRange((await this.Result.GetByDataViewWithBiDirectionalRecursion(id, token: token)).Results);
            return await this.CanDataViewResultsRelate(results, token);
        }
        protected async Task<bool> CanDataViewResultsRelate(IEnumerable<DataViewResultResult> results, CancellationToken token = default(CancellationToken))
        {
            if (!await this.Result.DoDataViewResultsCompose(results, token))
                return false;
            foreach (var resultGroup in results.GroupBy(g => g.ResultIndex))
            {
                var columns = resultGroup.SelectMany(r => r.DataViewColumns);
                if (!await this.Column.DoColumnsCompose(columns, token))
                    return false;
            }
            return true;
        }
        protected async override Task Add(DataViewForAdd view, Guid? userId, CancellationToken token)
        {
            if(!this.IsPassThrough)
            {
                view.Validate();
                if (view.DataViewResults != null && view.DataViewResults.Count > 0 &&
                    (view.DataViewResults.Select(dr => dr.ResultIndex).Distinct().Count() != view.DataViewResults.Count || !view.DataViewResults.Any(dr => dr.ResultIndex == 0)))
                    throw new GlobalizedValidationException("ADMIN_DATAVIEW_ADD_DATAVIEWRESULT_DUPLICATE");
                if(view.RelatedDataViews != null && view.RelatedDataViews.Count > 0)
                {
                    List<DataViewResultResult> results = new List<DataViewResultResult>();
                    foreach (var id in view.RelatedDataViews.Select(d => d.DataViewId.Value))
                        results.AddRange((await this.Result.GetByDataViewWithBiDirectionalRecursion(id, token: token)).Results);
                    if (view.DataViewResults != null)
                        results.AddRange(view.DataViewResults.Select(dr => dr.CreateRelatedInstance<DataViewResultResult>()));
                    if (!await this.CanDataViewResultsRelate(results, token))
                        throw new GlobalizedValidationException("ADMIN_DATAVIEW_ADD_RELATEDDATAVIEWS_COMPOSITION_INVALID");
                }
            }
            await base.Add(view, userId, token);
            if(view.DataViewResults != null)
            {
                foreach(var dr in view.DataViewResults)
                {
                    var d = dr.CreateRelatedInstance<DataViewResultForAdd>();
                    d.DataView = new DataViewKey();
                    d.DataView.DataViewId = view.DataViewId;
                    await this.Result.Add(d, token);
                }
            }
            if(view.RelatedDataViews != null)
            {
                foreach(var rd in view.RelatedDataViews)
                {
                    await this.PassThrough.RelateDataViews(view.DataViewId.Value, rd.DataViewId.Value, token);
                }
            }
        }
        protected async override Task Update(DataViewForUpdate view, Guid? userId, CancellationToken token)
        {
            if(!this.IsPassThrough)
            {
                view.Validate();
                var old = await this.Get(view.DataViewId.Value, token);
                if (old == null)
                    throw new GlobalizedValidationException("ADMIN_DATAVIEW_UPDATE_NOTFOUND");
                List<DataViewResultResult> results = null;
                if(view.RelatedDataViews != null && view.RelatedDataViews.Add.Count > 0)
                {
                    results = (await this.Result.GetByDataViewWithBiDirectionalRecursion(view.DataViewId.Value, token: token)).Results.ToList();
                    results.RemoveAll(dr => view.RelatedDataViews.Remove.Any(dv => dv.DataViewId == dr.DataView.DataViewId));
                    foreach(var dr in view.RelatedDataViews.Add)
                    {
                        results.AddRange((await this.Result.GetByDataViewWithBiDirectionalRecursion(dr.DataViewId.Value, token: token)).Results);
                    }
                }
                if(view.DataViewResults != null && view.DataViewResults.Count() > 0)
                {
                    results = results ?? (await this.Result.GetByDataViewWithBiDirectionalRecursion(view.DataViewId.Value, token: token)).Results.ToList();
                    if (results.Any(dr => dr.DataView.DataViewId == view.DataViewId && view.DataViewResults.Add.Any(d => d.ResultIndex == dr.ResultIndex)))
                        throw new GlobalizedValidationException("ADMIN_DATAVIEW_UPDATE_DATAVIEWRESULTS_ADD_DUPLICATE");
                    results.RemoveAll(dr => view.DataViewResults.HardDelete.Union(view.DataViewResults.SoftDelete).Any(d => d.DataViewResultId == dr.DataViewResultId));
                    foreach(var dr in view.DataViewResults.Update)
                    {
                        foreach(var orig in results.Where(r => r.DataViewResultId == dr.DataViewResultId))
                        {
                            if (orig == null)
                                throw new GlobalizedValidationException("ADMIN_DATAVIEW_UPDATE_DATAVIEWRESULTS_UPDATE_NOTFOUND");
                            List<DataViewColumnResult> columns = new List<DataViewColumnResult>(orig.DataViewColumns);
                            if (dr.DataViewColumns != null)
                            {
                                columns.RemoveAll(c => dr.DataViewColumns.HardDelete.Union(dr.DataViewColumns.SoftDelete).Any(cc => cc.DataViewColumnId == c.DataViewColumnId));
                                columns.RemoveAll(c => dr.DataViewColumns.Update.Any(cc => cc.DataViewColumnId == c.DataViewColumnId));
                                columns.AddRange(dr.DataViewColumns.Add.Select(c => c.CreateRelatedInstance<DataViewColumnResult>()));
                                columns.AddRange(dr.DataViewColumns.Update.Select(c => c.CreateRelatedInstance<DataViewColumnResult>()));
                            }
                            orig.ApplyProperties(dr);
                            orig.DataViewColumns = columns;
                        }
                    }
                    results.AddRange(view.DataViewResults.Add.Select(dr => dr.CreateRelatedInstance<DataViewResultResult>()));
                }
                if (results != null && !await this.CanDataViewResultsRelate(results, token))
                    throw new GlobalizedValidationException("ADMIN_DATAVIEW_UPDATE_DATAVIEWRESULTS_COMPOSITION_INVALID");
            }
            await base.Update(view, userId, token);
            if(view.RelatedDataViews != null)
            {
                foreach (var rd in view.RelatedDataViews.Remove)
                    await this.PassThrough.UnRelateDataViews(view.DataViewId.Value, rd.DataViewId.Value, token);
                foreach (var rd in view.RelatedDataViews.Add)
                    await this.PassThrough.RelateDataViews(view.DataViewId.Value, rd.DataViewId.Value, token);
            }
            if(view.DataViewResults != null)
            {
                foreach (var rd in view.DataViewResults.HardDelete)
                    await this.Result.DeleteHard(rd.DataViewResultId.Value, token);
                foreach (var rd in view.DataViewResults.SoftDelete)
                    await this.Result.DeleteSoft(rd.DataViewResultId.Value, token);
                foreach (var rd in view.DataViewResults.Add)
                {
                    var d = rd.CreateRelatedInstance<DataViewResultForAdd>();
                    d.DataView = new DataViewKey();
                    d.DataView.DataViewId = view.DataViewId;
                    await this.Result.Add(d, token);
                }
                foreach (var rd in view.DataViewResults.Update)
                    await this.Result.Update(rd, token);
            }
        }
    }
    public class DataViewPassThroughImplementation : DataViewImplementation, IDataViewPassThroughImplementation
    {
        protected override bool IsPassThrough
        {
            get
            {
                return true;
            }
        }
        public DataViewPassThroughImplementation(IDataViewRepository repository, ISecurityImplementation security, IReadOnlyDataViewImplementation readOnly, IDataViewResultPassThroughImplementation result,
            IDataViewColumnPassThroughImplementation column, IReadOnlyApplicationViewImplementation applicationView)
            : base(repository, security, readOnly, result, column, applicationView)
        {
            this.PassThrough = this;
        }
    }
}
