module HallData.EMS.Menu {
	"use strict";

	export interface IMenuScope extends IViewScope<IMenuController> {
		isActive: any;
		vm: MenuController;
		location: ng.ILocationService;
	}

}