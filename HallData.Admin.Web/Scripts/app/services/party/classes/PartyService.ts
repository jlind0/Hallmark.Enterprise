module HallData.EMS.Services.Party {
	"use strict";
	
	export class PartyService<TParty extends IParty> extends Service.DeletableService<TParty> implements IPartyService<TParty> {

		// TODO: what is the data object to be sent to the server?
		AddPartyCategory(partyId: string, categoryId: string): JQueryPromise<void> {
			console.log("AddPartyCategory");
			return Util.Service.Post<void>({}, partyId + "/Categories/" + categoryId + "/Add/");
		}

		RemovePartyCategory(partyId: string, categoryId: string): JQueryPromise<void> {
			console.log("RemovePartyCategory");
			return Util.Service.Delete<void>(partyId + "/Categories/" + categoryId + "/Remove/");
		}

		AddAddress(address: IPartyContactMechanism<IAddress>): JQueryPromise<HallData.Service.IQueryResult<IPartyContactMechanism<IAddress>>> {
			console.log("AddAddress");
			return Util.Service.Post<Service.IQueryResult<IPartyContactMechanism<IAddress>>>(address, "Address/Add/");
		}

		AddEmail(email: IPartyContactMechanism<IEmail>): JQueryPromise<HallData.Service.IQueryResult<IPartyContactMechanism<IEmail>>> {
			console.log("AddEmail");
			return Util.Service.Post<Service.IQueryResult<IPartyContactMechanism<IEmail>>>(email, "Email/Add/");
		}

		AddPhone(phone: IPartyContactMechanism<IPhone>): JQueryPromise<HallData.Service.IQueryResult<IPartyContactMechanism<IPhone>>> {
			console.log("AddPhone");
			return Util.Service.Post<Service.IQueryResult<IPartyContactMechanism<IPhone>>>(phone, "Phone/Add/");
		}

		UpdateAddress(address: IPartyContactMechanism<IAddress>): JQueryPromise<HallData.Service.IQueryResult<IPartyContactMechanism<IAddress>>> {
			console.log("UpdateAddress");
			return Util.Service.Put<HallData.Service.IQueryResult<IPartyContactMechanism<IAddress>>>(address, "Address/Update/");
		}

		UpdateEmail(email: IPartyContactMechanism<IEmail>): JQueryPromise<HallData.Service.IQueryResult<IPartyContactMechanism<IEmail>>> {
			console.log("UpdateEmail");
			return Util.Service.Put<HallData.Service.IQueryResult<IPartyContactMechanism<IEmail>>>(email, "Email/Update/");
		}

		UpdatePhone(phone: IPartyContactMechanism<IPhone>): JQueryPromise<HallData.Service.IQueryResult<IPartyContactMechanism<IPhone>>> {
			console.log("UpdatePhone");
			return Util.Service.Put<HallData.Service.IQueryResult<IPartyContactMechanism<IPhone>>>(phone, "Phone/Update/");
		}

		GetAddresses(partyId: string, filter: HallData.Service.IFilterContext, sort: HallData.Service.ISortContext, page: HallData.Service.IPageDescriptor): JQueryPromise<HallData.Service.IQueryResults<IPartyContactMechanism<IAddress>>> {
			console.log("GetAddress");
			return Util.Service.GetQueryResults<IPartyContactMechanism<IAddress>>(partyId + "/Addresses/", filter, sort, page);
		}

		GetEmails(partyId: string, filter: HallData.Service.IFilterContext, sort: HallData.Service.ISortContext, page: HallData.Service.IPageDescriptor): JQueryPromise<HallData.Service.IQueryResults<IPartyContactMechanism<IEmail>>> {
			console.log("GetEmails");
			return Util.Service.GetQueryResults<IPartyContactMechanism<IEmail>>(partyId + "/Emails/", filter, sort, page);
		}

		GetPhones(partyId: string, filter: HallData.Service.IFilterContext, sort: HallData.Service.ISortContext, page: HallData.Service.IPageDescriptor): JQueryPromise<HallData.Service.IQueryResults<IPartyContactMechanism<IPhone>>> {
			console.log("GetPhones");
			return Util.Service.GetQueryResults<IPartyContactMechanism<IPhone>>(partyId + "/Phones/", filter, sort, page);
		}

		GetAddress(partyId: string, contactMechanismId: string): JQueryPromise<HallData.Service.IQueryResult<IPartyContactMechanism<IAddress>>> {
			console.log("GetAddress");
			return Util.Service.GetQueryResult<IPartyContactMechanism<IAddress>>(partyId + "/Addresses/" + contactMechanismId);
		}

		GetEmail(partyId: string, contactMechanismId: string): JQueryPromise<HallData.Service.IQueryResult<IPartyContactMechanism<IEmail>>> {
			console.log("GetEmails");
			return Util.Service.GetQueryResult<IPartyContactMechanism<IEmail>>(partyId + "/Emails/" + contactMechanismId);
		}

		GetPhone(partyId: string, contactMechanismId: string): JQueryPromise<HallData.Service.IQueryResult<IPartyContactMechanism<IPhone>>> {
			console.log("GetPhones");
			return Util.Service.GetQueryResult<IPartyContactMechanism<IPhone>>(partyId + "/Phones/" + contactMechanismId);
		}
		
	}
} 