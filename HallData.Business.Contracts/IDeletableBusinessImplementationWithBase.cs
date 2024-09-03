using HallData.ApplicationViews;

namespace HallData.Business
{
    public interface IDeletableBusinessImplementationWithBase<in TKey, TView, in TViewForAdd, in TViewForUpdate> : IDeletableBusinessImplementation<TKey, TView, TView, TViewForAdd, TViewForUpdate>, 
        IBusinessImplementationWithBase<TKey, TView, TViewForAdd, TViewForUpdate>
        where TView : IHasKey<TKey>
        where TViewForAdd : IHasKey<TKey>
        where TViewForUpdate: IHasKey<TKey>{ }
    public interface IDeletableBusinessImplementationWithBase<in TKey, TView, in TViewBase> : IDeletableBusinessImplementationWithBase<TKey, TView, TViewBase, TViewBase>, 
        IBusinessImplementationWithBase<TKey, TView, TViewBase>, IDeletableBusinessImplementation<TKey, TView, TView, TViewBase>
        where TView : IHasKey<TKey>
        where TViewBase : IHasKey<TKey> { }
}