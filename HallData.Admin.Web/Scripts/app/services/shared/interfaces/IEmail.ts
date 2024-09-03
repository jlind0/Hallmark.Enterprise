module HallData.EMS.Services {
	"use strict";

	export interface IEmail extends IContactMechanism {
		EmailAddress?: string;
		ContactMechanismTypeName?: string; //TODO ContactMechanismTypeName?: IMechanismType;
	}

}