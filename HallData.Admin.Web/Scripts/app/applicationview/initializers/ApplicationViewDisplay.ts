module HallData.EMS.ApplicationView.Display {
	"use strict";

	export function init() {
		console.log("init");
		var ems = angular.module("ems");
		ems.controller("applicationViewDisplayController", ApplicationView.ApplicationViewDisplayController);

		ems.service("personalizationService", UI.Services.PersonalizationService);
		ems.service("applicationViewService", EMS.Services.ApplicationView.ApplicationViewService);
		ems.service("routeDictionaryService", Util.RouteDictionaryService);
		ems.service("redirectService", Util.RedirectService);
		ems.service("enumerationsService", EMS.Services.EnumerationsService);
	}

	init();
}