module HallData.EMS.InterfaceAttribute {
	"use strict";

	declare var growl: Function;
	declare var routeValueDictionary: Util.IKeyValuePair[];

	export class InterfaceAttributeAddController extends ViewController<IInterfaceAttributeAddScope> implements IInterfaceAttributeAddController {

		private interfaceAttributeForAdd: EMS.Services.InterfaceAttribute.IInterfaceAttribute;

		public static $inject = [
			"$injector",
			"$scope",
			"authentication",
			"interfaceService",
			"interfaceAttributeService"
		];

		constructor(
			$injector: any,
			$scope: IInterfaceAttributeAddScope,
			authentication: Authentication.IAuthenticationProvider,
			private interfaceService: EMS.Services.Interface.IInterfaceService,
			private interfaceAttributeService: EMS.Services.InterfaceAttribute.IInterfaceAttributeService
			
		) {
			super($injector, $scope, authentication);
			console.log("constructor");

			$scope.interfaceAttributeObj = this.createModel();
			$scope.isSaving = false;

			this.interfaceAttributeForAdd = <any>{};

			this.interfaceService = interfaceService;
			this.interfaceAttributeService = interfaceAttributeService;
		}

		protected doLoad(): JQueryPromise<void> {
			console.log("doLoad");

			return this.interfaceService.Get(
				Util.Obj.getValue(routeValueDictionary, "interfaceId"),
				"InterfaceResult"
				).then(
				interfaceObj => this.onLoadSuccess(interfaceObj),
				error => {
					this.onLoadError(error);
					return error;
				});
		}

		private onLoadSuccess(interfaceObj: Service.IQueryResult<Services.Interface.IInterface>): void {
			console.log("onLoadSuccess");
			console.log("interfaceObj: " + JSON.stringify(interfaceObj));

			this.$scope.interfaceObj = interfaceObj.Result;
			console.log("this.$scope.interfaceObj: " + JSON.stringify(this.$scope.interfaceObj));
		}

		private onLoadError(error: any): void {
			console.log("onLoadError: " + JSON.stringify(error));
			growl("error", "Error", "Error getting interface");
			return error;
		}

		private createModel(): Services.InterfaceAttribute.IInterfaceAttribute {
			console.log("createModel");
			return {
				Name: "",
				DisplayName: ""
			}
		}

		private setInterfaceAttributeForAdd(interfaceAttributeObj: Service.IQueryResult<Services.InterfaceAttribute.IInterfaceAttribute>): void {
			console.log("setInterfaceAttributeForAdd");

			this.interfaceAttributeForAdd.Name = this.$scope.interfaceAttributeObj.Name;
			this.interfaceAttributeForAdd.DisplayName = this.$scope.interfaceAttributeObj.DisplayName;

			console.log("interfaceAttributeForAdd: " + JSON.stringify(this.interfaceAttributeForAdd));
		}

		public add(isValid: boolean, interfaceObj: Service.IQueryResult<Services.InterfaceAttribute.IInterfaceAttribute>): void {
			console.log("add");

			if (isValid) {
				this.$scope.isSaving = true;
				var isFirstRequest = true;

				this.setInterfaceAttributeForAdd(interfaceObj);

				this.process(() => this.interfaceAttributeService.Add(this.interfaceAttributeForAdd, "InterfaceAttributeResult").then(
					interfaceAttributeObj => this.onAddSuccess(interfaceAttributeObj),
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

		private onAddSuccess(interfaceAttributeObj: Service.IQueryResult<Services.InterfaceAttribute.IInterfaceAttribute>): void {
			console.log("onAddSuccess");
			growl("notice", "Success", "Interface Attribute added successfully");

			setTimeout(() => {
				this.redirectService.redirect(this.routeDictionaryService.generateData(("InterfaceAttributeDetailsDisplay"),
					[
						{ Key: "interfaceAttributeId", Value: interfaceAttributeObj.Result.InterfaceAttributeId }
					]));
			}, 1000);
		}

		private onAddError(error: any): void {
			console.log("onAddError: " + JSON.stringify(error));
			var errorParse = $.parseJSON(error.responseText);
			var errorText = errorParse.Message;

			growl("error", "Error", "Error adding interface attribute:\n" + errorText);
			this.$scope.isSaving = false;
			this.$scope.$digest();
			return error;
		}
	}
}