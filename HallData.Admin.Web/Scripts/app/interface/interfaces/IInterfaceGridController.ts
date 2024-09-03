module HallData.EMS.Interface {
	"use strict";

	export interface IInterfaceGridController extends IViewController<IInterfaceGridScope> {
		grid: ViewModels.Interface.InterfaceGridModel;
	}
} 