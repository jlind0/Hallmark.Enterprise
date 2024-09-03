module HallData.UI.DataContracts {
    export interface IFilterKey {
        FilterId: number;
    }
    export interface IFilter<TFilterType extends IFilterTypeKey, TFilterColumn extends IApplicationViewColumnKey, TFilterOperationOption extends IFilterOperationOptionKey, TTemplate extends ITemplateKey>
        extends IFilterKey {
        FilterType: TFilterType;
        FilterColumn?: TFilterColumn;
        DelayFilter?: number;
        Template?: TTemplate;
        FilterOperationOptions?: TFilterOperationOption[];
    }
    export interface IFilterBase extends IFilter<IFilterTypeKey, IApplicationViewColumnKey, IFilterOperationOptionBase, ITemplateKey> { }
    export interface IFilterResult extends IFilter<IFilterTypeResult, IApplicationViewColumnResult, IFilterOperationOption, ITemplateResult> { }
}