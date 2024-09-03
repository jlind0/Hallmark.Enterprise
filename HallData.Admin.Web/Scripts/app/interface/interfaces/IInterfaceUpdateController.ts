module HallData.EMS.Interface {
	"use strict";

	export interface IInterfaceUpdateController extends IViewController<IInterfaceUpdateScope> {
		update(isValid: boolean, interfaceObj: Service.IQueryResult<Services.Interface.IInterface>): void;
	}
}