module HallData.Authentication {
	"use strict";

	declare var serviceLocation: string;
	declare var emsServiceLocation: string;

	export interface IAuthenticationService extends HallData.Service.IService {
		SignInWindowsAuth(): JQueryPromise<string>;
		SignIn(username: string, password?: string, token?: string): JQueryPromise<string>;
		Logout(sessionId: string): JQueryPromise<boolean>;
	}

	export class AuthenticationService extends HallData.Service.Service implements IAuthenticationService {

		protected authenticationServiceLocation: string;

		constructor() {
			super(serviceLocation);
			console.log("constructor");
			this.authenticationServiceLocation = emsServiceLocation + "sessions/create";
		}

		public SignInWindowsAuth(): JQueryPromise<string> {
			console.log("SignInWindowsAuth");
			return Util.Service.Get<string>(this.authenticationServiceLocation, true);
		}

		public SignIn(username: string, password?: string, token?: string): JQueryPromise<string> {
			console.log("SignIn");
			var url = this.ServiceLocation + "users/" + username + "/sessions/create/";
			if (password) {
				url += "?password=" + encodeURIComponent(password);
			} else if (token) {
				url += "?token=" + encodeURIComponent(token);
			}
			return Util.Service.Get<string>(url);
		}

		public Logout(sessionId: string): JQueryPromise<boolean> {
			console.log("Logout");
			return Util.Service.Get<boolean>(this.ServiceLocation + "sessions/" + sessionId + "/end/");
		}

	}
} 