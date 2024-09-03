/// <reference path="../../libraries/jquery/jquery.d.ts" />
/// <reference path="../../libraries/knockout/knockout.d.ts" />
/// <reference path="../../libraries/async/async.d.ts" />
module HallData.UI.Grid {
	"use strict";

	export enum GridAction {
		Init,
		Load,
		Unload
	}

	interface IGridActionParameters {
		action: GridAction;
		view?: DataContracts.IApplicationViewResult;
		isSecondTry?: boolean;
	}

	export class GridBase<TView, TService extends HallData.Service.IViewServiceBase> implements IGrid<TView> {

		public items: KnockoutObservableArray<TView>;
		//public items: KnockoutObservableArray<TView>;
		protected filterContext: HallData.Service.FilterContext;
		protected sortContext: HallData.Service.SortContext;
		public currentPage: KnockoutObservable<number>;
		public currentPageSize: KnockoutObservable<number>;
		public pager: Pager;
		public totalResultCount: KnockoutObservable<number>;
		public gridState: KnockoutObservable<GridState>;
		public isLoading: KnockoutComputed<boolean>;
		public isLoaded: KnockoutComputed<boolean>;
		public isUnloading: KnockoutComputed<boolean>;
		public isUnloaded: KnockoutComputed<boolean>;
		public isInitializing: KnockoutComputed<boolean>;
		public isInitialized: KnockoutComputed<boolean>;
		public isAuthenticationError: KnockoutObservable<boolean>;
		public isAuthenticated: KnockoutObservable<boolean>;
		public isAuthenticating: KnockoutObservable<boolean>;
		public isSignedIn: KnockoutObservable<boolean>;
		public isError: KnockoutComputed<boolean>;
		public errorMessage: KnockoutObservable<string>;
		public searchCriteria: KnockoutObservable<string> = ko.observable<string>();
		public uiColumns: KnockoutObservableArray<IColumn> = ko.observableArray<IColumn>();
		public visibleColumns: KnockoutObservableArray<IColumn> = ko.observableArray<IColumn>();
		public selectedColumns: KnockoutObservableArray<string> = ko.observableArray<string>();
		public authenticationState: KnockoutObservable<Authentication.AuthenticationState>;
		private applicationView: DataContracts.IApplicationViewResult;
		public hasPaging: KnockoutObservable<boolean> = ko.observable(true);
		public canSaveSettings: KnockoutObservable<boolean> = ko.observable(true);
		public sortSingleColumnMode: KnockoutObservable<boolean> = ko.observable(false);
		public canPickColumns: KnockoutObservable<boolean> = ko.observable(true);
		public canReorderColumns: KnockoutObservable<boolean> = ko.observable(true);
		private workerQueue: AsyncQueue<IGridActionParameters> = async.queue<IGridActionParameters>((p, c) => {
			var isFirst = true;
			switch (p.action) {
				case GridAction.Load: this.controller.process(() => this.doLoad().then(() => {
					this.gridState(GridState.Loaded);
					this.isLoadScheduled = false;
				}, error => {
						if (Util.String.isNullOrWhitespace(this.errorMessage()))
							this.errorMessage("There was an error loading the data");
						this.gridState(GridState.Error);
						this.isLoadScheduled = false;
						if (error.status == 403 && isFirst)
							isFirst = false;
						return error;
					})).then(() => c());
					break;
				case GridAction.Unload: this.controller.process(() => this.doUnload().then(() => {
					this.gridState(GridState.Unloaded);
				}, error => {
						this.gridState(GridState.Error);
						if (Util.String.isNullOrWhitespace(this.errorMessage()))
							this.errorMessage("There was an error unloading the data");
						if (error.status == 403 && isFirst)
							isFirst = false;
						return error;
					})).then(() => c());
					break;
				case GridAction.Init: this.controller.process(() => this.doInit().then(() => { }, error => {

					this.gridState(GridState.Error);
					if (Util.String.isNullOrWhitespace(this.errorMessage()))
						this.errorMessage("There was an error initializing the grid");
					if (error.status == 403 && isFirst)
						isFirst = false;
					return error;
				})).then(() => c());
					break;

			}
		}, 1);

