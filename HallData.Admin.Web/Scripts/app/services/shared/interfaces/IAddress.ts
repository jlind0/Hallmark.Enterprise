module HallData.EMS.Services {
	"use strict";
	
	export interface IAddress extends IContactMechanism {
		AddressLine1?: string;
		AddressLine2?: string;
		AddressLine3?: string;
		City?: string;
		PostalCode?: string;
		Country?: string;
	}
}