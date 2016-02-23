
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Pollar
{
	[Activity (Label = "QuestionData")]			
	public class QuestionData
	{
		public static readonly IQuestionDataService Service =
			new QuestionJsonService(
				System.IO.Path.Combine(
					Android.OS.Environment.ExternalStorageDirectory.Path,
					"PollarApp"));
		
		
	}
}

