module HallData.EMS.Services.Customer {
	"use strict";

	export interface ICustomerServiceGeneric<TCustomer extends ICustomer> extends Party.IPartyService<TCustomer> {

	}

	export interface ICustomerService extends ICustomerServiceGeneric<ICustomer> {

	}
}