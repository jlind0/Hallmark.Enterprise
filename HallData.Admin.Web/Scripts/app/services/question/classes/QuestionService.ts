module HallData.EMS.Services.Question {
	"use strict";

	declare var serviceLocation: string;
	
	export class QuestionService extends Party.PartyService<IQuestion> implements IQuestionService {

		constructor() {
			super(serviceLocation + "customers/");
			console.log("constructor");
		}

		protected GetUrlPathForKey(question: IQuestion): any {
			console.log("GetUrlPathForKey");
			return question.PartyGuid + (Util.String.isNullOrWhitespace(question.CustomerOfPartyGuid) ? "" : "/" + question.CustomerOfPartyGuid);
		}
	}
} 