module HallData.EMS.Services.DataView {
	"use strict";

	export interface IUiEntityColumns extends IUiEntity{
		entitycolumnid?: number;
		name?: string;
		displayname?: string;
		iskey?: boolean;
		iscollection?: string;
		childentityid?: string;
		isrecursive?: boolean;
	}
} 