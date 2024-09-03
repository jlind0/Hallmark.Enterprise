module HallData.EMS.Services {
	"use strict";

	export class AjaxSerializerFactory {

		//constructor() {
		//	console.log("transform");
		//	return this.transformRequest;
		//}
	
		constructor(data, getHeaders) {
			console.log("transformRequest");

			var headers = getHeaders();
			headers["Content-type"] = "application/x-www-form-urlencoded; charset=utf-8";

			// If this is not an object, defer to native stringification.
			if (!angular.isObject(data)) {
				return ((data == null) ? "" : data.toString());
			}

			var buffer = [];

			// Serialize each key in the object.
			for (var name in data) {

				if (!data.hasOwnProperty(name)) {
					continue;
				}

				var value = data[name];
				buffer.push(encodeURIComponent(name) + "=" + encodeURIComponent((value == null) ? "" : value));
			}

			// Serialize the buffer and clean it up for transportation.
			var source = buffer.join("&").replace(/%20/g, "+");

			return (source);
		}

		//transformRequest(data, getHeaders) {
		//	console.log("transformRequest");

		//	var headers = getHeaders();
		//	headers["Content-type"] = "application/x-www-form-urlencoded; charset=utf-8";
		//	return (this.serializeData(data));
		//}

		//serializeData(data): string {
		//	console.log("serializeData");

		//	// If this is not an object, defer to native stringification.
		//	if (!angular.isObject(data)) {
		//		return ((data == null) ? "" : data.toString());
		//	}

		//	var buffer = [];

		//	// Serialize each key in the object.
		//	for (var name in data) {

		//		if (!data.hasOwnProperty(name)) {
		//			continue;
		//		}

		//		var value = data[name];
		//		buffer.push(encodeURIComponent(name) + "=" + encodeURIComponent((value == null) ? "" : value));
		//	}

		//	// Serialize the buffer and clean it up for transportation.
		//	var source = buffer.join("&").replace(/%20/g, "+");

		//	return (source);
		//}
	}
}
