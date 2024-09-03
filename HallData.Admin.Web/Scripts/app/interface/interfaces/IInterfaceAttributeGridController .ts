module HallData.EMS.InterfaceAttribute {
	"use strict";

	export interface IInterfaceAttributeGridController extends IViewController<IInterfaceAttributeGridScope> {
		grid: ViewModels.Interface.InterfaceAttributeGridModel;
	}
} 