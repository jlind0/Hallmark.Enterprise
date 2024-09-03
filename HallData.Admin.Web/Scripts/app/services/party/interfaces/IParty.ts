module HallData.EMS.Services.Party {
	"use strict";

	export interface IParty {
		PartyGuid?: string;
		Status?: IStatusType;
		PartyType?: IPartyType;
	}
}