		private isLoadScheduled: boolean = false;
		public columns: KnockoutObservableArray<IColumn>;
		private columnDic: Collections.Dictionary<IColumn> = new Collections.Dictionary<IColumn>();
		public colSpan: KnockoutComputed<number>;
		public pageSizeOptions: KnockoutObservableArray<number> = ko.observableArray<number>();
		public hasSearchCriteria: KnockoutObservable<boolean> = ko.observable<boolean>(true);

		private static DefaultGridTemplate: string = "Grid";
		private static DefaultPagerTemplate: string = "GridPager";
		private static DefaultPageSizeOptions: number[] = [10, 25, 50];
		private static DefaultPageDisplaySize: number = 5;
		private static DefaultHasSearchCriteria: boolean = true;
		private static DefaultSearchCriteriaDelay: number = 1000;
		public gridTemplate: KnockoutObservable<string> = ko.observable<string>(GridBase.DefaultGridTemplate);
		public pagerTemplate: KnockoutObservable<string> = ko.observable<string>(GridBase.DefaultPagerTemplate);
		private searchCriteriaDelay: number = GridBase.DefaultSearchCriteriaDelay;
		public isSaving: KnockoutObservable<boolean> = ko.observable(false);
		public sortByBestMatch: KnockoutObservable<boolean> = ko.observable(false);
		public isGlobalFiltered: KnockoutComputed<boolean> = ko.computed(() => !Util.String.isNullOrWhitespace(this.searchCriteria()));
		public hasHeader: KnockoutComputed<boolean> = ko.computed(() => this.hasPaging() || this.hasSearchCriteria() || this.canPickColumns());

		constructor(protected service: TService, protected personalizationService: Services.IPersonalizationService, public viewName: string,
			public controller: Controllers.IControllerBase<Controllers.IScope<any>>, public redirectService: Util.RedirectService, public routeDictionaryService: Util.RouteDictionaryService) {
			this.controller.controllerStateChanged.add(state => {
				switch (state) {
					case Controllers.ControllerState.Loaded: this.init(this.applicationView); break;
				}
			});
			this.controller.controllerAuthenticationStateChanged.add(state => {
				this.authenticationState(state);
				this.isAuthenticated(state == Authentication.AuthenticationState.Authenticated);
				this.isAuthenticating(state == Authentication.AuthenticationState.Authenticating);
				this.isAuthenticationError(state == Authentication.AuthenticationState.AuthenticationError);
				this.isSignedIn(this.controller.$scope.isSignedIn);
			});
			this.authenticationState = ko.observable<Authentication.AuthenticationState>(this.controller.getAuthenticationState());
			this.items = ko.observableArray<TView>();
			this.filterContext = new HallData.Service.FilterContext();
			this.sortContext = new HallData.Service.SortContext();
			this.totalResultCount = ko.observable(0);
			this.gridState = ko.observable(GridState.Initializing);
			this.isLoading = ko.computed(() => this.gridState() == GridState.Loading);
			this.isLoaded = ko.computed(() => this.gridState() == GridState.Loaded);
			this.isUnloading = ko.computed(() => this.gridState() == GridState.Unloading);
			this.isUnloaded = ko.computed(() => this.gridState() == GridState.Unloaded);
			this.isError = ko.computed(() => this.gridState() == GridState.Error);
			this.isInitializing = ko.computed(() => this.gridState() == GridState.Initializing);
			this.isInitialized = ko.computed(() => this.gridState() == GridState.Initialized);
			this.isAuthenticated = ko.observable(this.controller.$scope.isAuthenticated);
			this.isAuthenticating = ko.observable(this.controller.$scope.isAuthenticating);
			this.isAuthenticationError = ko.observable(this.controller.$scope.isAuthenticationError);
			this.isSignedIn = ko.observable(this.controller.$scope.isSignedIn);
			this.errorMessage = ko.observable<string>();
			this.columns = ko.observableArray<IColumn>();
			this.searchCriteria.subscribe(() => setTimeout(() => this.updateSearchCriteria(), this.searchCriteriaDelay));

			this.currentPageSize = ko.observable<number>();
			this.currentPage = ko.observable<number>();
			this.currentPageSize.subscribe(() => this.load());
			this.pager = new Pager(this);
			this.colSpan = ko.computed<number>(() => this.visibleColumns().length);
			this.selectedColumns.subscribe(cols => {
				if (this.gridState() != GridState.Initializing) {
					var uicolumns = this.uiColumns();
					for (var i = 0; i < uicolumns.length; i++) {
						uicolumns[i].isVisible(cols.indexOf(uicolumns[i].property) >= 0);
					}
				}
			});
			this.isGlobalFiltered.subscribe(isFiltered => this.sortByBestMatch(isFiltered));
			this.sortByBestMatch.subscribe(sortBy => {
				if (sortBy) {
					this.clearSorts();
					this.sortContext.insertSort({ Direction: Service.SortDirection.Descending, Property: "__Rank?" });
				}
				else {
					this.sortContext.removeSort("__Rank?");
					for (var i = 0; i < this.visibleColumns.length; i++) {
						var c = this.visibleColumns()[i];
						if (c.applicationViewColumn.InitialSortDirectionId != DataContracts.SortDirectionOptions.None) {
							c.sortDirection(c.applicationViewColumn.ApplicationViewColumnId);
							this.sortContext.updateSort({ Direction: c.applicationViewColumn.InitialSortDirectionId == DataContracts.SortDirectionOptions.Ascending ? Service.SortDirection.Ascending : Service.SortDirection.Descending, Property: c.property });
						}
					}
				}
			});
			if(controller.getControllerState() == Controllers.ControllerState.Loaded)
				this.init();
		}

