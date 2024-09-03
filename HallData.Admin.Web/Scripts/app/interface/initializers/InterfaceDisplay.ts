module HallData.EMS.Interface.Display {
	"use strict";

	export function init() {
		console.log("init");
		var ems = angular.module("ems");
		ems.controller("interfaceDisplayController", Interface.InterfaceDisplayController);

		ems.service("personalizationService", UI.Services.PersonalizationService);
		ems.service("interfaceService", EMS.Services.Interface.InterfaceService);
		ems.service("interfaceAttributeService", EMS.Services.InterfaceAttribute.InterfaceAttributeService);
		ems.service("routeDictionaryService", Util.RouteDictionaryService);
		ems.service("redirectService", Util.RedirectService);
		ems.service("enumerationsService", EMS.Services.EnumerationsService);
	}

	init();
}