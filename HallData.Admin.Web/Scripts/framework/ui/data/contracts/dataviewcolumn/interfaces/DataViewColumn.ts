module HallData.UI.DataContracts {
    export interface IDataViewColumnKey {
        DataViewColumnId: number;
    }
    export interface IDataViewColumnDisplayName extends IDataViewColumnKey {
        DisplayName: string;
    }
    export interface IDataViewColumnResultName extends IDataViewColumnDisplayName {
        ResultName: string;
    }
    
    export interface IDataViewColumn<TDataView extends IDataViewKey> extends IDataViewColumnResultName {
        IsRequired: boolean;
        Name: string;
        DataView: TDataView;
        ResultSetIndex: number;
    }
    export interface IDataViewColumnBase extends IDataViewColumn<IDataViewKey> { }
    export interface IDataViewColumnResult extends IDataViewColumn<IDataViewName> { }
}