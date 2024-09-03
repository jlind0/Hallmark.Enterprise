module HallData.EMS.Services.Party {
	"use strict";

	export interface IPartyContactMechanism<TContactMechanism extends IContactMechanism> {
		PartyGuid: string;
		ContactMechanismGuid?: string;
		ContactMechanismTypeId: IContactMechanismType;
		OrderIndex: number;
		Attention: string;
		CreateDate?: Date;
		UpdateDate?: Date;
		IsPrimary?: boolean;
		StatusTypeId: IStatusType;
		ContactMechanism: TContactMechanism;
	}
}