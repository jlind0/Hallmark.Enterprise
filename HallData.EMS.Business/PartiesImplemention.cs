using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using HallData.Repository;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Models;
using System.Threading;
using HallData.EMS.Data;
using HallData.Security;
using HallData.Exceptions;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Business
{
	public class ReadOnlyPartyImplementation<TRepository, TKey, TPartyResult> : ReadOnlyBusinessRepositoryProxy<TRepository, TKey, TPartyResult>, IReadOnlyPartyImplementation<TKey, TPartyResult>
		where TRepository: IReadOnlyPartyRepository<TKey, TPartyResult>
		where TPartyResult: IPartyResult<TKey>
	{
		public ReadOnlyPartyImplementation(TRepository repository, ISecurityImplementation security) : 
			base(repository, security)
		{
		}
	}

	public class ReadOnlyPartyImplementation : ReadOnlyPartyImplementation<IReadOnlyPartyRepository, Guid, PartyResult>, IReadOnlyPartyImplementation
	{
		public ReadOnlyPartyImplementation(IReadOnlyPartyRepository repository, ISecurityImplementation security) :
			base(repository, security)
		{
		}
	}

	public class PartyImplementation<TRepository, TReadOnlyImplementation, TKey, TPartyResult, TPartyForAdd, TPartyForUpdate> : DeletableBusinessRepositoryProxyWithBase<TRepository, TKey, TPartyResult, TPartyForAdd, TPartyForUpdate>, IPartyImplementation<TKey, TPartyResult, TPartyForAdd, TPartyForUpdate>
		where TPartyResult : IPartyResult<TKey>
		where TPartyForAdd : IPartyForAdd<TKey>
		where TPartyForUpdate: IPartyForUpdate<TKey>
		where TRepository : IPartyRepository<TKey, TPartyResult, TPartyForAdd, TPartyForUpdate>
		where TReadOnlyImplementation : IReadOnlyPartyImplementation<TKey, TPartyResult>
	{
		protected IPartyContactImplementation PartyContact { get; private set; }
		protected TReadOnlyImplementation ReadOnly { get; private set; }
		protected IReadOnlyProductImplementation Product { get; private set; }

		public PartyImplementation(TRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact, TReadOnlyImplementation readOnly, IReadOnlyProductImplementation product) : 
			base(repository, security)
		{
			this.PartyContact = partyContact;
			this.ReadOnly = readOnly;
			this.Product = product;
		}

		public override Task<QueryResult<TPartyResult>> Get(TKey id, CancellationToken token = default(CancellationToken))
		{
			return ReadOnly.Get(id, token);
		}

		public override Task<QueryResults<TPartyResult>> GetMany(string viewName = null, FilterContext<TPartyResult> filter = null, SortContext<TPartyResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return ReadOnly.GetMany(viewName, filter, sort, page, token);
		}

		public override Task<QueryResults<JObject>> GetManyView(string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return ReadOnly.GetManyView(viewName, filter, sort, page, token);
		}

		public override Task<QueryResult<JObject>> GetView(TKey id, CancellationToken token = default(CancellationToken))
		{
			return ReadOnly.GetView(id, token);
		}

		public virtual async Task AddCategory(Guid partyID, int categoryID, CancellationToken token = default(CancellationToken))
		{
			var userID = await this.ActivateAndGetSignedInUserGuid(token);
			if (userID == null)
				throw new GlobalizedAuthenticationException();
			await this.Repository.AddCategory(partyID, categoryID, userID.Value, token);
		}

		public virtual async Task RemoveCategory(Guid partyID, int categoryID, CancellationToken token = default(CancellationToken))
		{
			var userID = await this.ActivateAndGetSignedInUserGuid(token);
			if (userID == null)
				throw new GlobalizedAuthenticationException();
			await this.Repository.RemoveCategory(partyID, categoryID, userID.Value, token);
		}



		public Task<QueryResult<PartyContactMechanismResult<Address>>> AddAddress(PartyContactMechanismForAdd<AddressForAdd> address, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> AddAddressView(PartyContactMechanismForAdd<AddressForAdd> address, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<PartyContactMechanismResult<Email>>> AddEmail(PartyContactMechanismForAdd<EmailForAdd> email, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> AddEmailView(PartyContactMechanismForAdd<EmailForAdd> email, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<PartyContactMechanismResult<Phone>>> AddPhone(PartyContactMechanismForAdd<PhoneForAdd> phone, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> AddPhoneView(PartyContactMechanismForAdd<PhoneForAdd> phone, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<PartyContactMechanismResult<ContactMechanismGeneric>>> AddContactMechanism(PartyContactMechanismForAdd<ContactMechanismGenericForAdd> mechanism, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> AddContactMechanismView(PartyContactMechanismForAdd<ContactMechanismGenericForAdd> mechanism, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<PartyContactMechanismResult<Address>>> UpdateAddress(PartyContactMechanismForUpdate<AddressForUpdate> address, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> UpdateAddressView(PartyContactMechanismForUpdate<AddressForUpdate> address, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<PartyContactMechanismResult<Email>>> UpdateEmail(PartyContactMechanismForUpdate<EmailForUpdate> email, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> UpdateEmailView(PartyContactMechanismForUpdate<EmailForUpdate> email, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<PartyContactMechanismResult<Phone>>> UpdatePhone(PartyContactMechanismForUpdate<PhoneForUpdate> phone, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> UpdatePhoneView(PartyContactMechanismForUpdate<PhoneForUpdate> phone, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<PartyContactMechanismResult<ContactMechanismGeneric>>> UpdateContactMechanism(PartyContactMechanismForUpdate<ContactMechanismGenericForUpdate> mechanism, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> UpdateContactMechanismView(PartyContactMechanismForUpdate<ContactMechanismGenericForUpdate> mechanism, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<PartyContactMechanismResult<Address>>> GetAddresses(string contactMechanismTypeName = null, string viewName = null, FilterContext<PartyContactMechanismResult<Address>> filter = null, SortContext<PartyContactMechanismResult<Address>> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetAddressesView(string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<PartyContactMechanismResult<Address>>> GetAddressesById(Guid id, string contactMechanismTypeName = null, string viewName = null, FilterContext<PartyContactMechanismResult<Address>> filter = null, SortContext<PartyContactMechanismResult<Address>> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetAddressesByIdView(Guid id, string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<PartyContactMechanismResult<Email>>> GetEmails(string contactMechanismTypeName = null, string viewName = null, FilterContext<PartyContactMechanismResult<Email>> filter = null, SortContext<PartyContactMechanismResult<Email>> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetEmailsView(string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<PartyContactMechanismResult<Email>>> GetEmailsById(Guid id, string contactMechanismTypeName = null, string viewName = null, FilterContext<PartyContactMechanismResult<Email>> filter = null, SortContext<PartyContactMechanismResult<Email>> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetEmailsByIdView(Guid id, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<PartyContactMechanismResult<Phone>>> GetPhones(string viewName = null, string contactMechanismTypeName = null, FilterContext<PartyContactMechanismResult<Phone>> filter = null, SortContext<PartyContactMechanismResult<Phone>> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetPhonesView(string viewName = null, string contactMechanismTypeName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<PartyContactMechanismResult<Phone>>> GetPhonesById(Guid id, string contactMechanismTypeName = null, string viewName = null, FilterContext<PartyContactMechanismResult<Phone>> filter = null, SortContext<PartyContactMechanismResult<Phone>> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetPhonesByIdView(Guid id, string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<PartyContactMechanismResult<ContactMechanismGeneric>>> GetContactMechanisms(string contactMechanismTypeName = null, string viewName = null, FilterContext<PartyContactMechanismResult<ContactMechanismGeneric>> filter = null, SortContext<PartyContactMechanismResult<ContactMechanismGeneric>> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetContactMechanismsView(string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<PartyContactMechanismResult<ContactMechanismGeneric>>> GetContactMechanismsById(Guid id, string contactMechanismTypeName = null, string viewName = null, FilterContext<PartyContactMechanismResult<ContactMechanismGeneric>> filter = null, SortContext<PartyContactMechanismResult<ContactMechanismGeneric>> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResults<JObject>> GetContactMechanismsByIdView(Guid id, string contactMechanismTypeName = null, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<PartyContactMechanismResult<Address>>> GetAddress(Guid id, Guid addressId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> GetAddressView(Guid id, Guid addressId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<PartyContactMechanismResult<Email>>> GetEmail(Guid id, Guid emailId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> GetEmailView(Guid id, Guid emailId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<PartyContactMechanismResult<Phone>>> GetPhone(Guid id, Guid phoneId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> GetPhoneView(Guid id, Guid phoneId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<PartyContactMechanismResult<ContactMechanismGeneric>>> GetContactMechanism(Guid id, Guid contactMechanismId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<QueryResult<JObject>> GetContactMechanismView(Guid id, Guid contactMechanismId, string contactMechanismTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task DeleteAddressSoft(Guid id, Guid addressId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task DeletePhoneSoft(Guid id, Guid phoneId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task DeleteEmailSoft(Guid id, Guid emailId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task DeleteAddressHard(Guid id, Guid addressId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task DeletePhoneHard(Guid id, Guid phoneId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task DeleteEmailHard(Guid id, Guid emailId, string contactMechanismTypeName, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task DeleteContactMechanismHard(PartyContactMechanismId key, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task DeleteContactMechanismSoft(PartyContactMechanismId key, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Address>>> ChangeStatusAddress(Guid id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Email>>> ChangeStatusEmail(Guid id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Phone>>> ChangeStatusPhone(Guid id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<ContactMechanismGeneric>>> ChangeStatusContactMechanism(Guid id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Address>>> ChangeStatusAddressStatus(Guid id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Email>>> ChangeStatusEmailStatus(Guid id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Phone>>> ChangeStatusPhoneStatus(Guid id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Phone>>> ChangeStatusContactMechanismStatus(Guid id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Address>>> ChangeStatusAddressForce(Guid id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Email>>> ChangeStatusEmailForce(Guid id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Phone>>> ChangeStatusPhoneForce(Guid id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<ContactMechanismGeneric>>> ChangeStatusContactMechanismForce(Guid id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Address>>> ChangeStatusAddressStatusForce(Guid id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Email>>> ChangeStatusEmailStatusForce(Guid id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<Phone>>> ChangeStatusPhoneStatusForce(Guid id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<PartyContactMechanismResult<ContactMechanismGeneric>>> ChangeStatusContactMechanismStatusForce(Guid id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressView(Guid id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailView(Guid id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneView(Guid id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismView(Guid id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressStatusView(Guid id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailStatusView(Guid id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneStatusView(Guid id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismStatusView(Guid id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressForceView(Guid id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailForceView(Guid id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneForceView(Guid id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismForceView(Guid id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusAddressStatusForceView(Guid id, Guid addressId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusEmailStatusForceView(Guid id, Guid emailId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusPhoneStatusForceView(Guid id, Guid phoneId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ChangeStatusQueryResult<JObject>> ChangeStatusContactMechanismStatusForceView(Guid id, Guid contactMechanismId, string contactMechanismTypeName, string statusTypeName, string viewName = null, CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}
	}

	public class PartyImplementation<TRepository, TReadOnlyImplementation, TPartyResult, TPartyForAdd, TPartyForUpdate> : PartyImplementation<TRepository, TReadOnlyImplementation, Guid, TPartyResult, TPartyForAdd, TPartyForUpdate>
		where TRepository: IPartyRepository<TPartyResult, TPartyForAdd, TPartyForUpdate>
		where TPartyForAdd : IPartyForAdd
		where TPartyForUpdate: IPartyForUpdate
		where TPartyResult: IPartyResult
		where TReadOnlyImplementation: IReadOnlyPartyImplementation<Guid, TPartyResult>
	{
		public PartyImplementation(TRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact, TReadOnlyImplementation readOnly, IReadOnlyProductImplementation product) :
			base(repository, security, partyContact, readOnly, product)
		{
		}
	}

	public class PartyImplementation : PartyImplementation<IPartyRepository, IReadOnlyPartyImplementation, PartyResult, PartyForAdd, PartyForUpdate>, IPartyImplementation
	{
		public PartyImplementation(IPartyRepository repository, ISecurityImplementation security, IPartyContactImplementation partyContact, IReadOnlyPartyImplementation readOnly, IReadOnlyProductImplementation product) :
			base(repository, security, partyContact, readOnly, product)
		{
		}
	}
}
