
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
using Microsoft.WindowsAzure.MobileServices;
using BarChart;
using Android.Graphics;
using Newtonsoft.Json;

namespace Pollar
{
	[Activity (Label = "QuestionDataController")]			
	public class QuestionDataController : Activity
	{
		static string Url = "https://pollzit.azure-mobile.net/";
		static string Code = "whXGrRTLuaGVmITvLBWYYvSKXNhyGM32";

		List<Question> questions = new List<Question>();

		MobileServiceClient MobileService = new MobileServiceClient(Url,Code);
		public IMobileServiceTable<Question> QuestionTable;

		Question question;
		string _questionID;
		View vwChart;
		BarChartView barChart;
		private QuestionJsonService questionService = new QuestionJsonService (System.IO.Path.Combine(
			Android.OS.Environment.ExternalStorageDirectory.Path,
			"PollarApp"));

		static readonly BarModel[] TestData = new BarModel[] {
			new BarModel () { Value =   -1f, Legend = "0", Color = Color.Red },
			new BarModel () { Value =    2f, Legend = "1" },
			new BarModel () { Value =    0f, Legend = "2" },
			new BarModel () { Value =    1f, Legend = "3" },
			new BarModel () { Value =   -1f, Legend = "4", Color = Color.Red },
			new BarModel () { Value =    1f, Legend = "5" },
			new BarModel () { Value =   -1f, Legend = "6", Color = Color.Red },
			new BarModel () { Value =    2f, Legend = "7" },
			new BarModel () { Value = -0.1f, Legend = "8", Color = Color.Red }
		};

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//Remove Title Bar
			RequestWindowFeature(WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.QuestionData);

			TextView txtQuestion = FindViewById<TextView> (Resource.Id.txtQuestion);
			Button btnReturn = FindViewById<Button> (Resource.Id.btnReturn);
			barChart = FindViewById<BarChartView> (Resource.Id.barChart1);


			btnReturn.Click += delegate {
				var activity = new Intent (this, typeof(MyQuestionController));
				StartActivity (activity);
			};

			//Recieve previous intent data
			_questionID = Intent.GetStringExtra ("selectedItem") ?? "Data not available";

			question = JsonConvert.DeserializeObject<Question> (_questionID);

			//Show Our Question
			txtQuestion.Text = question.question;

			barChart.ItemsSource = GenerateData ();
		}
			
		public List<BarModel> GenerateData ()
		{
			var models = new List<BarModel> ();
			int total = 0;

			for (var i = 0; i < question.answers.Count; i += 1) 
			{
				Console.WriteLine("Count : " + question.answers [i].count);
				models.Add (new BarModel () {
					Value = question.answers [i].count,
					Color = Color.White,
					Legend = question.answers [i].answer
				});
				total+= question.answers[i].count;
			}

			models.Add (new BarModel () { 
				Value = total, 
				Color = Color.White,
				Legend = "Total" 
			});

			return models;
		}
	}
}
