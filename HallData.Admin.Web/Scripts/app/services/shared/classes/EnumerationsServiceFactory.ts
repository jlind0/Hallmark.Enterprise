module HallData.EMS.Services {

	export class EnumerationsServiceFactory {

		private static factory: () => IEnumerationsService;

		public static Initialize(factory: () => IEnumerationsService): void {
			console.log("Initialize");
			this.factory = factory;
		}

		public static Create(): IEnumerationsService {
			console.log("Create");
			return this.factory();
		}
	}
} 