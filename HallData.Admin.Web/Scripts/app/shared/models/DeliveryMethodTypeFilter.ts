module HallData.EMS.ViewModels {
	"use strict";

	export class DeliveryMethodFilter extends UI.Grid.DropDownFilter {

		constructor(controller: Controllers.IControllerBase<Controllers.IScope<any>>,delayFilter?: number, initialFilterOption?: UI.DataContracts.FilterOperationOptions, supportedFilterOperations?: UI.DataContracts.FilterOperationOptions[], filterProperty?: string, filterTemplate?: string) {
			super(controller, delayFilter, initialFilterOption, supportedFilterOperations, filterProperty, filterTemplate);
			console.log("constructor");
		}

		getItems(): JQueryPromise<UI.Grid.DropDownFilterItem[]> {
			console.log("getItems");
			return Services.EnumerationsServiceFactory.Create().GetDeliveryMethodTypes().then(
				deliveryMethodTypes => {
					var items: UI.Grid.DropDownFilterItem[] = [];
					for (var i = 0; i < deliveryMethodTypes.length; i++) {
						items.push(new UI.Grid.DropDownFilterItem(deliveryMethodTypes[i].DeliveryMethodTypeId.toString(), deliveryMethodTypes[i].Name));
					}
					return items;
				}, error => {
						this.errorMessage(error.responseText);
						return null;
				});
		}
	}
}