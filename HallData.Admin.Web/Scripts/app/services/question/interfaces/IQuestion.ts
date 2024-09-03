module HallData.EMS.Services.Question {
	"use strict";

	export interface IQuestion extends Party.IParty {
		CustomerOfPartyGuid?: string;
		CustomerRelationshipStatus?: IStatusType;
		CustomerOf?: Organization.IOrganization;
	}
}