module HallData.EMS.Services.DataView {
	"use strict";

	export interface IDataView {
		uientity?: IUiEntity;
		uientitycolumns?: IUiEntityColumns;
		uidataviews?: IUiDataView;
		uidataviewresults?: IUiDataViewResults;
		uidataviewcolumns?: IUiDataViewColumns;
		uiapplicationviews?: IUiApplicationViews;
	}
} 