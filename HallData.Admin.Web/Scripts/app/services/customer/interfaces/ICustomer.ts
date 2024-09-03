module HallData.EMS.Services.Customer {
	"use strict";

	export interface ICustomer extends Party.IParty {
		CustomerOfPartyGuid?: string;
		CustomerRelationshipStatus?: IStatusType;
		CustomerOf?: Organization.IOrganization;
	}
}