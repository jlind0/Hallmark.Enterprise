module HallData.EMS.DataView {
	"use strict";

	export interface IDataViewAddController extends IViewController<IDataViewAddScope> {
		add(isValid: boolean, dataView: Service.IQueryResult<Services.DataView.IDataView>): void;
	}
}