		private configureSettings(pageSizeOptions: number[], initialPageSize: number, initialPage: number, pageDisplayCount: number, hasSearchCriteria: boolean,
			searchCriteriaDelay: number, gridTemplate: string, pagerTemplate: string, hasPaging: boolean, canSaveSettings: boolean, sortSingleColumnMode: boolean,
			canPickColumns: boolean, canReorderColumns: boolean): void {
			console.log("configureSettings");
			this.pageSizeOptions.removeAll();
			if (Util.Obj.isNullOrUndefined(hasPaging))
				hasPaging = true;
			this.hasPaging(hasPaging);
			if (Util.Obj.isNullOrUndefined(canSaveSettings))
				canSaveSettings = true;
			this.canSaveSettings(canSaveSettings);
			if (Util.Obj.isNullOrUndefined(sortSingleColumnMode))
				sortSingleColumnMode = false;
			this.sortSingleColumnMode(sortSingleColumnMode);
			if (Util.Obj.isNullOrUndefined(canPickColumns))
				canPickColumns = true;
			this.canPickColumns(canPickColumns);
			if (Util.Obj.isNullOrUndefined(canReorderColumns))
				canReorderColumns = true;
			this.canReorderColumns(true);
			if (Util.Obj.isNullOrUndefined(pageSizeOptions)) {
				pageSizeOptions = GridBase.DefaultPageSizeOptions;
			}
			for (var i = 0; i < pageSizeOptions.length; i++) {
				this.pageSizeOptions.push(pageSizeOptions[i]);
			}
			if (Util.Obj.isNullOrUndefined(initialPage))
				initialPage = 1;
			this.currentPage(initialPage);
			if (Util.Obj.isNullOrUndefined(pageDisplayCount))
				pageDisplayCount = GridBase.DefaultPageDisplaySize;
			this.pager.pageDisplayCount = pageDisplayCount;
			
			if (Util.Obj.isNullOrUndefined(hasSearchCriteria))
				hasSearchCriteria = GridBase.DefaultHasSearchCriteria;
			this.hasSearchCriteria(hasSearchCriteria);
			if (Util.Obj.isNullOrUndefined(searchCriteriaDelay))
				searchCriteriaDelay = GridBase.DefaultSearchCriteriaDelay;
			this.searchCriteriaDelay = searchCriteriaDelay;
			if (Util.Obj.isNullOrUndefined(initialPageSize))
				initialPageSize = this.pageSizeOptions[0];
			this.currentPageSize(initialPageSize);
			if (Util.Obj.isNullOrUndefined(gridTemplate))
				gridTemplate = GridBase.DefaultGridTemplate;
			this.gridTemplate(gridTemplate);
			if (Util.Obj.isNullOrUndefined(pagerTemplate))
				pagerTemplate = GridBase.DefaultPagerTemplate;
			this.pagerTemplate(pagerTemplate);
		}

