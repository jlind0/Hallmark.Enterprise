using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.Repository
{
	/// <summary>
	/// Base interface for a repository
	/// </summary>
	/// <remarks>Should be implemented by repositories that do not follow a standard CRUD pattern</remarks>
	public interface IRepository
	{
	}
	/// <summary>
	/// Untyped readonly repository
	/// </summary>
	/// <remarks>Should NOT be directly implemented</remarks>
	public interface IReadOnlyRepository : IRepository
	{
		/// <summary>
		/// Gets dynamically typed results for a view
		/// </summary>
		/// <param name="viewName">Name of the view, default view if null</param>
		/// <param name="userId">Signed in user id</param>
		/// <param name="filter">The filter for the results</param>
		/// <param name="sort">The sort for the results</param>
		/// <param name="page">The page for the results</param>
		/// <param name="token">Canellation Token</param>
		/// <returns>Query Results of type JObject</returns>
		Task<QueryResults<JObject>> GetView(string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
	}

	/// <summary>
	/// Strongly typed read only repository with Key
	/// </summary>
	/// <typeparam name="TKey">Type of key</typeparam>
	/// <typeparam name="TView">Type of view</typeparam>
	/// <remarks>Should be implemented by repositories that require read only access and have a key other than integer</remarks>
	public interface IReadOnlyRepository<in TKey, TView> : IReadOnlyRepository
	{
		/// <summary>
		/// Gets strongly typed results for TView
		/// </summary>
		/// <param name="userId">Signed in user id</param>
		/// <param name="filter">The filter for TView</param>
		/// <param name="sort">The sort for TView</param>
		/// <param name="page">The page for the results</param>
		/// <param name="token">Cancellation Token</param>
		/// <returns>Query Results for TView</returns>
		Task<QueryResults<TView>> Get(string viewName = null, Guid? userId = null, FilterContext<TView> filter = null, SortContext<TView> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		/// <summary>
		/// Gets a strongly typed result for TView
		/// </summary>
		/// <param name="id">Target id</param>
		/// <param name="userId">Signed in user id</param>
		/// <param name="token">Cancellation Token</param>
		/// <returns>Strongly typed QueryResult of TView<remarks>Is null if id not found</remarks></returns>
		Task<QueryResult<TView>> Get(TKey id, Guid? userId = null, CancellationToken token = default(CancellationToken));
		/// <summary>
		/// Gets a dynamically typed result for a view
		/// </summary>
		/// <param name="id">Target id</param>
		/// <param name="viewName">Name of the view, default view if null</param>
		/// <param name="userId">Signed in user id</param>
		/// <param name="token">Cancellation Token</param>
		/// <returns>Query Result of type JObject<remarks>Is null if id not found</remarks></returns>
		Task<QueryResult<JObject>> GetView(TKey id, Guid? userId = null, CancellationToken token = default(CancellationToken));
	}

	/// <summary>
	/// Strongly typed read only repository with integer key
	/// </summary>
	/// <typeparam name="TView">Type of view</typeparam>
	/// <remarks>Should be implemented by repositories that require read only access and have an integer key</remarks>
	public interface IReadOnlyRepository<TView> : IReadOnlyRepository<int, TView> { }

	public interface IRepository<in TKey, TView, in TViewForAdd, in TViewForUpdate> : IReadOnlyRepository<TKey, TView>
		where TView: IHasKey<TKey>
		where TViewForAdd: IHasKey<TKey>
		where TViewForUpdate: IHasKey<TKey>
	{
		/// <summary>
		/// Adds a view
		/// </summary>
		/// <param name="view">Target view</param>
		/// <param name="userId">Signed in user id</param>
		/// <param name="token">Cancellation Token</param>
		/// <returns>Task representing add work</returns>
		/// <remarks>Upon a successful add view.Key will be set to the newly created identity</remarks>
		Task Add(TViewForAdd view, Guid? userId = null, CancellationToken token = default(CancellationToken));
		/// <summary>
		/// Updates a view
		/// </summary>
		/// <param name="view">Target view</param>
		/// <param name="userId">Signed in user id</param>
		/// <param name="token">Cancellation Token</param>
		/// <returns>Task representing update work</returns>
		Task Update(TViewForUpdate view, Guid? userId = null, CancellationToken token = default(CancellationToken));
		/// <summary>
		/// Changes a status type for a view
		/// </summary>
		/// <param name="id">Target id</param>
		/// <param name="statusTypeName">New Status Type Name</param>
		/// <param name="userId">Signed in user id</param>
		/// <param name="token">Cancellation Token</param>
		/// <param name="force">Indicates if the status change query should ignore warnings and force the change (requestor accepts the warning message)</param>
		/// <returns>Task representing Change Status work</returns>
		Task<ChangeStatusResult> ChangeStatus(TKey id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken));
	}
	public interface IRepository<in TKey, TView, in TViewForAdd, in TViewForUpdate, in TViewForAddRelationship, in TViewForUpdateRelationship> : IRepository<TKey, TView, TViewForAdd, TViewForUpdate>
		where TView: IHasKey<TKey>
		where TViewForAdd: IHasKey<TKey>
		where TViewForAddRelationship: IHasKey<TKey>
		where TViewForUpdate: IHasKey<TKey>
		where TViewForUpdateRelationship: IHasKey<TKey>
	{
		Task AddRelationship(TViewForAddRelationship view, Guid? userId = null, CancellationToken token = default(CancellationToken));
		Task UpdateRelationship(TViewForUpdateRelationship view, Guid? userId = null, CancellationToken token = default(CancellationToken));
		Task<ChangeStatusResult> ChangeStatusRelationship(TKey id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken));
	}
	/// <summary>
	/// Strongly typed writeable repository with key, view and viewbase
	/// </summary>
	/// <typeparam name="TKey">The key</typeparam>
	/// <typeparam name="TView">The view result</typeparam>
	/// <typeparam name="TViewBase">The view being consumed</typeparam>
	/// <remarks>Should be implemented by repositories that have read/write but not delete functionality, and have a different consuming view than results view</remarks>
	public interface IRepository<in TKey, TView, in TViewBase> : IRepository<TKey, TView, TViewBase, TViewBase>
		where TView : IHasKey<TKey>
		where TViewBase : IHasKey<TKey>
	{
		
	}
	/// <summary>
	/// Strongly typed writeable repository with key and view
	/// </summary>
	/// <typeparam name="TKey">The key</typeparam>
	/// <typeparam name="TView">The view result and base</typeparam>
	/// <remarks>Should be implemented by repositories that have read/write but not delete functionality, and have the same consuming and results view and does not have an integer key</remarks>
	public interface IRepository<in TKey, TView> : IRepository<TKey, TView, TView>
		where TView : IHasKey<TKey> { }
	/// <summary>
	/// Strongly typed writeable repository for integer key views
	/// </summary>
	/// <typeparam name="TView">The view result and base</typeparam>
	/// <remarks>Should be implemented by repositories that have read/write but not delete functionality, and have the same consuming and results view and has an integer key</remarks>
	public interface IRepository<TView> : IRepository<int, TView>, IReadOnlyRepository<TView>
		where TView : IHasKey
	{ }
	public interface IDeletableRepository<in TKey, TView, in TViewForAdd, in TViewForUpdate> : IRepository<TKey, TView, TViewForAdd, TViewForUpdate>
		where TView: IHasKey<TKey>
		where TViewForAdd: IHasKey<TKey>
		where TViewForUpdate: IHasKey<TKey>
	{
		/// <summary>
		/// Deletes a view
		/// </summary>
		/// <param name="id">Target id</param>
		/// <param name="userId">Signed in user id</param>
		/// <param name="cascade">Indicates if the delete should be a cascade</param>
		/// <param name="isHard">Indicates if the delete should be hard</param>
		/// <param name="token">Cancellation token</param>
		/// <returns>Task representing delete work</returns>
		Task Delete(TKey id, Guid? userId = null, bool cascade = false, bool isHard = false, CancellationToken token = default(CancellationToken));
	}
	public interface IDeletableRepository<in TKey, TView, in TViewForAdd, in TViewForUpdate, in TViewForAddRelationship, in TViewForUpdateRelationship> :
		IDeletableRepository<TKey, TView, TViewForAdd, TViewForUpdate>
		where TView: IHasKey<TKey>
		where TViewForAdd: IHasKey<TKey>
		where TViewForAddRelationship: IHasKey<TKey>
		where TViewForUpdate: IHasKey<TKey>
		where TViewForUpdateRelationship: IHasKey<TKey>
	{
		Task DeleteRelationship(TKey id, Guid? userId = null, bool cascade = false, bool isHard = false, CancellationToken token = default(CancellationToken));
	}
	/// <summary>
	/// Strongly typed deleteable repository with key, view and viewbase
	/// </summary>
	/// <typeparam name="TKey">The key</typeparam>
	/// <typeparam name="TView">The view result</typeparam>
	/// <typeparam name="TViewBase">The view being consumed</typeparam>
	/// <remarks>Should be implemented by repositories that have read/write/delete functionality and have a different consuming view than results view</remarks>
	public interface IDeletableRepository<in TKey, TView, in TViewBase> : IDeletableRepository<TKey, TView, TViewBase, TViewBase>, IRepository<TKey, TView, TViewBase>
		where TView : IHasKey<TKey>
		where TViewBase: IHasKey<TKey>
	{
		
	}
	/// <summary>
	/// Strongly typed deleteable repository with key and view
	/// </summary>
	/// <typeparam name="TKey">The key</typeparam>
	/// <typeparam name="TView">The view result and base</typeparam>
	/// <remarks>Should be implemented by repoistories that have read/write/delete functionality and have the same consuming and results view and does not have an integer key</remarks>
	public interface IDeletableRepository<in TKey, TView> : IDeletableRepository<TKey, TView, TView>, IRepository<TKey, TView>
		where TView : IHasKey<TKey> { }
	/// <summary>
	/// Strongly typed deleteable repository with view and integer key
	/// </summary>
	/// <typeparam name="TView">The view result and base</typeparam>
	/// <remarks>Should be implemented by repositories that have read/write/delete functionality and have the same consuming and results view and has an integer key</remarks>
	public interface IDeletableRepository<TView> : IDeletableRepository<int, TView>, IRepository<TView>
		where TView: IHasKey
	{ }
}
