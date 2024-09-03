module HallData.Service {
	"use strict";

	export class FilterDescriptor implements IFilterDescriptor {
		constructor(public Property: string, public Operation: FilterOperation, public CompareValue: any) {
		}
		public static Create(filter: IFilterDescriptor): FilterDescriptor {
			return new FilterDescriptor(filter.Property, filter.Operation, filter.CompareValue);
		}
	}

	export class FilterContextDTO implements IFilterContext {
		public Filters: IFilterDescriptor[];
		public SearchCriteria: string;
		constructor(filterContext: IFilterContext) {
			this.Filters = filterContext.Filters;
			this.SearchCriteria = filterContext.SearchCriteria;
		}
	}

	export class FilterContext implements IFilterContext {

		private filterDictionary: HallData.Collections.Dictionary<IFilterDescriptor> = new HallData.Collections.Dictionary<IFilterDescriptor>();
		public Filters: IFilterDescriptor[];
		public SearchCriteria: string;

		constructor(filters?: IFilterDescriptor[]) {
			this.Filters = [];
			if (!Util.Obj.isNullOrUndefined(filters)) {
				for (var i = 0; i < filters.length; i++) {
					var realFilter = FilterDescriptor.Create(filters[i]);
					this.filterDictionary.add(filters[i].Property, realFilter);
					this.Filters.push(realFilter);
				}
			}
		}

		public addFilter(filter: IFilterDescriptor) {
			if (!this.filterDictionary.containsKey(filter.Property)) {
				var realFilter = FilterDescriptor.Create(filter);
				this.filterDictionary.add(filter.Property, realFilter);
				this.Filters.push(realFilter);
			}
			else {
				var realFilter = this.filterDictionary.getValue(filter.Property);
				realFilter.Operation = filter.Operation;
				realFilter.CompareValue = filter.CompareValue;
			}
		}

		public removeFilter(property: string) {
			if (this.filterDictionary.containsKey(property)) {
				var filter = this.filterDictionary.getValue(property);
				var index = this.Filters.indexOf(filter);
				this.Filters.splice(index, 1);
				this.filterDictionary.remove(property);
			}
		}

		public clear(): void {
			this.filterDictionary.clear();
			this.Filters = [];
		}
	}


	export class SortDescriptor implements ISortDescriptor {

		constructor(public Direction: SortDirection, public Property: string) {
			
		}

		public static Create(sort: ISortDescriptor): SortDescriptor {
			return new SortDescriptor(sort.Direction, sort.Property);
		}
	}

	export class SortContextDTO implements ISortContext {

		public Sorts: ISortDescriptor[];

		constructor(sortContext: ISortContext) {
			this.Sorts = sortContext.Sorts;
		}
	}
	export class SortContext implements ISortContext {

		public Sorts: ISortDescriptor[];
		private sortDictionary: HallData.Collections.Dictionary<ISortDescriptor> = new HallData.Collections.Dictionary<ISortDescriptor>();

		constructor(sorts?: ISortDescriptor[]) {
			this.Sorts = [];
			if (!Util.Obj.isNullOrUndefined(sorts)) {
				for (var i = 0; i < sorts.length; i++) {
					var realSort = SortDescriptor.Create(sorts[i]);
					this.sortDictionary.add(realSort.Property, realSort);
					this.Sorts.push(realSort);
				}
			}
		}

		addSort(sort: ISortDescriptor) {
			this.removeSort(sort.Property);
			var realSort = SortDescriptor.Create(sort);
			this.sortDictionary.add(realSort.Property, realSort);
			this.Sorts.push(realSort);
		}

		removeSort(property: string) {
			if (this.sortDictionary.containsKey(property)) {
				var realSort = this.sortDictionary.getValue(property);
				var index = this.Sorts.indexOf(realSort);
				this.Sorts.splice(index, 1);
				this.sortDictionary.remove(property);
			}
		}

		updateSort(sort: ISortDescriptor) {
			if (this.sortDictionary.containsKey(sort.Property)) {
				var realSort = this.sortDictionary.getValue(sort.Property);
				realSort.Direction = sort.Direction;
			}
			else
				this.addSort(sort);
		}

		public insertSort(sort: ISortDescriptor, index?: number) {
			if (Util.Obj.isNullOrUndefined(index))
				index = 0;
			this.removeSort(sort.Property);
			var realSort = SortDescriptor.Create(sort);
			this.sortDictionary.add(sort.Property, realSort);
			this.Sorts.splice(index, 0, realSort);
		}

		public clear(): void {
			this.sortDictionary.clear();
			this.Sorts = [];
		}
	}

	export class PageDescriptor implements IPageDescriptor {
		constructor(public CurrentPage: number, public PageSize: number) { }
	}
} 