		public getApplicationViewForParty(): DataContracts.IApplicationViewForParty {
			console.log("getApplicationViewForParty");
			var cols: DataContracts.IApplicationViewColumnForParty[] = [];
			var uiCols = this.uiColumns();
			for (var i = 0; i < uiCols.length; i++) {
				cols.push(uiCols[i].getApplicationViewColumnForParty());
			}
			var view: DataContracts.IApplicationViewForParty ={
				ApplicationViewId: this.applicationView.ApplicationViewId,
				Columns: cols,
				HasSearchCriteria: this.applicationView.HasSearchCriteria,
				InitialPage: this.applicationView.InitialPage,
				InitialPageSize: this.applicationView.InitialPageSize,
				Name: this.applicationView.Name,
				PageDisplayCount: this.applicationView.PageDisplayCount,
				PageOptions: this.applicationView.PageOptions,
				SearchCriteriaDelay: this.applicationView.SearchCriteriaDelay
			};
			if (!Util.Obj.isNullOrUndefined(this.applicationView.GridTemplate))
				view.GridTemplate = { TemplateId: this.applicationView.GridTemplate.TemplateId };
			if (!Util.Obj.isNullOrUndefined(this.applicationView.PagerTemplate))
				view.PagerTemplate = { TemplateId: this.applicationView.PagerTemplate.TemplateId };
			return view;
		}

		public saveSettings() {
			console.log("saveSettings");
			this.isSaving(true);
			this.controller.process(() => this.personalizationService.Personlize(this.getApplicationViewForParty()).then(view => {
				this.isSaving(false); this.init(view)
			}, error => {
					this.isSaving(false);
					this.errorMessage(error.responseText);
					this.gridState(GridState.Error);
					return error;
				}));
		}

		protected doSaveSettings(): JQueryPromise<void> {
			this.isSaving(true);
			return;
		}

		protected loadColumns(cols: IColumn[]): void {
			this.columnDic.clear();
			this.columns.removeAll();
			this.uiColumns.removeAll();
			this.filterContext.clear();
			this.sortContext.clear();
			this.visibleColumns.removeAll();
			this.selectedColumns.removeAll();
			var columns = Enumerable.From(cols).OrderBy(c => c.displayOrderIndex()).ToArray();
			for (var i = 0; i < columns.length; i++) {
				var c = columns[i];
				c.grid = this;
				if (!c.isNotSorted())
					this.sortContext.addSort(new Service.SortDescriptor(c.isSortedAscending() ? Service.SortDirection.Ascending : Service.SortDirection.Descending, c.property));
				this.columns.push(c);
				this.columnDic.add(c.property, c);
				if (c.isVisible()) {
					this.visibleColumns.push(c);
					this.selectedColumns.push(c.property);
				}
			}
			var uiCols = Enumerable.From(cols).Where(c => c.canAddToUI).OrderBy(c => c.configureOrderIndex).ToArray();
			for (var i = 0; i < uiCols.length; i++) {
				this.uiColumns.push(uiCols[i]);
			}
			
			this.colSpan.notifySubscribers();
		}

