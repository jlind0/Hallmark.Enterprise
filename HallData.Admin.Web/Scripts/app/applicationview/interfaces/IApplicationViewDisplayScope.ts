module HallData.EMS.ApplicationView {
	"use strict";
	
	export interface IApplicationViewDisplayScope extends IViewScope<IApplicationViewDisplayController> {
		applicationView: EMS.Services.ApplicationView.IApplicationView;
	}

}