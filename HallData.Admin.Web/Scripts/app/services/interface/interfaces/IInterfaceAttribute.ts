module HallData.EMS.Services.InterfaceAttribute {
	"use strict";

	export interface IInterfaceAttribute {
		Interface?: Interface.IInterface;
		//Type?: Interface.IInterface;
		Name?: string;
		DisplayName?: string;
		IsKey?: boolean;
		IsCollection?: boolean;
		InterfaceAttributeId?: number;
	}
}