module HallData.EMS.Perspectives {
	"use strict";

	export interface IPerspectivesScope extends IViewScope<IPerspectivesController> {
		vm: PerspectivesController;
	}

}