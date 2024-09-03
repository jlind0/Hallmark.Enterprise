module HallData.EMS.Services.Party {
	"use strict";

	export interface IPartyContactService {
		AddAddress(address: IPartyContactMechanism<IAddress>): JQueryPromise<Service.IQueryResult<IPartyContactMechanism<IAddress>>>;
		AddEmail(address: IPartyContactMechanism<IEmail>): JQueryPromise<Service.IQueryResult<IPartyContactMechanism<IEmail>>>;
		AddPhone(address: IPartyContactMechanism<IPhone>): JQueryPromise<Service.IQueryResult<IPartyContactMechanism<IPhone>>>;

		UpdateAddress(address: IPartyContactMechanism<IAddress>): JQueryPromise<Service.IQueryResult<IPartyContactMechanism<IAddress>>>;
		UpdateEmail(address: IPartyContactMechanism<IEmail>): JQueryPromise<Service.IQueryResult<IPartyContactMechanism<IEmail>>>;
		UpdatePhone(address: IPartyContactMechanism<IPhone>): JQueryPromise<Service.IQueryResult<IPartyContactMechanism<IPhone>>>;

		GetAddresses(partyId: string, filter: Service.IFilterContext, sort: Service.ISortContext, page: Service.IPageDescriptor): JQueryPromise<Service.IQueryResults<IPartyContactMechanism<IAddress>>>;
		GetEmails(partyId: string, filter: Service.IFilterContext, sort: Service.ISortContext, page: Service.IPageDescriptor): JQueryPromise<Service.IQueryResults<IPartyContactMechanism<IEmail>>>;
		GetPhones(partyId: string, filter: Service.IFilterContext, sort: Service.ISortContext, page: Service.IPageDescriptor): JQueryPromise<Service.IQueryResults<IPartyContactMechanism<IPhone>>>;

		GetAddress(partyId: string, contactMechanismId: string): JQueryPromise<Service.IQueryResult<IPartyContactMechanism<IAddress>>>;
		GetEmail(partyId: string, contactMechanismId: string): JQueryPromise<Service.IQueryResult<IPartyContactMechanism<IEmail>>>;
		GetPhone(partyId: string, contactMechanismId: string): JQueryPromise<Service.IQueryResult<IPartyContactMechanism<IPhone>>>;
	}
}