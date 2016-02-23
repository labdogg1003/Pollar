using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Android.Net;
using Newtonsoft.Json;


namespace Pollar
{
	[Activity (Label = "Pollar", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		string room_id;

		static string Url = "https://pollzit.azure-mobile.net/";
		static string Code = "whXGrRTLuaGVmITvLBWYYvSKXNhyGM32";

		List<Question> questions = new List<Question>();

		MobileServiceClient MobileService = new MobileServiceClient(Url,Code);
		public IMobileServiceTable<Question> QuestionTable;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//Remove Title Bar
			RequestWindowFeature(WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource
			Button btnEnter = FindViewById<Button> (Resource.Id.btnEnter);
			Button btnAsk = FindViewById<Button> (Resource.Id.btnAsk);
			Button btnMine = FindViewById<Button> (Resource.Id.btnMine);

			// Get our Text field from the layout resource
			EditText fldRoomID = FindViewById<EditText> (Resource.Id.fldRoomID);

			//Check Enter Button
			btnEnter.Click += delegate {

				room_id = fldRoomID.Text.ToUpper();
				Console.WriteLine("Enter button clicked");

				//Check if network is available
				if(DetectNetwork())
				{
					//Connect To Our Database
					CurrentPlatform.Init();

					//Retrieve Our Question
					retrieveQuestion(room_id);


				}
				else
				{
					Toast.MakeText (this, "No Network Detected", ToastLength.Long).Show();
				}


			};

			//Check Ask Button
			btnAsk.Click += delegate {

				//Check if network is available
				if(DetectNetwork())
				{
					var activity = new Intent (this, typeof(AskViewController));
					StartActivity (activity);
				}
				else
				{
					Toast.MakeText (this, "No Network Detected", ToastLength.Long).Show();
				}
			};

			//Check Mine Button
			btnMine.Click += delegate {
				var activity = new Intent (this, typeof(MyQuestionController));
				StartActivity (activity);
			};
		}

		public async void retrieveQuestion(string room_id)
		{

			//Get Our Table From Azure
			QuestionTable = MobileService.GetTable<Question>();

			try
			{
				questions = await QuestionTable
					.Where (question => question.RoomID == room_id)
					.ToListAsync ();

				Console.WriteLine("Questions : " + questions.Count);

				questions [0].convertJsonToAnswers ();

				if(questions.Count == 0)
				{
					Console.WriteLine("There Are No Questions With That Room ID");
				}
				else
				{
					var activity = new Intent (this, typeof(AnswerViewController));
					activity.PutExtra("question",JsonConvert.SerializeObject(questions[0]));

					StartActivity (activity);
				}
			}
			catch(Exception e)
			{
				Console.WriteLine ("Nothing was entered or the room does not exist :");
				Toast.MakeText (this, "No Questions With That Room ID", ToastLength.Long).Show();
				Console.WriteLine (e);
			}

		}

		public bool DetectNetwork()
		{
			ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
			NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;

			return (activeConnection != null) && activeConnection.IsConnected;
		}
	}
}


