module HallData.EMS.Services.Customer {
	"use strict";

	declare var serviceLocation: string;

	export class CustomerPersonService extends Person.PersonService<ICustomerPerson> implements ICustomerPersonService {

		constructor() {
			super(serviceLocation + "customers/people/");
			console.log("constructor");
		}

		protected GetUrlPathForKey(customer: ICustomer): any {
			console.log("GetUrlPathForKey");
			return customer.PartyGuid + (Util.String.isNullOrWhitespace(customer.CustomerOfPartyGuid) ? "" : "/" + customer.CustomerOfPartyGuid);
		}
	}
}