/// <reference path="../../libraries/jquery/jquery.d.ts" />
module HallData.Service {
	"use strict";

	export interface IQueryResults<TResult> {
		Results?: TResult[];
		TotalResultsCount?: number;
		AvailableProperties?: string[];
	}

	export interface IQueryResult<TResult> {
		Result: TResult;
		AvailableProperties: string[];
	}

	export enum FilterOperation {
		Equals,
		Like
	}

	export interface IFilterDescriptor {
		Property: string;
		Operation: FilterOperation;
		CompareValue: any;
	}

	export interface IFilterContext {
		Filters: IFilterDescriptor[];
		SearchCriteria: string;
	}

	export enum SortDirection {
		Ascending,
		Descending
	}

	export interface ISortDescriptor {
		Direction: SortDirection;
		Property: string;
	}

	export interface ISortContext {
		Sorts: ISortDescriptor[];
	}

	export interface IPageDescriptor {
		CurrentPage: number;
		PageSize: number;
	}

	export interface IService {
		ServiceLocation: string
	}

	export interface IViewServiceBase extends IService {
		GetViewMany(viewName?: string, filter?: IFilterContext, sort?: ISortContext, page?: IPageDescriptor): JQueryPromise<IQueryResults<any>>;
	}

	export interface IReadOnlyServiceBase<TView> extends IViewServiceBase {
		GetMany(filter?: IFilterContext, sort?: ISortContext, page?: IPageDescriptor, viewName?: string): JQueryPromise<IQueryResults<TView>>;
	}

	export interface IReadOnlyService<TView> extends IReadOnlyServiceBase<TView> {
		Get(id: any, viewName?: string): JQueryPromise<IQueryResult<TView>>;
		GetView(id: any, viewName?: string): JQueryPromise<IQueryResult<any>>;
	}

	export interface IWriteableService<TView> extends IReadOnlyService<TView> {
		Update(view: any, viewName?: string): JQueryPromise<IQueryResult<TView>>;
		Add(view: any, viewName?: string): JQueryPromise<IQueryResult<TView>>;
	}

	export interface IDeleteableService<TView> extends IWriteableService<TView> {
		Delete(id: any): JQueryPromise<void>;
	}

	//export interface IQueryResults<TResult> {
	//	Results: TResult[];
	//	TotalResultsCount: number;
	//	AvailableProperties: string[];
	//}

	//export interface IQueryResult<TResult> {
	//	Result: TResult;
	//	AvailableProperties: string[]
	//}

	//export enum FilterOperation {
	//	Equals,
	//	Like
	//}

	//export interface IFilterDescriptor {
	//	Property: string;
	//	Operation: FilterOperation;
	//	CompareValue: any;
	//}

	//export interface IFilterContext {
	//	Filters: IFilterDescriptor[];
	//	SearchCriteria: string;
	//}

	//export enum SortDirection {
	//	Ascending,
	//	Descending
	//}

	//export interface ISortDescriptor {
	//	Direction: SortDirection;
	//	Property: string;
	//}

	//export interface ISortContext {
	//	Sorts: ISortDescriptor[];
	//}

	//export interface IPageDescriptor {
	//	CurrentPage: number;
	//	PageSize: number;
	//}

	//export interface IService {
	//	ServiceLocation: string
	//}

	//export interface IViewServiceBase extends IService {
	//	GetViewMany(viewName?: string, filter?: IFilterContext, sort?: ISortContext, page?: IPageDescriptor): JQueryPromise<IQueryResults<any>>;
	//}

	//export interface IReadOnlyServiceBase<TView> extends IViewServiceBase {
	//	GetMany(filter?: IFilterContext, sort?: ISortContext, page?: IPageDescriptor, viewName?: string): JQueryPromise<IQueryResults<TView>>;
	//}

	//export interface IReadOnlyService<TKey, TView> extends IReadOnlyServiceBase<TView> {
	//	Get(id: TKey, viewName?: string): JQueryPromise<IQueryResult<TView>>;
	//	GetView(id: TKey, viewName?: string): JQueryPromise<IQueryResult<any>>;
	//}

	//export interface IWriteableService<TKey, TView, TViewForAdd, TViewForUpdate> extends IReadOnlyService<TKey, TView> {
	//	Update(view: TViewForUpdate, viewName?: string): JQueryPromise<IQueryResult<TView>>;
	//	Add(view: TViewForAdd, viewName?: string): JQueryPromise<IQueryResult<TView>>;
	//}

	//export interface IWriteableServiceWithBase<TKey, TView, TViewBase> extends IWriteableService<TKey, TView, TViewBase, TViewBase> { }

	//export interface IDeleteableService<TKey, TView, TViewForAdd, TViewForUpdate> extends IWriteableService<TKey, TView, TViewForAdd, TViewForUpdate> {
	//	Delete(id: TKey): JQueryPromise<void>;
	//}

	//export interface IDeleteableServiceWithBase<TKey, TView, TViewBase> extends IDeleteableService<TKey, TView, TViewBase, TViewBase>, IWriteableServiceWithBase<TKey, TView, TViewBase> {
			
	//}
}