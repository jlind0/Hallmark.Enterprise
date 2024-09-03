module HallData.EMS.Services.Customer {
	"use strict";

	declare var serviceLocation: string;

	export class CustomerOrganizationService extends Organization.OrganizationService<ICustomerOrganization> implements ICustomerOrganizationService {
		constructor() {
			super(serviceLocation + "customers/organizations/");
		}
		protected GetUrlPathForKey(customer: ICustomer): any {
			return customer.PartyGuid + (Util.String.isNullOrWhitespace(customer.CustomerOfPartyGuid) ? "" : "/" + customer.CustomerOfPartyGuid);
		}
	}
} 