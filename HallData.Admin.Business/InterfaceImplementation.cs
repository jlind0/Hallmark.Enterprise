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
    public class ReadOnlyInterfaceImplementation : ReadOnlyBusinessRepositoryProxy<IReadOnlyInterfaceRepository, InterfaceResult>, IReadOnlyInterfaceImplementation
    {
        public ReadOnlyInterfaceImplementation(IReadOnlyInterfaceRepository repository, ISecurityImplementation security) : base(repository, security) { }

        public Task<bool> DoesNameExist(string name, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(async userId => await this.Repository.GetByName(name, userId, token) != null, token);
        }

        public Task<bool> AreInterfacesCommon(int interfaceId1, int interfaceId2, CancellationToken token = default(CancellationToken))
        {
            return ExecuteQuery(userId => this.Repository.AreInterfacesCommon(interfaceId1, interfaceId2, userId, token));
        }


        public async Task<bool> AreInterfacesCommon(IEnumerable<int> interfaceIds, CancellationToken token = default(CancellationToken))
        {
            List<Tuple<int, int>> procesedTypes = new List<Tuple<int, int>>();
            foreach (var type1 in interfaceIds)
            {
                foreach (var type2 in interfaceIds.Where(t => t != type1))
                {
                    if (procesedTypes.Contains(new Tuple<int, int>(type1, type2)) || procesedTypes.Contains(new Tuple<int, int>(type2, type1)))
                        continue;
                    procesedTypes.Add(new Tuple<int, int>(type1, type2));
                    if (!await this.AreInterfacesCommon(type1, type2))
                        return false;
                }
            }
            return true;
        }
    }
    public class InterfaceImplementationBase : BusinessProxy, IInterfaceImplementationBase
    {
        protected IReadOnlyInterfaceImplementation ReadOnly { get; private set; }
        protected IInterfaceAttributeImplementationBase AttributeBase{ get; private set; }
        protected IReadOnlyInterfaceAttributeImplementation ReadOnlyAttribute { get; private set; }
        public InterfaceImplementationBase(ISecurityImplementation security, IReadOnlyInterfaceImplementation readOnly, IInterfaceAttributeImplementationBase attributeBase, IReadOnlyInterfaceAttributeImplementation attributeReadOnly) 
            :base(security)
        {
            this.ReadOnly = readOnly;
            this.AttributeBase = attributeBase;
            this.ReadOnlyAttribute = attributeReadOnly;
        }


        public Task<bool> CanInterfacesRelate(int interfaceId, int relatedInterfaceId, CancellationToken token = default(CancellationToken))
        {
            return this.CanInterfacesRelate(new int[] { interfaceId, relatedInterfaceId }, token);
        }

        public async Task<bool> CanInterfacesRelate(IEnumerable<int> interfaceIds, CancellationToken token = default(CancellationToken))
        {
            if (interfaceIds.Distinct().Count() != interfaceIds.Count())
                return false;
            List<InterfaceAttributeResult> results = new List<InterfaceAttributeResult>();
            foreach (var id in interfaceIds)
                results.AddRange((await this.ReadOnlyAttribute.GetByInterfaceWithBiDirectionalRecursion(id, token: token)).Results);
            return await this.AttributeBase.CanInterfaceAttributesCompose(results, token: token);
        }
    }
    public class InterfaceImplementation : DeletableBusinessRepositoryProxyWithBase<IInterfaceRepository, int, InterfaceResult, InterfaceForAdd, InterfaceForUpdate>, IInterfaceImplementation
    {
        protected IReadOnlyInterfaceImplementation ReadOnly { get; private set; }
        protected IInterfaceAttributeImplementation Attribute { get; private set; }
        protected IInterfaceImplementationBase Base { get; private set; }
        protected virtual bool IsPassThrough { get { return false; } }
        public InterfaceImplementation(IInterfaceRepository repository, ISecurityImplementation security, IReadOnlyInterfaceImplementation readOnly, IInterfaceImplementationBase baseImpl, IInterfaceAttributePassThroughImplementation attribute)
            :base(repository, security)
        {
            this.ReadOnly = readOnly;
            this.Base = baseImpl;
            this.Attribute = attribute;
        }
        public override Task<QueryResult<InterfaceResult>> Get(int id, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.Get(id, token);
        }
        public override Task<QueryResults<InterfaceResult>> GetMany(string viewName = null, FilterContext<InterfaceResult> filter = null, SortContext<InterfaceResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.GetMany(viewName, filter, sort, page, token);
        }
        public override Task<QueryResults<JObject>> GetManyView(string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return ReadOnly.GetManyView(viewName, filter, sort, page, token);
        }
        public Task<QueryResults<InterfaceAttributeResult>> GetAttributes(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Attribute.GetByInterface(interfaceId, viewName, filter, sort, page, token);
        }

        public async Task<QueryResult<InterfaceResult>> RelateInterfaces(int interfaceId, int releatedInterfaceId, CancellationToken token = default(CancellationToken))
        {
            if(!this.IsPassThrough)
            {
                if (!await this.Base.CanInterfacesRelate(interfaceId, releatedInterfaceId, token))
                    throw new GlobalizedValidationException("ADMIN_INTERFACES_RELATE_NOTVALID");
            }
            return await ExecuteAction(userId => RelateInterfaces(interfaceId, releatedInterfaceId, userId, token), userId => this.Get(interfaceId, token), token);
        }
        protected Task RelateInterfaces(int interfaceId, int relatedInterfaceId, Guid? userId, CancellationToken token)
        {
            return this.Repository.RelateInterfaces(interfaceId, relatedInterfaceId, userId, token);
        }
        public Task<bool> DoesNameExist(string name, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.DoesNameExist(name, token);
        }

        public Task<bool> AreInterfacesCommon(int interfaceId1, int interfaceId2, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.AreInterfacesCommon(interfaceId1, interfaceId2, token);
        }

        public Task<bool> CanInterfacesRelate(int interfaceId, int relatedInterfaceId, CancellationToken token = default(CancellationToken))
        {
            return this.Base.CanInterfacesRelate(interfaceId, relatedInterfaceId, token);
        }

        public Task<bool> CanInterfacesRelate(IEnumerable<int> interfaceIds, CancellationToken token = default(CancellationToken))
        {
            return this.Base.CanInterfacesRelate(interfaceIds, token);
        }
        protected override async Task Add(InterfaceForAdd view, Guid? userId, CancellationToken token)
        {
            if (!this.IsPassThrough)
            {
                view.Validate();
                if (await this.DoesNameExist(view.Name, token))
                    throw new GlobalizedValidationException("ADMIN_INTERFACE_ADD_NAME_DUPLICATE");
                if (view.RelatedInterfaces != null && view.RelatedInterfaces.Count > 0)
                {
                    if (view.RelatedInterfaces.Select(ri => ri.InterfaceId.Value).Distinct().Count() != view.RelatedInterfaces.Count)
                        throw new GlobalizedValidationException("ADMIN_INTERFACE_ADD_RELATEDINTERFACE_DUPLICATE");
                    if (view.Attributes != null && view.Attributes.Count > 0)
                    {
                        List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>();
                        foreach (var ri in view.RelatedInterfaces)
                            attributes.AddRange((await this.GetAttributesWithBiDirectionalRecursion(ri.InterfaceId.Value, token: token)).Results);

                        attributes.AddRange(view.Attributes.Select(a => a.CreateRelatedInstance<InterfaceAttributeResult>()));
                        if (!await this.Attribute.CanInterfaceAttributesCompose(attributes, token: token))
                            throw new GlobalizedValidationException("ADMIN_INTERFACE_ADD_ATTRIBUTE_CONFLICT");
                    }
                    else if (view.RelatedInterfaces.Count > 1 && !await this.CanInterfacesRelate(view.RelatedInterfaces.Select(ri => ri.InterfaceId.Value).Distinct(), token))
                        throw new GlobalizedValidationException("ADMIN_INTERFACE_ADD_ATTRIBUTE_CONFLICT");
                }
            }
            await base.Add(view, userId, token);
            if(view.RelatedInterfaces != null)
            {
                foreach (var i in view.RelatedInterfaces)
                    await this.RelateInterfaces(view.InterfaceId.Value, i.InterfaceId.Value, userId, token);
            }
            if(view.Attributes != null)
            {
                foreach (var a in view.Attributes)
                {
                    a.Interface = new InterfaceKey();
                    a.Interface.InterfaceId = view.InterfaceId;
                    await this.Attribute.Add(a, token);
                }
            }

        }
        protected override async Task Update(InterfaceForUpdate view, Guid? userId, CancellationToken token)
        {
            if (!this.IsPassThrough)
            {
                view.Validate();
                var oldInterface = await this.Get(view.InterfaceId.Value, token);
                if (oldInterface == null)
                    throw new GlobalizedValidationException("ADMIN_INTERFACE_UPDATE_NOT_FOUND");
                if (!string.Equals(view.Name, oldInterface.Result.Name, StringComparison.InvariantCultureIgnoreCase))
                    throw new GlobalizedValidationException("ADMIN_INTERFACE_UPDATE_NAME_DUPLICATE");
                IEnumerable<int> relatedInterfaceIds = oldInterface.Result.RelatedInterfaces.Select(ri => ri.InterfaceId.Value);
                if (view.RelatedInterfaces != null && view.RelatedInterfaces.Add.Count > 0)
                {
                    var newRelatedInterfaceIds = view.RelatedInterfaces.Add.Select(ri => ri.InterfaceId.Value);
                    var oldRelatedInterfaceIds = oldInterface.Result.RelatedInterfaces.Select(ri => ri.InterfaceId.Value);
                    if (oldRelatedInterfaceIds.Except(newRelatedInterfaceIds).Count() != oldRelatedInterfaceIds.Count())
                        throw new GlobalizedValidationException("ADMIN_INTERFACE_UPDATE_RELATEDINTERFACE_ADD_EXISTS");
                    relatedInterfaceIds = oldRelatedInterfaceIds.Union(newRelatedInterfaceIds).Except(view.RelatedInterfaces.Remove.Select(ri => ri.InterfaceId.Value));
                }
                if ((view.RelatedInterfaces != null && view.RelatedInterfaces.Add.Count > 0) || (view.Attributes != null && (view.Attributes.Add.Count > 0 || view.Attributes.Update.Count > 0)))
                {
                    List<InterfaceAttributeResult> attributes = new List<InterfaceAttributeResult>((await this.Attribute.GetByInterfaceWithBiDirectionalRecursion(view.InterfaceId.Value, token: token)).Results);
                    foreach (var i in relatedInterfaceIds)
                        attributes.AddRange((await this.Attribute.GetByInterfaceWithBiDirectionalRecursion(i, token: token)).Results);
                    if (view.Attributes != null)
                    {
                        attributes.AddRange(view.Attributes.Add.Select(a => a.CreateRelatedInstance<InterfaceAttributeResult>()));
                        attributes.RemoveAll(a => view.Attributes.Update.Any(u => u.InterfaceAttributeId == a.InterfaceAttributeId) ||
                            view.Attributes.HardDelete.Any(d => d.InterfaceAttributeId == a.InterfaceAttributeId) ||
                            view.Attributes.SoftDelete.Any(d => d.InterfaceAttributeId == a.InterfaceAttributeId));
                        attributes.AddRange(view.Attributes.Update.Select(u => u.CreateRelatedInstance<InterfaceAttributeResult>()));
                    }
                    if (!await this.Attribute.CanInterfaceAttributesCompose(attributes, token: token))
                        throw new GlobalizedValidationException("ADMIN_INTERFACE_UPDATE_ATTRIBUTE_CONFLICT");
                }
            }
            await base.Update(view, userId, token);
            if(view.RelatedInterfaces != null)
            {
                foreach (var ri in view.RelatedInterfaces.Remove)
                    await this.UnRelateInterfaces(view.InterfaceId.Value, ri.InterfaceId.Value, token);
                foreach (var ri in view.RelatedInterfaces.Add)
                    await this.RelateInterfaces(view.InterfaceId.Value, ri.InterfaceId.Value, token);
            }
            if(view.Attributes != null)
            {
                foreach (var a in view.Attributes.SoftDelete)
                    await this.Attribute.DeleteSoft(a.InterfaceAttributeId.Value, token);
                foreach (var a in view.Attributes.HardDelete)
                    await this.Attribute.DeleteHard(a.InterfaceAttributeId.Value, token);
                foreach (var a in view.Attributes.Add)
                {
                    a.Interface = new InterfaceKey();
                    a.Interface.InterfaceId = view.InterfaceId;
                    await this.Attribute.Add(a, token);
                }
                foreach (var a in view.Attributes.Update.Select(u => u.CreateRelatedInstance<InterfaceAttributeForUpdate>()))
                    await this.Attribute.Update(a, token);
            }
        }


        public Task UnRelateInterfaces(int interfaceId, int releatedInterfaceId, CancellationToken token = default(CancellationToken))
        {
            return ExecuteAction(userId => this.Repository.UnRelateInterfaces(interfaceId, releatedInterfaceId, userId, token), token);
        }


        public Task<QueryResults<InterfaceAttributeResult>> GetAttributesWithDownwardRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Attribute.GetByInterfaceWithDownwardRecursion(interfaceId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<InterfaceAttributeResult>> GetAttributesWithUpwardRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Attribute.GetByInterfaceWithUpwardRecursion(interfaceId, viewName, filter, sort, page, token);
        }

        public Task<QueryResults<InterfaceAttributeResult>> GetAttributesWithBiDirectionalRecursion(int interfaceId, string viewName = null, FilterContext<InterfaceAttributeResult> filter = null, SortContext<InterfaceAttributeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            return this.Attribute.GetByInterfaceWithBiDirectionalRecursion(interfaceId, viewName, filter, sort, page, token);
        }


        public Task<bool> AreInterfacesCommon(IEnumerable<int> interfaceIds, CancellationToken token = default(CancellationToken))
        {
            return this.ReadOnly.AreInterfacesCommon(interfaceIds, token);
        }
    }
}
