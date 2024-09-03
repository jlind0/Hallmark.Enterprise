module HallData.EMS.DataView {
	"use strict";
	
	export interface IDataViewDisplayScope extends IViewScope<IDataViewDisplayController> {
		dataView: EMS.Services.DataView.IDataView;
	}

}