module HallData.EMS.Perspectives {
	"use strict";

	export function init() {
		console.log("init");
		var ems = angular.module("ems");
		ems.controller("perspectivesController", Perspectives.PerspectivesController);

		ems.service("routeDictionaryService", Util.RouteDictionaryService);
		ems.service("perspectiveDictionaryService", Util.PerspectiveDictionaryService);
		ems.service("redirectService", Util.RedirectService);
	}

	init();
}