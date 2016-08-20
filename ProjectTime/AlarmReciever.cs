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

namespace ProjectTime {

	[BroadcastReceiver(Enabled = true)]
	[IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted })]
	class AlarmReciever : BroadcastReceiver {

		static bool alarmOff;

		public override void OnReceive(Context context, Intent intent) {


			Console.WriteLine("Tut");

			var message = intent.GetStringExtra("message");
			var title = intent.GetStringExtra("title");



			if (intent.GetStringExtra("type") == "alarmOff") {
				alarmOff = true;
				Console.WriteLine("Alarm turned off!");
				return;
			}

			/*
			var notIntent = new Intent(context, typeof(MainActivity));
			var contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.CancelCurrent);*/

			Intent afterIntent = new Intent(context, this.Class);

			afterIntent
				.PutExtra("type", "alarmOff")
				.PutExtra("message", "")
				.PutExtra("title", "");

			PendingIntent contentIntent = PendingIntent.GetBroadcast(context, 0, afterIntent, PendingIntentFlags.CancelCurrent);

		

			var style = new Notification.BigTextStyle();
			style.BigText(message);



			//Generate a notification with just short text and small icon
			var builder = new Notification.Builder(context)
				.SetContentIntent(contentIntent)
				.SetSmallIcon(Resource.Drawable.IconSmall)
				.SetContentTitle(title)
				.SetContentText(message)
				.SetStyle(style)
				.SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis())
				.SetAutoCancel(true)
				.SetCategory(Notification.CategoryAlarm)
				.SetPriority((int)NotificationPriority.High);


			Task vibrate = new Task(() => {

				int i = 0;

				int timeOut = 10;

				while (i <= timeOut) {

					Vibrator vibrator = (Vibrator)context.GetSystemService(Context.VibratorService);
					vibrator.Vibrate(1000);

					if (alarmOff) {
						Console.WriteLine("Return of Alarm!");
						vibrator.Cancel();
						alarmOff = false;
                        return;
					}

					Thread.Sleep(2000);

					if (alarmOff) {
						Console.WriteLine("Return of Alarm!");
						vibrator.Cancel();
						alarmOff = false;
						return;
					}

					//Console.WriteLine("Brum!");

					i++;

					
				}


			});

			vibrate.Start();

			var manager = context.GetSystemService(Context.NotificationService) as NotificationManager;
			manager.Notify(0, builder.Build());
		}

		public void CancelAlarm(Context context) {
			Intent intent = new Intent(context, this.Class);
			PendingIntent sender = PendingIntent.GetBroadcast(context, 0, intent, 0);
			AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
			alarmManager.Cancel(sender);
		}

		public void SetAlarm(Context context, int alertTime) {
            AlarmManager am = (AlarmManager)context.GetSystemService(Context.AlarmService);

			Intent intent = new Intent(context, this.Class);

			intent
				.PutExtra("type","call")
				.PutExtra("message","Jahrestag von Dir und Jemanden morgen!")
				.PutExtra("title","Jahrestag!");

			PendingIntent pi = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);


			am.SetExact(AlarmType.ElapsedRealtimeWakeup, DateTime.Now.Millisecond + 30000, pi);
		}
	}
}