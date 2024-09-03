module HallData.EMS.ApplicationView.Add {
	"use strict";
	export function init() {
		console.log("init");
		var ems = angular.module("ems");
		ems.controller("applicationViewAddController", ApplicationView.ApplicationViewAddController);

		ems.service("applicationViewService", EMS.Services.ApplicationView.ApplicationViewService);
		ems.service("routeDictionaryService", Util.RouteDictionaryService);
		ems.service("redirectService", Util.RedirectService);
	}

	init();
}