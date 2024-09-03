module HallData.EMS.Menu {
	"use strict";
	export function init() {
		console.log("init");
		var ems = angular.module("ems");
		ems.controller("menuController", Menu.MenuController);

		ems.service("routeDictionaryService", Util.RouteDictionaryService);
		ems.service("redirectService", Util.RedirectService);

		//ems.controller('MyCtrl', function ($scope, $location) {
		//	console.log("Controller");
		//	$scope.isActive = function (route) {
		//		console.log("isActive");
		//		return route === $location.path();
		//	}
		//});
	}

	init();
}