		public setColumnVisibility(column: IColumn, isVisible: boolean): void {
			console.log("setColumnVisibility");
			this.colSpan.notifySubscribers();
			if (isVisible) {
				column.displayOrderIndex(this.visibleColumns().length > 0 ? Enumerable.From(this.visibleColumns()).Max(c => c.displayOrderIndex()) + 1: 1);
				this.visibleColumns.push(column);
			}
			else {
				column.displayOrderIndex(null);
				this.visibleColumns.remove(column);
				var vArray = this.visibleColumns();
				for (var j = 0; j < vArray.length; j++) {
					vArray[j].displayOrderIndex(j + 1);
				}
			}
			$("[data-id='filter']").multiselect({
				buttonText: function () {
					return "";
				}
			});

			$("[data-id='filterSelect']").multiselect({
				buttonText: function () {
					return "";
				}
			});

			//$("[data-id='dropdownFilterSelect']").multiselect({
			//	buttonText: function () {
			//		return "Filter...";
			//	}
			//});
		}

		public isColumnVisible(columnName: string): boolean {
			console.log("isColumnVisible");
			return this.columnDic.getValue(columnName).isVisible();
		}

		public reorderColumn(column: IColumn, newOrderIndex: number): void {
			console.log("reorderColumn");
			if (column.displayOrderIndex() == newOrderIndex)
				return;
			var movedForward = column.displayOrderIndex() < newOrderIndex;
			var columnsE = Enumerable.From(this.uiColumns()).Where(c => movedForward ? c.displayOrderIndex() <= newOrderIndex : c.displayOrderIndex() >= newOrderIndex).Where(c => c !== column);
			if (movedForward)
				columnsE = columnsE.OrderByDescending(c => c.displayOrderIndex());
			else
				columnsE = columnsE.OrderBy(c => c.displayOrderIndex());
			column.displayOrderIndex(newOrderIndex);
			var orderIndexIncrementer = movedForward ? -1 : 1;
			var columns = columnsE.ToArray();
			for (var i = 0; i < columns.length; i++) {
				columns[i].displayOrderIndex(newOrderIndex + (i + 1) * orderIndexIncrementer);
			}
			this.visibleColumns.sort((left, right) => left.displayOrderIndex() > right.displayOrderIndex() ? 1 : -1);
		}

		private updateSearchCriteria() {
			console.log("updateSearchCriteria");
			if (this.filterContext.SearchCriteria != this.searchCriteria()) {
				this.filterContext.SearchCriteria = this.searchCriteria();
				this.currentPage(1);
				this.load();
			}
		}

		public updateFilter(property: string, operation: HallData.Service.FilterOperation, compareValue: any): void {
			console.log("updateFilter");
			this.filterContext.addFilter(new HallData.Service.FilterDescriptor(property, operation, compareValue));
			this.currentPage(1);
			this.load();
		}

		public removeFilter(property: string): void {
			console.log("removeFilter");
			this.filterContext.removeFilter(property);
			this.currentPage(1);
			this.load();
		}

		public updateSort(property: string, direction: HallData.Service.SortDirection): void {
			console.log("updateSort");
			if (this.sortSingleColumnMode())
				this.clearSorts(this.columnDic.getValue(property));
			var sort: Service.ISortDescriptor = { Direction: direction, Property: property };
			if (this.isGlobalFiltered()) {
				var cols = Enumerable.From(this.visibleColumns()).Where(c => !c.isNotSorted()).Count();
				this.sortContext.insertSort(sort, Math.max(0, cols - 1));
			}
			else
				this.sortContext.updateSort(sort);
			this.load();
		}

		protected clearSorts(col? : IColumn): void {
			for (var i = 0; i < this.visibleColumns().length; i++) {
				var c = this.visibleColumns()[i];
				if (c !== col && c.sortDirection() != DataContracts.SortDirectionOptions.None) {
					c.sortDirection(DataContracts.SortDirectionOptions.None);
					this.sortContext.removeSort(c.property);
				}
			}
		}

		public removeSort(property: string): void {
			console.log("removeSort");
			this.sortContext.removeSort(property);
			this.load();
		}

