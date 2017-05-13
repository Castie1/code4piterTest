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
        Bitmap img;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);



            SetContentView(Resource.Layout.Petite);

            time = FindViewById<TextView>(Resource.Id.textTime);
            address = FindViewById<TextView>(Resource.Id.textAddress);
            zkh = FindViewById<TextView>(Resource.Id.textZKH);
            message = FindViewById<TextView>(Resource.Id.textMessage);

            message.Text = Resource.variable.address;


            //imageview.SetImageBitmap(img);
        }
    }
}