module HallData.EMS.Interface {
	"use strict";
	
	export interface IInterfaceAddScope extends IViewScope<IInterfaceAddController> {
		interfaceObj: Services.Interface.IInterface;
		isSaving: boolean;
		availableInterfaces?: Services.Interface.IInterface;
	}
}