using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;

namespace Pollar
{
	[Activity (Label = "QuestionListActivity")]
	public class QuestionListActivity : Activity
	{
		ListView _questionListView;
		QuestionListViewAdapter _adapter;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.MyQuestions);

			_questionListView = FindViewById<ListView> (Resource.Id.lstQuestions);
			_adapter = new QuestionListViewAdapter (this);
			_questionListView.Adapter = _adapter;
			_questionListView.ItemClick += QuestionClicked;

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

			Criteria criteria = new Criteria ();
			criteria.Accuracy = Accuracy.NoRequirement;
			criteria.PowerRequirement = Power.NoRequirement;
		}

		protected override void OnPause ()
		{
			base.OnPause ();
		}

		/*public override bool OnOptionsItemSelected (IMenuItem item)
		{
			/*switch (item.ItemId)
			{
			case Resource.Id.actionNew:
				StartActivity (typeof(QuestionDataController));
				return true;

			case Resource.Id.actionRefresh: 
				QuestionData.Service.RefreshCache ();
				_adapter.NotifyDataSetChanged ();
				return true;

			default :
				return base.OnOptionsItemSelected(item);
			}*/
		//}

		protected void QuestionClicked(object sender, ListView.ItemClickEventArgs e)
		{
			// setup the intent to pass the POI id to the detail view
			Intent QuestionDetailIntent = new Intent (this, typeof(QuestionDataController));
			QuestionDetailIntent.PutExtra ("questioniId", (int) e.Id);
			StartActivity (QuestionDetailIntent);
		}

		public void OnProviderDisabled (string provider)
		{
		}

		public void OnProviderEnabled (string provider)
		{
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
		}
	}
}




