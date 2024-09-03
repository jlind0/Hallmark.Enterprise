module HallData.EMS.Services.Person {
	"use strict";

	export class PersonService<TPerson extends IPerson> extends Party.PartyService<TPerson> implements IPersonService<TPerson> {

	}
}