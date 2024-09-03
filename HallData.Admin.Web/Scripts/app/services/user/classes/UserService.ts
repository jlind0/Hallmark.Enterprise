module HallData.EMS.Services.User {
	"use strict";
	
	declare var serviceLocation: string;

	export class UserService extends Party.PartyService<IUser> implements IUserService {

		constructor() {
			super(serviceLocation + "users/");
			console.log("constructor");
		}

		protected GetUrlPathForKey(user: IUser): any {
			console.log("GetUrlPathForKey");
			return user.PartyGuid;
		}
	}
} 