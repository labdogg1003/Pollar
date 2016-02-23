
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.MobileServices;

namespace Pollar
{
	[Activity (Label = "AnswerViewController")]			
	public class AnswerViewController : Activity
	{
		static string Url = "https://pollzit.azure-mobile.net/";
		static string Code = "whXGrRTLuaGVmITvLBWYYvSKXNhyGM32";

		List<Question> questions = new List<Question>();

		MobileServiceClient MobileService = new MobileServiceClient(Url,Code);
		public IMobileServiceTable<Question> QuestionTable;

		Question question;
		Button btnAns1;
		Button btnAns2;
		Button btnAns3;
		Button btnAns4;
		List<Button> answers = new List<Button>();
		int i = 0;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//Remove Title Bar
			RequestWindowFeature(WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Answer);

			// Get our question from the layout resource
			TextView txtQuestion = FindViewById<TextView> (Resource.Id.txtQuestion);

			// Get our button from the layout resource
			btnAns1 = FindViewById<Button> (Resource.Id.btnAns1);
			btnAns2 = FindViewById<Button> (Resource.Id.btnAns2);
			btnAns3 = FindViewById<Button> (Resource.Id.btnAns3);
			btnAns4 = FindViewById<Button> (Resource.Id.btnAns4);
		
			var q = Intent.GetStringExtra("question");
			question = JsonConvert.DeserializeObject<Question> (q);

			txtQuestion.Text = question.question;

			answers.Add (btnAns1);
			answers.Add (btnAns2);
			answers.Add (btnAns3);
			answers.Add (btnAns4);

			foreach (Button b in answers)
			{
				b.Text = "";
			}

			foreach (Answer a in question.answers)
			{
				answers [i].Text = a.answer;
				i++;
				Console.WriteLine("answer added : " + a.answer);
			}

			foreach (Button b in answers)
			{
				if (b.Text == "")
					b.Enabled = false;
			}

			checkForAnswers ();
		}

		private void checkForAnswers()
		{
			btnAns1.Click += delegate {
				Console.WriteLine("button 1 clicked");
				((Answer)question.answers [0]).count++;
				Console.WriteLine (question.answers [0].answer + " count = "
					+ question.answers [0].count);
				updateQuestion();

				var activity = new Intent (this, typeof(MainActivity));
				StartActivity (activity);

			};

			btnAns2.Click += delegate {
				((Answer)question.answers [1]).count++;
				Console.WriteLine (question.answers [1].answer + " count = "
					+ question.answers [1].count);
				updateQuestion();

				var activity = new Intent (this, typeof(MainActivity));
				StartActivity (activity);
			};

			btnAns3.Click += delegate {
				((Answer)question.answers [2]).count++;
				Console.WriteLine (question.answers [2].answer + " count = " 
					+ question.answers [2].count);
				updateQuestion();

				var activity = new Intent (this, typeof(MainActivity));
				StartActivity (activity);
			};

			btnAns4.Click += delegate {
				((Answer)question.answers [3]).count++;
				Console.WriteLine (question.answers [3].answer + " count = " 
					+ question.answers [3].count);
				updateQuestion();

				var activity = new Intent (this, typeof(MainActivity));
				StartActivity (activity);
			};
		}

		public async void updateQuestion()
		{
			question.convertAnswersToJson();

			JObject jo = new JObject();

			jo.Add("id", question.RoomID);
			jo.Add("question", question.question);
			jo.Add("answers", question.jsonAnswers);
			MobileService.GetTable("Question").UpdateAsync(jo);
		}

	}
}

