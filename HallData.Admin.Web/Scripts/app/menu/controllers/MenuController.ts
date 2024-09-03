module HallData.EMS.Menu {
	"use strict";

	declare var growl: Function;

	export class MenuController extends ViewController<IMenuScope> implements IMenuController {

		private tier: EMS.Services.ITier[];

		public static $inject = [
			"$injector",
			"$location",
			"$scope",
			"authentication"
		];

		constructor(
			$injector: any,
			$location: ng.ILocationService,
			$scope: IMenuScope,
			authentication: Authentication.IAuthenticationProvider
			) {
			super($injector, $scope, authentication);
			console.log("constructor");
			$scope.vm = this;
		}

		private isActive(menuTag): any {
			console.log("isActive");
			return menuTag === document.getElementById("menuTag").innerHTML;
		}

		private isActiveSub(subMenuTag): any {
			console.log("isActiveSub");
			return subMenuTag === document.getElementById("subMenuTag").innerHTML;
		}
	}
}