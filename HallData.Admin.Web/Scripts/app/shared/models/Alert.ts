/// <reference path="../interfaces/IAlert.ts"/>

module HallData.Message {
	'use strict';

	export class Alert implements IAlert {

		constructor(input: IAlert) {
			for (var key in input) {
				if (key) {
					this[key] = input[key];
				}
			}
		}

		public alertType;
		public heading;
		public message;
		public title;

	}

}