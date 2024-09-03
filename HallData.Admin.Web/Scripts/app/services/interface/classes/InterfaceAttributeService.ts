module HallData.EMS.Services.InterfaceAttribute {
	"use strict";

	declare var serviceLocation: string;

	export class InterfaceAttributeService extends Service.DeletableService<IInterfaceAttribute> implements IInterfaceAttributeService {

		constructor() {
			super(serviceLocation + "interfaces/attributes/");
			console.log("constructor");
		}

		protected GetUrlPathForKey(interfaceAttribute: IInterfaceAttribute): any {
			console.log("GetUrlPathForKey");
			return interfaceAttribute.InterfaceAttributeId;
		}

	}
} 