using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews;
using HallData.EMS.Models;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
    public interface IGenericContactImplementation<TKey, TId, THolderBase, TAddressResult, TAddressForAdd, TAddressForUpdate,
        TEmailResult, TEmailForAdd, TEmailForUpdate,
        TPhoneResult, TPhoneForAdd, TPhoneForUpdate,
        TGenericResult, TGenericForAdd, TGenericForUpdate> : IBusinessImplementation
        where THolderBase : IContactMechanismHolderKey<TKey>
        where TKey : IContactMechanismHolderId<TId>
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
        [AddMethod]
        [ServiceRoute("AddAddress", "Addresses/")]
        [ServiceRoute("AddAddressTyped", "Addresses/TypedView/{viewName}/")]
        [ServiceRoute("AddAddressTypedDefualt", "Addresses/TypedView/")]
        Task<QueryResult<TAddressResult>> AddAddress([Description("Address to add")][Content]TAddressForAdd address, string viewName = null, CancellationToken token = default(CancellationToken));
        [AddMethod]
        [ServiceRoute("AddAddressView", "Addresses/View/{viewName}/")]
        [ServiceRoute("AddAddressViewDefualt", "Addresses/View/")]
        [Description("Add an address to a party")]
        Task<QueryResult<JObject>> AddAddressView([Description("Address to add")][Content]TAddressForAdd address, string viewName = null, CancellationToken token = default(CancellationToken));

        [AddMethod]
        [ServiceRoute("AddEmail", "Emails/")]
        [ServiceRoute("AddEmailTyped", "Emails/TypedView/{viewName}/")]
        [ServiceRoute("AddEmailTypedDefualt", "Emails/TypedView/")]
        [Description("Add an email to a party")]
        Task<QueryResult<TEmailResult>> AddEmail([Description("Email to add")][Content]TEmailForAdd email, string viewName = null, CancellationToken token = default(CancellationToken));
        [AddMethod]
        [ServiceRoute("AddEmailView", "Emails/View/{viewName}/")]
        [ServiceRoute("AddEmailViewDefualt", "Emails/View/")]
        Task<QueryResult<JObject>> AddEmailView([Description("Email to add")][Content]PartyContactMechanismForAdd<EmailForAdd> email, string viewName = null, CancellationToken token = default(CancellationToken));

        [AddMethod]
        [ServiceRoute("AddPhone", "Phones/")]
        [ServiceRoute("AddPhoneTyped", "Phones/TypedView/{viewName}/")]
        [ServiceRoute("AddPhoneTypedDefualt", "Phones/TypedView/")]
        Task<QueryResult<TPhoneResult>> AddPhone([Description("Phone to add")][Content]TPhoneForAdd phone, string viewName = null, CancellationToken token = default(CancellationToken));
        [AddMethod]
        [ServiceRoute("AddPhoneView", "Phones/View/{viewName}/")]
        [ServiceRoute("AddPhoneTypedDefualt", "Phones/View/")]
        Task<QueryResult<JObject>> AddPhoneView([Description("Phone to add")][Content]TPhoneForAdd phone, string viewName = null, CancellationToken token = default(CancellationToken));

        [AddMethod]
        [ServiceRoute("AddContactMechanism", "ContactMechanisms/")]
        [ServiceRoute("AddContactMechanismTyped", "ContactMechanisms/TypedView/{viewName}/")]
        [ServiceRoute("AddContactMechanismTypedDefualt", "ContactMechanisms/TypedView/")]
        Task<QueryResult<TGenericResult>> AddContactMechanism([Description("Contact Mechanism to add")][Content]TGenericForAdd mechanism, string viewName = null, CancellationToken token = default(CancellationToken));
        [AddMethod]
        [ServiceRoute("AddContactMechanismView", "ContactMechanisms/View/{viewName}/")]
        [ServiceRoute("AddContactMechanismViewDefualt", "ContactMechanisms/View/")]
        Task<QueryResult<JObject>> AddContactMechanismView([Description("Contact Mechanism to add")][Content]TGenericForAdd mechanism, string viewName = null, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("UpdateAddress", "Addresses/")]
        [ServiceRoute("UpdateAddressTyped", "Addresses/TypedView/{viewName}/")]
        [ServiceRoute("UpdateAddressTypedDefualt", "Addresses/TypedView/")]
        Task<QueryResult<TAddressResult>> UpdateAddress([Description("Address to update")][Content]TAddressForUpdate address, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("UpdateAddressView", "Addresses/View/{viewName}/")]
        [ServiceRoute("UpdateAddressViewDefualt", "Addresses/View/")]
        Task<QueryResult<JObject>> UpdateAddressView([Description("Address to update")][Content]TAddressForUpdate address, string viewName = null, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("UpdateEmail", "Emails/")]
        [ServiceRoute("UpdateEmailTyped", "Emails/TypedView/{viewName}/")]
        [ServiceRoute("UpdateEmailTypedDefualt", "Emails/TypedView/")]
        Task<QueryResult<TEmailResult>> UpdateEmail([Description("Email to update")][Content]TEmailForUpdate email, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("UpdateEmailView", "Emails/View/{viewName}/")]
        [ServiceRoute("UpdateEmailViewDefualt", "Emails/View/")]
        Task<QueryResult<JObject>> UpdateEmailView([Description("Email to update")][Content]TEmailForUpdate email, string viewName = null, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("UpdatePhone", "Phones/")]
        [ServiceRoute("UpdatePhoneTyped", "Phones/TypedView/{viewName}/")]
        [ServiceRoute("UpdatePhoneTypedDefualt", "Phones/TypedView/")]
        Task<QueryResult<TPhoneResult>> UpdatePhone([Description("Phone to update")][Content]TPhoneForUpdate phone, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("UpdatePhoneView", "Phones/View/{viewName}/")]
        [ServiceRoute("UpdatePhoneViewDefualt", "Phones/View/")]
        Task<QueryResult<JObject>> UpdatePhoneView([Description("Phone to update")][Content]TPhoneForUpdate phone, string viewName = null, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("UpdateContactMechanism", "ContactMechanisms/")]
        [ServiceRoute("UpdateContactMechanismTyped", "ContactMechanisms/TypedView/{viewName}/")]
        [ServiceRoute("UpdateContactMechanismDefualt", "ContactMechanisms/TypedView/")]
        Task<QueryResult<TGenericResult>> UpdateContactMechanism([Description("Contact mechanism to update")][Content]TGenericForUpdate mechanism, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("UpdateContactMechanismView", "ContactMechanisms/View/{viewName}/")]
        [ServiceRoute("UpdateContactMechanismViewDefualt", "ContactMechanisms/View/")]
        [Description("Update a party's contact mechanisms")]
        Task<QueryResult<JObject>> UpdateContactMechanismView([Description("Contact mechanism to update")][Content]TGenericForUpdate mechanism, string viewName = null, CancellationToken token = default(CancellationToken));

        [GetMethod]
        [ServiceRoute("GetAddresses", "Addresses/")]
        [ServiceRoute("GetAddressesTypedView", "Addresses/TypedView/{viewName}/")]
        [ServiceRoute("GetAddressesTypedViewDefault", "Addresses/TypedView/")]
        [ServiceRoute("GetAddressesContactType", "Addresses/{contactMechanismTypeName}")]
        [ServiceRoute("GetAddressesContactTypeTypedView", "Addresses/{contactMechanismTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("GetAddressesContactTypeTypedViewDefault", "Addresses/{contactMechanismTypeName}/TypedView/")]
        Task<QueryResults<TAddressResult>> GetAddresses(string contactMechanismTypeName = null, string viewName = null, FilterContext<TAddressResult> filter = null, SortContext<TAddressResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetAddressesView", "Addresses/View/{viewName}/")]
        [ServiceRoute("GetAddressesViewDefault", "Addresses/View/")]
        [ServiceRoute("GetAddressesViewContactType", "Addresses/{contactMechanismTypeName}/View/{viewName}/")]
        [ServiceRoute("GetAddressesViewDefaultContactType", "Addresses/{contactMechanismTypeName}/View/")]
        Task<QueryResults<JObject>> GetAddressesView(string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetAddressesById", "{id}/Addresses/")]
        [ServiceRoute("GetAddressesByIdTypedView", "{id}/Addresses/TypedView/{viewName}/")]
        [ServiceRoute("GetAddressesByIdTypedViewDefault", "{id}/Addresses/TypedView/")]
        Task<QueryResults<TAddressResult>> GetAddressesById(TId id, string contactMechanismTypeName = null, string viewName = null,
            [Description("Filter for address results, must be url encoded JSON")][JsonEncode]FilterContext<TAddressResult> filter = null,
            [Description("Sort for address results, must be url encoded JSON")][JsonEncode]SortContext<TAddressResult> sort = null,
            [Description("Page for address results, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetAddressesByIdView", "{id}/Addresses/View/{viewName}/")]
        [ServiceRoute("GetAddressesByIdViewDefault", "{id}/Addresses/View/")]
        Task<QueryResults<JObject>> GetAddressesByIdView(TId id, string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));

        [GetMethod]
        [ServiceRoute("GetEmails", "Emails/")]
        [ServiceRoute("GetEmailsTypedView", "Emails/TypedView/{viewName}/")]
        [ServiceRoute("GetEmailsTypedViewDefault", "Emails/TypedView/")]
        Task<QueryResults<TEmailResult>> GetEmails(string contactMechanismTypeName = null, string viewName = null, FilterContext<TEmailResult> filter = null, SortContext<TEmailResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetEmailsView", "Emails/View/{viewName}/")]
        [ServiceRoute("GetEmailsViewDefault", "Emails/View/")]
        Task<QueryResults<JObject>> GetEmailsView(string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetEmailsById", "{id}/Emails/")]
        [ServiceRoute("GetEmailsByIdTypedView", "{id}/Emails/TypedView/{viewName}/")]
        [ServiceRoute("GetEmailsByIdViewDefault", "{id}/Emails/TypedView/")]
        Task<QueryResults<TEmailResult>> GetEmailsById(TId id, string contactMechanismTypeName = null, string viewName = null,
            [Description("Filter for email results, must be url encoded JSON")][JsonEncode]FilterContext<TEmailResult> filter = null,
            [Description("Sort for email results, must be url encoded JSON")][JsonEncode]SortContext<TEmailResult> sort = null,
            [Description("Page for email results, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetEmailsByIdView", "{id}/Emails/View/{viewName}/")]
        [ServiceRoute("GetEmailsByIdViewDefault", "{id}/Emails/View/")]
        Task<QueryResults<JObject>> GetEmailsByIdView(TId id, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));

        [GetMethod]
        [ServiceRoute("GetPhones", "Phones/")]
        [ServiceRoute("GetPhonesTypedView", "Phones/TypedView/{viewName}/")]
        [ServiceRoute("GetPhonesTypedViewDefault", "Phones/TypedView/")]
        Task<QueryResults<TPhoneResult>> GetPhones(string viewName = null, string contactMechanismTypeName = null, FilterContext<TPhoneResult> filter = null, SortContext<TPhoneResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetPhonesView", "Phones/View/{viewName}/")]
        [ServiceRoute("GetPhonesViewDefault", "Phones/View/")]
        Task<QueryResults<JObject>> GetPhonesView(string viewName = null, string contactMechanismTypeName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetPhonesById", "{id}/Phones/")]
        [ServiceRoute("GetPhonesByIdTypedView", "{id}/Phones/TypedView/{viewName}/")]
        [ServiceRoute("GetPhonesByIdTypedViewDefault", "{id}/Phones/TypedView/")]
        Task<QueryResults<TPhoneResult>> GetPhonesById(TId id, string contactMechanismTypeName = null, string viewName = null,
            [Description("Filter for phone results, must be url encoded JSON")][JsonEncode]FilterContext<TPhoneResult> filter = null,
            [Description("Sort for phone results, must be url encoded JSON")][JsonEncode]SortContext<TPhoneResult> sort = null,
            [Description("Page for phone results, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetPhonesByIdView", "{id}/Phones/View/{viewName}/")]
        [ServiceRoute("GetPhonesByIdViewDefault", "{id}/Phones/View/")]
        Task<QueryResults<JObject>> GetPhonesByIdView(TId id, string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));

        [GetMethod]
        [ServiceRoute("GetContactMechanisms", "ContactMechanisms/")]
        [ServiceRoute("GetContactMechanismsTypedView", "ContactMechanisms/TypedView/{viewName}/")]
        [ServiceRoute("GetContactMechanismsTypedViewDefault", "ContactMechanisms/TypedView/")]
        Task<QueryResults<TGenericResult>> GetContactMechanisms(string contactMechanismTypeName = null, string viewName = null, FilterContext<TGenericResult> filter = null, SortContext<TGenericResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetContactMechanismsView", "ContactMechanisms/View/{viewName}/")]
        [ServiceRoute("GetContactMechanismsViewDefault", "ContactMechanisms/View/")]
        [ServiceMethodResult(typeof(QueryResults<PartyContactMechanismResult<ContactMechanismGeneric>>))]
        Task<QueryResults<JObject>> GetContactMechanismsView(string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetContactMechanismsById", "{id}/ContactMechanisms/")]
        [ServiceRoute("GetContactMechanismsByIdTypedView", "{id}/ContactMechanisms/TypedView/{viewName}/")]
        [ServiceRoute("GetContactMechanismsByIdTypedViewDefault", "{id}/ContactMechanisms/TypedView/")]
        Task<QueryResults<TGenericResult>> GetContactMechanismsById(TId id, string contactMechanismTypeName = null, string viewName = null,
            [Description("Filter for contact mechanism results, must be url encoded JSON")][JsonEncode]FilterContext<TGenericResult> filter = null,
            [Description("Sort for contact mechanism results, must be url encoded JSON")][JsonEncode]SortContext<TGenericResult> sort = null,
            [Description("Page for contact mechanism results, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetContactMechanismsByIdView", "{id}/ContactMechanisms/View/{viewName}/")]
        [ServiceRoute("GetContactMechanismsByIdViewDefault", "{id}/ContactMechanisms/View/")]
        Task<QueryResults<JObject>> GetContactMechanismsByIdView(TId id, string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));

        [GetMethod]
        [ServiceRoute("GetAddress", "{id}/Addresses/{addressId}/")]
        [ServiceRoute("GetAddressTypedView", "{id}/Addresses/{addressId}/TypedView/{viewName}/")]
        [ServiceRoute("GetAddressTypedViewDefault", "{id}/Addresses/{addressId}/TypedView/")]
        Task<QueryResult<TAddressResult>> GetAddress(TId id, [Description("Target address id")]Guid addressId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetAddressView", "{id}/Addresses/{addressId}/View/{viewName}/")]
        [ServiceRoute("GetAddressViewDefault", "{id}/Addresses/{addressId}/View/")]
        Task<QueryResult<JObject>> GetAddressView(TId id, Guid addressId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

        [GetMethod]
        [ServiceRoute("GetEmail", "{id}/Emails/{emailId}/")]
        [ServiceRoute("GetEmailTypedView", "{id}/Emails/{emailId}/TypedView/{viewName}/")]
        [ServiceRoute("GetEmailTypedViewDefault", "{id}/Emails/{emailId}/TypedView/")]
        Task<QueryResult<TEmailResult>> GetEmail(TId id, [Description("Target email id")]Guid emailId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetEmailView", "{id}/Emails/{emailId}/View/{viewName}/")]
        [ServiceRoute("GetEmailViewDefault", "{id}/Emails/{emailId}/View/")]
        Task<QueryResult<JObject>> GetEmailView(TId id, Guid emailId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

        [GetMethod]
        [ServiceRoute("GetPhone", "{id}/Phones/{phoneId}/")]
        [ServiceRoute("GetPhoneTypedView", "{id}/Phones/{phoneId}/TypedView/{viewName}/")]
        [ServiceRoute("GetPhoneTypedViewDefault", "{id}/Phones/{phoneId}/TypedView/")]
        Task<QueryResult<TPhoneResult>> GetPhone(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetPhoneViewView", "{id}/Phones/{phoneId}/View/{viewName}/")]
        [ServiceRoute("GetPhoneViewDefault", "{id}/Phones/{phoneId}/View/")]
        Task<QueryResult<JObject>> GetPhoneView(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

        [GetMethod]
        [ServiceRoute("GetContactMechanism", "{id}/ContactMechanisms/{contactMechanismId}/")]
        [ServiceRoute("GetContactMechanismTypedView", "{id}/ContactMechanisms/{contactMechanismId}/TypedView/{viewName}/")]
        [ServiceRoute("GetContactMechanismTypedViewDefault", "{id}/ContactMechanisms/{contactMechanismId}/TypedView/")]
        Task<QueryResult<TGenericResult>> GetContactMechanism(TId id, Guid contactMechanismId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetContactMechanismViewView", "{id}/ContactMechanisms/{contactMechanismId}/View/{viewName}/")]
        [ServiceRoute("GetContactMechanismViewDefault", "{id}/ContactMechanisms/{contactMechanismId}/View/")]
        Task<QueryResult<JObject>> GetContactMechanismView(TId id, [Description("Target phone id")]Guid contactMechanismId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

        [DeleteMethod]
        [ServiceRoute("DeleteAddress", "{id}/Addresses/{addressId}/")]
        [ServiceRoute("DeleteAddressSoft", "{id}/Addresses/{addressId}/Soft/")]
        Task DeleteAddressSoft(TId id, [Description("Target address id")]Guid addressId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken));
        [DeleteMethod]
        [ServiceRoute("DeletePhone", "{id}/Phones/{phoneId}/")]
        [ServiceRoute("DeletePhoneSoft", "{id}/Phones/{phoneId}/Soft/")]
        Task DeletePhoneSoft(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken));
        [DeleteMethod]
        [ServiceRoute("DeleteEmail", "{id}/Emails/{emailId}/")]
        [ServiceRoute("DeleteEmailSoft", "{id}/Emails/{emailId}/Soft/")]
        Task DeleteEmailSoft(TId id, [Description("Target email id")]Guid emailId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken));
        [DeleteMethod]
        [ServiceRoute("DeleteAddressHard", "{id}/Addresses/{addressId}/Hard/")]
        Task DeleteAddressHard(TId id, [Description("Target address id")]Guid addressId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken));
        [DeleteMethod]
        [ServiceRoute("DeletePhoneHard", "{id}/Phones/{phoneId}/Hard/")]
        Task DeletePhoneHard(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken));
        [DeleteMethod]
        [ServiceRoute("DeleteEmailHard", "{id}/Emails/{emailId}/Hard/")]
        Task DeleteEmailHard(TId id, [Description("Target email id")]Guid emailId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken));

        Task DeleteContactMechanismHard(TKey key, CancellationToken token = default(CancellationToken));
        Task DeleteContactMechanismSoft(TKey key, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("ChangeStatusAddress", "{id}/Addresses/{addressId}/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusAddressTyped", "{id}/Addresses/{addressId}/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusAddressTypedDefault", "{id}/Addresses/{addressId}/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<TAddressResult>> ChangeStatusAddress(TId id, [Description("Target address id")]Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmail", "{id}/Emails/{emailId}/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusEmailTyped", "{id}/Emails/{emailId}/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmailTypedDefault", "{id}/Emails/{emailId}/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<TEmailResult>> ChangeStatusEmail(TId id, [Description("Target email id")]Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusPhone", "{id}/Phones/{phoneId}/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusPhoneTyped", "{id}/Phones/{phoneId}/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusPhoneTypedDefault", "{id}/Phones/{phoneId}/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<TPhoneResult>> ChangeStatusPhone(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusContactMechanism", "{id}/ContactMechanisms/{contactMechanismId}/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusContactMechanismTyped", "{id}/ContactMechanisms/{contactMechanismId}/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusContactMechanismTypedDefault", "{id}/ContactMechanisms/{contactMechanismId}/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<TGenericResult>> ChangeStatusContactMechanism(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("ChangeStatusAddressStatus", "{id}/Addresses/{addressId}/Address/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusAddressStatusTyped", "{id}/Addresses/{addressId}/Address/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusAddressStatusTypedDefault", "{id}/Addresses/{addressId}/Address/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<TAddressResult>> ChangeStatusAddressStatus(TId id, [Description("Target address id")]Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmailStatus", "{id}/Emails/{emailId}/Email/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusEmailStatusTyped", "{id}/Emails/{emailId}/Email/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmailStatusTypedDefault", "{id}/Emails/{emailId}/Email/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<TEmailResult>> ChangeStatusEmailStatus(TId id, [Description("Target email id")]Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusPhoneStatus", "{id}/Phones/{phoneId}/Phone/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusPhoneStatusTyped", "{id}/Phones/{phoneId}/Phone/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusPhoneStatusTypedDefault", "{id}/Phones/{phoneId}/Phone/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<TPhoneResult>> ChangeStatusPhoneStatus(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusContactMechanismStatus", "{id}/ContactMechanisms/{contactMechanismId}/ContactMechanism/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusContactMechanismStatusTyped", "{id}/ContactMechanisms/{contactMechanismId}/ContactMechanism/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusContactMechanismStatusTypedDefault", "{id}/ContactMechanisms/{contactMechanismId}/ContactMechanism/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<TPhoneResult>> ChangeStatusContactMechanismStatus(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("ChangeStatusAddressForce", "{id}/Addresses/{addressId}/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusAddressForceTyped", "{id}/Addresses/{addressId}/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusAddressForceTypedDefault", "{id}/Addresses/{addressId}/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<TAddressResult>> ChangeStatusAddressForce(TId id, [Description("Target address id")]Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmailForce", "{id}/Emails/{emailId}/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusEmailForceTyped", "{id}/Emails/{emailId}/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmailForceTypeDefault", "{id}/Emails/{emailId}/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<TEmailResult>> ChangeStatusEmailForce(TId id, [Description("Target email id")]Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusPhoneForce", "{id}/Phones/{phoneId}/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusPhoneForceTyped", "{id}/Phones/{phoneId}/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusPhoneForceTypedDefault", "{id}/Phones/{phoneId}/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<TPhoneResult>> ChangeStatusPhoneForce(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusContactMechanismForce", "{id}/ContactMechanisms/{contactMechanismId}/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusContactMechanismForceTyped", "{id}/ContactMechanisms/{contactMechanismId}/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusContactMechanismForceTypedDefault", "{id}/ContactMechanisms/{contactMechanismId}/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<TGenericResult>> ChangeStatusContactMechanismForce(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("ChangeStatusAddressStatusForce", "{id}/Addresses/{addressId}/Address/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusAddressStatusForceTyped", "{id}/Addresses/{addressId}/Address/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusAddressStatusForceTypedDefault", "{id}/Addresses/{addressId}/Address/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<TAddressResult>> ChangeStatusAddressStatusForce(TId id, [Description("Target address id")]Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmailStatusForce", "{id}/Emails/{emailId}/Email/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusEmailStatusForceTyped", "{id}/Emails/{emailId}/Email/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmailStatusForceTypedDefault", "{id}/Emails/{emailId}/Email/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<TEmailResult>> ChangeStatusEmailStatusForce(TId id, [Description("Target email id")]Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusPhoneStatusForce", "{id}/Phones/{phoneId}/Phone/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusPhoneStatusForceTyped", "{id}/Phones/{phoneId}/Phone/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusPhoneStatusForceTypedDefault", "{id}/Phones/{phoneId}/Phone/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<TPhoneResult>> ChangeStatusPhoneStatusForce(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusContactMechanismStatusForce", "{id}/ContactMechanisms/{contactMechanismId}/ContactMechanism/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusContactMechanismStatusForceTyped", "{id}/ContactMechanisms/{contactMechanismId}/ContactMechanism/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusContactMechanismStatusForceTypedDefault", "{id}/ContactMechanisms/{contactMechanismId}/ContactMechanism/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<TGenericResult>> ChangeStatusContactMechanismStatusForce(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("ChangeStatusAddressView", "{id}/Addresses/{addressId}/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusAddressViewDefault", "{id}/Addresses/{addressId}/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressView(TId id, [Description("Target address id")]Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmailView", "{id}/Emails/{emailId}/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmailViewDefault", "{id}/Emails/{emailId}/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailView(TId id, [Description("Target email id")]Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusPhoneView", "{id}/Phones/{phoneId}/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusPhoneViewDefault", "{id}/Phones/{phoneId}/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneView(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusContactMechanismView", "{id}/ContactMechanisms/{contactMechanismId}/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusContactMechanismViewDefault", "{id}/ContactMechanisms/{contactMechanismId}/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismView(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("ChangeStatusAddressStatusView", "{id}/Addresses/{addressId}/Address/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusAddressStatusViewDefault", "{id}/Addresses/{addressId}/Address/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressStatusView(TId id, [Description("Target address id")]Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmailStatusView", "{id}/Emails/{emailId}/Email/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmailStatusViewDefault", "{id}/Emails/{emailId}/Email/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailStatusView(TId id, [Description("Target email id")]Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusPhoneStatusView", "{id}/Phones/{phoneId}/Phone/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusPhoneStatusViewDefault", "{id}/Phones/{phoneId}/Phone/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneStatusView(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusContactMechanismStatusView", "{id}/ContactMechanisms/{contactMechanismId}/ContactMechanism/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusContactMechanismStatusViewDefault", "{id}/ContactMechanisms/{contactMechanismId}/ContactMechanism/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismStatusView(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("ChangeStatusAddressForceView", "{id}/Addresses/{addressId}/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusAddressForceViewDefault", "{id}/Addresses/{addressId}/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressForceView(TId id, [Description("Target address id")]Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmailForceView", "{id}/Emails/{emailId}/Status/{statusTypeName}/Force/View/{viewName}/")]
        [ServiceRoute("ChangeStatusEmailForceViewDefault", "{id}/Emails/{emailId}/Status/{statusTypeName}/Force/View/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailForceView(TId id, [Description("Target email id")]Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusPhoneForceView", "{id}/Phones/{phoneId}/Status/{statusTypeName}/Force/View/{viewName}/")]
        [ServiceRoute("ChangeStatusPhoneForceViewDefault", "{id}/Phones/{phoneId}/Status/{statusTypeName}/Force/View/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneForceView(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusContactMechanismForceView", "{id}/ContactMechanisms/{contactMechanismId}/Status/{statusTypeName}/Force/View/{viewName}/")]
        [ServiceRoute("ChangeStatusContactMechanismForceViewDefault", "{id}/ContactMechanisms/{contactMechanismId}/Status/{statusTypeName}/Force/View/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismForceView(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

        [UpdateMethod]
        [ServiceRoute("ChangeStatusAddressStatusForceView", "{id}/Addresses/{addressId}/Address/Status/{statusTypeName}/Force/View/{viewName}/")]
        [ServiceRoute("ChangeStatusAddressStatusForceViewDefault", "{id}/Addresses/{addressId}/Address/Status/{statusTypeName}/Force/View/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressStatusForceView(TId id, [Description("Target address id")]Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmailStatusForceView", "{id}/Emails/{emailId}/Email/Status/{statusTypeName}/Force/View/{viewName}/")]
        [ServiceRoute("ChangeStatusEmailStatusForceViewDefault", "{id}/Emails/{emailId}/Email/Status/{statusTypeName}/Force/View/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailStatusForceView(TId id, [Description("Target email id")]Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusPhoneStatusForceView", "{id}/Phones/{phoneId}/Phone/Status/{statusTypeName}/Force/View/{viewName}/")]
        [ServiceRoute("ChangeStatusPhoneStatusForceViewDefault", "{id}/Phones/{phoneId}/Phone/Status/{statusTypeName}/Force/View/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneStatusForceView(TId id, [Description("Target phone id")]Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusContactMechanismStatusForceView", "{id}/ContactMechanisms/{contactMechanismId}/ContactMechanism/Status/{statusTypeName}/Force/View/{viewName}/")]
        [ServiceRoute("ChangeStatusContactMechanismStatusForceViewDefault", "{id}/ContactMechanisms/{contactMechanismId}/ContactMechanism/Status/{statusTypeName}/Force/View/")]
        Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismStatusForceView(TId id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken));

    }
}