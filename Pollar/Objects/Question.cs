using System;
using System.Collections;
using Newtonsoft.Json;
using System.Linq; 
using Foundation;
using System.CodeDom.Compiler;
using UIKit;
using Microsoft.WindowsAzure.MobileServices;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace Pollzit
{
	public class Question
	{
		private static string Url = "https://pollzit.azure-mobile.net/";
		private static string Code = "whXGrRTLuaGVmITvLBWYYvSKXNhyGM32";
		List<Question> questions;

		MobileServiceClient MobileService = new MobileServiceClient(Url,Code);
		private IMobileServiceSyncTable<Question> QuestionTable;

		[JsonProperty(PropertyName = "question")]
		public string question { get; set;}

		[JsonProperty(PropertyName = "id")]
		public string RoomID {get; set;}

		[JsonProperty(PropertyName = "answers")]
		public String jsonAnswers;

		[JsonProperty(PropertyName = "answersArray")]
		public List<Answer> answers = new List<Answer>();

		private const int ROOM_ID_LENGTH = 4;

		public Question()
		{
			//When A Question Is Created We Generate A New ID, We only sync the ID if we Push to the Cloud
			RoomID = RandomString (ROOM_ID_LENGTH);
		}

		public Question(string RoomID)
		{
			//When A Question Is Created We Use A Predetermined ID, We only sync the ID if we Push to the Cloud
			this.RoomID = RoomID;
		}

		private string RandomStringWithDatabaseCheck(int length)
		{
			//Generate Our First String
			string RoomID = RandomString(length);

			//Check Our Database For The String
			retrieveQuestion (RoomID);

			//While The list has more than 0 questions
			while (questions.Count > 0) 
			{
				//Generate Another String
				RoomID = RandomString(length);

				//Check Our Database For The String
				retrieveQuestion (RoomID);
			}

			//If It Doesn't Match Any Other Questions This Is Our New RoomID
			return RoomID;
		}

		private static string RandomString(int length)
		{
			//Our List Of Characters
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

			//Using Our List Of Characters Generate A String on Length length
			var random = new Random();
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}

		private async void retrieveQuestion(string room_id)
		{
			//Reset The List
			questions = new List<Question> ();

			//Get Any Questions That Match Our Room ID
			var qs = await QuestionTable
				.Where (question => question.RoomID == room_id)
				.ToListAsync ();
		}

		public void convertAnswersToJson()
		{
			this.jsonAnswers = JsonConvert.SerializeObject (this.answers);
		}

		public void convertJsonToAnswers()
		{
			List<Answer> list = JsonConvert.DeserializeObject<List<Answer>> (this.jsonAnswers);
			answers = list;
		}


	}
}

