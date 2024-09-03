using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;

namespace HallData.Business
{
	public interface IDeletableBusinessImplementation<in TKey, TView, TModel, in TModelForAdd, in TModelForUpdate> : IBusinessImplementation<TKey, TView, TModel, TModelForAdd, TModelForUpdate>
		where TView : IHasKey<TKey>
		where TModel : IHasKey<TKey>
		where TModelForAdd : IHasKey<TKey>
		where TModelForUpdate: IHasKey<TKey>
	{
		[DeleteMethod]
		[ServiceRoute("Delete", "{id}/Soft")]
		[ServiceRoute("DeleteDefault", "{id}/")]
		[Description("Soft deletes a view by id using the implementing service")]
		Task DeleteSoft([Description("The target id")] TKey id, CancellationToken token = default(CancellationToken));
		[DeleteMethod]
		[ServiceRoute("DeleteHard", "{id}/Hard")]
		[Description("Hard deletes a view by id using the implementing service")]
		Task DeleteHard([Description("The target id")] TKey id, CancellationToken token = default(CancellationToken));
	}

	public interface IDeletableBusinessImplementation<in TKey, TView, TModel, in TModelBase> : IDeletableBusinessImplementation<TKey, TView, TModel, TModelBase, TModelBase>, IBusinessImplementation<TKey, TView, TModel, TModelBase>
		where TView: IHasKey<TKey>
		where TModel: IHasKey<TKey>
		where TModelBase : IHasKey<TKey> { }
	public interface IDeletableBusinessImplementation<in TKey, TView, TModel> : IDeletableBusinessImplementation<TKey, TView, TModel, TModel>, IBusinessImplementation<TKey, TView, TModel>
		where TView : IHasKey<TKey>
		where TModel : IHasKey<TKey>
	{
	}

	public interface IDeletableBusinessImplementation<in TKey, TView> : IDeletableBusinessImplementation<TKey, TView, TView, TView>, IBusinessImplementation<TKey, TView>, IDeletableBusinessImplementationWithBase<TKey, TView, TView>
		where TView : IHasKey<TKey> { }

	public interface IDeletableBusinessImplementation<TView> : IDeletableBusinessImplementation<int, TView>, IBusinessImplementation<TView>
		where TView : IHasKey<int> { }
}