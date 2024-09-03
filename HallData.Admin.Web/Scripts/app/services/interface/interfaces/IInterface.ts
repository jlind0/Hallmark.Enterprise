module HallData.EMS.Services.Interface {
	import InterfaceAttribute = HallData.EMS.Services.InterfaceAttribute.IInterfaceAttribute;
	"use strict";

	export interface IInterface {
		Name?: string;
		DisplayName?: string;
		RelatedInterfaces?: IInterface[];
		Attributes?: InterfaceAttribute[];
		InterfaceId?: number;
		ticked?: boolean;
	}
}