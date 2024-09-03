using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Repository;
using HallData.Security;

namespace HallData.Business
{
    public class DeletableBusinessRepositoryProxyWithBase<TRepository, TKey, TView, TViewForAdd, TViewForUpdate> : DeletableBusinessRepositoryProxy<TRepository, TKey, TView, TViewForAdd, TViewForUpdate, TView, TViewForAdd, TViewForUpdate>, 
        IDeletableBusinessImplementationWithBase<TKey, TView, TViewForAdd, TViewForUpdate>
        where TRepository: IDeletableRepository<TKey, TView, TViewForAdd, TViewForUpdate>
        where TView: IHasKey<TKey>
        where TViewForAdd : IHasKey<TKey>
        where TViewForUpdate: IHasKey<TKey>
    {
        public DeletableBusinessRepositoryProxyWithBase(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
        protected override TViewForAdd CreateViewForAdd(TViewForAdd model)
        {
            return model;
        }
        protected override TViewForUpdate CreateViewForUpdate(TViewForUpdate model)
        {
            return model;
        }
        protected override TView CreateModel(TView view)
        {
            return view;
        }
    }
    public class DeletableBusinessRepositoryProxyWithBase<TRepository, TKey, TView, TViewBase> : DeletableBusinessRepositoryProxyWithBase<TRepository, TKey, TView, TViewBase, TViewBase>,
        IDeletableBusinessImplementationWithBase<TKey, TView, TViewBase>
        where TRepository: IDeletableRepository<TKey, TView, TViewBase, TViewBase>
        where TView: IHasKey<TKey>
        where TViewBase: IHasKey<TKey>
    {
        public DeletableBusinessRepositoryProxyWithBase(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
    }
}