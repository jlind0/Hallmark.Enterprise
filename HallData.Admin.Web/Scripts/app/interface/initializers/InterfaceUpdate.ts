module HallData.EMS.Interface.Update {
	"use strict";
	export function init() {
		console.log("init");
		var ems = angular.module("ems");
		ems.controller("interfaceUpdateController", Interface.InterfaceUpdateController);

		ems.directive("formatInterfaceName", Interface.formatInterfaceName);
		//ems.directive("generateCode", Interface.generateCode);

		ems.service("interfaceService", EMS.Services.Interface.InterfaceService);
		ems.service("routeDictionaryService", Util.RouteDictionaryService);
		ems.service("redirectService", Util.RedirectService);
	}

	init();
}