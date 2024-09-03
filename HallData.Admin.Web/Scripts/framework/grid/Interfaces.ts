/// <reference path="../../libraries/jquery/jquery.d.ts" />
/// <reference path="../../libraries/knockout/knockout.d.ts" />
module HallData.UI.Grid {
    export interface IGridBase {
        updateFilter(property: string, operation: HallData.Service.FilterOperation, compareValue: any): void;
        removeFilter(property: string): void;
        updateSort(property: string, direction: HallData.Service.SortDirection): void;
        removeSort(property: string): void;
        pageSizeOptions: KnockoutObservableArray<number>;
        currentPage: KnockoutObservable<number>;
        currentPageSize: KnockoutObservable<number>;
        totalResultCount: KnockoutObservable<number>;
        load(): JQueryPromise<void>;
        columns: KnockoutObservableArray<IColumn>;
        pager: Pager;
        gridState: KnockoutObservable<GridState>;
        authenticationState: KnockoutObservable<Authentication.AuthenticationState>;
        isLoading: KnockoutComputed<boolean>;
        isLoaded: KnockoutComputed<boolean>;
        isUnloading: KnockoutComputed<boolean>;
        isUnloaded: KnockoutComputed<boolean>;
        isError: KnockoutComputed<boolean>;
        isInitializing: KnockoutComputed<boolean>;
        isInitialized: KnockoutComputed<boolean>;
        errorMessage: KnockoutObservable<string>;
        searchCriteria: KnockoutObservable<string>;
        isColumnVisible(columnName: string): boolean;
        colSpan: KnockoutComputed<number>;
        hasSearchCriteria: KnockoutObservable<boolean>;
        gridTemplate: KnockoutObservable<string>;
        pagerTemplate: KnockoutObservable<string>;
        viewName: string;
        setColumnVisibility(column: IColumn, isVisible: boolean): void;
        reorderColumn(column: IColumn, newOrderIndex: number): void;
        controller: Controllers.IControllerBase<Controllers.IScope<any>>;
        redirectService: Util.RedirectService;
        routeDictionaryService: Util.RouteDictionaryService;
    }
    export interface IKeyValuePath {
        Key: string;
        ValuePath: string;
    }
    export interface IRouteData {
        ActionName: string;
        RouteMaps: IKeyValuePath[];
    }
    export interface IGrid<TView> extends IGridBase {
        items: KnockoutObservableArray<TView>;
    }
    export enum SortDirectionOption {
        None,
        Ascending,
        Descending
    }
    export enum FilterOperationOption {
        StartsWith,
        EndsWith,
        Contains,
        Equals
    }
    export interface IColumn {
        property: string;
        sortDirection: KnockoutObservable<DataContracts.SortDirectionOptions>;
        isSortedAscending: KnockoutComputed<boolean>;
        isSortedDescending: KnockoutComputed<boolean>;
        isNotSorted: KnockoutComputed<boolean>;
        filter: IFilter;
        grid: IGridBase;
        toggleSort(): void;
        removeSort(): void;
        headerText: string;
        canSort: boolean;
        canFilter: boolean;
        isVisible: KnockoutObservable<boolean>;
        columnTemplate: string;
        headerTemplate: string;
        cellTemplate: string;
        canAddToUI: boolean;
        configureOrderIndex: number;
        displayOrderIndex: KnockoutObservable<number>;
        getApplicationViewColumnForParty(): DataContracts.IApplicationViewColumnForParty;
        applicationViewColumn: DataContracts.IApplicationViewColumnResult;
    }
    export interface IFilter {
        filterOperations: FilterOperationDescriptor[];
        filterCompareValue: KnockoutObservable<string>;
        filterOperation: KnockoutObservable<FilterOperationDescriptor>;
        column: IColumn;
        filterProperty?: string;
        filterTemplate: string;
    }
    export enum DropDownFilterState {
        Loading,
        Loaded,
        Error
    }
    export enum GridState {
        Initializing,
        Initialized,
        Loading,
        Loaded,
        Unloading,
        Unloaded,
        Error
    }
} 