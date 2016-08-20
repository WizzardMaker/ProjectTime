using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ProjectTime.Activitys {

	[Activity(Label = "Meine Ereignisse", MainLauncher = true, Icon = "@drawable/Icon", Theme = "@android:style/Theme.Light.NoTitleBar")]
	public class StartActivity : Activity {
		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Start);

			ProgressBar spinner = FindViewById<ProgressBar>(Resource.Id.progressBar1);

			new Task(() => { Thread.Sleep(2000); StartApp(); }).Start();

			// Create your application here
		}

		public void StartApp() {
			StartActivity(typeof(MainActivity));

			Finish();
		}
	}
}