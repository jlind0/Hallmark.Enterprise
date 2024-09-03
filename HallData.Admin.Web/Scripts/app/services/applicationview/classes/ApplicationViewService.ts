module HallData.EMS.Services.ApplicationView {
	"use strict";

	declare var serviceLocation: string;

	export class ApplicationViewService extends Service.DeletableService<IApplicationView> implements IApplicationViewService {

		constructor() {
			super(serviceLocation + "applicationviews/");
			console.log("constructor");
		}
	}
} 