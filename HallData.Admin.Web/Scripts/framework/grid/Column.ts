/// <reference path="../../libraries/jquery/jquery.d.ts" />
/// <reference path="../../libraries/knockout/knockout.d.ts" />
module HallData.UI.Grid {
    export class Column implements IColumn {
        public sortDirection: KnockoutObservable<DataContracts.SortDirectionOptions>;
        public isSortedAscending: KnockoutComputed<boolean>;
        public isSortedDescending: KnockoutComputed<boolean>;
        public isNotSorted: KnockoutComputed<boolean>;
        public grid: IGridBase;
        public isVisible: KnockoutObservable<boolean>;
        public displayOrderIndex: KnockoutObservable<number>;
        private static DefaultColumnTemplate: string = "";
        private static DefaultHeaderTemplate: string = "";
        private static DefaultCellTemplate: string = "";
        public reorderColumn: KnockoutComputed<IColumn> = ko.computed<IColumn>(() => null, undefined, {
            read: () => null,
            write: value => this.grid.reorderColumn(value, this.displayOrderIndex()),
            owner: this
        });
        private routeData: IRouteData;
        constructor(public property: string, public headerText: string, public applicationViewColumn: DataContracts.IApplicationViewColumnResult, public canFilter?: boolean, public filter?: IFilter, public canSort?: boolean,
            initialSortDirectionOption?: DataContracts.SortDirectionOptions, isVisible?: boolean, public columnTemplate?: string, public headerTemplate?: string, public cellTemplate?: string,
            public canAddToUI?: boolean, public configureOrderIndex?: number, displayOrderIndex?: number) {
            if (!Util.String.isNullOrWhitespace(applicationViewColumn.RouteData))
                this.routeData = <IRouteData>JSON.parse(applicationViewColumn.RouteData);
            if (Util.Obj.isNullOrUndefined(this.canFilter))
                this.canFilter = false;
            if (Util.Obj.isNullOrUndefined(this.canSort))
                this.canSort = true;
            if (canFilter && Util.Obj.isNullOrUndefined(this.filter))
                this.filter = new Filter(this.grid.controller);
            if (!Util.Obj.isNullOrUndefined(this.filter))
                this.filter.column = this;
            if (Util.Obj.isNullOrUndefined(initialSortDirectionOption))
                initialSortDirectionOption = DataContracts.SortDirectionOptions.None;
            if (Util.Obj.isNullOrUndefined(isVisible))
                isVisible = false;
            if (Util.Obj.isNullOrUndefined(displayOrderIndex))
                displayOrderIndex = null;
            this.displayOrderIndex = ko.observable(displayOrderIndex);
            this.displayOrderIndex.subscribe(val => {
                this.applicationViewColumn.DisplayOrderIndex = val;
            });
            this.sortDirection = ko.observable(initialSortDirectionOption);
            this.isNotSorted = ko.computed(() => this.sortDirection() == DataContracts.SortDirectionOptions.None);
            this.isSortedAscending = ko.computed(() => this.sortDirection() == DataContracts.SortDirectionOptions.Ascending);
            this.isSortedDescending = ko.computed(() => this.sortDirection() == DataContracts.SortDirectionOptions.Descending);
            this.isVisible = ko.observable(isVisible);
            this.isVisible.subscribe(vis => {
                this.grid.setColumnVisibility(this, vis);
                this.applicationViewColumn.IsVisisble = vis;
                if (!vis) {
                    if (this.canFilter && !Util.String.isNullOrWhitespace(this.filter.filterCompareValue()))
                        this.filter.filterCompareValue("");
                    if (this.canSort && this.sortDirection() != DataContracts.SortDirectionOptions.None)
                        this.updateSort(DataContracts.SortDirectionOptions.None);
                }
            });
        }
        public getApplicationViewColumnForParty(): DataContracts.IApplicationViewColumnForParty {
            var view: DataContracts.IApplicationViewColumnForParty = {
                ApplicationView: { ApplicationViewId: this.applicationViewColumn.ApplicationView.ApplicationViewId, Name: this.applicationViewColumn.ApplicationView.Name },
                ApplicationViewColumnId: this.applicationViewColumn.ApplicationViewColumnId,
                CanFilter: this.applicationViewColumn.CanFilter,
                CanSort: this.applicationViewColumn.CanSort,
                DisplayOrderIndex: this.applicationViewColumn.DisplayOrderIndex,
                HeaderText: this.applicationViewColumn.HeaderText,
                InitialSortDirectionId: this.applicationViewColumn.InitialSortDirectionId,
                IsVisisble: this.applicationViewColumn.IsVisisble
            }
            if (!Util.Obj.isNullOrUndefined(this.applicationViewColumn.CellTemplate))
                view.CellTemplate = { TemplateId: this.applicationViewColumn.CellTemplate.TemplateId };
            if (!Util.Obj.isNullOrUndefined(this.applicationViewColumn.HeaderTemplate))
                view.HeaderTemplate = { TemplateId: this.applicationViewColumn.HeaderTemplate.TemplateId };
            if (!Util.Obj.isNullOrUndefined(this.applicationViewColumn.ColumnTemplate))
                view.ColumnTemplate = { TemplateId: this.applicationViewColumn.ColumnTemplate.TemplateId };
            if (!Util.Obj.isNullOrUndefined(this.applicationViewColumn.Filter)) {
                view.Filter = {
                    DelayFilter: this.applicationViewColumn.Filter.DelayFilter,
                    FilterId: this.applicationViewColumn.Filter.FilterId,
                    FilterType: { FilterTypeId: this.applicationViewColumn.Filter.FilterType.FilterTypeId },
                    Template: { TemplateId: this.applicationViewColumn.Filter.Template.TemplateId }
                };
                
                var filterOperationOptions: DataContracts.IFilterOperationOptionBase[] = [];
                for (var i = 0; i < this.applicationViewColumn.Filter.FilterOperationOptions.length; i++) {
                    var fo = this.applicationViewColumn.Filter.FilterOperationOptions[i];
                    filterOperationOptions.push({ FilterOperationOptionId: fo.FilterOperationOptionId, OrderIndex: fo.OrderIndex });
                }
                view.Filter.FilterOperationOptions = filterOperationOptions;
                if (!Util.Obj.isNullOrUndefined(this.applicationViewColumn.Filter.FilterColumn)) {
                    view.Filter.FilterColumn = { ApplicationViewColumnId: this.applicationViewColumn.Filter.FilterColumn.ApplicationViewColumnId };
                }
            }
            return view;
        }
        private updateSort(sortDirection: DataContracts.SortDirectionOptions): void {
            this.sortDirection(sortDirection);
            if (sortDirection == DataContracts.SortDirectionOptions.None) {
                this.grid.removeSort(this.property);
            }
            else {
                var sortBase: HallData.Service.SortDirection;
                
                switch (sortDirection) {
                    case DataContracts.SortDirectionOptions.Ascending: sortBase = HallData.Service.SortDirection.Ascending; break;
                    case DataContracts.SortDirectionOptions.Descending: sortBase = HallData.Service.SortDirection.Descending; break;
                }
                this.grid.updateSort(this.property, sortBase);
            }
        }
        public toggleSort(): void {
            if (this.sortDirection() == DataContracts.SortDirectionOptions.None || this.sortDirection() == DataContracts.SortDirectionOptions.Descending)
                this.updateSort(DataContracts.SortDirectionOptions.Ascending);
            else
                this.updateSort(DataContracts.SortDirectionOptions.Descending);
        }
        public removeSort(): void {
            this.updateSort(DataContracts.SortDirectionOptions.None);
        }
        public bindValue(item: any): any {
            return this.getValue(item, this.property);
        }
        protected getValue(item: any, path: string): any{
            var propertyKey = path.split('.');
            for (var p in item) {
                if (p == propertyKey[0]) {
                    var val = item[p];
                    var previousVal = val;
                    for (var i = 1; i < propertyKey.length; i++) {
                        for (var child in val) {
                            if (child == propertyKey[i]) {
                                previousVal = val;
                                val = val[child];
                                var koVal = val;
                                if (ko.isObservable(val) && i < propertyKey.length + 1)
                                    val = val();
                                if (Util.Obj.isArray(val))
                                    return koVal;
                                break;
                            }

                        }
                        if (val === previousVal)
                            return null;
                    }
                    return val;
                }
            }
            return null;
        }
        public redirect(item: any): void {
            if (!Util.Obj.isNullOrUndefined(this.routeData)) {
                var keys: Util.IKeyValuePair[] = [];
                this.routeData.RouteMaps.forEach((map, index, array) => {
                    var val = this.getValue(item, map.ValuePath);
                    if (!Util.Obj.isNullOrUndefined(val))
                        keys.push({ Key: map.Key, Value: val });
                });
                this.grid.redirectService.redirect(this.grid.routeDictionaryService.generateData(this.routeData.ActionName, keys));
            }
        }
    }
}