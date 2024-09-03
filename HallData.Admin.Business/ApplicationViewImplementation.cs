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
    public class ReadOnlyApplicationViewImplementation : ReadOnlyBusinessRepositoryProxy<IReadOnlyApplicationViewRepository, ApplicationViewResult>, IReadOnlyApplicationViewImplementation
    {
        public ReadOnlyApplicationViewImplementation(IReadOnlyApplicationViewRepository repository, ISecurityImplementation security) : base(repository, security) { }

        public async Task<bool> DoesNameExist(string name, CancellationToken token = default(CancellationToken))
        {
            return (await this.GetByName(name, token)) != null;
        }

        public Task<QueryResult<ApplicationViewResult>> GetByName(string name, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByName(name, userId, token), token);
        }

        public Task<QueryResults<ApplicationViewResult>> GetByDataView(int dataViewId, string viewName = null, FilterContext<ApplicationViewResult> filter = null, SortContext<ApplicationViewResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByDataView(dataViewId, viewName, userId, filter, sort, page, token), token);
        }
    }
    public class ApplicationViewImplementation : DeletableBusinessRepositoryProxyWithBase<IApplicationViewRepository, int, ApplicationViewResult, ApplicationViewForAdd, ApplicationViewForUpdate>, IApplicationViewImplementation
    {
        protected IReadOnlyApplicationViewImplementation ReadOnly { get; private set; }
        protected IApplicationViewColumnPassThroughImplementation Column { get; private set; }
        protected IReadOnlyTemplateImplementation Template { get; private set; }
        protected virtual bool IsPassThrough { get { return false; } }
        public ApplicationViewImplementation(IApplicationViewRepository repository, ISecurityImplementation security, IReadOnlyApplicationViewImplementation readOnly, 
            IApplicationViewColumnPassThroughImplementation column, IReadOnlyTemplateImplementation template) : base(repository, security)
        {
            this.ReadOnly = readOnly;
            this.Column = column;
            this.Template = template;
        }

        public async Task<QueryResult<ApplicationViewResult>> Copy(int sourceApplicationViewId, string targetName, CancellationToken token = default(CancellationToken))
        {
            int? newApplicationId = null;
            return await ExecuteAction(async userId => newApplicationId = await this.Repository.Copy(sourceApplicationViewId, targetName, userId, token), 
                userId => this.Get(newApplicationId.Value, token), token);
        }

        public Task<QueryResults<ApplicationViewColumnResult>> GetColumns(int applicationViewId, string viewName = null, FilterContext<ApplicationViewColumnResult> filter = null, SortContext<ApplicationViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Column.GetByApplicationView(applicationViewId, viewName, filter, sort, page, token);
        }

        public Task<bool> DoesNameExist(string name, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.DoesNameExist(name, token);
        }

        public Task<QueryResult<ApplicationViewResult>> GetByName(string name, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByName(name, token);
        }

        public Task<QueryResults<ApplicationViewResult>> GetByDataView(int dataViewId, string viewName = null, FilterContext<ApplicationViewResult> filter = null, SortContext<ApplicationViewResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByDataView(dataViewId, viewName, filter, sort, page, token);
        }
        protected async override Task Add(ApplicationViewForAdd view, Guid? userId, CancellationToken token)
        {
            if(!this.IsPassThrough)
            {
                view.Validate();
                ApplicationViewValidator viewValidator = new ApplicationViewValidator(this.Template);
                await viewValidator.Validate(viewValidator.CreateRelatedInstance<ApplicationViewResult>());
                if(view.Columns != null && view.Columns.Count > 0)
                {
                    ApplicationViewColumnValidator columnValidator = new ApplicationViewColumnValidator(this.Template);
                    foreach (var col in view.Columns)
                        await columnValidator.Validate(col.CreateRelatedInstance<ApplicationViewColumnResult>());
                }
            }
            await base.Add(view, userId, token);
            if(view.Columns != null)
            {
                foreach(var col in view.Columns)
                {
                    var c = col.CreateRelatedInstance<ApplicationViewColumnForAdd>();
                    c.ApplicationView = new ApplicationViewKey();
                    c.ApplicationView.ApplicationViewId = view.ApplicationViewId;
                    await this.Column.Add(c, token);
                }
            }
        }
        protected async override Task Update(ApplicationViewForUpdate view, Guid? userId, CancellationToken token)
        {
            if(!this.IsPassThrough)
            {
                view.Validate();
                ApplicationViewValidator viewValidator = new ApplicationViewValidator(this.Template);
                await viewValidator.Validate(viewValidator.CreateRelatedInstance<ApplicationViewResult>());
                if(view.Columns != null && (view.Columns.Add.Count > 0 || view.Columns.Update.Count > 0))
                {
                    ApplicationViewColumnValidator columnValidator = new ApplicationViewColumnValidator(this.Template);
                    foreach (var col in view.Columns.Add.Select(a => a.CreateRelatedInstance<ApplicationViewColumnResult>()).Union(
                        view.Columns.Update.Select(a => a.CreateRelatedInstance<ApplicationViewColumnResult>())))
                        await columnValidator.Validate(col);
                }
            }
            await base.Update(view, userId, token);
            if(view.Columns != null)
            {
                foreach(var col in view.Columns.Add)
                {
                    var c = col.CreateRelatedInstance<ApplicationViewColumnForAdd>();
                    c.ApplicationView = new ApplicationViewKey();
                    c.ApplicationView.ApplicationViewId = view.ApplicationViewId;
                    await this.Column.Add(c, token);
                }
                foreach (var col in view.Columns.Update)
                    await this.Column.Update(col, token);
                foreach (var col in view.Columns.HardDelete)
                    await this.Column.DeleteHard(col.ApplicationViewColumnId.Value, token);
                foreach (var col in view.Columns.SoftDelete)
                    await this.Column.DeleteSoft(col.ApplicationViewColumnId.Value, token);
            }
        }
        protected override Task<ChangeStatusResult> ChangeStatus(int id, string statusTypeName, bool force, Guid? userId, CancellationToken token)
        {
            throw new NotImplementedException();
        }
        public override Task<QueryResult<ApplicationViewResult>> Get(int id, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.Get(id, token);
        }
        public override Task<QueryResults<ApplicationViewResult>> GetMany(string viewName = null, FilterContext<ApplicationViewResult> filter = null, SortContext<ApplicationViewResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
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
    public class ApplicationViewPassThroughImplementation : ApplicationViewImplementation, IApplicationViewPassThroughImplementation
    {
        protected override bool IsPassThrough
        {
            get
            {
                return true;
            }
        }
        public ApplicationViewPassThroughImplementation(IApplicationViewRepository repository, ISecurityImplementation security, IReadOnlyApplicationViewImplementation readOnly,
            IApplicationViewColumnPassThroughImplementation column, IReadOnlyTemplateImplementation template)
            : base(repository, security, readOnly, column, template) { }
    }
}
