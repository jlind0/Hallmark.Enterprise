module HallData.EMS.Services {

	export interface IEnumerationsService extends Service.IService {
		GetStatusTypes(): JQueryPromise<IStatusType[]>;
		GetDeliveryMethodTypes(): JQueryPromise<IDeliveryMethodType[]>;
		GetTierTypes(): JQueryPromise<ITier[]>;
		GetFrequencies(): JQueryPromise<IFrequency[]>;
		GetProductTypes(): JQueryPromise<IProductType[]>;
	}
}