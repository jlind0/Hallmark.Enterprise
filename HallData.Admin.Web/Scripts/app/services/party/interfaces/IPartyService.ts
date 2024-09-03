module HallData.EMS.Services.Party {
	"use strict";

	export interface IPartyService<TParty extends IParty> extends Service.IDeleteableService<TParty>, IPartyContactService
	{
		AddPartyCategory(partyId: string, categoryId: string): JQueryPromise<void>;
		RemovePartyCategory(partyId: string, categoryId: string): JQueryPromise<void>;
	}
}