module HallData.Authentication {
	"use strict";

	export interface IAuthenticationProvider {
		SignInWindowsAuth(): JQueryPromise<boolean>;
		Authenticate(): JQueryPromise<boolean>;
		SignIn(username: string, password?: string, token?: string): JQueryPromise<boolean>;
		Logout(): JQueryPromise<boolean>;
		AuthenticationStatusChanged: HallData.Events.ITypedEvent<AuthenticationState>;
		IsSignedIn(): boolean;
	}

	export enum AuthenticationState {
		Authenticating,
		Authenticated,
		AuthenticationError,
		NotAuthenticated,
		Ending
	}

	export class AuthenticationProvider implements IAuthenticationProvider {

		public AuthenticationStatusChanged: HallData.Events.ITypedEvent<AuthenticationState> = new HallData.Events.TypedEvent();
		private authWorker: JQueryPromise<boolean>;

		constructor(protected service: IAuthenticationService, protected loginPageUrl: string) {
			console.log("constructor");
		}

		public SignInWindowsAuth(): JQueryPromise<boolean> {
			console.log("SignInWindowsAuth");
			this.AuthenticationStatusChanged.trigger(AuthenticationState.Authenticating);
			Util.CookieManager.deleteSessionId();
			return this.service.SignInWindowsAuth().then(
				s => {
					Util.CookieManager.setSessionId(s);
					Util.CookieManager.setIsWindowsAuth(true);
					this.AuthenticationStatusChanged.trigger(AuthenticationState.Authenticated);
					return true;
				},
				error => {
					this.AuthenticationStatusChanged.trigger(AuthenticationState.AuthenticationError);
					return error;
				}
			);
		}

		public SignIn(username: string, password?: string, token?: string): JQueryPromise<boolean> {
			console.log("SignIn");
			var d = $.Deferred<boolean>();
			if (!password && !token)
				d.reject("Password or token is required");
			else {
				this.AuthenticationStatusChanged.trigger(AuthenticationState.Authenticating);
				Util.CookieManager.deleteSessionId();
				return this.service.SignIn(username, password, token).then(
					s => {
						Util.CookieManager.setSessionId(s);
						Util.CookieManager.setIsWindowsAuth(false);
						this.AuthenticationStatusChanged.trigger(AuthenticationState.Authenticated);
						return true;
					},
					error => {
						this.AuthenticationStatusChanged.trigger(AuthenticationState.AuthenticationError);
						return error;
					}
				);
			}
			return d.promise();
		}

		public Logout(): JQueryPromise<boolean> {
			console.log("Logout");
			var d = $.Deferred<boolean>();
			var sessionId = Util.CookieManager.getSessionId();
			if (!sessionId) {
				d.reject("The user is not logged in");
			} else {
				this.AuthenticationStatusChanged.trigger(AuthenticationState.Ending);
				return this.service.Logout(sessionId).then(
					l => {
						if (l) {
							Util.CookieManager.deleteSessionId();
							this.AuthenticationStatusChanged.trigger(AuthenticationState.NotAuthenticated);
						} else {
							this.AuthenticationStatusChanged.trigger(AuthenticationState.AuthenticationError);
						}
						return l;
					},
					error => {
						this.AuthenticationStatusChanged.trigger(AuthenticationState.AuthenticationError);
						return error;
					}
				);
			}
			return d.promise();
		}

		public IsSignedIn(): boolean {
			console.log("IsSignedIn");
			var sessionId = Util.CookieManager.getSessionId();
			if (sessionId) {
				return true;
			}
			return false;
		}

		public Authenticate(): JQueryPromise<boolean> {
			console.log("Authenticate");
			if (!Util.Obj.isNullOrUndefined(this.authWorker)) {
				return this.authWorker;
			} else if (Util.CookieManager.getIsWindowsAuth()) {
				this.authWorker = this.SignInWindowsAuth().then(
					r => {
						this.authWorker = null;
						return r;
					},
					error => {
						this.authWorker = null;
						if (error.status === 403) {
							window.location.href = this.loginPageUrl + "?returnPath=" + encodeURIComponent(window.location.href);
						}
						return error;
					}
				);
				return this.authWorker;
			} else {
				window.location.href = this.loginPageUrl + "?returnPath=" + encodeURIComponent(window.location.href);
			}
		}
	}
}