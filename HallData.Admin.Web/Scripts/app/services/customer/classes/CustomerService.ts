module HallData.EMS.Services.Customer {
	"use strict";

	declare var serviceLocation: string;
	
	export class CustomerService extends Party.PartyService<ICustomer> implements ICustomerService {

		constructor() {
			super(serviceLocation + "customers/");
			console.log("constructor");
		}

		protected GetUrlPathForKey(customer: ICustomer): any {
			console.log("GetUrlPathForKey");
			return customer.PartyGuid + (Util.String.isNullOrWhitespace(customer.CustomerOfPartyGuid) ? "" : "/" + customer.CustomerOfPartyGuid);
		}
	}
} 