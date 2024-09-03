using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Repository;
using HallData.Security;

namespace HallData.Business
{
    public class BusinessRepositoryProxyWithBase<TRepository, TKey, TView, TViewForAdd, TViewForUpdate> : BusinessRepositoryProxy<TRepository, TKey, TView, TViewForAdd, TViewForUpdate, TView, TViewForAdd, TViewForUpdate>,
        IBusinessImplementationWithBase<TKey, TView, TViewForAdd, TViewForUpdate>
        where TView : IHasKey<TKey>
        where TViewForAdd: IHasKey<TKey>
        where TViewForUpdate: IHasKey<TKey>
        where TRepository : IRepository<TKey, TView, TViewForAdd, TViewForUpdate>
    {
        public BusinessRepositoryProxyWithBase(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
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
    public class BusinessRepositoryProxyWithBase<TRepository, TKey, TView, TViewBase> : BusinessRepositoryProxyWithBase<TRepository, TKey, TView, TViewBase, TViewBase>, 
        IBusinessImplementationWithBase<TKey, TView, TViewBase>
        where TRepository : IRepository<TKey, TView, TViewBase, TViewBase>
        where TView : IHasKey<TKey>
        where TViewBase : IHasKey<TKey>
    {
        public BusinessRepositoryProxyWithBase(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
    }
}