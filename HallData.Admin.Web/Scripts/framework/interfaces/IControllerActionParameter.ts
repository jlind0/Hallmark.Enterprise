module HallData.Controllers {
	"use strict";

	export interface IControllerActionParameter {
		action: ControllerAction;
		processDelegate?: () => JQueryPromise<void>;
		isSecondTry?: boolean;
		isSync?: boolean;
	}
}