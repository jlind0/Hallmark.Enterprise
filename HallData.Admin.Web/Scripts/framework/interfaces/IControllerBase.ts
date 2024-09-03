module HallData.Controllers {
	"use strict";

	export interface IControllerBase<TScope extends IScope<any>> {
		controllerStateChanged: Events.ITypedEvent<ControllerState>;
		controllerAuthenticationStateChanged: Events.ITypedEvent<Authentication.AuthenticationState>;
		$scope: TScope;
		init(isSecondTry?: boolean): JQueryPromise<void>;
		load(isSecondTry?: boolean): JQueryPromise<void>;
		unload(isSecondTry?: boolean): JQueryPromise<void>;
		authenticate(delegate?: () => JQueryPromise<void>): JQueryPromise<void>;
		getControllerState(): ControllerState;
		getAuthenticationState(): Authentication.AuthenticationState;
		process(delegate: () => JQueryPromise<void>, isSecondTry?: boolean): JQueryPromise<void>;
	}
}