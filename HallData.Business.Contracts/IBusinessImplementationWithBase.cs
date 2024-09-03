using HallData.ApplicationViews;

namespace HallData.Business
{
    public interface IBusinessImplementationWithBase<in TKey, TView, in TViewForAdd, in TViewForUpdate> : IBusinessImplementation<TKey, TView, TView, TViewForAdd, TViewForUpdate>
        where TView : IHasKey<TKey>
        where TViewForAdd : IHasKey<TKey>
        where TViewForUpdate: IHasKey<TKey>{ }
    public interface IBusinessImplementationWithBase<in TKey, TView, in TViewForAdd, in TViewForUpdate, in TViewForAddRelationship, in TViewForUpdateRelationship> :
        IBusinessImplementation<TKey, TView, TView, TViewForAdd, TViewForUpdate, TViewForAddRelationship, TViewForUpdateRelationship>,
        IBusinessImplementationWithBase<TKey, TView, TViewForAdd, TViewForUpdate>
        where TView : IHasKey<TKey>
        where TViewForAdd : IHasKey<TKey>
        where TViewForUpdate : IHasKey<TKey>
        where TViewForAddRelationship: IHasKey<TKey>
        where TViewForUpdateRelationship: IHasKey<TKey>
    { }
    public interface IBusinessImplementationWithBase<in TKey, TView, in TViewBase> : IBusinessImplementationWithBase<TKey, TView, TViewBase, TViewBase>, IBusinessImplementation<TKey, TView, TView, TViewBase>
        where TView : IHasKey<TKey>
        where TViewBase : IHasKey<TKey> { }
}