module HallData.EMS.ApplicationView {
	"use strict";

	export interface IApplicationViewAddController extends IViewController<IApplicationViewAddScope> {
		add(isValid: boolean, applicationView: Service.IQueryResult<Services.ApplicationView.IApplicationView>): void;
	}
}