module HallData.EMS.Services.User {
	"use strict";

	export interface IUserForUpdate extends Person.IPerson {
		UserRelationshipStatus?: IStatusType; //partyrelationship table
		UserOf?: Organization.IOrganization;
		Title?: ITitleType;
		PrimaryEmail?: IPrimaryEmail;
		UserName?: string;
		Password?: string;
		PartyCategories?: IPartyCategoryForUpdate[];
		DefaultPartyCategory?: IPartyCategory;
	}
} 