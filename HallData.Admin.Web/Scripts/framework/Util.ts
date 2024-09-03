/// <reference path="../libraries/jquery/jquery.d.ts" />
module HallData.Util {
	export class String {
		public static isNullOrWhitespace(input: string): boolean {
			if (typeof input === "undefined" || input === null) return true;
			return input.replace("/\sg", '').length < 1;
		}

		public static emptyIfNullOrWhitespace(input: string): string {
			if (this.isNullOrWhitespace(input))
				return "";
			return input;
		}
	}

	export class Obj {
		public static isNullOrUndefined(input: any): boolean {
			return typeof input === "undefined" || input === null;
		}
        public static getValue(values: Util.IKeyValuePair[], key: string): any {
            for (var i = 0; i < values.length; i++) {
                if (key == values[i].Key)
                    return values[i].Value;
            }
            return null;
        }
		//http://typescript.codeplex.com/discussions/463561
		public static getName(ent: any): string {
			if (typeof ent == "string") return ent;

			if (ent.constructor && ent.constructor.name != "Function") {
				return ent.constructor.name || (ent.toString().match(/function (.+?)\(/) || [, ''])[1];
			} else {
				return ent.name;
			}
		}

		//http://stackoverflow.com/questions/4775722/check-if-object-is-array
		public static isArray(ent: any): boolean {
			return Object.prototype.toString.call(ent) === '[object Array]';
		}

		public static createHallDataInstance<T>(name: string, ...args: any[]): T {
			var classPath = name.split(".");
			var val: any = HallData;
			for (var m in HallData) {
				if (m == classPath[0]) {
					val = val[m];
					var previousVal = val;
					for (var i = 1; i < classPath.length; i++) {
						for (var child in val) {
							if (child == classPath[i]) {
								previousVal = val;
								val = val[classPath[i]];
								break;
							}
						}
						if (val === previousVal)
							return null;
					}
					break;
				}
			}
			var instance = Object.create(val.prototype);
			if (instance !== null && instance !== undefined) {
				instance.constructor.apply(instance, args);
			}
			return <T>instance;
		}
	}

	export class DateTime {
		private static jsonDateRegex = new RegExp("(\\d{4})-(\\d{2})-(\\d{2})T(\\d{2}):(\\d{2}):(\\d{2}(?:\\.\\d*))(?:Z|(\\+|-)([\\d|:]*))?$");
		public static getDateForJsonString(value: string): Date {
			var arr = value && this.jsonDateRegex.exec(value);
			if (arr) {
				return new Date(value);
			}
			return null;
		}
	}

	export class Service {

		public static Get<TResult>(url: string, passCredientials?: boolean): JQueryPromise<TResult> {
			return <JQueryPromise<TResult>>$.ajax({
				type: "GET",
				url: url,
				beforeSend: request => this.addSessionIdHeader(request),
				xhrFields: {
					withCredentials: passCredientials
				}
			});
		}

		public static GetQueryResults<TResult>(baseUrl: string, filter: HallData.Service.IFilterContext, sort: HallData.Service.ISortContext, page: HallData.Service.IPageDescriptor): JQueryPromise<HallData.Service.IQueryResults<TResult>> {
			var url: string = baseUrl + "?time=" + new Date().getTime();

			if (!Util.Obj.isNullOrUndefined(filter)) {
				url += "&filter=" + encodeURIComponent(JSON.stringify(new HallData.Service.FilterContextDTO(filter)));
			}
				
			if (!Util.Obj.isNullOrUndefined(sort)) {
				url += "&sort=" + encodeURIComponent(JSON.stringify(new HallData.Service.SortContextDTO(sort)));
			}
				
			if (!Util.Obj.isNullOrUndefined(page)) {
				url += "&page=" + encodeURIComponent(JSON.stringify(page));
			}
				
			return this.Get<HallData.Service.IQueryResults<TResult>>(url);
		}

		public static GetQueryResult<TResult>(baseUrl: string): JQueryPromise<HallData.Service.IQueryResult<TResult>> {
			var url = baseUrl + "?time=" + new Date().getTime();
			return this.Get<HallData.Service.IQueryResult<TResult>>(url);
		}

		public static Put<TResult>(data: any, url: string, passCredientials?: boolean): JQueryPromise<TResult> {
			return <JQueryPromise<TResult>>$.ajax({
				type: "PUT",
				url: url,
				data: JSON.stringify(data),
				beforeSend: request => this.addSessionIdHeader(request),
				contentType: "application/json; charset=utf-8",
				dataType: 'json',
				xhrFields: {
					withCredentials: passCredientials
				}
			});
		}

		public static Post<TResult>(data: any, url: string, passCredientials?: boolean): JQueryPromise<TResult> {
			return <JQueryPromise<TResult>>$.ajax({
				type: "POST",
				url: url,
				beforeSend: request => this.addSessionIdHeader(request),
				data: JSON.stringify(data),
				contentType: "application/json; charset=utf-8",
				dataType: 'json',
				xhrFields: {
					withCredentials: passCredientials
				}
			});
		}

		public static Delete<TResult>(url: string, passCredientials?: boolean): JQueryPromise<TResult> {
			return <JQueryPromise<TResult>>$.ajax({
				type: "DELETE",
				url: url,
				beforeSend: request => this.addSessionIdHeader(request),
				xhrFields: {
					withCredentials: passCredientials
				}
			});
		}
		protected static addSessionIdHeader(request: JQueryXHR): void {
			console.log(JSON.stringify(request));
			var sessionId = CookieManager.getSessionId();
			if (sessionId)
				request.setRequestHeader("session.id", sessionId);
		}
	}
	//http://www.sitepoint.com/how-to-deal-with-cookies-in-javascript/
	export class CookieManager {
		public static createCookie(name: string, value: any, expires?: number | Date, path?: string, domain?: string): void {
			var cookie = name + "=" + encodeURIComponent(value) + ";";
			if (expires) {
				// If it's a date
				if (<any>expires instanceof Date) {
					// If it isn't a valid date
					if (isNaN((<Date>expires).getTime()))
						expires = new Date();
				}
				else
					expires = new Date(new Date().getTime() + <number>expires * 1000 * 60 * 60 * 24);

				cookie += "expires=" + (<Date>expires).toUTCString() + ";";
			}

			if (path)
				cookie += "path=" + path + ";";
			if (domain)
				cookie += "domain=" + domain + ";";

			document.cookie = cookie;
		}

		public static getCookie(name: string): string {
			var regexp = new RegExp("(?:^" + name + "|;\s*" + name + ")=(.*?)(?:;|$)", "g");
			var result = regexp.exec(document.cookie);
			return (result === null) ? null : result[1];
		}

		public static deleteCookie(name: string, path?: string, domain?: string): void {
			// If the cookie exists
			if (this.getCookie(name))
				this.createCookie(name, "", null, path, domain);
		}

		public static setSessionId(sessionId: string): void {
			this.createCookie("sessionId", sessionId);
		}

		public static getSessionId(): string {
			return this.getCookie("sessionId");
		}

		public static deleteSessionId(): void {
			this.deleteCookie("sessionId");
		}

		public static setIsWindowsAuth(isWindowsAuth: boolean) {
			this.createCookie("isWindowsAuth", isWindowsAuth);
		}

		public static getIsWindowsAuth(): boolean {
			return this.getCookie("isWindowsAuth") !== 'false';
		}
	}
}