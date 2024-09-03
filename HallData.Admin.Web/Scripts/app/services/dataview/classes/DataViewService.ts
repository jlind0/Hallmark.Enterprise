module HallData.EMS.Services.DataView {
	"use strict";

	declare var serviceLocation: string;

	export class DataViewService extends Service.DeletableService<IDataView> implements IDataViewService {

		constructor() {
			super(serviceLocation + "dataviews/");
			console.log("constructor");
		}
	}
} 