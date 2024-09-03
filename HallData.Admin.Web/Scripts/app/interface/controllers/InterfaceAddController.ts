module HallData.EMS.Interface {
	"use strict";

	declare var growl: Function;
	declare var routeValueDictionary: Util.IKeyValuePair[];

	export class InterfaceAddController extends ViewController<IInterfaceAddScope> implements IInterfaceAddController {

		private interfaceForAdd: EMS.Services.Interface.IInterface;
		private availableInterfaces: EMS.Services.Interface.IInterface;

		public static $inject = [
			"$injector",
			"$scope",
			"authentication",
			"interfaceService"
		];

		constructor(
			$injector: any,
			$scope: IInterfaceAddScope,
			authentication: Authentication.IAuthenticationProvider,
			private interfaceService: EMS.Services.Interface.IInterfaceService
			
		) {
			super($injector, $scope, authentication);
			console.log("constructor");

			$scope.interfaceObj = this.createModel();
			$scope.isSaving = false;

			this.interfaceForAdd = <any>{};

			this.interfaceService = interfaceService;
		}

		protected doLoad(): JQueryPromise<void> {
			console.log("doLoad: InterfaceAddController");

			return this.interfaceService.GetViewMany("InterfaceResult", null, null, null).then(
				interfaceObj => this.onLoadSuccess(interfaceObj),
				error => {
					this.onLoadError(error);
					return error;
				});
		}

		private onLoadSuccess(interfaceObj: Service.IQueryResults<any>): void {
			console.log("onLoadSuccess");

			this.$scope.availableInterfaces = interfaceObj.Results;
			//console.log("this.$scope.availableInterfaces: " + JSON.stringify(this.$scope.availableInterfaces));
		}

		private onLoadError(error: any): void {
			console.log("onLoadError: " + JSON.stringify(error));
			growl("error", "Error", "Error getting interface");
			return error;
		}

		private createModel(): Services.Interface.IInterface {
			console.log("createModel");
			return {
				//Name: "",
				//DisplayName: "",
				//RelatedInterfaces: [],
				//Attributes: []
			}
		}

		private setInterfaceForAdd(interfaceObj: Service.IQueryResult<Services.Interface.IInterface>): void {
			console.log("setInterfaceForAdd");

			this.interfaceForAdd.Name = this.$scope.interfaceObj.Name;
			this.interfaceForAdd.DisplayName = this.$scope.interfaceObj.DisplayName;
			this.interfaceForAdd.RelatedInterfaces = this.$scope.interfaceObj.RelatedInterfaces;

			console.log("interfaceForAdd: " + JSON.stringify(this.interfaceForAdd));
		}

		public add(isValid: boolean, interfaceObj: Service.IQueryResult<Services.Interface.IInterface>): void {
			console.log("add");
			//console.log("interfaceObj: " + JSON.stringify(this.$scope.interfaceObj));

			if (isValid) {
				this.$scope.isSaving = true;
				var isFirstRequest = true;

				this.setInterfaceForAdd(interfaceObj);

				this.process(() => this.interfaceService.Add(this.interfaceForAdd, "InterfaceResult").then(
					interfaceObj => this.onAddSuccess(interfaceObj),
					error => {
						if (error.status === 403 && isFirstRequest) {
							isFirstRequest = false;
						} else {
							this.onAddError(error);
						}
						return error;
					}
					));
			} else {
				console.log("invalid");
				this.$scope.isSaving = false;
			}
		}

		private onAddSuccess(interfaceObj: Service.IQueryResult<Services.Interface.IInterface>): void {
			console.log("onAddSuccess");
			growl("notice", "Success", "Interface added successfully");

			setTimeout(() => {
				this.redirectService.redirect(this.routeDictionaryService.generateData(("InterfaceDetailsDisplay"),
					[
						{ Key: "interfaceId", Value: interfaceObj.Result.InterfaceId }
					]));
			}, 1000);
		}

		private onAddError(error: any): void {
			console.log("onAddError: " + JSON.stringify(error));
			var errorParse = $.parseJSON(error.responseText);
			var errorText = errorParse.Message;

			growl("error", "Error", "Error adding interface:\n" + errorText);
			this.$scope.isSaving = false;
			this.$scope.$digest();
			return error;
		}
	}
}