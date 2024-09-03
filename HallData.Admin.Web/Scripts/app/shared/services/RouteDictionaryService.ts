module HallData.Util {
	"use strict";

	// generates an object compatible with List<KeyValuePair<string,object>>(C#) that contains Guid names and values as well as route name
	// ajaxData is used by AjaxAwareRedirectResult(C#) in BaseController(C#) to generate a string route value with populated Guids
	export class RouteDictionaryService {

		private ajaxData: {}[] = [];

		constructor() {
			console.log("constructor");
		}

		public generateData(routeName: string, routeData: IKeyValuePair[], routeValueDictionary?: IKeyValuePair[]): {}[] {
			console.log("generateData");

			this.addRouteName(routeName);

			var keyValuePair: {} = {};
			if (!Util.Obj.isNullOrUndefined(routeValueDictionary)) {
				routeValueDictionary.forEach((el: IKeyValuePair, index, array) => {
					keyValuePair[el.Key] = el.Value;
					this.ajaxData.push(keyValuePair);
					keyValuePair = {};
				}, this);
			}

			routeData.forEach((el: IKeyValuePair, index, array) => {
				keyValuePair[el.Key] = el.Value;
				this.ajaxData.push(keyValuePair);
				keyValuePair = {};
			}, this);

			this.toString();

			return this.ajaxData;
		}

		private addRouteName(routeName: string): void {
			console.log("addRouteName");
			this.ajaxData.push({ routeName: routeName });
		}

		private toString(): void {
			console.log("ajaxData: " + JSON.stringify(this.ajaxData));
		}
	}

}