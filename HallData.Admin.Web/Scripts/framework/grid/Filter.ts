/// <reference path="../../libraries/jquery/jquery.d.ts" />
/// <reference path="../../libraries/knockout/knockout.d.ts" />
module HallData.UI.Grid {
	export class FilterOperationDescriptor {
		constructor(public filterOperation: DataContracts.FilterOperationOptions, public displayText: string) { }
	}
	export class Filter implements IFilter {
		public filterOperations: FilterOperationDescriptor[] = [];
		public filterCompareValue: KnockoutObservable<string>;
		public filterOperation: KnockoutObservable<FilterOperationDescriptor>;
		public column: IColumn;
		private currentCompareValue: string = "";
		private currentFilterOperation: DataContracts.FilterOperationOptions;
		constructor(protected controller: Controllers.IControllerBase<Controllers.IScope<any>>, protected delayFilter?: number, initialFilterOption?: DataContracts.FilterOperationOptions, supportedFilterOperations?: DataContracts.FilterOperationOptions[], public filterProperty?: string, public filterTemplate?: string) {
			this.filterCompareValue = ko.observable("");
			if (Util.Obj.isNullOrUndefined(filterTemplate))
				this.filterTemplate = "Filter";
			if (Util.Obj.isNullOrUndefined(supportedFilterOperations))
				supportedFilterOperations = [DataContracts.FilterOperationOptions.Contains, DataContracts.FilterOperationOptions.StartsWith, DataContracts.FilterOperationOptions.EndsWith, DataContracts.FilterOperationOptions.Equals];
			if (Util.Obj.isNullOrUndefined(initialFilterOption))
				initialFilterOption = supportedFilterOperations[0];
			if (Util.Obj.isNullOrUndefined(this.delayFilter))
				this.delayFilter = 500;
			var initialFilterDescriptor: FilterOperationDescriptor = null;
			for (var i = 0; i < supportedFilterOperations.length; i++) {
				var displayText: string = null;
				switch (supportedFilterOperations[i]) {
					case DataContracts.FilterOperationOptions.Contains: displayText = "Contains"; break;
					case DataContracts.FilterOperationOptions.EndsWith: displayText = "Ends With"; break;
					case DataContracts.FilterOperationOptions.Equals: displayText = "Equals"; break;
					case DataContracts.FilterOperationOptions.StartsWith: displayText = "Starts With"; break;
				}
				var filterDescriptor = new FilterOperationDescriptor(supportedFilterOperations[i], displayText);
				this.filterOperations.push(filterDescriptor);
				if (filterDescriptor.filterOperation == initialFilterOption)
					initialFilterDescriptor = filterDescriptor;
			}
			this.filterOperation = ko.observable(initialFilterDescriptor);
			if (!Util.Obj.isNullOrUndefined(initialFilterDescriptor))
				this.currentFilterOperation = initialFilterDescriptor.filterOperation;
			this.filterCompareValue.subscribe(() => setTimeout(() => this.updateFilter(), this.delayFilter));
			this.filterOperation.subscribe(() => this.updateFilter());
		}

		private updateFilter(): void {
			console.log("updateFilter");
			if (!Util.Obj.isNullOrUndefined(this.filterCompareValue())) {
				if (this.filterOperation().filterOperation != this.currentFilterOperation || this.filterCompareValue().toLowerCase() != this.currentCompareValue) {
					this.currentFilterOperation = this.filterOperation().filterOperation;
					this.currentCompareValue = this.filterCompareValue().toLowerCase();
					var compareValue: string = this.filterCompareValue();
					if (!HallData.Util.String.isNullOrWhitespace(compareValue)) {
						switch (this.currentFilterOperation) {
							case DataContracts.FilterOperationOptions.Contains: compareValue = '%' + this.filterCompareValue() + '%'; break;
							case DataContracts.FilterOperationOptions.EndsWith: compareValue = '%' + this.filterCompareValue(); break;
							case DataContracts.FilterOperationOptions.StartsWith: compareValue = this.filterCompareValue() + '%'; break;
						}
						var baseFilterOperation: HallData.Service.FilterOperation;
						switch (this.currentFilterOperation) {
							case DataContracts.FilterOperationOptions.Contains:
							case DataContracts.FilterOperationOptions.EndsWith:
							case DataContracts.FilterOperationOptions.StartsWith: baseFilterOperation = HallData.Service.FilterOperation.Like; break;
							case DataContracts.FilterOperationOptions.Equals: baseFilterOperation = HallData.Service.FilterOperation.Equals; break;
						}
						this.column.grid.updateFilter(Util.Obj.isNullOrUndefined(this.filterProperty) ? this.column.property : this.filterProperty, baseFilterOperation, compareValue);
					}
					else {
						this.currentCompareValue = "";
						this.column.grid.removeFilter(Util.Obj.isNullOrUndefined(this.filterProperty) ? this.column.property : this.filterProperty);
					}
				}
			}
		}
	}

