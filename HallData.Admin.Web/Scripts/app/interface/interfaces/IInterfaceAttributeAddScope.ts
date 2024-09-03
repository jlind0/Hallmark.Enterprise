module HallData.EMS.InterfaceAttribute {
	"use strict";
	
	export interface IInterfaceAttributeAddScope extends IViewScope<IInterfaceAttributeAddController> {
		interfaceObj: Services.Interface.IInterface;
		interfaceAttributeObj: Services.InterfaceAttribute.IInterfaceAttribute;
		isSaving: boolean;
	}
}