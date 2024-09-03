module HallData.EMS.ViewModels {
	"use strict";

	export class TierTypeFilter extends UI.Grid.DropDownFilter {

		constructor(controller: Controllers.IControllerBase<Controllers.IScope<any>>,delayFilter?: number, initialFilterOption?: UI.DataContracts.FilterOperationOptions, supportedFilterOperations?: UI.DataContracts.FilterOperationOptions[], filterProperty?: string, filterTemplate?: string) {
			super(controller, delayFilter, initialFilterOption, supportedFilterOperations, filterProperty, filterTemplate);
			console.log("constructor");
		}

		getItems(): JQueryPromise<UI.Grid.DropDownFilterItem[]> {
			console.log("Tier Type: getItems");
			return Services.EnumerationsServiceFactory.Create().GetTierTypes().then(
				tier => {
					var items: UI.Grid.DropDownFilterItem[] = [];
					for (var i = 0; i < tier.length; i++) {
						items.push(new UI.Grid.DropDownFilterItem(tier[i].TierTypeId.toString(), tier[i].Name));
					}
					return items;
				}, error => {
						this.errorMessage(error.responseText);
						return null;
				});
		}
	}
}