module HallData.EMS.DataView {
	"use strict";
	
	export interface IDataViewAddScope extends IViewScope<IDataViewAddController> {
		dataView: Services.DataView.IDataView;
		isSaving: boolean;
	}

}