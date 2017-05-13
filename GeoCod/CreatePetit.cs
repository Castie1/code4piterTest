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
using Android.Locations;
using Android.Util;

namespace GeoCod
{
    [Activity(Label = "CreatePetition", Theme = "@android:style/Theme.NoTitleBar")]
    public class CreatePetit : Activity, ILocationListener
    {
        LocationManager locMgr;
        string tag = "CreatePetition";
        Button button;
        TextView latitudeY;
        TextView longitudeX;
        TextView provider;
        TextView address;
        Button button1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ////////////
            locMgr = GetSystemService(Context.LocationService) as LocationManager;
            if (locMgr.AllProviders.Contains(LocationManager.NetworkProvider)
                    && locMgr.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, 2000, 1, this);
            }
            else
            {
                Toast.MakeText(this, "The Network Provider does not exist or is not enabled!", ToastLength.Long).Show();
            }
            //////////

            // Create your application here
            SetContentView(Resource.Layout.CreatePetition);

            latitudeY = FindViewById<TextView>(Resource.Id.textY);
            longitudeX = FindViewById<TextView>(Resource.Id.textX);
            //address = FindViewById<TextView>(Resource.Id.textAddress);
            button1 = FindViewById<Button>(Resource.Id.buttonSend);

            button1.Click += OnButtonClicked;

            latitudeY.Text = "    Y:  " + Resource.variable.Y;
            longitudeX.Text = "    X: " + Resource.variable.X;

        }

        /*void OnButtonClicked(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(Petite));
            StartActivity(intent);
        }*/
    

     async void OnButtonClicked(object sender, System.EventArgs e)
         {         
            var geo = new Geocoder(this);
             var addresses = await geo.GetFromLocationAsync(Resource.variable.X, Resource.variable.Y, 1);

             // var addressText = FindViewById<TextView>(Resource.Id.addressText);
             if (addresses.Any())
             {
                 addresses.ToList().ForEach(addr => Resource.variable.address = addr + System.Environment.NewLine + System.Environment.NewLine);
             }
             else
             {
                 Resource.variable.address = "Could not find any addresses.";
             }

            var intent = new Intent(this, typeof(Petite));
            StartActivity(intent);

        }

        protected override void OnStart()
        {
            base.OnStart();
            Log.Debug(tag, "OnStart called");
        }

        // OnResume gets called every time the activity starts, so we'll put our RequestLocationUpdates
        // code here, so that 
        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug(tag, "OnResume called");

            // initialize location manager
            locMgr = GetSystemService(Context.LocationService) as LocationManager;
            if (locMgr.AllProviders.Contains(LocationManager.NetworkProvider)
                     && locMgr.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, 1, 1, this);
            }
            else
            {
                Toast.MakeText(this, "The Network Provider does not exist or is not enabled!", ToastLength.Long).Show();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            // stop sending location updates when the application goes into the background
            // to learn about updating location in the background, refer to the Backgrounding guide
            // http://docs.xamarin.com/guides/cross-platform/application_fundamentals/backgrounding/


            // RemoveUpdates takes a pending intent - here, we pass the current Activity
            locMgr.RemoveUpdates(this);
            Log.Debug(tag, "Location updates paused because application is entering the background");
        }

        protected override void OnStop()
        {
            base.OnStop();
            Log.Debug(tag, "OnStop called");
        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
            Log.Debug(tag, "Location changed");
            Resource.variable.X = location.Latitude;
            Resource.variable.Y = location.Longitude;
            latitudeY.Text = "    Y:  " + Resource.variable.Y;
            longitudeX.Text = "    X: " + Resource.variable.X;
            //provider.Text = "Provider: " + location.Provider.ToString();
        }
        public void OnProviderDisabled(string provider)
        {
            Log.Debug(tag, provider + " disabled by user");
        }
        public void OnProviderEnabled(string provider)
        {
            Log.Debug(tag, provider + " enabled by user");
        }
        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            Log.Debug(tag, provider + " availability has changed to " + status.ToString());
        }
    }
}