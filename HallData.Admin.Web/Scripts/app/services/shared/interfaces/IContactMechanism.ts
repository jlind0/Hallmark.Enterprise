module HallData.EMS.Services {
	"use strict";

	export interface IContactMechanism {
		ContactMechanismGuid?: string;
		MechanismType?: IMechanismType;
		Status?: IStatusType;
		ContactMechanismTypeName?: string;
		ContactMechanismType?: IContactMechanismType;
		PrimaryRelationshipStatus?: IStatusType;
	}
}