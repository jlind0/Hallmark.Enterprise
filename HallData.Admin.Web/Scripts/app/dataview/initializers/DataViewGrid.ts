module HallData.EMS.DataView {
	"use strict";

	export function init() {
		console.log("init");
		Services.EnumerationsServiceFactory.Initialize(() => new Services.EnumerationsService());
		var mod = angular.module("ems");
		mod.controller("dataViewGridController", DataViewGridController);
		mod.service("dataViewService", Services.DataView.DataViewService);
		mod.service("personalizationService", UI.Services.PersonalizationService);
		mod.service("routeDictionaryService", Util.RouteDictionaryService);
		mod.service("redirectService", Util.RedirectService);
		mod.service("enumerationsService", EMS.Services.EnumerationsService);
	}

	init();
}