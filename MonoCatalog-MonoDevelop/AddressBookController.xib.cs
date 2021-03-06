
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.AddressBookUI;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MonoCatalog
{
	public partial class AddressBookController : UIViewController
	{

		ABPeoplePickerNavigationController p;

		public AddressBookController () : base ("AddressBookController", null)
		{
		}
	
		protected override void Dispose (bool disposing)
		{
			if (p != null) {
				p.Dispose ();
				p = null;
			}
			base.Dispose (disposing);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			NavigationController.NavigationBar.Translucent = false;
		}
	
		ABPeoplePickerNavigationController GetPicker ()
		{
			if (p != null)
				return p;

			p = new ABPeoplePickerNavigationController ();
			p.SelectPerson += (o, e) => {
				Console.Error.WriteLine ("# select Person: {0}", e.Person);
				toString.Text   = e.Person.ToString ();
				firstName.Text  = e.Person.FirstName;
				lastName.Text   = e.Person.LastName;
				property.Text   = "";
				identifier.Text = "";
				e.Continue = selectProperty.On;
				if (!e.Continue)
					DismissModalViewControllerAnimated (true);
			};
			p.PerformAction += (o, e) => {
				Console.Error.WriteLine ("# perform action; person={0}", e.Person);
				toString.Text   = e.Person.ToString ();
				firstName.Text  = e.Person.FirstName;
				lastName.Text   = e.Person.LastName;
				property.Text   = e.Property.ToString ();
				identifier.Text = e.Identifier.HasValue ? e.Identifier.ToString () : "";
				e.Continue = performAction.On;
				if (!e.Continue)
					DismissModalViewControllerAnimated (true);
			};
			p.Cancelled += (o, e) => {
				Console.Error.WriteLine ("# select Person cancelled.");
				toString.Text   = "Cancelled";
				firstName.Text  = "";
				lastName.Text   = "";
				property.Text   = "";
				identifier.Text = "";
				DismissModalViewControllerAnimated (true);
			};
			return p;
		}
	
		partial void showPicker (MonoTouch.UIKit.UIButton sender)
		{
			Console.Error.WriteLine ("# Select Contacts pushed!");
			PresentModalViewController (GetPicker (), true);
		}
	}
}
