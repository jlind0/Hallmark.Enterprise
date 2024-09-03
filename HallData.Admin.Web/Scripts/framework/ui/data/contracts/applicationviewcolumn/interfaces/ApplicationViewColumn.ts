module HallData.UI.DataContracts {
    export enum SortDirectionOptions {
        None = 1,
        Ascending = 2,
        Descending = 3
    }
    export interface IApplicationViewColumnKey {
        ApplicationViewColumnId: number;
    }
    export interface IApplicationViewColumnWithApplicationView extends IApplicationViewColumnKey {
        ApplicationView: IApplicationViewName;
    }
    export interface IApplicationViewColumn<TTemplate extends ITemplateKey, TFilter extends IFilterKey> extends IApplicationViewColumnWithApplicationView {
        IsVisisble?: boolean;
        IsIncludedInResult?: boolean;
        DisplayOrderIndex?: number;
        CanSort?: boolean;
        InitialSortDirectionId?: SortDirectionOptions;
        CanFilter?: boolean;
        HeaderText?: string;
        ColumnTemplate?: TTemplate;
        CellTemplate?: TTemplate;
        HeaderTemplate?: TTemplate;
        Filter?: TFilter;
    }
    export interface IApplicationViewColumnDataViewColumn<TTemplateType extends ITemplateKey, TFilter extends IFilterKey, TDataViewColumn extends IDataViewColumnKey> extends
        IApplicationViewColumn<TTemplateType, TFilter>{
        DataViewColumn: TDataViewColumn;
    }
    export interface IApplicationViewColumnBase<TTemplate extends ITemplateKey, TFilter extends IFilterKey, TDataViewColumn extends IDataViewColumnKey>
        extends IApplicationViewColumnDataViewColumn<TTemplate, TFilter, TDataViewColumn> {
        CanBeAddedToUI: boolean;
        ConfigureOrderIndex: number;
        RouteData?: string;
    }
    export interface IApplicationViewColumnForParty extends IApplicationViewColumn<ITemplateKey, IFilterBase> { }
    export interface IApplicationViewColumnBaseImplemention extends IApplicationViewColumnBase<ITemplateKey, IFilterBase, IDataViewColumnKey> { }
    export interface IApplicationViewColumnResult extends IApplicationViewColumnBase<ITemplateResult, IFilterResult, IDataViewColumnResultName> { }
    export interface IApplicationViewColumnViewWithData<TDataViewColumn extends IDataViewColumnKey> extends IApplicationViewColumnWithApplicationView {
        DataViewColumn: TDataViewColumn;
    }
    export interface IApplicationViewColumnWithDataResult extends IApplicationViewColumnViewWithData<IDataViewColumnDisplayName> { }
}