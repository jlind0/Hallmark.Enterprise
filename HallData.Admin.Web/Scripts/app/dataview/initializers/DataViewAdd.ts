module HallData.EMS.DataView.Add {
	"use strict";
	export function init() {
		console.log("init");
		var ems = angular.module("ems");
		ems.controller("dataViewAddController", DataView.DataViewAddController);

		ems.service("dataViewService", EMS.Services.DataView.DataViewService);
		ems.service("routeDictionaryService", Util.RouteDictionaryService);
		ems.service("redirectService", Util.RedirectService);
	}

	init();
}