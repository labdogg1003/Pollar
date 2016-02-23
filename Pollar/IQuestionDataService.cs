
using System;
using System.Collections.Generic;

namespace Pollar
{
	public interface IQuestionDataService
	{
		IReadOnlyList<Question> Questions { get; }
		void RefreshCache();
		Question GetQuestion (int id);
		void SaveQuestion(Question questions);
		void DeleteQuestion(Question questions);
	}
}

