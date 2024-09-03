module HallData.Util {
	"use strict";

	declare var perspectiveValueDictionary: string;

	export class PerspectiveDictionaryService {

		constructor() {
			console.log("perspective " + perspectiveValueDictionary);
			return perspectiveValueDictionary;
		}

	}
}