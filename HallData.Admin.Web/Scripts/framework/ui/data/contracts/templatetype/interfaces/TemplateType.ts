module HallData.UI.DataContracts {
    export interface ITemplateTypeKey {
        TemplateTypeId: number;
    }
    export interface ITemplateType extends ITemplateTypeKey {
        Name?: string;
    }
}