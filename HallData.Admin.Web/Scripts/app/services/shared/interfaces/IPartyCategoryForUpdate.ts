module HallData.EMS.Services {
	"use strict";

	export interface IPartyCategoryForUpdate {
		Add?: IPartyCategory;
		Update?: IPartyCategory;
		HardDelete?: IPartyCategory;
		SoftDelete?: IPartyCategory;
	}
}