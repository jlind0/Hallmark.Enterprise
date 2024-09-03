module HallData.EMS.InterfaceAttribute.Add {
	"use strict";
	export function init() {
		console.log("init");
		var ems = angular.module("ems");
		ems.controller("interfaceAttributeAddController", InterfaceAttribute.InterfaceAttributeAddController);

		ems.directive("formatAttributeName", InterfaceAttribute.formatAttributeName);

		ems.service("interfaceService", EMS.Services.Interface.InterfaceService);
		ems.service("interfaceAttributeService", EMS.Services.InterfaceAttribute.InterfaceAttributeService);
		ems.service("routeDictionaryService", Util.RouteDictionaryService);
		ems.service("redirectService", Util.RedirectService);
	}
	init();
}