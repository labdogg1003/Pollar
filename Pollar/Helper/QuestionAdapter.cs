
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
	[Activity (Label = "QuestionAdapter")]			
	public class QuestionAdapter : BaseAdapter
	{
		List<Question> _questionList;
		Activity _activity;

		public QuestionAdapter (Activity activity, IReadOnlyList<Question> questions)
		{
			_activity = activity;
			FillQuestions(questions);
		}

		void FillQuestions (IReadOnlyList<Question> questions)
		{
			_questionList = (List<Question>)questions;
			Console.WriteLine ("Questions Retrieved : " + _questionList.Count);
		}

		public override int Count {
			get { return _questionList.Count; }
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null)
			{ // otherwise create a new one
				view = _activity.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
			}
			view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = _questionList[position].RoomID;
			return view;
		}
	}

}