module HallData.EMS.Interface {
	"use strict";

	export interface IInterfaceAddController extends IViewController<IInterfaceAddScope> {
		add(isValid: boolean, interfaceObj: Service.IQueryResult<Services.Interface.IInterface>): void;
	}
}