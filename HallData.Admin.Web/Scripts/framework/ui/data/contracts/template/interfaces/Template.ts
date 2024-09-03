module HallData.UI.DataContracts {
    export interface ITemplateKey {
        TemplateId: number;
    }
    export interface ITemplate<TTemplateType extends ITemplateTypeKey> extends ITemplateKey {
        Name?: string;
        Code?: string;
        TemplateType?: TTemplateType;
        IsDefault: boolean;
    }
    export interface ITemplateWithChildren<TTemplateType extends ITemplateTypeKey, TChildTemplate extends ITemplateKey, TDataViewColumn extends IDataViewColumnKey>
        extends ITemplate<TTemplateType> {
        ChildTemplates?: TChildTemplate[];
        DataViewColumns?: TDataViewColumn[];
    }
    export interface ITemplateBase extends ITemplateWithChildren<ITemplateTypeKey, ITemplateKey, IDataViewColumnKey> { }
    export interface ITemplateResult extends ITemplate<ITemplateType> { }
    export interface ITemplateWithChildrenResult extends ITemplateWithChildren<ITemplateType, ITemplateResult, IDataViewColumnResult> { }
}