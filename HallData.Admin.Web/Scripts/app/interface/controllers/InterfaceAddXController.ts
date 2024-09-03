//module HallData.EMS.Interface {
//	"use strict";

//	declare var growl: Function;

//	export class InterfaceAddXController extends ViewController<IInterfaceAddScope> implements IInterfaceAddController {

//		private interfaceForAdd: EMS.Services.Interface.IInterface;

//		public static $inject = [
//			"$injector",
//			"$scope",
//			"authentication",
//			"interfaceService"
//		];

//		constructor(
//			$injector: any,
//			$scope: IInterfaceAddScope,
//			authentication: Authentication.IAuthenticationProvider,
//			private interfaceService: EMS.Services.Interface.IInterfaceService
			
//		) {
//			super($injector, $scope, authentication);
//			console.log("constructor");

//			$scope.interfaceObj = this.createModel();
//			$scope.isSaving = false;

//			$scope.boolValues = [
//				{ value: true, name: 'True' }, { value: false, name: 'False' }
//			];

//			this.interfaceForAdd = <any>{};

//			this.interfaceService = interfaceService;
//		}

//		private createModel(): Services.Interface.IInterface {
//			console.log("createModel");
//			return {
//				Name: "",
//				DisplayName: "",
//				RelatedInterfaces: [],
//				Attributes: []
//			}
//		}

//		private createAttributeModel(): Services.Interface.IInterface {
//			console.log("createAttributeModel");
//			return {
//						Interface: {},
//						Type: {},
//						Name: '',
//						DisplayName: '',
//						IsKey: null,
//						IsCollection: null,
//						InterfaceAttributeId: null
//			}
//		}

//		private showBoolValues(attrib, $filter): void {
//			var selected = [];
//			if (attrib.boolValues) {
//				selected = $filter('filter')(this.$scope.boolValues, { value: attrib.boolValues });
//			}
//			return selected.length ? selected[0].text : 'Not set';
//		}

//		private addAttribute(): void {
//			console.log("addAttribute");

//			this.$scope.addInterfaceAttribute = this.createAttributeModel();
//			this.$scope.addInterface.Attributes.push(this.$scope.addInterfaceAttribute);
//		}

//		private saveAttribute(data, id): void {
//			console.log("saveAttribute");

//			angular.extend(data, { id: id });
//		}

//		private removeAttribute(index): void {
//			console.log("removeAttribute");

//			this.$scope.addInterface.Attributes.splice(index, 1);
//		}







//		private setInterfaceForAdd(addInterface: Service.IQueryResult<Services.Interface.IInterface>): void {
//			console.log("setInterfaceForAdd");

//			this.interfaceForAdd.Name = this.$scope.addInterface.Name;
//			this.interfaceForAdd.DisplayName = this.$scope.addInterface.DisplayName;
//			this.interfaceForAdd.RelatedInterfaces = this.$scope.addInterface.RelatedInterfaces;
//			this.interfaceForAdd.Attributes = this.$scope.addInterface.Attributes;

//			console.log("interfaceForAdd: " + JSON.stringify(this.interfaceForAdd));
//		}

//		public add(isValid: boolean, addInterface: Service.IQueryResult<Services.Interface.IInterface>): void {
//			console.log("add");
//			//console.log("addInterface: " + JSON.stringify(this.$scope.addInterface));

//			if (isValid) {
//				this.$scope.isSaving = true;
//				var isFirstRequest = true;

//				this.setInterfaceForAdd(addInterface);

//				this.process(() => this.interfaceService.Add(this.interfaceForAdd, "InterfaceResult").then(
//					addInterface => this.onAddSuccess(addInterface),
//					error => {
//						if (error.status === 403 && isFirstRequest) {
//							isFirstRequest = false;
//						} else {
//							this.onAddError(error);
//						}
//						return error;
//					}
//					));
//			} else {
//				console.log("invalid");
//				this.$scope.isSaving = false;
//			}
//		}

//		private onAddSuccess(addInterface: Service.IQueryResult<Services.Interface.IInterface>): void {
//			console.log("onAddSuccess");
//			growl("notice", "Success", "Interface added successfully");

//			setTimeout(() => {
//				this.redirectService.redirect(this.routeDictionaryService.generateData(("InterfaceDetailsDisplay"),
//					[
//						{ Key: "interfaceId", Value: addInterface.Result.InterfaceId }
//					]));
//			}, 1000);
//		}

//		private onAddError(error: any): void {
//			console.log("onAddError: " + JSON.stringify(error));
//			var errorParse = $.parseJSON(error.responseText);
//			var errorText = errorParse.Message;

//			growl("error", "Error", "Error adding interface:\n" + errorText);
//			this.$scope.isSaving = false;
//			this.$scope.$digest();
//			return error;
//		}
//	}
//}