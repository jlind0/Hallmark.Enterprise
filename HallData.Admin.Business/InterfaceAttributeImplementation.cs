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
    public class ReadOnlyInterfaceAttributeImplementation : ReadOnlyBusinessRepositoryProxy<IReadOnlyInterfaceAttributeRepository, InterfaceAttributeResult>, IReadOnlyInterfaceAttributeImplementation
    {
        public ReadOnlyInterfaceAttributeImplementation(IReadOnlyInterfaceAttributeRepository repository, ISecurityImplementation security) : base(repository, security) { }
        public async Task<QueryResults<InterfaceAttributeResult>> GetByInterface(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            var userId = await this.ActivateAndGetSignedInUserGuid(token);
            return await this.Repository.GetByInterface(interfaceId, RecursionLevel.None, viewName, userId, filter, sort, page, token);
        }

        public Task<QueryResults<InterfaceAttributeResult>> GetByInterfaceWithDownwardRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByInterface(interfaceId, RecursionLevel.Down, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<InterfaceAttributeResult>> GetByInterfaceWithUpwardRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByInterface(interfaceId, RecursionLevel.Up, viewName, userId, filter, sort, page, token), token);
        }

        public Task<QueryResults<InterfaceAttributeResult>> GetByInterfaceWithBiDirectionalRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.GetByInterface(interfaceId, RecursionLevel.BiDirectional, viewName, userId, filter, sort, page, token), token);
        }
    }
    public class InterfaceAttributeImplementationBase : BusinessProxy, IInterfaceAttributeImplementationBase
    {
        protected IReadOnlyInterfaceAttributeImplementation ReadOnly { get; private set; }
        protected IReadOnlyInterfaceImplementation Interface { get; private set; }
        public InterfaceAttributeImplementationBase(ISecurityImplementation security, IReadOnlyInterfaceAttributeImplementation readOnly, IReadOnlyInterfaceImplementation interfaceImpl)
            : base(security)
        {
            this.ReadOnly = readOnly;
            this.Interface = interfaceImpl;
        }
        public async Task<bool> CanInterfaceAttributesCompose(IEnumerable<InterfaceAttributeResult> attributes, List<IEnumerable<int>> processedInterfaceRelations = null, CancellationToken token = default(CancellationToken))
        {
            if (processedInterfaceRelations == null)
                processedInterfaceRelations = new List<IEnumerable<int>>();
            if (attributes.GetMismatchedComposedAttributes().Count() > 0)
                return false;
            foreach (var mismatchedAttributeGroup in attributes.GetTypeMismatchedComposedAttributes().GroupBy(g => g.Key.Name.ToLower()))
            {
                List<InterfaceAttributeResult> mismatchedAttributes = new List<InterfaceAttributeResult>();
                var typesWithNull = mismatchedAttributeGroup.Select(g => g.Key.TypeId).Distinct();
                if (typesWithNull.Any(t => t == null))
                    return false;
                var types = typesWithNull.Where(t => t != null).Select(t => t.Value);
                if (!processedInterfaceRelations.Any(pir => pir.Count() == types.Count() && pir.All(p => types.Any(t => t == p))))
                {
                    if (!await this.Interface.AreInterfacesCommon(types, token))
                        return false;
                    processedInterfaceRelations.Add(types);
                    foreach (var typeId in types)
                    {
                        mismatchedAttributes.AddRange((await ReadOnly.GetByInterfaceWithBiDirectionalRecursion(typeId, token: token)).Results);
                    }
                    if (!await this.CanInterfaceAttributesCompose(mismatchedAttributes, processedInterfaceRelations, token))
                        return false;
                }
            }
            return true;
        }
    }
    public class InterfaceAttributeImplementation : DeletableBusinessRepositoryProxyWithBase<IInterfaceAttributeRepository, int, InterfaceAttributeResult, InterfaceAttributeForAdd, InterfaceAttributeForUpdate>,
        IInterfaceAttributeImplementation
    {
        protected IReadOnlyInterfaceAttributeImplementation ReadOnly { get; private set; }
        protected IInterfaceAttributeImplementationBase Base { get; private set; }
        protected IReadOnlyDataViewColumnImplementation Column { get; private set; }
        protected IReadOnlyDataViewResultImplementation Result { get; private set; }
        protected virtual bool IsPassThrough
        {
            get { return false; }
        }
        public InterfaceAttributeImplementation(IInterfaceAttributeRepository repository, ISecurityImplementation security, IReadOnlyInterfaceAttributeImplementation readOnly, 
            IInterfaceAttributeImplementationBase baseImpl, IReadOnlyDataViewColumnImplementation column, IReadOnlyDataViewResultImplementation result)
            : base(repository, security)
        {
            this.ReadOnly = readOnly;
            this.Base = baseImpl;
            this.Column = column;
            this.Result = result;
        }
        public override Task<QueryResult<InterfaceAttributeResult>> Get(int id, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.Get(id, token);
        }
        public override Task<QueryResults<InterfaceAttributeResult>> GetMany(string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
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

        public Task<QueryResults<InterfaceAttributeResult>> GetByInterface(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.GetByInterface(interfaceId, viewName, filter, sort, page, token);
        }
        protected override async Task Add(InterfaceAttributeForAdd view, Guid? userId, CancellationToken token)
        {
            if (!this.IsPassThrough)
            {
                view.Validate();
                var attributesResult = await this.GetByInterfaceWithBiDirectionalRecursion(view.Interface.InterfaceId.Value, token: token);
                if (attributesResult.Results.Where(r => r.Interface.InterfaceId == view.Interface.InterfaceId).Any(r => string.Equals(r.Name, view.Name, StringComparison.InvariantCultureIgnoreCase)))
                    throw new GlobalizedValidationException("ADMIN_INTERFACEATTRIBUTES_ADD_NAME_NOT_UNIQUE");
                List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>(attributesResult.Results);
                attributes.Add(view.CreateRelatedInstance<InterfaceAttributeResult>());
                if (!await this.CanInterfaceAttributesCompose(attributes, token: token))
                    throw new GlobalizedValidationException("ADMIN_INTERFACEATTRIBUTES_ADD_CAUSESMISMATCH");
            }
            await base.Add(view, userId, token);
        }
        protected override async Task Update(InterfaceAttributeForUpdate view, Guid? userId, CancellationToken token)
        {
            if (!this.IsPassThrough)
            {
                view.Validate();
                var attribute = await this.Get(view.InterfaceAttributeId.Value, token);
                if (attribute == null)
                    throw new GlobalizedValidationException("ADMIN_INTERFACEATTRIBUTE_UPDATE_NOTFOUND");
                bool nameChanged = !string.Equals(view.Name, attribute.Result.Name, StringComparison.InvariantCultureIgnoreCase);
                int? newTypeId = view.Type != null ? view.Type.InterfaceId : null as int?;
                int? oldTypeId = attribute.Result.Type != null ? attribute.Result.Type.InterfaceId : null as int?;
                if (nameChanged || newTypeId != oldTypeId)
                {
                    var attributesResult = await this.GetByInterfaceWithBiDirectionalRecursion(attribute.Result.Interface.InterfaceId.Value, token: token);
                    if (nameChanged && attributesResult.Results.Where(r => r.Interface.InterfaceId == attribute.Result.Interface.InterfaceId).Any(
                        r => string.Equals(r.Name, view.Name, StringComparison.InvariantCultureIgnoreCase)))
                        throw new GlobalizedValidationException("ADMIN_INTERFACEATTRIBUTES_ADD_NAME_NOT_UNIQUE");
                    List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>(attributesResult.Results.Where(r => r.InterfaceAttributeId != view.InterfaceAttributeId));
                    attributes.Add(view.CreateRelatedInstance<InterfaceAttributeResult>());
                    if (!await this.CanInterfaceAttributesCompose(attributes, token: token))
                        throw new GlobalizedValidationException("ADMIN_INTERFACEATTRIBUTES_ADD_CAUSESMISMATCH");
                }
            }
            await base.Update(view, userId, token);
        }
        protected override Task<ChangeStatusQueryResult<InterfaceAttributeResult>> ChangeStatus(int id, string statusTypeName, bool force, CancellationToken token)
        {
            throw new NotImplementedException();
        }
        public Task<bool> CanInterfaceAttributesCompose(IEnumerable<InterfaceAttributeResult> attributes, List<IEnumerable<int>> processedInterfaceRelations = null, CancellationToken token = default(CancellationToken))
        {
            return this.Base.CanInterfaceAttributesCompose(attributes, processedInterfaceRelations, token);
        }


        public Task<QueryResults<InterfaceAttributeResult>> GetByInterfaceWithDownwardRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByInterfaceWithDownwardRecursion(interfaceId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<InterfaceAttributeResult>> GetByInterfaceWithUpwardRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByInterfaceWithUpwardRecursion(interfaceId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<InterfaceAttributeResult>> GetByInterfaceWithBiDirectionalRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.GetByInterfaceWithBiDirectionalRecursion(interfaceId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetDataViewResultsByCollection(int interfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Result.GetByCollectionAttribute(interfaceAttributeId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetDataViewResultsByCollectionWithDownwardRecursion(int interfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Result.GetByCollectionAttributeWithDownwardRecursion(interfaceAttributeId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetDataViewResultsByCollectionWithUpwardRecursion(int interfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Result.GetByCollectionAttributeWithUpwardRecursion(interfaceAttributeId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<DataViewResultResult>> GetDataViewResultsByCollectionWithBiDirectionalRecursion(int interfaceAttributeId, string viewName = null, FilterContext<DataViewResultResult> filter = null, SortContext<DataViewResultResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Result.GetByCollectionAttributeWithBiDirectionalRecursion(interfaceAttributeId, viewName, filter, sort, page, token);
        }


        public Task<QueryResults<DataViewColumnResult>> GetColumns(int interfaceAttributeId, string viewName = null, FilterContext<DataViewColumnResult> filter = null, SortContext<DataViewColumnResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Column.GetByInterfaceAttribute(interfaceAttributeId, viewName, filter, sort, page, token);
        }
    }
    public class InterfaceAttributePassThroughImplementation : InterfaceAttributeImplementation, IInterfaceAttributePassThroughImplementation
    {
        public InterfaceAttributePassThroughImplementation(IInterfaceAttributeRepository repository, ISecurityImplementation security, IReadOnlyInterfaceAttributeImplementation readOnly,
            IInterfaceAttributeImplementationBase baseImpl, IReadOnlyDataViewColumnImplementation column, IReadOnlyDataViewResultImplementation result)
            : base(repository, security, readOnly, baseImpl, column, result) { }
        protected override bool IsPassThrough
        {
            get
            {
                return true;
            }
        }
    }
}
