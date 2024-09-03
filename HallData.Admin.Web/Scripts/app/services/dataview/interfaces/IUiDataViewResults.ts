module HallData.EMS.Services.DataView {
	"use strict";

	export interface IUiDataViewResults extends IUiDataView{
		dataviewresultid?: number;
		parentdataviewresultid?: number;
		resultindex?: number;
		entitycollectionid?: number;
	}
} 