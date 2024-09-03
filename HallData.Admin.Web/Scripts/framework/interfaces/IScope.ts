module HallData.Controllers {
	"use strict";

	export interface IScope<TController> extends ng.IScope {
		vm: TController;
		isSignedIn: boolean;
		isAuthenticating: boolean;
		isAuthenticationError: boolean;
		isInitializing: boolean;
		isLoading: boolean;
		isLoaded: boolean;
		isInitialized: boolean;
		hasErrored: boolean;
		isUnloading: boolean;
		isUnloaded: boolean;
		isAuthenticated: boolean;
		isLoggingOut: boolean;
		errorMessage: string;
	}
}