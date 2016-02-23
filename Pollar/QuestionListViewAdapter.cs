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
using Android.Locations;
using Android.Graphics;

namespace Pollar
{
	public class QuestionListViewAdapter : BaseAdapter<Question>
	{
		private readonly Activity _context;
		private QuestionJsonService questionService = new QuestionJsonService (System.IO.Path.Combine(
			Android.OS.Environment.ExternalStorageDirectory.Path,
			"PollarApp"));


		public QuestionListViewAdapter(Activity context)
		{
			_context = context;
		}

		public override int Count
		{
			get { return  questionService.Questions.Count(); }
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Question this[int position]
		{
			get { return QuestionData.Service.Questions[position]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (view == null)
				view = _context.LayoutInflater.Inflate(Resource.Layout.QuestionCellView, null);

			QuestionData.Service.RefreshCache ();
			Question q = QuestionData.Service.Questions [position];
			Console.WriteLine ("Question Added");

			view.FindViewById<TextView> (Resource.Id.textViewQuestion).Text = q.question;
			view.FindViewById<TextView> (Resource.Id.textViewRoomID).Text = q.RoomID;

			return view;
		}
	}
}