		public load(isSecondTry? : boolean): JQueryPromise<void> {
			console.log("load");
			var d = $.Deferred<void>();
			if (this.isInitializing()) {
				d.resolve();
				return d.promise();
			}
			if (this.isLoading() || this.isLoaded()) {
				this.unload(isSecondTry);
				this.isLoadScheduled = false;
			}
			if (!this.isLoadScheduled) {
				this.isLoadScheduled = true;
				this.workerQueue.push({ action: GridAction.Load, isSecondTry: isSecondTry }, () => d.resolve());
			}
			else
				d.resolve();
			return d.promise();
		}

		public unload(isSecondTry?: boolean): JQueryPromise<void> {
			console.log("unload");
			var d = $.Deferred<void>();
			if (!this.isUnloading() && !this.isUnloaded())
				this.workerQueue.push({ action: GridAction.Unload, isSecondTry: isSecondTry },() => d.resolve());
			else
				d.resolve();
			return d.promise();
		}

		public init(view?: DataContracts.IApplicationViewResult, isSecondTry? : boolean): JQueryPromise<void> {
			console.log("init");
			var d = $.Deferred<void>();
			if (this.isLoaded() || this.isLoading())
				this.unload(isSecondTry);
			this.workerQueue.push({ action: GridAction.Init, view: view, isSecondTry: isSecondTry },() => d.resolve());
			return d.promise();
		}

		protected doLoad(): JQueryPromise<void> {
			this.gridState(GridState.Loading);
			var page = this.hasPaging() ? new HallData.Service.PageDescriptor(this.currentPage(), this.currentPageSize()) : null;
			return this.getItems(page).then(results => {
				if (results.TotalResultsCount > 0) {
					for (var i = 0; i < this.columns.length; i++) {
						var col: IColumn = this.columns[i];
						col.isVisible(results.AvailableProperties.indexOf(col.property) >= 0);
					}
					this.colSpan.notifySubscribers();
				}
				for (var i = 0; i < results.Results.length; i++) {
					this.items.push(results.Results[i]);
				}
				this.totalResultCount(results.TotalResultsCount);
				this.pager.buildPages();
			});
		}

		protected doUnload(): JQueryPromise<void> {
			this.gridState(GridState.Unloading);
			var d = $.Deferred<void>();
			this.items.removeAll();
			d.resolve();
			return d.promise();
		}

		protected doInit(view?: DataContracts.IApplicationViewResult): JQueryPromise<void> {
			this.gridState(GridState.Initializing);
			if (!Util.Obj.isNullOrUndefined(view)) {
				var d = $.Deferred<void>();
				this.loadApplicationView(view);
				d.resolve();
				return d.promise();
			}
			var promise = this.personalizationService.Get(this.viewName);
			return promise.then(v => this.loadApplicationView(v), error => this.processError(error));
		}

		protected processError(error: any) :any {
			this.gridState(GridState.Error);
			this.errorMessage(error.responseText);
			return error;
		}

