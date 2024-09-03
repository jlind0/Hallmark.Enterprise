module HallData.EMS.InterfaceAttribute {
	"use strict";

	export interface IInterfaceAttributeAddController extends IViewController<IInterfaceAttributeAddScope> {
		add(isValid: boolean, interfaceAttributeObj: Service.IQueryResult<Services.InterfaceAttribute.IInterfaceAttribute>): void;
	}
}