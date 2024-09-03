module HallData.EMS.Services.Question {
	"use strict";

	export interface IQuestionServiceGeneric<TCustomer extends IQuestion> extends Party.IPartyService<TCustomer> {

	}

	export interface IQuestionService extends IQuestionServiceGeneric<IQuestion> {

	}
}