	export class DropDownFilterItem {
		constructor(public filterCompareValue: string, public displayText: string) { }
	}
	export class BooleanFilter extends Filter {
        constructor(controller: Controllers.IControllerBase<Controllers.IScope<any>>, delayFilter?: number, initialFilterOption?: DataContracts.FilterOperationOptions, supportedFilterOperations?: DataContracts.FilterOperationOptions[], filterProperty?: string, filterTemplate?: string) {
            super(controller, delayFilter, initialFilterOption, Util.Obj.isNullOrUndefined(supportedFilterOperations) ? [DataContracts.FilterOperationOptions.Equals] : supportedFilterOperations, filterProperty, filterTemplate);
			if (Util.Obj.isNullOrUndefined(delayFilter))
				this.delayFilter = 0;
		}
	}
	export class DropDownFilter extends Filter {
		public filterState: KnockoutObservable<DropDownFilterState>;
		public filterItems: KnockoutObservableArray<DropDownFilterItem>;
		public selectedFilterItem: KnockoutObservable<DropDownFilterItem>;
		public isLoading: KnockoutComputed<boolean>;
		public isLoaded: KnockoutComputed<boolean>;
		public isError: KnockoutComputed<boolean>;
		public errorMessage: KnockoutObservable<string> = ko.observable<string>();
        constructor(controller: Controllers.IControllerBase<Controllers.IScope<any>>, delayFilter?: number, initialFilterOption?: DataContracts.FilterOperationOptions, supportedFilterOperations?: DataContracts.FilterOperationOptions[], filterProperty?: string, filterTemplate?: string) {
            super(controller, delayFilter, initialFilterOption, Util.Obj.isNullOrUndefined(supportedFilterOperations) ? [DataContracts.FilterOperationOptions.Equals] : supportedFilterOperations, filterProperty, filterTemplate);
			this.filterState = ko.observable(DropDownFilterState.Loading);
			this.selectedFilterItem = ko.observable<DropDownFilterItem>(null);
			this.filterItems = ko.observableArray<DropDownFilterItem>();
			this.isLoading = ko.computed(() => this.filterState() == DropDownFilterState.Loading);
			this.isLoaded = ko.computed(() => this.filterState() == DropDownFilterState.Loaded);
			this.isError = ko.computed(() => this.filterState() == DropDownFilterState.Error);
			if (Util.Obj.isNullOrUndefined(filterTemplate))
				this.filterTemplate = "DropDownFilter";
			if (Util.Obj.isNullOrUndefined(delayFilter))
				this.delayFilter = 0;
			this.selectedFilterItem.subscribe(val => {
				this.filterCompareValue(Util.Obj.isNullOrUndefined(val) ? "" : val.filterCompareValue);
			});
			this.load();
		}
		private load(): void {
			console.log("load");
			this.filterState(DropDownFilterState.Loading);
			this.filterItems.removeAll();
			this.controller.process(() => this.getItems().then(items => {
				this.filterItems.push(new DropDownFilterItem("", ""));
				for (var i = 0; i < items.length; i++) {
					this.filterItems.push(items[i]);
				}
				this.filterState(DropDownFilterState.Loaded);
				this.selectedFilterItem(this.filterItems[0]);
			},() => {
					if(Util.String.isNullOrWhitespace(this.errorMessage()))
						this.errorMessage("There was an error loading the filter items");
					this.filterState(DropDownFilterState.Error);
				}));
		}
		getItems(): JQueryPromise<DropDownFilterItem[]> {
			console.log("getItems");
			var d = $.Deferred<DropDownFilterItem[]>();
			d.resolve([]);
			return d.promise();
		}
	}
} 