module HallData.EMS.ApplicationView {
	"use strict";

	export function init() {
		console.log("init");
		Services.EnumerationsServiceFactory.Initialize(() => new Services.EnumerationsService());
		var mod = angular.module("ems");
		mod.controller("applicationViewGridController", ApplicationViewGridController);
		mod.service("applicationViewService", Services.ApplicationView.ApplicationViewService);
		mod.service("personalizationService", UI.Services.PersonalizationService);
		mod.service("routeDictionaryService", Util.RouteDictionaryService);
		mod.service("redirectService", Util.RedirectService);
		mod.service("enumerationsService", EMS.Services.EnumerationsService);
	}

	init();
}