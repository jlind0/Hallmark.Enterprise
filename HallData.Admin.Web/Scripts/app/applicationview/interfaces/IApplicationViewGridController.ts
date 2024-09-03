module HallData.EMS.ApplicationView {
	"use strict";

	export interface IApplicationViewGridController extends IViewController<IApplicationViewGridScope> {
		grid: ViewModels.ApplicationView.ApplicationViewGridModel;
	}
} 