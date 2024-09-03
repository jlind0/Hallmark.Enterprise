module HallData.EMS.Services.Customer {
	"use strict";

	export interface ICustomerPerson extends Person.IPerson, Customer.ICustomer {
		JobTitle?: string;
		WorksFor?: string;
	}
}