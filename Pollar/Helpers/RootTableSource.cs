using System;
using UIKit;
using Foundation;
using System.Collections.Generic;

namespace Pollzit
{
	public class RootTableSource : UITableViewSource
	{
		// there is NO database or storage of Tasks in this example, just an in-memory List<>
		List<Question> tableItems;
		QuestionJsonService dataService = new QuestionJsonService (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments));
		private UITableViewController parentController;
		string cellIdentifier = "taskcell"; // set in the Storyboard

		public RootTableSource (List<Question> items, UITableViewController parent)
		{
			tableItems = items;
			this.parentController = parent;
		}
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return tableItems.Count;
		}
		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			// in a Storyboard, Dequeue will ALWAYS return a cell,
			CustomCellView cell = tableView.DequeueReusableCell (cellIdentifier) as CustomCellView;
			//UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);

			Question question = ((Question)tableItems [indexPath.Row]);
			Console.WriteLine (question.question);
			if (cell != null) {
				cell.UpdateCell (question.RoomID, question.question);
			} else
				Console.WriteLine ("cell is null");
			//Converts our table item to a DataSet
			// now set the properties as normal
			//cell.TextLabel.Text = tableItems[indexPath.Row].dataSetName;
			//cell.ImageView.Image = UIImage.FromFile (System.IO.Path.Combine (dataService._storagePath, dataSet.dataSetName + "Data" + dataSet.Id + ".jpg"));
			//tableView.RowHeight = 40;
			return cell;
		}
		public Question GetItem(int id)
		{
			return tableItems[id];
		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
		{
			switch (editingStyle) {
			case UITableViewCellEditingStyle.Delete:
				// remove the item from the underlying data source
				Question question = ((Question)tableItems [indexPath.Row]);
				dataService.DeleteQuestion (question);
				dataService.RefreshCache ();

				//Remove From The Table
				tableItems.RemoveAt (indexPath.Row);
				// delete the row from the table
				tableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
				break;
			case UITableViewCellEditingStyle.None:
				Console.WriteLine ("CommitEditingStyle:None called");
				break;
			}
		}
		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return true; // return false if you wish to disable editing for a specific indexPath or for all rows
		}
		public override string TitleForDeleteConfirmation (UITableView tableView, NSIndexPath indexPath)
		{   // Optional - default text is 'Delete'
			return "Trash";
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			this.parentController.PerformSegue("DataViewSegue", indexPath);
			tableView.DeselectRow (indexPath, true);
		}
	}
}


