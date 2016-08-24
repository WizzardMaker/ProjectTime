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
using Android.Graphics;

namespace ProjectTime.Adapter {
	public class DateAdapterItem {
		public enum DateTypes {
			Birthday,
			Anniversary,
			Event,
		}
		public const DateTypes RepeatingEvents = DateTypes.Birthday | DateTypes.Anniversary;

		public DateTime date;
		public string name;
		public DateTypes type;

		public DateAdapterItem(string name, DateTime date, DateTypes type = DateTypes.Event) {
			this.name = name;
			this.date = date;
			this.type = type;
		}
	}

	class DateAdapter : BaseAdapter<DateAdapterItem> {

		public const string arrowUp = "\u2B06";
		public const string arrowDown = "\u2B07";

		DateAdapterItem[] items;
		Activity context;

		public DateAdapter(Activity context, DateAdapterItem[] items) : base() {
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position) {
			return position;
		}
		public override DateAdapterItem this[int position] {
			get { return items[position]; }
		}
		public override int Count {
			get { return items.Length; }
		}

		int Years(DateTime start, DateTime end) {
			return (end.Year - start.Year - 1) +
				(((end.Month > start.Month) ||
				((end.Month == start.Month) && (end.Day >= start.Day))) ? 1 : 0);
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Resource.Layout.DateLayout, null);

			view.FindViewById<TextView>(Resource.Id.textName).Text = items[position].name;
			view.FindViewById<TextView>(Resource.Id.textDate).Text = items[position].date.ToString("d. MMM yyy");

			TimeSpan preDate = new TimeSpan();
			TimeSpan sufDate = items[position].date.Subtract(DateTime.Now);
			TextView timeLeft = view.FindViewById<TextView>(Resource.Id.textTimeLeft);

			

			if ((items[position].type & DateAdapterItem.RepeatingEvents) == items[position].type) {
				if (items[position].type == DateAdapterItem.DateTypes.Birthday) {
					DateTime next = items[position].date;
					next = next.AddYears(Years(items[position].date, DateTime.Today.AddYears(1)));

					preDate = next - DateTime.Today;


					timeLeft.Text = preDate.Days.ToString() + " " + arrowDown;

					if (preDate.Days < 5)
						timeLeft.SetTextColor(Color.Orange);

					if (preDate.Days < 2)
						timeLeft.SetTextColor(Color.Red);



				}else if(items[position].type == DateAdapterItem.DateTypes.Anniversary) {


					timeLeft.Text = DateTime.Today.Subtract( items[position].date).Days.ToString() + " " + arrowUp; 

				}

			} else {
				timeLeft.Text = sufDate.Days.ToString() + " " + arrowDown;

				if (sufDate.Days < 5)
					timeLeft.SetTextColor(Color.Orange);

				if (sufDate.Days < 2)
					timeLeft.SetTextColor(Color.Red);
			}
			
			

			return view;
		}
	}
}