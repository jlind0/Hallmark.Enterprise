module HallData.EMS.Interface {
	"use strict";
	
	export interface IInterfaceDisplayScope extends IViewScope<IInterfaceDisplayController> {
		interfaceObj: EMS.Services.Interface.IInterface;
	}

}