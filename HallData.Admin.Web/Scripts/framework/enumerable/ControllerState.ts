module HallData.Controllers {
	"use strict";

	export enum ControllerState {
		Initializing,
		Initialized,
		Loading,
		Loaded,
		Unloading,
		Unloaded,
		Error
	}
}