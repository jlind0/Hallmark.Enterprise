module HallData.EMS.InterfaceAttribute {
	"use strict";

	export function init() {
		console.log("init");
		Services.EnumerationsServiceFactory.Initialize(() => new Services.EnumerationsService());
		var mod = angular.module("ems");
		mod.controller("interfaceAttributeGridController", InterfaceAttributeGridController);
		mod.service("interfaceAttributeService", Services.InterfaceAttribute.InterfaceAttributeService);
		mod.service("personalizationService", UI.Services.PersonalizationService);
		mod.service("routeDictionaryService", Util.RouteDictionaryService);
		mod.service("redirectService", Util.RedirectService);
		mod.service("enumerationsService", EMS.Services.EnumerationsService);
	}

	init();
}