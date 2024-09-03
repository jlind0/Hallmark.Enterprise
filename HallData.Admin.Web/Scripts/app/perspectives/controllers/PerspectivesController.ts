module HallData.EMS.Perspectives {
	"use strict";

	declare var growl: Function;

	export class PerspectivesController extends ViewController<IPerspectivesScope> implements IPerspectivesController {

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
			$scope: IPerspectivesScope,
			authentication: Authentication.IAuthenticationProvider
		) {
			super($injector, $scope, authentication);
			console.log("constructor");
			$scope.vm = this;
		}
	}
}