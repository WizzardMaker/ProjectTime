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
using Android.Views.InputMethods;

namespace ProjectTime.Activitys {
	[Activity(Label = "AddEventActivity", Theme = "@style/Theme.AppCompat.Light")]
	public class AddEventActivity : Activity {
		
		public string name {
			get { return FindViewById<EditText>(Resource.Id.editTextName).Text; }
			set {
				EditText name = FindViewById<EditText>(Resource.Id.editTextName);
				name.Text = value;
			}
		}

		public DateTime _date;
		public DateTime date {
			get { return _date; }
			set {
				TextView datePicker = FindViewById<TextView>(Resource.Id.buttonDatePick);
				datePicker.Text = value.ToString("dd, MMM. yyyy");
				_date = value;
			}
		}

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.AddEvent);

			EditText nameView = FindViewById<EditText>(Resource.Id.editTextName);
			TextView datePicker = FindViewById<TextView>(Resource.Id.buttonDatePick);

			//name.TextChanged += (s, e) => {
			//	this.name = name.Text;
			//};

			date = DateTime.Today;

			datePicker.Click += (o,e) => {
				DateTime _date = DateTime.Today;

				InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
				imm.HideSoftInputFromWindow(nameView.WindowToken, 0);

				Dialog dialog = new Android.App.DatePickerDialog(this, _listener, _date.Year, _date.Month - 1, _date.Day);
				dialog.Show();
			};

			TextView submit = FindViewById<TextView>(Resource.Id.buttonSubmit);

			submit.Click += (s, e) => {

				name = name.TrimEnd(' ').TrimStart(' ');

				MainActivity.items.Add(new Adapter.DateAdapterItem(name, date));

				var intent = new Intent(this, typeof(MainActivity))
				   .SetFlags(ActivityFlags.ReorderToFront);
				StartActivity(intent);
				Finish();
			};

			ImageView exit = FindViewById<ImageView>(Resource.Id.buttonReturn);

			exit.Click += (s, e) => {
				var intent = new Intent(this, typeof(MainActivity))
				   .SetFlags(ActivityFlags.ReorderToFront);
				StartActivity(intent);
				Finish();
			};
			// Create your application here
		}

		private void _listener(object sender, DatePickerDialog.DateSetEventArgs e) {
			TextView datePicker = FindViewById<TextView>(Resource.Id.buttonDatePick);

			date = e.Date;
		}
	}
}