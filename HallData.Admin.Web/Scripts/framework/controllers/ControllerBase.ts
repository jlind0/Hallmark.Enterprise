module HallData.Controllers {
	"use strict";

	declare var growl: Function;

	export class ControllerBase<TScope extends IScope<any>> implements IControllerBase<TScope>{

		private _controllerState: ControllerState = ControllerState.Initializing;
		private _authenticationState: Authentication.AuthenticationState;
		public controllerStateChanged: Events.ITypedEvent<ControllerState> = new Events.TypedEvent();
		public controllerAuthenticationStateChanged: Events.ITypedEvent<Authentication.AuthenticationState> = new Events.TypedEvent();

		constructor(
			public $scope: TScope,
			protected $http: ng.IHttpService,
			protected authentication: Authentication.IAuthenticationProvider,
			protected requiresSignedInUser: boolean
		) {
			console.log("constructor");
			this.$scope.vm = this;

			this.authentication.AuthenticationStatusChanged.add(auth => {
				this.$scope.isSignedIn = this.authentication.IsSignedIn();
				this.setControllerAuthenticationState(auth);
			});

			this.$scope.isSignedIn = authentication.IsSignedIn();

			if (!this.$scope.isSignedIn) {
				this.setControllerAuthenticationState(Authentication.AuthenticationState.NotAuthenticated);
			} else {
				this.setControllerAuthenticationState(Authentication.AuthenticationState.Authenticated);
			}
				
			if (requiresSignedInUser) {
				if (this.$scope.isSignedIn) {
					this.init();
				} else {
					this.authenticate(() => this.init());
				}
			} else {
				this.init();
			}
		}

		public getControllerState(): ControllerState {
			console.log("getControllerState");
			return this._controllerState;
		}

		public getAuthenticationState(): Authentication.AuthenticationState {
			console.log("getAuthenticationState");
			return this._authenticationState;
		}

		private queue: AsyncQueue<IControllerActionParameter> = async.queue<IControllerActionParameter>((parm, c) => {
			console.log("queue");

			var doAuth: (processDelagte: () => void) => JQueryPromise<void> = processDelagte => {
				return this.doAuthenticate().then(
					() => {
						this.setControllerAuthenticationState(Authentication.AuthenticationState.Authenticated);
						if (!Util.Obj.isNullOrUndefined(processDelagte)) {
							processDelagte();
						}
					},
					error => {
						console.log(error.responseText);
						this.raiseAuthenticationError("There was an error autheneticating the user");
					});
			};

			var process = () => {
				switch (parm.action) {
					case ControllerAction.Initialize: this.doInit().then(
						() => {
							this.setControllerState(ControllerState.Initialized); c();
						},
						error => {
							this.processError("There was an error initializing the controller", error);
							if (this.$scope.isAuthenticationError && !parm.isSecondTry) {
								this.authenticate(() => this.init(true));
							}
							c();
							return error;
						});
						break;
					case ControllerAction.Load: this.doLoad().then(
						() => {
							this.setControllerState(ControllerState.Loaded); c();
						},
						error => {
							this.processError("There was an error loading the controller", error);
							if (this.$scope.isAuthenticationError && !parm.isSecondTry)
								this.authenticate(() => this.load(true));
							c();
							return error;
						});
						break;
					case ControllerAction.Unload: this.doUnload().then(
						() => {
							this.setControllerState(ControllerState.Unloaded); c();
						},
						error => {
							this.processError("There was an error unloading the controller", error);
							if (this.$scope.isAuthenticationError && !parm.isSecondTry)
								this.authenticate(() => this.unload(true));
							c();
							return error;
						});
						break;
					case ControllerAction.Authenticate: doAuth(parm.processDelegate).then(
							() => c(),
							() => c()
						);
						break;
					case ControllerAction.Process:
						parm.processDelegate().then(
							() => { if (parm.isSync) c(); },
							error => {
								this.processError("An error occured", error);
								if (this.$scope.isAuthenticationError && !parm.isSecondTry) {
									doAuth(() => {
										parm.processDelegate().then(() => {
											if (parm.isSync)
												c();
										},
											error => { this.processError("An error occured", error); if (parm.isSync) { c(); } return error; });
									}).then(() => { }, error => { if (parm.isSync) { c(); } return error; });
								} else if (parm.isSync) {
									c();
								}
							return error;
						});
						if (!parm.isSync) c();
				}
			};
			
			if (parm.action !== ControllerAction.Authenticate && this.$scope.isAuthenticationError && !parm.isSecondTry) {
				doAuth(process).then(() => { },() => c());
			} else if (parm.action !== ControllerAction.Authenticate && this.$scope.isAuthenticationError && parm.isSecondTry) {
				c();
			} else {
				process();
			}
		}, 1);

		protected setControllerState(state: ControllerState) {
			console.log("setControllerState");
			this._controllerState = state;
			this.$scope.isInitializing = state === ControllerState.Initializing;
			this.$scope.isLoading = state === ControllerState.Loading;
			this.$scope.isInitialized = state === ControllerState.Initialized;
			this.$scope.isLoaded = state === ControllerState.Loaded;
			this.$scope.isUnloaded = state === ControllerState.Unloaded;
			this.$scope.isUnloading = state === ControllerState.Unloading;
			this.applyIfNeeded();
			this.controllerStateChanged.trigger(state);
		}

		protected setControllerAuthenticationState(state: Authentication.AuthenticationState) {
			console.log("setControllerAuthenticationState");
			this._authenticationState = state;
			this.$scope.isAuthenticating = state === Authentication.AuthenticationState.Authenticating;
			this.$scope.isAuthenticationError = state === Authentication.AuthenticationState.AuthenticationError;
			this.$scope.isAuthenticated = state === Authentication.AuthenticationState.Authenticated;
			this.$scope.isLoggingOut = state === Authentication.AuthenticationState.Ending;
			this.applyIfNeeded();
			this.controllerAuthenticationStateChanged.trigger(state);
		}

		

		protected processError(errorMessage: string, error: any) {
			console.log("processError");
			if (error) {
				console.log(error.responseText);
				if (error.status === 403) {
					this.raiseAuthenticationError(errorMessage);
				}
				else
					this.raiseError(errorMessage);
			}
		}

		public init(isSecondTry?: boolean): JQueryPromise<void> {
			console.log("init");
			var d = $.Deferred<void>();
			this.queue.push({ action: ControllerAction.Initialize, isSecondTry: isSecondTry },() => {
				d.resolve();
				if (!this.$scope.isAuthenticationError && !this.$scope.hasErrored)
					this.load();
			});
			return d.promise();
		}

		protected doInit(): JQueryPromise<void> {
			console.log("doInit");
			var d = $.Deferred<void>();
			d.resolve();
			return d.promise();
		}

		public load(isSecondTry?: boolean): JQueryPromise<void> {
			console.log("load");
			var d = $.Deferred<void>();
			this.unload();
			this.queue.push({ action: ControllerAction.Load, isSecondTry: isSecondTry },() => d.resolve());
			return d.promise();
		}

		protected doLoad(): JQueryPromise<void> {
			console.log("doLoad");
			var d = $.Deferred<void>();
			d.resolve();
			return d.promise();
		}

		public unload(isSecondTry?: boolean): JQueryPromise<void> {
			console.log("unload");
			var d = $.Deferred<void>();
			this.queue.push({ action: ControllerAction.Unload, isSecondTry: isSecondTry },() => d.resolve());
			return d.promise();
		}

		protected doUnload(): JQueryPromise<void> {
			console.log("doUnload");
			var d = $.Deferred<void>();
			d.resolve();
			return d.promise();
		}

		protected raiseError(errorMessage: string): void {
			console.log("raiseError");
			this.$scope.errorMessage = errorMessage;
			this.setControllerState(ControllerState.Error);
			this.doRaiseError(errorMessage);
		}

		protected raiseAuthenticationError(errorMessage: string): void {
			console.log("raiseAuthenticationError");
			this.$scope.errorMessage = errorMessage;
			this.setControllerAuthenticationState(Authentication.AuthenticationState.AuthenticationError);
			this.doRaiseAuthenticationError(errorMessage);
		}

		protected doRaiseError(errorMessage: string): void {
			console.log("doRaiseError: " + errorMessage);
		}

		protected doRaiseAuthenticationError(errorMessage: string): void {
			console.log("doRaiseAuthenticationError");
		}

		public authenticate(delegate?: () => JQueryPromise<void>): JQueryPromise<void> {
			console.log("authenticate");
			var d = $.Deferred<void>();
			this.queue.push({ action: ControllerAction.Authenticate, processDelegate: delegate },() => d.resolve());
			return d.promise();
		}

		protected doAuthenticate(): JQueryPromise<void> {
			console.log("doAuthenticate");
			this.setControllerAuthenticationState(Authentication.AuthenticationState.Authenticating);
			var d = $.Deferred<void>();
			this.authentication.Authenticate().then(s => {
				this.setControllerAuthenticationState(Authentication.AuthenticationState.Authenticated);
				this.onAuthenticated();
				d.resolve();
			}, error => d.fail(error));
			return d.promise();
		}

		public process(delegate: () => JQueryPromise<void>, isSync?: boolean): JQueryPromise<void> {
			console.log("process");
			var d = $.Deferred<void>();
			this.queue.push({ action: ControllerAction.Process, processDelegate: delegate , isSync: isSync},() => d.resolve());
			return d.promise();  
		}
		
		protected onAuthenticated() {
			console.log("onAuthenticated");
			if (!this.$scope.isSignedIn && this.requiresSignedInUser)
				this.raiseAuthenticationError("The user was not signed in");
		}

		protected applyIfNeeded(): void {
			console.log("applyIfNeeded");
			var phase = this.$scope.$root.$$phase;
			if (phase !== "$apply" && phase !== "$digest")
				this.$scope.$apply();
		}
	}
}