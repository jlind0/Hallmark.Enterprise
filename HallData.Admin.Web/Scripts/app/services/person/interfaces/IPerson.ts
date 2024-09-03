module HallData.EMS.Services.Person {
	"use strict";

	export interface IPerson extends Party.IParty {
		FirstName?: string;
		LastName?: string;
		Salutation?: string;
		MiddleName?: string;
		Suffix?: string;
		ArName?: string;
	}
}