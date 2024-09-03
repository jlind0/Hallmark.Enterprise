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
    public class ReadOnlyApplicationViewColumnImplementation : ReadOnlyBusinessRepositoryProxy<IReadOnlyApplicationViewColumnRepository, ApplicationViewColumnResult>, IReadOnlyApplicationViewColumnImplementation
    {
        public ReadOnlyApplicationViewColumnImplementation(IReadOnlyApplicationViewColumnRepository repository, ISecurityImplementation security) : base(repository, security) { }


        public Task<QueryResults<ApplicationViewColumnResult>> GetByApplicationView(int applicationViewId, string viewName = null, FilterContext<ApplicationViewColumnResult> filter = null, SortContext<ApplicationViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByApplicationView(applicationViewId, viewName, userId, filter, sort, page, token), token);
        }
    }
    public class ApplicationViewColumnImplementation : DeletableBusinessRepositoryProxyWithBase<IApplicationViewColumnRepository, int, ApplicationViewColumnResult, ApplicationViewColumnForAdd, ApplicationViewColumnForUpdate>, 
        IApplicationViewColumnImplementation
    {
        protected IReadOnlyApplicationViewColumnImplementation ReadOnly { get; private set; }
        protected virtual bool IsPassThrough { get { return false; } }
        protected IReadOnlyTemplateImplementation Template { get; private set; }
        public ApplicationViewColumnImplementation(IApplicationViewColumnRepository repository, ISecurityImplementation security, IReadOnlyApplicationViewColumnImplementation readOnly, IReadOnlyTemplateImplementation template) : 
            base(repository, security)
        {
            this.ReadOnly = readOnly;
            this.Template = template;
        }

        public Task<QueryResults<ApplicationViewColumnResult>> GetByApplicationView(int applicationViewId, string viewName = null, FilterContext<ApplicationViewColumnResult> filter = null, SortContext<ApplicationViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByApplicationView(applicationViewId, viewName, filter, sort, page, token);
        }
        protected override Task<ChangeStatusResult> ChangeStatus(int id, string statusTypeName, bool force, Guid? userId, CancellationToken token)
        {
            throw new NotImplementedException();
        }
        protected override async Task Add(ApplicationViewColumnForAdd view, Guid? userId, CancellationToken token)
        {
            if(!this.IsPassThrough)
            {
                view.Validate();
                ApplicationViewColumnValidator validator = new ApplicationViewColumnValidator(this.Template);
                await validator.Validate(view.CreateRelatedInstance<ApplicationViewColumnResult>(), token);
            }
            await base.Add(view, userId, token);
        }
        protected async override Task Update(ApplicationViewColumnForUpdate view, Guid? userId, CancellationToken token)
        {
            if (!this.IsPassThrough)
            {
                view.Validate();
                ApplicationViewColumnValidator validator = new ApplicationViewColumnValidator(this.Template);
                await validator.Validate(view.CreateRelatedInstance<ApplicationViewColumnResult>(), token);
            }
            await base.Update(view, userId, token);
        }
        public override Task<QueryResult<ApplicationViewColumnResult>> Get(int id, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.Get(id, token);
        }
        public override Task<QueryResults<ApplicationViewColumnResult>> GetMany(string viewName = null, FilterContext<ApplicationViewColumnResult> filter = null, SortContext<ApplicationViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
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
    public class ApplicationViewColumnPassThroughImplementation : ApplicationViewColumnImplementation, IApplicationViewColumnPassThroughImplementation
    {
        protected override bool IsPassThrough
        {
            get
            {
                return true;
            }
        }
        public ApplicationViewColumnPassThroughImplementation(IApplicationViewColumnRepository repository, ISecurityImplementation security, IReadOnlyApplicationViewColumnImplementation readOnly, IReadOnlyTemplateImplementation template) :
            base(repository, security, readOnly, template) { }
        
    }
}
