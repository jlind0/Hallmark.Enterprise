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
    public class ReadOnlyTemplateImplementation : ReadOnlyBusinessRepositoryProxy<IReadOnlyTemplateRepository, TemplateResult>, IReadOnlyTemplateImplementation
    {
        public ReadOnlyTemplateImplementation(IReadOnlyTemplateRepository repository, ISecurityImplementation security) : base(repository, security) { }

        public Task<QueryResults<TemplateResult>> GetByTemplateType(int templateTypeId, string viewName = null, FilterContext<TemplateResult> filter = null, SortContext<TemplateResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.GetByTemplateType(templateTypeId, viewName, filter, sort, page, token), token);
        }
    }
    public class TemplateImplementation : DeletableBusinessRepositoryProxyWithBase<ITemplateRepository, int, TemplateResult, TemplateForAdd, TemplateForUpdate>, ITemplateImplementation
    {
        protected IReadOnlyTemplateImplementation ReadOnly { get; private set; }
        public TemplateImplementation(ITemplateRepository repository, ISecurityImplementation security, IReadOnlyTemplateImplementation readOnly) : base(repository, security)
        {
            this.ReadOnly = readOnly;
        }

        public Task<QueryResults<TemplateResult>> GetByTemplateType(int templateTypeId, string viewName = null, FilterContext<TemplateResult> filter = null, SortContext<TemplateResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByTemplateType(templateTypeId, viewName, filter, sort, page, token);
        }
        protected override Task<ChangeStatusResult> ChangeStatus(int id, string statusTypeName, bool force, Guid? userId, CancellationToken token)
        {
            throw new NotImplementedException();
        }
        public override Task<QueryResult<TemplateResult>> Get(int id, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.Get(id, token);
        }
        public override Task<QueryResults<TemplateResult>> GetMany(string viewName = null, FilterContext<TemplateResult> filter = null, SortContext<TemplateResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
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
    }
}
