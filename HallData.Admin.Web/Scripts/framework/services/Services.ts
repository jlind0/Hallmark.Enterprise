/// <reference path="../../libraries/jquery/jquery.d.ts" />
module HallData.Service {
	"use strict";

	export class Service implements IService {
		constructor(public ServiceLocation: string) {
			console.log("constructor");
		}
	}

	export class ReadOnlyService<TView> extends Service implements IReadOnlyService<TView> {

		GetMany(filter?: IFilterContext, sort?: ISortContext, page?: IPageDescriptor, viewName?: string): JQueryPromise<IQueryResults<TView>> {
			console.log("GetMany");
			return Util.Service.GetQueryResults<TView>(this.ServiceLocation + (Util.String.isNullOrWhitespace(viewName) ? "" : "TypedView/" + viewName), filter, sort, page);
		}

		Get(id: any, viewName?: string): JQueryPromise<IQueryResult<TView>> {
			console.log("Get");
			//return Util.Service.GetQueryResult<TView>(this.ServiceLocation + this.GetUrlPathForKey(id) + (Util.String.isNullOrWhitespace(viewName) ? "/" : "/TypedView/" + viewName));
			return Util.Service.GetQueryResult<TView>(this.ServiceLocation + this.GetUrlPathForKey(id));
		}

		GetViewMany(viewName?: string, filter?: IFilterContext, sort?: ISortContext, page?: IPageDescriptor): JQueryPromise<IQueryResults<any>> {
			console.log("GetViewMany");
			return Util.Service.GetQueryResults<any>(this.ServiceLocation + "View/" + Util.String.emptyIfNullOrWhitespace(viewName), filter, sort, page);
		}

		GetView(id: any, viewName?: string): JQueryPromise<IQueryResult<any>> {
			console.log("GetView");
			//return Util.Service.GetQueryResult<any>(this.ServiceLocation + this.GetUrlPathForKey(id) + "/View/" + Util.String.emptyIfNullOrWhitespace(viewName));
			return Util.Service.GetQueryResult<any>(this.ServiceLocation + this.GetUrlPathForKey(id));
		}

		protected GetUrlPathForKey(key: any): any {
			console.log("GetUrlPathForKey");
			return key;
		}
	}

	export class WriteableService<TView> extends ReadOnlyService<TView> implements IWriteableService<TView> {

		public Update(view: any, viewName?: string): JQueryPromise<IQueryResult<TView>> {
			console.log("Update");
			//return Util.Service.Put<IQueryResult<TView>>(view, this.ServiceLocation + (Util.String.isNullOrWhitespace(viewName) ? "" : "TypedView/" + viewName));
			return Util.Service.Put<IQueryResult<TView>>(view, this.ServiceLocation);
		}

		public Add(view: any, viewName?: string): JQueryPromise<IQueryResult<TView>> {
			console.log("Add");
			//return Util.Service.Post<IQueryResult<TView>>(view, this.ServiceLocation + (Util.String.isNullOrWhitespace(viewName) ? "" : "TypedView/" + viewName));
			return Util.Service.Post<IQueryResult<TView>>(view, this.ServiceLocation);
		}
	}

	export class DeletableService<TView> extends WriteableService<TView> implements IDeleteableService<TView> {

		public Delete(id: any): JQueryPromise<void> {
			console.log("Delete");
			return Util.Service.Delete<void>(this.ServiceLocation + this.GetUrlPathForKey(id) + "/");
		}
	}
}