using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Repository;
using HallData.Security;
using System;

namespace HallData.Business
{
    public class DeletableBusinessRepositoryProxy<TRepository, TView> : DeletableBusinessRepositoryProxy<TRepository, int, TView>, IDeletableBusinessImplementation<TView>
        where TRepository : IDeletableRepository<int, TView, TView, TView>
        where TView : IHasKey<int>
    {
        public DeletableBusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
    }

    public class DeletableBusinessRepositoryProxy<TRepository, TKey, TView> : DeletableBusinessRepositoryProxy<TRepository, TKey, TView, TView, TView, TView>, IDeletableBusinessImplementation<TKey, TView>
        where TRepository : IDeletableRepository<TKey, TView, TView, TView>
        where TView : IHasKey<TKey>
    {
        public DeletableBusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }

        protected override TView CreateViewForAdd(TView model)
        {
            return model;
        }
        protected override TView CreateViewForUpdate(TView model)
        {
            return model;
        }
        protected override TView CreateModel(TView view)
        {
            return view;
        }
    }

    public abstract class DeletableBusinessRepositoryProxy<TRepository, TKey, TView, TModel> : DeletableBusinessRepositoryProxy<TRepository, TKey, TView, TView, TModel, TModel>
        where TRepository : IDeletableRepository<TKey, TView, TView, TView>
        where TView : IHasKey<TKey>
        where TModel: IHasKey<TKey>
    {
        public DeletableBusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
    }
    public abstract class DeletableBusinessRepositoryProxy<TRepository, TKey, TView, TViewBase, TModel, TModelBase> : DeletableBusinessRepositoryProxy<TRepository, TKey, TView, TViewBase, TViewBase, TModel, TModelBase, TModelBase>,
        IDeletableBusinessImplementation<TKey, TView, TModel, TModelBase>
        where TRepository: IDeletableRepository<TKey, TView, TViewBase, TViewBase>
        where TView: IHasKey<TKey>
        where TViewBase: IHasKey<TKey>
        where TModel: IHasKey<TKey>
        where TModelBase: IHasKey<TKey>
    {
        public DeletableBusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
    }
    public abstract class DeletableBusinessRepositoryProxy<TRepository, TKey, TView, TViewForAdd, TViewForUpdate, TModel, TModelForAdd, TModelForUpdate> : 
        BusinessRepositoryProxy<TRepository, TKey, TView, TViewForAdd, TViewForUpdate, TModel, TModelForAdd, TModelForUpdate>, 
        IDeletableBusinessImplementation<TKey, TView, TModel, TModelForAdd, TModelForUpdate>
        where TRepository : IDeletableRepository<TKey, TView, TViewForAdd, TViewForUpdate>
        where TView : IHasKey<TKey>
        where TViewForAdd : IHasKey<TKey>
        where TViewForUpdate: IHasKey<TKey>
        where TModel : IHasKey<TKey>
        where TModelForAdd : IHasKey<TKey>
        where TModelForUpdate: IHasKey<TKey>
    {
        public DeletableBusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
        protected virtual bool CascadeDelete
        {
            get { return false; }
        }
        protected virtual Task Delete(TKey id, Guid? userId, bool isHard, CancellationToken token)
        {
            return this.Repository.Delete(id, userId, this.CascadeDelete, isHard, token);
        }
        public virtual Task DeleteSoft(TKey id, CancellationToken token = default(CancellationToken))
        {
            return ExecuteAction(userId => this.Delete(id, userId, false, token), token);
        }
        public virtual Task DeleteHard(TKey id, CancellationToken token = default(CancellationToken))
        {
            return ExecuteAction(userId => this.Delete(id, userId, true, token), token);
        }
    }
}