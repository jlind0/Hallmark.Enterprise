module HallData.UI.DataContracts {
    export interface IFilterTypeKey {
        FilterTypeId: number;
    }
    export interface IFilterType<TTemplate extends ITemplateKey, TFilterOperationOption extends IFilterOperationOptionKey> extends IFilterTypeKey {
        Name?: string;
        DataType?: string;
        Template?: TTemplate;
        FilterOperations?: TFilterOperationOption[];
        IsDefault: boolean;
    }
    export interface IFilterTypeWithColumns<TTemplate extends ITemplateKey, TFilterOperationOption extends IFilterOperationOptionKey, TDataViewColumn extends IDataViewColumnKey>
        extends IFilterType<TTemplate, TFilterOperationOption> {
        AvailableColumns: TDataViewColumn[];
    }
    export interface IFilterTypeBase extends IFilterTypeWithColumns<ITemplateKey, IFilterOperationOptionBase, IDataViewColumnKey> { }
    export interface IFilterTypeWithColumnsResult extends IFilterTypeWithColumns<ITemplateResult, IFilterOperationOption, IDataViewColumnResult> { }
    export interface IFilterTypeResult extends IFilterType<ITemplateResult, IFilterOperationOption> { }
} 