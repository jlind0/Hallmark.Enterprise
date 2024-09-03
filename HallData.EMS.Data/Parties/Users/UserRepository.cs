using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository;
using HallData.EMS.ApplicationViews.Results;
using System.Data.SqlClient;
using HallData.Data;
using System.Threading;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Data
{
	public class UserRepository : PersonRepository<UserResult, UserForAdd, UserForUpdate>, IUserRepository
	{
        public const string SelectAllUserProcedure = "usp_select_users";
		public const string SelectUserQuery = "select * from v_users where [partyguid#] = @partyguid and [__userguid?] = @__userguid";
        public const string InsertUserProcedure = "usp_insert_users";
        public const string UpdateUserProcedure = "usp_update_users";
        public const string DeleteUserProcedure = "usp_delete_users";

		public UserRepository(Database db)
            : base(db, SelectAllUserProcedure, SelectUserQuery, InsertUserProcedure, UpdateUserProcedure, DeleteUserProcedure)
		{

		}


		public async Task<bool> ChangePassword(string username, string currentPassword, string newPassword, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand("usp_change_users_password");
			cmd.AddParameter("username", username);
			cmd.AddParameter("currentpassword", currentPassword);
			cmd.AddParameter("newpassword", newPassword);
			PopulateUserIdParameter(cmd, userId);
			return (bool) await Execute(cmd, () => this.Database.ExecuteScalarAsync(cmd, token));
		}

		public async Task<bool> ChangePasswordAdmin(Guid targetUserId, string password, Guid userId, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand("usp_change_users_password_admin");
			cmd.AddParameter("partyguid", targetUserId);
			cmd.AddParameter("password", password);
			PopulateUserIdParameter(cmd, userId);
			return (bool)await Execute(cmd, () => this.Database.ExecuteScalarAsync(cmd, token));
		}

		public Task<ChangeStatusResult> ChangeStatusUserRelationship(Guid targetUserId, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand("usp_changestatus_users_relationship");
			PopulateChangeStatusCommand(cmd, targetUserId);
			return ExecuteChangeStatus(cmd, statusTypeName, force, userId, token);
		}

		public Task<QueryResult<UserResult>> GetByUserName(string username, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand("usp_select_users");
			cmd.AddParameter("username", username);
			return ReadQueryResult<UserResult>(cmd, userId, token);
		}


		public Task<QueryResult<JObject>> GetByUserNameView(string username, string viewname = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand("usp_select_users");
			cmd.AddParameter("username", username);
			return ReadView(cmd, userId, token);
		}
	}
}
