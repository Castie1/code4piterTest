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
using Android.Locations;
using Android.Content.PM;

namespace GeoCod
{
    [Activity(Label = "Petite", Theme = "@android:style/Theme.NoTitleBar")]
    public class Petite : Activity
    {
        TextView time;
        TextView address;
        TextView zkh;
        TextView message;
        ImageView imageview;
        Button buttonSend;
        //Bitmap img;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestedOrientation = ScreenOrientation.Portrait;



            SetContentView(Resource.Layout.Petite);

            time = FindViewById<TextView>(Resource.Id.textTime);
            address = FindViewById<TextView>(Resource.Id.textAddress);
            zkh = FindViewById<TextView>(Resource.Id.textZKH);
            message = FindViewById<TextView>(Resource.Id.textMessage);
            imageview = FindViewById<ImageView>(Resource.Id.image);
            buttonSend = FindViewById<Button>(Resource.Id.buttonSend);

            message.Text = Variable.address;
            imageview.SetImageBitmap(Variable.img);
            buttonSend.Click += OnButtonClicked;

            //imageview.SetImageBitmap(img);
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Типа отправил!", ToastLength.Long).Show();
        }
    }
}