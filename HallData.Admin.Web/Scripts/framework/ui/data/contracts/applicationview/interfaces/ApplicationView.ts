module HallData.UI.DataContracts {
	export interface IApplicationViewKey {
		ApplicationViewId: number;
	}
	export interface IApplicationViewName extends IApplicationViewKey {
		Name: string;
	}
	export interface IApplicationView<TApplicationViewColumn extends IApplicationViewColumnKey, TTemplate extends ITemplateKey> extends IApplicationViewName {
		Columns: TApplicationViewColumn[];
		GridTemplate?: TTemplate;
		PagerTemplate?: TTemplate;
		HasSearchCriteria?: boolean;
		SearchCriteriaDelay?: number;
		InitialPage?: number;
		InitialPageSize?: number;
		PageDisplayCount?: number;
		PageOptions?: IPageOption[];
		HasPaging?: boolean;
		CanSaveSettings?: boolean;
		SortSingleColumnMode?: boolean;
		CanPickColumns?: boolean;
		CanReorderColumns?: boolean;
	}
	export interface IApplicationViewDataView<TApplicationColumn extends IApplicationViewColumnKey, TTemplate extends ITemplateKey, TDataView extends IDataViewKey> extends
		IApplicationView<TApplicationColumn, TTemplate> {
		DataView: TDataView;
	}
	export interface IPageOption {
		PageOption: number;
	}
	export interface IApplicationViewBase extends IApplicationViewDataView<IApplicationViewColumnBaseImplemention, ITemplateKey, IDataViewKey> { }
	export interface IApplicationViewForParty extends IApplicationView<IApplicationViewColumnForParty, ITemplateKey> { }
	export interface IApplicationViewResult extends IApplicationViewDataView<IApplicationViewColumnResult, ITemplateResult, IDataViewName> { }
}