		private loadApplicationView(view: DataContracts.IApplicationViewResult) {
			console.log("loadApplicationView: " + view);
			this.applicationView = view;
			var gridTemplate: string = null;
			var pagerTemplate: string = null;
			if (!Util.Obj.isNullOrUndefined(view.GridTemplate))
				gridTemplate = view.GridTemplate.Code;
			if (!Util.Obj.isNullOrUndefined(view.PagerTemplate))
				pagerTemplate = view.PagerTemplate.Code;
			var pageOptions: number[] = [];
			if (!Util.Obj.isNullOrUndefined(view.PageOptions)) {
				for (var i = 0; i < view.PageOptions.length; i++) {
					pageOptions.push(view.PageOptions[i].PageOption);
				}
			}
			if (pageOptions.length == 0)
				pageOptions = null;
			this.configureSettings(pageOptions, view.InitialPageSize, view.InitialPage, view.PageDisplayCount, view.HasSearchCriteria, view.SearchCriteriaDelay,
				gridTemplate, pagerTemplate, view.HasPaging, view.CanSaveSettings, view.SortSingleColumnMode, view.CanPickColumns, view.CanReorderColumns);
			var columns: IColumn[] = [];
			for (var i = 0; i < view.Columns.length; i++) {
				var c = view.Columns[i];
				var filter: IFilter = null;
				if (!Util.Obj.isNullOrUndefined(c.Filter)) {
					var filterColumnName: string = null;

					if (!Util.Obj.isNullOrUndefined(c.Filter.FilterColumn))
						filterColumnName = this.transformResultName(c.Filter.FilterColumn.DataViewColumn.ResultName);
					var operations: DataContracts.FilterOperationOptions[] = [];
					if (!Util.Obj.isNullOrUndefined(c.Filter.FilterOperationOptions)) {
						for (var j = 0; j < c.Filter.FilterOperationOptions.length; j++) {
							operations.push(c.Filter.FilterOperationOptions[j].FilterOperationOptionId);
						}
					}
					if (operations.length == 0)
						operations = null;
					var template: string = null;
					if (!Util.Obj.isNullOrUndefined(c.Filter.Template))
						template = c.Filter.Template.Code;
					filter = Util.Obj.createHallDataInstance<IFilter>(c.Filter.FilterType.DataType, this.controller, c.Filter.DelayFilter, null, operations, filterColumnName, template);
				}
				var columnTemplate: string = null;
				var headerTemplate: string = null;
				var cellTemplate: string = null;
				if (!Util.Obj.isNullOrUndefined(c.ColumnTemplate))
					columnTemplate = c.ColumnTemplate.Code;
				if (!Util.Obj.isNullOrUndefined(c.HeaderTemplate))
					headerTemplate = c.HeaderTemplate.Code;
				if (!Util.Obj.isNullOrUndefined(c.CellTemplate))
					cellTemplate = c.CellTemplate.Code;
				columns.push(new Column(this.transformResultName(c.DataViewColumn.ResultName), c.HeaderText, c, c.CanFilter, filter, c.CanSort, c.InitialSortDirectionId,
					c.IsVisisble, columnTemplate, headerTemplate, cellTemplate, c.CanBeAddedToUI, c.ConfigureOrderIndex, c.DisplayOrderIndex));
			}
			this.loadColumns(columns);
			this.gridState(GridState.Initialized);
			//$("." + this.viewName + " .column-selector").dropdownchecklist({
			//	textFormatFunction: options => {
			//		return "Show/Hide Columns";
			//	},
			//	icon: { placement: "left", toOpen: "fa fa-caret-right", toClose: "fa fa-caret-up" },
			//	width: 200
			//});
			$("." + this.viewName + " .column-selector").multiselect();

			$("[data-id='filterSelect']").multiselect({
				buttonText: function () {
					console.log("filterSelect.multiselect()");
					return "";
				}
			});

			//$("[data-id='dropdownFilterSelect']").multiselect({
			//	buttonText: function () {
			//		console.log("dropdownFilterSelect.multiselect()");
			//		return "Filter...";
			//	}
			//});

			this.load();
		}

		protected transformResultName(resultName: string): string {
			var collections = resultName.split('|');
			for (var i = 0; i < collections.length - 1; i++){
				collections[i] = collections[i].slice(collections[i].lastIndexOf("$") + 1, collections[i].length);
			}
			return collections.join(".").replace("#", "");
		}

		//protected getItems(page: HallData.Service.IPageDescriptor): JQueryPromise<HallData.Service.IQueryResults> {
		//	var promise = <JQueryPromise<HallData.Service.IQueryResults>>this.service.GetViewMany(this.viewName, this.filterContext, this.sortContext, page);
		//	return promise.then(items => items, error => 
		//		this.processError(error)
		//	);
		//}

		protected getItems(page: HallData.Service.IPageDescriptor): JQueryPromise<HallData.Service.IQueryResults<TView>> {
			var promise = <JQueryPromise<HallData.Service.IQueryResults<TView>>>this.service.GetViewMany(this.viewName, this.filterContext, this.sortContext, page);
			return promise.then(items => items, error =>
				this.processError(error)
				);
		}
	}
}