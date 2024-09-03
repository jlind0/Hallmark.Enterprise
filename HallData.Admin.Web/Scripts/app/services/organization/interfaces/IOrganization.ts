module HallData.EMS.Services.Organization {
	"use strict";

	export interface IOrganization extends Party.IParty {
		Name?: string;
		Ein?: string;
		Website?: string;
		Code?: string;
		Tier?: ITier[];
		ArName?: string;
	}
}