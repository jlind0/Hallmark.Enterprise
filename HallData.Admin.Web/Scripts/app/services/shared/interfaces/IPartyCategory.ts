module HallData.EMS.Services {
	"use strict";

	export interface IPartyCategory {
		Id?: number;
		RoleId?: IRole;
		Status?: IStatusType;
		IsDefault?: boolean;
		OrderIndex?: number;
	}
}