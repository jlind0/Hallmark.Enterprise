module HallData.EMS.DataView {
	"use strict";

	export interface IDataViewGridController extends IViewController<IDataViewGridScope> {
		grid: ViewModels.DataView.DataViewGridModel;
	}
} 