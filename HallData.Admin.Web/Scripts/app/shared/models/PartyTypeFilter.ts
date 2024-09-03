module HallData.EMS.ViewModels {
	"use strict";

	export class PartyTypeFilter extends UI.Grid.DropDownFilter {

		constructor(controller: Controllers.IControllerBase<Controllers.IScope<any>>, delayFilter?: number, initialFilterOption?: UI.DataContracts.FilterOperationOptions, supportedFilterOperations?: UI.DataContracts.FilterOperationOptions[], filterProperty?: string, filterTemplate?: string) {
			super(controller, delayFilter, initialFilterOption, supportedFilterOperations, filterProperty, filterTemplate);
			console.log("constructor");
		}

		getItems(): JQueryPromise<UI.Grid.DropDownFilterItem[]> {
			console.log("getItems");
			var d = $.Deferred<UI.Grid.DropDownFilterItem[]>();
			d.resolve([new UI.Grid.DropDownFilterItem("2", "Organization"), new UI.Grid.DropDownFilterItem("5", "Person")]);
			return d.promise();
		}
	}
} 