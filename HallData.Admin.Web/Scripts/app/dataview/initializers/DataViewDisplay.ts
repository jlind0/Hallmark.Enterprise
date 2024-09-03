module HallData.EMS.DataView.Display {
	"use strict";

	export function init() {
		console.log("init");
		var ems = angular.module("ems");
		ems.controller("dataViewDisplayController", DataView.DataViewDisplayController);

		ems.service("personalizationService", UI.Services.PersonalizationService);
		ems.service("dataViewService", EMS.Services.DataView.DataViewService);
		ems.service("routeDictionaryService", Util.RouteDictionaryService);
		ems.service("redirectService", Util.RedirectService);
		ems.service("enumerationsService", EMS.Services.EnumerationsService);
	}

	init();
}