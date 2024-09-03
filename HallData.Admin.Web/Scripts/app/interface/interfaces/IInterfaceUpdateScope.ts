module HallData.EMS.Interface {
	"use strict";
	
	export interface IInterfaceUpdateScope extends IViewScope<IInterfaceUpdateController> {
		interfaceObj: Services.Interface.IInterface;
		availableInterfaces?: Services.Interface.IInterface[];
		isSaving: boolean;
	}
}