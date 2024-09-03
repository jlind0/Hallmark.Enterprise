module HallData.EMS.Services.DataView {
	"use strict";

	export interface IUiDataViewColumns extends IUiEntity{
		dataviewcolumnid?: number;
		resultname?: string;
		isrequired?: boolean;
		entitycolumnid?: number;
		isvirtual?: boolean;
		alias?: string;
		recursivekeydataviewcolumnid?: number;
	}
} 