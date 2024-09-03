module HallData.EMS.Shared {
	"use strict";

	export interface ISharedScope extends Controllers.IScope<SharedController> {
		
	}

	export class SharedController extends Controllers.ControllerBase<ISharedScope>{
		public static $inject = [
			"$scope",
			"$http",
			"authentication"
		];
		constructor($scope: ISharedScope, $http: ng.IHttpService, authentication: Authentication.IAuthenticationProvider) {
			super($scope, $http, authentication, true);
			console.log("constructor");
		}
	}

	declare var loginPageUrl: string;
	declare var authenticationProvider: Authentication.IAuthenticationProvider;

	export function init() {
		console.log("init");
		var mod = angular.module("ems", ['ngMessages', 'ui.select', 'ui.multiselect', 'xeditable', 'isteven-multi-select']);
		mod.controller("sharedController", SharedController);
		mod.service("enumerationsService", HallData.EMS.Services.EnumerationsService);
		mod.factory("authentication",() => {
			if (Util.Obj.isNullOrUndefined(authenticationProvider)) {
				var authService = new Authentication.AuthenticationService();
				authenticationProvider = new Authentication.AuthenticationProvider(authService, loginPageUrl);
			}
			return authenticationProvider;
		});
		mod.config(['$httpProvider', $httpProvider => $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest']);
	}
}