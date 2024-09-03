module HallData.EMS.Services {
	"use strict";

	export interface IPrimaryEmail extends IEmail {
		ContactMechanismType?: IContactMechanismType;
	}

}