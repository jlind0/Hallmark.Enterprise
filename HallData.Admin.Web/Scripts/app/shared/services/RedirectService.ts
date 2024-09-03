module HallData.Util {
	"use strict";

	declare var websiteLocation: string;

	// accepts an object compatible with List<KeyValuePair<string,object>>(C#) and passes it to GetRoute action in BaseController(C#)
	// when MVC(C#) successfully generates a string route, promise success clause redirects to that route
	export class RedirectService {

		private http: ng.IHttpService;

		public static $inject = [
			"$http"
		];

		constructor(
			private $http: ng.IHttpService
		) {
			console.log("constructor");
			this.http = $http;
		}

		public redirect(routeValueDictionary: {}[]): void {
			console.log("redirect");

			var response = this.http({
				method: "POST",
				url: websiteLocation + "routes/get",
				data: JSON.stringify(routeValueDictionary),
			});

			response.then(this.onRedirectSuccess, this.onRedirectError);
		}

		public onRedirectSuccess(data: any): any {
			console.log("onRedirectSuccess: " + JSON.stringify(data));
			window.location.href = data.data.toString();
		}

		public onRedirectError(error: any): any {
			console.log("onRedirectError:" + JSON.stringify(error));
		}

	}
}