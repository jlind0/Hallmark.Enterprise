
module HallData.EMS {
	"use strict";

	declare var routeValueDictionary: Util.IKeyValuePair[];

	export class ViewController<T extends IViewScope<any>> extends Controllers.ControllerBase<T> implements IViewController<T> {
		
		protected http: ng.IHttpService;
		protected enumerationsService: Services.IEnumerationsService;
		protected routeDictionaryService: Util.RouteDictionaryService;
		protected redirectService: Util.RedirectService;
		protected routeValueDictionary: Util.IKeyValuePair[];
		protected perspectiveDictionaryService: Util.PerspectiveDictionaryService;

		public static $inject = [
			"$injector",
			"$scope",
			"authentication"
		];

		constructor(
			private $injector: any,
			$scope: T,
			authentication: Authentication.IAuthenticationProvider
		) {
			super($scope, $injector.get("$http"), authentication, true);
			console.log("constructor");

			this.http = $injector.get("$http");
			this.enumerationsService = $injector.get("enumerationsService");
			this.routeDictionaryService = $injector.get("routeDictionaryService");
			this.perspectiveDictionaryService = $injector.get("perspectiveDictionaryService");
			this.redirectService = $injector.get("redirectService");

			this.routeValueDictionary = routeValueDictionary;

			//this.perspectiveValueDictionary = perspectiveValueDictionary;
		}
	}
}