
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
using Microsoft.WindowsAzure.MobileServices;

namespace Pollar
{
	[Activity (Label = "MyQuestionController")]
	
	public class MyQuestionController : Activity
	{
		ListView _questionListView;
		QuestionListViewAdapter _adapter;
		Question question;

		static string Url = "https://pollzit.azure-mobile.net/";
		static string Code = "whXGrRTLuaGVmITvLBWYYvSKXNhyGM32";

		List<Question> questions = new List<Question>();

		MobileServiceClient MobileService = new MobileServiceClient(Url,Code);
		public IMobileServiceTable<Question> QuestionTable;
		private QuestionJsonService questionService = new QuestionJsonService (System.IO.Path.Combine(
			Android.OS.Environment.ExternalStorageDirectory.Path,
			"PollarApp"));

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView (Resource.Layout.MyQuestions);

			_questionListView = FindViewById<ListView> (Resource.Id.lstQuestions);

			_adapter = new QuestionListViewAdapter (this);
			_questionListView.Adapter = _adapter;
			_questionListView.ItemClick += QuestionClicked;
			_adapter.NotifyDataSetChanged ();

			//Get Our Items from Resources
			Button btnBack = FindViewById<Button> (Resource.Id.btnBack);

			//Check Back Button
			btnBack.Click += delegate 
			{
				var activity = new Intent (this, typeof(MainActivity));
				StartActivity (activity);
			};
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			_adapter.NotifyDataSetChanged ();
		}

		protected override void OnPause ()
		{
			base.OnPause ();
		}

		protected void QuestionClicked(object sender, ListView.ItemClickEventArgs e)
		{

			//Get Question From the list
			question = questionService.GetQuestion ((int)e.Id);

			retrieveQuestion (question.RoomID);
		}

		public void OnProviderDisabled (string provider)
		{
		}

		public void OnProviderEnabled (string provider)
		{
		}

		public async void retrieveQuestion(string room_id)
		{

			//Get Our Table From Azure
			QuestionTable = MobileService.GetTable<Question>();

			try
			{
				questions = await QuestionTable
					.Where (question => question.RoomID == room_id)
					.ToListAsync();

				Console.WriteLine("Questions : " + questions.Count);

				questions [0].convertJsonToAnswers ();

				if(questions.Count == 0)
				{
					Console.WriteLine("There Are No Questions With That Room ID");
				}
				else
				{
					question = questions [0];
					Console.WriteLine("Ready To Go");

					// setup the intent to pass the POI id to the detail view
					Intent QuestionDetailIntent = new Intent (this, typeof(QuestionDataController));

					QuestionDetailIntent.PutExtra("selectedItem", JsonConvert.SerializeObject(question).ToString() );

					StartActivity (QuestionDetailIntent);
				}
			}
			catch(Exception e) 
			{
				Console.WriteLine (e);
			}
		}

			
	}
}

