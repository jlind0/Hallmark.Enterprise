module HallData.UI.DataContracts {
    export interface IDataViewKey {
        DataViewId: number;
    }
    export interface IDataViewName extends IDataViewKey {
        Name: string;
    }
    export interface IDataView<TDataViewColumn extends IDataViewColumnKey> extends IDataViewName {
        Columns: TDataViewColumn[];
    }
    export interface IDataViewBase extends IDataView<IDataViewColumnKey> { }
    export interface IDataViewResult extends IDataView<IDataViewColumnBase> { }
}