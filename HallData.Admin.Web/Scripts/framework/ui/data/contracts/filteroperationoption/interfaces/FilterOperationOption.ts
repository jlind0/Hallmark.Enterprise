module HallData.UI.DataContracts {
    export enum FilterOperationOptions {
        Contains = 1,
        StartsWith = 2,
        EndsWith = 3,
        Equals = 4
    }
    export interface IFilterOperationOptionKey {
        FilterOperationOptionId: FilterOperationOptions;
    }
    export interface IFilterOperationOptionBase extends IFilterOperationOptionKey {
        OrderIndex?: number;
    }
    export interface IFilterOperationOption extends IFilterOperationOptionBase {
        Name?: string;
    }
} 