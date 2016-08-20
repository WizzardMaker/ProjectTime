using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Media.Plugin;
using Android.Graphics;

using Android.Support.V7;

using ProjectTime.Adapter;
using System;
using System.Collections.Generic;
using Android.Support.Design.Widget;
using Android.Content.Res;

namespace ProjectTime.Activitys {

	[Activity(Label = "Meine Ereignisse", MainLauncher = false, Icon = "@drawable/Icon", Theme = "@style/Theme.AppCompat")]
	public class MainActivity : Activity {

		Context context = Application.Context;

		AlarmReciever rc = new AlarmReciever();

		public static List<DateAdapterItem> items;

		int oldScrollY = 0;

		ListView list;
		FloatingActionButton fab;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			list = FindViewById<ListView>(Resource.Id.dateView);
			

			if (items == null) {
				items = new List<DateAdapterItem> {
					new DateAdapterItem("Meine Ereignisse",new DateTime(2016,8,15), DateAdapterItem.DateTypes.Birthday),
				};
			}

			list.Adapter = (new DateAdapter(this, items.ToArray()));
			list.ItemClick += OnListItemClick;
			//Android.App.AlarmManager am = (AlarmManager)GetSystemService(Context.AlarmService);
			fab = FindViewById<FloatingActionButton>(Resource.Id.fab);

			fab.Click += (a, b) => StartActivity(typeof(AddEventActivity));
			fab.BackgroundTintList = (ColorStateList.ValueOf(Color.ParseColor("#ff00ddff")));
			list.Scroll += List_ScrollChange;
			//Intent inten = new Intent("A");

			//am.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, 6000, 6000, PendingIntent.);
			


		}



		protected override void OnResume() {
			base.OnResume();


			list = FindViewById<ListView>(Resource.Id.dateView);
			list.Adapter = (new DateAdapter(this, items.ToArray()));

			foreach (DateAdapterItem item in items) {
				Console.WriteLine(item.name);
			}
		}

		private void List_ScrollChange(object sender, AbsListView.ScrollEventArgs e) {

			//Console.WriteLine((list.FirstVisiblePosition - oldScrollY) < 0);
			if ((list.FirstVisiblePosition - oldScrollY) < 0)
				fab.Show();
			else if ((list.FirstVisiblePosition - oldScrollY) > 0)
				fab.Hide();
			oldScrollY = list.FirstVisiblePosition;
		}

		protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e) {
			Console.WriteLine("toast");
			var t = items[e.Position];
			Toast.MakeText(this, t.name, ToastLength.Short).Show();
		}
	}

}

