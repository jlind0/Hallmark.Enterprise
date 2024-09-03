module HallData.EMS.ApplicationView {
	"use strict";
	
	export interface IApplicationViewAddScope extends IViewScope<IApplicationViewAddController> {
		applicationView: Services.ApplicationView.IApplicationView;
		isSaving: boolean;
	}

}