
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
using Android.Net;
using Newtonsoft.Json.Linq;

namespace Pollar
{
	[Activity (Label = "AskViewController")]			
	public class AskViewController : Activity
	{
		static string Url = "https://pollzit.azure-mobile.net/";
		static string Code = "whXGrRTLuaGVmITvLBWYYvSKXNhyGM32";
		List<EditText> answerFields = new List<EditText> ();
		Question question;
		EditText fldQuestion;
		TextView txtException;
		TextView txtRoomID; 
		MobileServiceClient MobileService = new MobileServiceClient(Url,Code);
		private IMobileServiceTable QuestionTable;
		private QuestionJsonService questionService = new QuestionJsonService (System.IO.Path.Combine(
			Android.OS.Environment.ExternalStorageDirectory.Path,
			"PollarApp"));
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//Remove Title Bar
			RequestWindowFeature(WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Ask);

			// Get our question from the layout resource
			fldQuestion = FindViewById<EditText> (Resource.Id.fldQuestion);

			// Get our answers from the layout resource
			EditText fldAns1 = FindViewById<EditText> (Resource.Id.fldAns1);
			EditText fldAns2 = FindViewById<EditText> (Resource.Id.fldAns2);
			EditText fldAns3 = FindViewById<EditText> (Resource.Id.fldAns3);
			EditText fldAns4 = FindViewById<EditText> (Resource.Id.fldAns4);

			// Get everything else from the layout resource
			Button btnAsk = FindViewById<Button> (Resource.Id.btnAsk);
			Button btnBack = FindViewById<Button> (Resource.Id.btnBack);
			txtException = FindViewById<TextView> (Resource.Id.txtException);
			txtRoomID = FindViewById<TextView> (Resource.Id.txtRoomID);

			//Add Our Fields To Our list
			answerFields.Add(fldAns1);
			answerFields.Add(fldAns2);
			answerFields.Add(fldAns3);
			answerFields.Add(fldAns4);
			Console.WriteLine (answerFields.Count);

			//Reset Our Warning Label
			txtException.Text = "";

			//Handle Question Submission
			btnAsk.Click += delegate 
			{
				//Check if network is available
				if(DetectNetwork())
				{
					//Connect To Our Database
					CurrentPlatform.Init();

					//Ask Our Question
					askQuestion();
				}
				else
				{
					Toast.MakeText (this, "No Network Detected", ToastLength.Long).Show();
				}
			};

			//Handle Back Button
			btnBack.Click += delegate 
			{
				var activity = new Intent (this, typeof(MainActivity));
				StartActivity (activity);
			};
		}

		public async void askQuestion()
		{
			int i = 0;

			foreach (EditText field in answerFields) 
			{
				//Count How Many Answers There Are
				if (!(field.Text == "")) 
				{
					i++;
					Console.WriteLine (i);
				}
			}	

			if (i < 2)
			{
				//If there are less than 2 answers we don't have a poll
				txtException.Text = "You Need To Have 2 Or More Answers";
			} 
			else if (fldQuestion.Text.Equals("")) 
			{
				//There Must Be A Question
				txtException.Text = "You Do Not Have A Question";
			} 
			else 
			{
				
					//Console.WriteLine (i);

					//Reset Our Exception Text
					txtException.Text = "";

					question = new Question ();
					Answer ans;

					//Loop Through The Answer Fields
					foreach (EditText field in answerFields)
				{
						//Check If Field Is Empty
						if (!(field.Text == "")) 
					{
							ans = new Answer ();
							Console.WriteLine ("Field Text : " + field.Text);
							ans.answer = field.Text;
							Console.WriteLine ("Add answer to answer list");

							try {
								//Add Each Answer To The Question
								question.answers.Add (ans);
							} catch (Exception e) {
								Console.WriteLine (e);
							}
						}
					}
					Console.WriteLine ("Answer Extraction Passed");

					try 
					{
						question.question = fldQuestion.Text;

						JObject jo = new JObject ();

						//Show The Room Code For The Question
						txtRoomID.Text = question.RoomID;

						//Convert The Answers To Json
						question.convertAnswersToJson ();

						//Send Our Question To Azure
						jo.Add ("id", question.RoomID);
						jo.Add ("question", question.question);
						jo.Add ("answers", question.jsonAnswers);
						await MobileService.GetTable ("Question").InsertAsync (jo);

						//Save Our Question To File : This Will Help Us Retrieve Questions Later
						questionService.SaveQuestion (question);
						questionService.RefreshCache();

					} catch (Exception e) {
						Console.WriteLine (e);
					}
					Console.WriteLine ("Question Submission Passed");
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

