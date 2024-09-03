using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository;
using HallData.Security;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Models;
using HallData.EMS.Data;
using System.Threading;
using Newtonsoft.Json.Linq;
using HallData.Validation;
using HallData.ApplicationViews;

namespace HallData.EMS.Business
{
    public class ReadOnlyUserImplementation : ReadOnlyPersonImplementation<IReadOnlyUserRepository, Guid, UserResult>, IReadOnlyUserImplementation
    {
        public ReadOnlyUserImplementation(IReadOnlyUserRepository repository, ISecurityImplementation security, IReadOnlyEmployeeRepository employee)
            : base(repository, security, employee) { }
        public async Task<QueryResult<UserResult>> GetByUserName(string username, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            return await this.Repository.GetByUserName(username, viewName, await this.ActivateAndGetSignedInUserGuid(token), token);
        }
        public async Task<QueryResult<JObject>> GetByUserNameView(string username, string viewname = null, CancellationToken token = default(CancellationToken))
        {
            return await this.Repository.GetByUserNameView(username, viewname, await this.ActivateAndGetSignedInUserGuid(token), token);
        }
    }
    public class UserImplementation : PersonImplementation<IUserRepository, IReadOnlyUserImplementation, Guid, UserResult, UserForAdd, UserForUpdate>, IUserImplementation
    {
        protected ISecurityTokenizer Tokenizer { get; private set; }
        public UserImplementation(IUserRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact,
             IReadOnlyUserImplementation readOnly, IProductImplementation product, ISecurityTokenizer tokenizer)
            : base(repository, security, partyContact, readOnly, product)
        {
            this.Tokenizer = tokenizer;
        }


         public async Task<ChangePasswordResult> ChangePassword(string username, ChangePasswordParameters parameters, CancellationToken token = default(CancellationToken))
         {
             var userGuid = await this.ActivateAndGetSignedInUserGuid(token);
             bool changed = await this.Repository.ChangePassword(username, this.Tokenizer.Hash(parameters.CurrentPassword), this.Tokenizer.Hash(parameters.NewPassword), userGuid, token);
             return new ChangePasswordResult(await this.Get(userGuid.Value, token), changed);
         }

         public Task<ChangePasswordResult> ChangePasswordAdmin(Guid userId, string password, CancellationToken token = default(CancellationToken))
         {
             //TODO: this.Repository.ChangePasswordAdmin
             throw new NotImplementedException();
         }

         public Task<ChangeStatusQueryResult<UserResult>> ChangeStatusUserRelationship(Guid userId, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
         {
             //TODO: this.Repository.ChangeStatusUserRelationship
             throw new NotImplementedException();
         }

         public Task<ChangeStatusQueryResult<UserResult>> ChangeStatusUserRelationshipForce(Guid userId, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
         {
             //TODO: this.Repository.ChangeStatusUserRelationship
             throw new NotImplementedException();
         }

         public Task<Guid?> SignIn(string userName, string password = null, string token = null, CancellationToken cancellationToken = default(CancellationToken))
         {
             return this.Security.SignIn(userName, password, token, cancellationToken);
         }

         public Task<QueryResult<UserResult>> GetByUserName(string username, string viewName = null, CancellationToken token = default(CancellationToken))
         {
             return this.ReadOnly.GetByUserName(username, viewName, token);
         }

         public Task<QueryResult<JObject>> GetByUserNameView(string username, string viewname = null, CancellationToken token = default(CancellationToken))
         {
             return this.ReadOnly.GetByUserNameView(username, viewname, token);
         }
         public override Task<QueryResult<UserResult>> Add(UserForAdd view, CancellationToken token = default(CancellationToken))
         {
             view.Validate();
             if (view.Password != null)
                 view.PasswordHash = this.Tokenizer.Hash(view.Password);
             return base.Add(view, token);
         }
    }
}
