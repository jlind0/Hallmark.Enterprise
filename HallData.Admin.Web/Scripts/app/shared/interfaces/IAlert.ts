/// <reference path="../enumerable/AlertType.ts"/>
module HallData.Message {
	"use strict";

	export interface IAlert {
		alertType: AlertType;
		heading: string;
		message: string;
		title: string;
	}

}