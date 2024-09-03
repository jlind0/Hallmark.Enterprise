interface JQuery {
    dropdownchecklist(options?: IDropDownCheckBoxListOptions) : void;
}
interface IDropDownCheckBoxListOptions {
    bgiframe?: boolean;
    closeRadioOnClick?: boolean;
    emptyText?: string;
    firstItemChecksAll?: any;
    forceMultiple?: boolean;
    icon?: IDropDownCheckBoxListIcon;
    maxDropHeight?: number;
    minWidth?: number;
    onComplete?: (selector: JQuery) => void;
    onItemClick?: (checkbox: JQuery, selector: JQuery) => void;
    positionHow?: string;
    textFormatFunction?: (options: JQuery) => string;
    width?: number;
    zIndex?: number;
}
interface IDropDownCheckBoxListIcon {
    placement?: string;
    toOpen?: string;
    toClose?: string;
}