module HallData.EMS.Services.Category {
	"use strict";

	export interface ICategory {
		Name?: string;
		CategoryType?: ICategoryType;
		Path?: string;
		Id?: number;
		Status?: IStatusType;
		Level?: number;
		OrderIndex?: number;
		ParentId?: number;
		IsRoleRequired?: boolean;
		IsDynamic?: boolean;
	}
}