module HallData.EMS.Services {

	declare var serviceLocation: string;

	export class EnumerationsService extends Service.Service implements IEnumerationsService {

		constructor() {
			super(serviceLocation + "enums/");
			console.log("constructor");
		}

		GetStatusTypes(): JQueryPromise<IStatusType[]> {
			console.log("GetStatusTypes");
			return Util.Service.Get<IStatusType[]>(this.ServiceLocation + "StatusTypes/");
		}

		GetDeliveryMethodTypes(): JQueryPromise<IDeliveryMethodType[]> {
			console.log("GetDeliveryMethodTypes");
			return Util.Service.Get<IDeliveryMethodType[]>(this.ServiceLocation + "DeliveryMethodTypes/");
		}

		GetTierTypes(): JQueryPromise<ITier[]> {
			console.log("GetTierTypes");
			return Util.Service.Get<ITier[]>(this.ServiceLocation + "TierTypes/");
		}

		GetFrequencies(): JQueryPromise<IFrequency[]> {
			console.log("GetFrequencies");
			return Util.Service.Get<IFrequency[]>(this.ServiceLocation + "Frequencies/");
		}

		GetProductTypes(): JQueryPromise<IProductType[]> {
			console.log("GetProductTypes");
			return Util.Service.Get<IProductType[]>(this.ServiceLocation + "ProductTypes/");
		}
	}
}