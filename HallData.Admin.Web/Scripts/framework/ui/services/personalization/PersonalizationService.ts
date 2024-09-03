/// <reference path="../../../../libraries/jquery/jquery.d.ts" />
module HallData.UI.Services {
	export interface IPersonalizationService extends Service.IService {
		Get(viewName: string): JQueryPromise<DataContracts.IApplicationViewResult>;
		Personlize(view: DataContracts.IApplicationViewForParty): JQueryPromise<DataContracts.IApplicationViewResult>;
		RestoreDefaultSettings(viewName: string): JQueryPromise<DataContracts.IApplicationViewResult>;
	}
	declare var serviceLocation: string;
	export class PersonalizationService extends Service.Service implements IPersonalizationService {
		constructor() {
			super(serviceLocation);
			this.ServiceLocation += "ui/personalization/";
		}
		Get(viewName: string): JQueryPromise<DataContracts.IApplicationViewResult> {
			return Util.Service.Get<DataContracts.IApplicationViewResult>(this.ServiceLocation + "Get/" + viewName);
		}
		Personlize(view: DataContracts.IApplicationViewForParty): JQueryPromise<DataContracts.IApplicationViewResult> {
			return Util.Service.Put<DataContracts.IApplicationViewResult>(view, this.ServiceLocation + "Personalize/");
		}
		RestoreDefaultSettings(viewName: string): JQueryPromise<DataContracts.IApplicationViewResult> {
			return Util.Service.Put<DataContracts.IApplicationViewResult>(viewName, this.ServiceLocation + "Restore/");
		}
	}
}