using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Enums;
using HallData.ApplicationViews;
using HallData.EMS.Data;
using HallData.Repository;
using HallData.Business;
using System.Threading;
using Newtonsoft.Json.Linq;
using HallData.Security;
using System.Collections.Concurrent;
using System.Reflection;
using HallData.Validation;
using HallData.Exceptions;

namespace HallData.EMS.Business
{
    public abstract class GenericContactImplementation<TRepoistory, TKey, TId, THolderBase, TAddressResult, TAddressForAdd, TAddressForUpdate,
        TEmailResult, TEmailForAdd, TEmailForUpdate,
        TPhoneResult, TPhoneForAdd, TPhoneForUpdate,
        TGenericResult, TGenericForAdd, TGenericForUpdate> : BusinessRepositoryProxy<TRepoistory>,
        IGenericContactImplementation<TKey, TId, THolderBase, TAddressResult, TAddressForAdd, TAddressForUpdate,
            TEmailResult, TEmailForAdd, TEmailForUpdate,
            TPhoneResult, TPhoneForAdd, TPhoneForUpdate,
            TGenericResult, TGenericForAdd, TGenericForUpdate>
        where TRepoistory: IContactHolderRepository<TKey, TId, THolderBase>
        where THolderBase : IContactMechanismHolderKey<TKey>
        where TKey: IContactMechanismHolderId<TId>
        where TAddressResult : IContactMechanismHolderResult<TKey, Address>, THolderBase
        where TAddressForAdd : IContactMechanismHolderForAdd<TKey, AddressForAdd>, THolderBase
        where TAddressForUpdate : IContactMechanismHolderForUpdate<TKey, AddressForUpdate>, THolderBase
        where TEmailResult : IContactMechanismHolderResult<TKey, Email>, THolderBase
        where TEmailForAdd : IContactMechanismHolderForAdd<TKey, EmailForAdd>, THolderBase
        where TEmailForUpdate : IContactMechanismHolderForUpdate<TKey, EmailForUpdate>, THolderBase
        where TPhoneResult : IContactMechanismHolderResult<TKey, Phone>, THolderBase
        where TPhoneForAdd : IContactMechanismHolderForAdd<TKey, PhoneForAdd>, THolderBase
        where TPhoneForUpdate : IContactMechanismHolderForUpdate<TKey, PhoneForUpdate>, THolderBase
        where TGenericResult : IContactMechanismHolderResult<TKey, ContactMechanismGeneric>, THolderBase
        where TGenericForAdd : IContactMechanismHolderForAdd<TKey, ContactMechanismGenericForAdd>, THolderBase
        where TGenericForUpdate : IContactMechanismHolderForUpdate<TKey, ContactMechanismGenericForUpdate>, THolderBase
    {
        protected IContactMechanismRepository ContactMechanismRepository { get; private set; }
        private static readonly Lazy<ConcurrentDictionary<Type, ContactMechanismHolderDefaultViewAttribute>> HolderDefaultViewMappingLazy = 
            new Lazy<ConcurrentDictionary<Type, ContactMechanismHolderDefaultViewAttribute>>(() => new ConcurrentDictionary<Type, ContactMechanismHolderDefaultViewAttribute>(), LazyThreadSafetyMode.PublicationOnly);
        private static readonly Lazy<ConcurrentDictionary<Type, ContactMechanismTypeAttribute>> ContactMechanismTypeMappingLazy = new Lazy<ConcurrentDictionary<Type, ContactMechanismTypeAttribute>>(() =>
            new ConcurrentDictionary<Type, ContactMechanismTypeAttribute>(), LazyThreadSafetyMode.PublicationOnly);
        private static ConcurrentDictionary<Type, ContactMechanismHolderDefaultViewAttribute> HolderDefaultViewMapping
        {
            get { return HolderDefaultViewMappingLazy.Value; }
        }
        private static ConcurrentDictionary<Type, ContactMechanismTypeAttribute> ContactMechanismTypeMapping
        {
            get { return ContactMechanismTypeMappingLazy.Value; }
        }
        public GenericContactImplementation(TRepoistory repository, ISecurityImplementation security, IContactMechanismRepository contactMechanismRepository) : base(repository, security)
        {
            this.ContactMechanismRepository = contactMechanismRepository;
        }
        protected abstract TKey CreateKey(TId id, Guid contactMechanismId, string contactMechanismTypeName);

        protected virtual Task<QueryResults<TResultHolder>> GetContactMechanisms<TResultHolder, TContactMechanism>(MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, FilterContext<TResultHolder> filter = null, SortContext<TResultHolder> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
            where TResultHolder: THolderBase, IContactMechanismHolderResult<TKey, TContactMechanism>
            where TContactMechanism: IContactMechanismResult
        {
            viewName = viewName ?? GetDefaultViewNameMany<TResultHolder, TContactMechanism>();
            return this.ExecuteQuery(userId => this.Repository.GetContactMechanisms<TResultHolder, TContactMechanism>(mechanismType, contactMechanismTypeName, viewName, userId, filter, sort, page, token), token);
        }
        protected virtual Task<QueryResults<JObject>> GetContactMechanismsView<TResultHolder, TContactMechanism>(MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
            where TResultHolder : THolderBase, IContactMechanismHolderResult<TKey, TContactMechanism>
            where TContactMechanism : IContactMechanismResult
        {
            viewName = viewName ?? GetDefaultViewNameMany<TResultHolder, TContactMechanism>();
            return this.ExecuteQuery(userId => this.Repository.GetContactMechanismsView(mechanismType, contactMechanismTypeName, viewName, userId, filter, sort, page, token), token);
        }
        protected virtual Task<QueryResults<TResultHolder>> GetContactMechanisms<TResultHolder, TContactMechanism>(TId id, MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, FilterContext<TResultHolder> filter = null, SortContext<TResultHolder> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
            where TResultHolder : THolderBase, IContactMechanismHolderResult<TKey, TContactMechanism>
            where TContactMechanism : IContactMechanismResult
        {
            viewName = viewName ?? GetDefaultViewNameMany<TResultHolder, TContactMechanism>();
            return this.ExecuteQuery(userId => this.Repository.GetContactMechanismsById<TResultHolder, TContactMechanism>(id, mechanismType, contactMechanismTypeName, viewName, userId, filter, sort, page, token), token);
        }
        protected virtual Task<QueryResults<JObject>> GetContactMechanismsView<TResultHolder, TContactMechanism>(TId id, MechanismTypes? mechanismType = null, string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
            where TResultHolder : THolderBase, IContactMechanismHolderResult<TKey, TContactMechanism>
            where TContactMechanism : IContactMechanismResult
        {
            viewName = viewName ?? GetDefaultViewNameMany<TResultHolder, TContactMechanism>();
            return this.ExecuteQuery(userId => this.Repository.GetContactMechanismsByIdView(id, mechanismType, contactMechanismTypeName, viewName, userId, filter, sort, page, token), token);
        }

        protected virtual Task<QueryResult<THolder>> GetContactMechanism<THolder, TContactMechanism>(TId id, Guid contactMechanismId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
            where THolder : THolderBase, IContactMechanismHolderResult<TKey, TContactMechanism>
            where TContactMechanism: IContactMechanismResult
        {
            return GetContactMechanism<THolder, TContactMechanism>(CreateKey(id, contactMechanismId, contactMechanismTypeName), viewName, token);
        }
        protected virtual async Task<QueryResult<THolder>> GetContactMechanism<THolder, TContactMechanism>(TKey key, string viewName = null, CancellationToken token = default(CancellationToken))
            where THolder : THolderBase, IContactMechanismHolderResult<TKey, TContactMechanism>
            where TContactMechanism: IContactMechanismResult
        {
            viewName = viewName ?? this.GetDefaultViewNameSingle<THolder, TContactMechanism>();
            var result = await ExecuteQuery(u => this.Repository.GetContactMechanism<THolder, TContactMechanism>(key, viewName, u, token), token);
            if(result != null)
                ValidateContactMechanism<THolder, TContactMechanism>(result.Result);
            return result;
        }
        protected virtual void ValidateContactMechanism<THolder, TContactMechanism>(THolder result)
            where THolder : THolderBase, IContactMechanismHolderResult<TKey, TContactMechanism>
            where TContactMechanism: IContactMechanismResult
        {
            if (result != null && result.ContactMechanism != null && result.ContactMechanism.MechanismType != null && result.ContactMechanism.MechanismType.MechanismTypeId != null)
            {
                var attr = GetContactMechanismTypeAttribute<TContactMechanism>();
                if (attr != null && attr.MechanismType != (MechanismTypes)result.ContactMechanism.MechanismType.MechanismTypeId.Value)
                    throw new GlobalizedValidationException(attr.ErrorCode);
            }
        }
        protected virtual void ValidateContactMechanism<TContactMechanism>(JObject result)
            where TContactMechanism: IContactMechanismResult
        {
            dynamic r = result;
            if (r.ContactMechanism != null && r.ContactMechanism.MechanismType != null)
            {
                int? mechanismType = r.ContactMechanism.MechanismType.MechanismTypeId as int?;
                if (mechanismType != null)
                {
                    var attr = GetContactMechanismTypeAttribute<TContactMechanism>();
                    if (attr != null && attr.MechanismType != (MechanismTypes)mechanismType.Value)
                        throw new GlobalizedValidationException(attr.ErrorCode);
                }
            }
        }
        protected async virtual Task<QueryResult<JObject>> GetContactMechanismView<THolder, TContactMechanism>(TId id, Guid contactMechanismId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
            where THolder : THolderBase, IContactMechanismHolderResult<TKey, TContactMechanism>
            where TContactMechanism : IContactMechanismResult
        {
            viewName = viewName ?? this.GetDefaultViewNameSingle<THolder, TContactMechanism>();
            var key = CreateKey(id, contactMechanismId, contactMechanismTypeName);
            var result = await ExecuteQuery(u => this.Repository.GetContactMechanismView(key, viewName, u, token), token);
            if (result != null)
                ValidateContactMechanism<TContactMechanism>(result.Result);
            return result;
        }
        protected async virtual Task<QueryResult<TResultHolder>> AddContactMechanism<TAddHolder, TAddContactMechanism, TResultHolder>(
            TAddHolder view, Func<TId, Guid, string, string, CancellationToken, Task<QueryResult<TResultHolder>>> result, string viewName = null, CancellationToken token = default(CancellationToken))
            where TAddHolder : THolderBase, IContactMechanismHolderForAdd<TKey, TAddContactMechanism>
            where TAddContactMechanism: IContactMechanismForAdd
        {
            await this.AddContactMechanism<TAddHolder, TAddContactMechanism>(view, token);
            return await result(view.Key.Id, view.ContactMechanismGuid.Value, view.ContactMechanismTypeName, viewName, token);
        }
        protected virtual Task AddContactMechanism<THolder, TContactMechanism>(THolder view, CancellationToken token = default(CancellationToken))
            where THolder : THolderBase, IContactMechanismHolderForAdd<TKey, TContactMechanism>
            where TContactMechanism: IContactMechanismForAdd
        {
            view.Validate();
            return this.ExecuteAction(u => this.Repository.AddContactMechanism<THolder, TContactMechanism>(view, u, token), token);
        }
        protected async virtual Task<QueryResult<TResultHolder>> UpdateContactMechanism<TUpdateHolder, TUpdateContactMechanism, TResultHolder>(
            TUpdateHolder view, Func<TId, Guid, string, string, CancellationToken, Task<QueryResult<TResultHolder>>> result, string viewName = null, CancellationToken token = default(CancellationToken))
            where TUpdateHolder : THolderBase, IContactMechanismHolderForUpdate<TKey, TUpdateContactMechanism>
            where TUpdateContactMechanism : IContactMechanismForUpdate
        {
            await this.UpdateContactMechanism<TUpdateHolder, TUpdateContactMechanism>(view, token);
            return await result(view.Key.Id, view.ContactMechanismGuid.Value, view.ContactMechanismTypeName, viewName, token);
        }
        protected virtual Task UpdateContactMechanism<THolder, TContactMechanism>(THolder view, CancellationToken token = default(CancellationToken))
            where THolder : THolderBase, IContactMechanismHolderForUpdate<TKey, TContactMechanism>
            where TContactMechanism : IContactMechanismForUpdate
        {
            view.Validate();
            return this.ExecuteAction(u => this.Repository.UpdateContactMechanism<THolder, TContactMechanism>(view, u, token), token);
        }
        protected virtual Task<ChangeStatusQueryResult<TResult>> ChangeStatus<TResult>(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, Func<TId, Guid, string, string, CancellationToken, Task<QueryResult<TResult>>> result,
            string viewName = null, CancellationToken token = default(CancellationToken))
        {
            return ChangeStatus(userId => ChangeStatus(id, contactMechanismId, contactMechanismTypeName, statusTypeName, false, userId, token), () => result(id, contactMechanismId, contactMechanismTypeName, viewName, token), token);
        }
        protected virtual Task<ChangeStatusQueryResult<TResult>> ChangeStatusForce<TResult>(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, Func<TId, Guid, string, string, CancellationToken, Task<QueryResult<TResult>>> result,
            string viewName = null, CancellationToken token = default(CancellationToken))
        {
            return ChangeStatus(userId => ChangeStatus(id, contactMechanismId, contactMechanismTypeName, statusTypeName, true, userId, token), () => result(id, contactMechanismId, contactMechanismTypeName, viewName, token), token);
        }
        protected virtual Task<ChangeStatusResult> ChangeStatus(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            return this.Repository.ChangeStatusContactMechanism(CreateKey(id, contactMechanismId, contactMechanismTypeName), statusTypeName, force, userId, token);
        }
        protected virtual Task<ChangeStatusQueryResult<TResult>> ChangeStatusContactMechanism<TResult>(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, Func<TId, Guid, string, string, CancellationToken, Task<QueryResult<TResult>>> result,
            string viewName = null, CancellationToken token = default(CancellationToken))
        {
            return ChangeStatus(userId => ChangeStatusContactMechanism(contactMechanismId, statusTypeName, false, userId, token), () => result(id, contactMechanismId, contactMechanismTypeName, viewName, token), token);
        }
        protected virtual Task<ChangeStatusQueryResult<TResult>> ChangeStatusContactMechanismForce<TResult>(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, Func<TId, Guid, string, string, CancellationToken, Task<QueryResult<TResult>>> result,
            string viewName = null, CancellationToken token = default(CancellationToken))
        {
            return ChangeStatus(userId => ChangeStatusContactMechanism(contactMechanismId, statusTypeName, true, userId, token), () => result(id, contactMechanismId, contactMechanismTypeName, viewName, token), token);
        }
        protected virtual Task<ChangeStatusResult> ChangeStatusContactMechanism(Guid contactMechanismId, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
        {
            return this.ContactMechanismRepository.ChangeStatus(contactMechanismId, statusTypeName, force, userId, token);
        }
        protected static ContactMechanismHolderDefaultViewAttribute GetDefaultViewAttribute<THolder>()
            where THolder: THolderBase
        {
            Type type = typeof(THolder);
            ContactMechanismHolderDefaultViewAttribute attr;
            if (!HolderDefaultViewMapping.TryGetValue(type, out attr))
            {
                attr = type.GetCustomAttribute<ContactMechanismHolderDefaultViewAttribute>(true);
                HolderDefaultViewMapping.TryAdd(type, attr);
            }
            return attr;
        }
        protected static ContactMechanismTypeAttribute GetContactMechanismTypeAttribute<TContactMechanism>()
            where TContactMechanism: IContactMechanismResult
        {
            Type type = typeof(TContactMechanism);
            ContactMechanismTypeAttribute attr;
            if(!ContactMechanismTypeMapping.TryGetValue(type, out attr))
            {
                attr = type.GetCustomAttribute<ContactMechanismTypeAttribute>(true);
                ContactMechanismTypeMapping.TryAdd(type, attr);
            }
            return attr;
        }
        protected virtual string GetDefaultViewName<THolder, TContactMechanism>()
            where THolder : THolderBase, IContactMechanismHolderResult<TKey, TContactMechanism>
            where TContactMechanism: IContactMechanismResult
        {
            var attr = GetDefaultViewAttribute<THolder>();
            return attr != null ? attr.GetDefaultViewName<TContactMechanism>() : null as string;
        }
        protected virtual string GetDefaultViewNameSingle<THolder, TContactMechanism>()
            where THolder : THolderBase, IContactMechanismHolderResult<TKey, TContactMechanism>
            where TContactMechanism : IContactMechanismResult
        {
            var attr = GetDefaultViewAttribute<THolder>();
            return attr != null ? attr.GetDefaultViewNameSingle<TContactMechanism>() : GetDefaultViewName<THolder, TContactMechanism>();
        }
        protected virtual string GetDefaultViewNameMany<THolder, TContactMechanism>()
            where THolder : THolderBase, IContactMechanismHolderResult<TKey, TContactMechanism>
            where TContactMechanism : IContactMechanismResult
        {
            var attr = GetDefaultViewAttribute<THolder>();
            return attr != null ? attr.GetDefaultViewNameMany<TContactMechanism>() : GetDefaultViewName<THolder, TContactMechanism>();
        }


        public Task<QueryResult<TAddressResult>> AddAddress(TAddressForAdd address, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> AddAddressView(TAddressForAdd address, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TEmailResult>> AddEmail(TEmailForAdd email, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> AddEmailView(PartyContactMechanismForAdd<EmailForAdd> email, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TPhoneResult>> AddPhone(TPhoneForAdd phone, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> AddPhoneView(TPhoneForAdd phone, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TGenericResult>> AddContactMechanism(TGenericForAdd mechanism, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> AddContactMechanismView(TGenericForAdd mechanism, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TAddressResult>> UpdateAddress(TAddressForUpdate address, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> UpdateAddressView(TAddressForUpdate address, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TEmailResult>> UpdateEmail(TEmailForUpdate email, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> UpdateEmailView(TEmailForUpdate email, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TPhoneResult>> UpdatePhone(TPhoneForUpdate phone, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> UpdatePhoneView(TPhoneForUpdate phone, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TGenericResult>> UpdateContactMechanism(TGenericForUpdate mechanism, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> UpdateContactMechanismView(TGenericForUpdate mechanism, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<TAddressResult>> GetAddresses(string contactMechanismTypeName = null, string viewName = null, FilterContext<TAddressResult> filter = null, SortContext<TAddressResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<JObject>> GetAddressesView(string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<TAddressResult>> GetAddressesById(TId id, string contactMechanismTypeName = null, string viewName = null, FilterContext<TAddressResult> filter = null, SortContext<TAddressResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<JObject>> GetAddressesByIdView(TId id, string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<TEmailResult>> GetEmails(string contactMechanismTypeName = null, string viewName = null, FilterContext<TEmailResult> filter = null, SortContext<TEmailResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<JObject>> GetEmailsView(string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<TEmailResult>> GetEmailsById(TId id, string contactMechanismTypeName = null, string viewName = null, FilterContext<TEmailResult> filter = null, SortContext<TEmailResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<JObject>> GetEmailsByIdView(TId id, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<TPhoneResult>> GetPhones(string viewName = null, string contactMechanismTypeName = null, FilterContext<TPhoneResult> filter = null, SortContext<TPhoneResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<JObject>> GetPhonesView(string viewName = null, string contactMechanismTypeName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<TPhoneResult>> GetPhonesById(TId id, string contactMechanismTypeName = null, string viewName = null, FilterContext<TPhoneResult> filter = null, SortContext<TPhoneResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<JObject>> GetPhonesByIdView(TId id, string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<TGenericResult>> GetContactMechanisms(string contactMechanismTypeName = null, string viewName = null, FilterContext<TGenericResult> filter = null, SortContext<TGenericResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<JObject>> GetContactMechanismsView(string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<TGenericResult>> GetContactMechanismsById(TId id, string contactMechanismTypeName = null, string viewName = null, FilterContext<TGenericResult> filter = null, SortContext<TGenericResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResults<JObject>> GetContactMechanismsByIdView(TId id, string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TAddressResult>> GetAddress(TId id, Guid addressId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> GetAddressView(TId id, Guid addressId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TEmailResult>> GetEmail(TId id, Guid emailId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> GetEmailView(TId id, Guid emailId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TPhoneResult>> GetPhone(TId id, Guid phoneId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> GetPhoneView(TId id, Guid phoneId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<TGenericResult>> GetContactMechanism(TId id, Guid contactMechanismId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<JObject>> GetContactMechanismView(TId id, Guid contactMechanismId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeleteAddressSoft(TId id, Guid addressId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeletePhoneSoft(TId id, Guid phoneId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeleteEmailSoft(TId id, Guid emailId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeleteAddressHard(TId id, Guid addressId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeletePhoneHard(TId id, Guid phoneId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeleteEmailHard(TId id, Guid emailId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeleteContactMechanismHard(TKey key, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeleteContactMechanismSoft(TKey key, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TAddressResult>> ChangeStatusAddress(TId id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TEmailResult>> ChangeStatusEmail(TId id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TPhoneResult>> ChangeStatusPhone(TId id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TGenericResult>> ChangeStatusContactMechanism(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TAddressResult>> ChangeStatusAddressStatus(TId id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TEmailResult>> ChangeStatusEmailStatus(TId id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TPhoneResult>> ChangeStatusPhoneStatus(TId id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TPhoneResult>> ChangeStatusContactMechanismStatus(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TAddressResult>> ChangeStatusAddressForce(TId id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TEmailResult>> ChangeStatusEmailForce(TId id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TPhoneResult>> ChangeStatusPhoneForce(TId id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TGenericResult>> ChangeStatusContactMechanismForce(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TAddressResult>> ChangeStatusAddressStatusForce(TId id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TEmailResult>> ChangeStatusEmailStatusForce(TId id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TPhoneResult>> ChangeStatusPhoneStatusForce(TId id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<TGenericResult>> ChangeStatusContactMechanismStatusForce(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressView(TId id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailView(TId id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneView(TId id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismView(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressStatusView(TId id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailStatusView(TId id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneStatusView(TId id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismStatusView(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressForceView(TId id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailForceView(TId id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneForceView(TId id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismForceView(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressStatusForceView(TId id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailStatusForceView(TId id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneStatusForceView(TId id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismStatusForceView(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
