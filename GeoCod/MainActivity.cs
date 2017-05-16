using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using Android.Util;
using Android.Provider;

namespace GeoCod
{

    [Activity(Label = "Ж[ЭКО]", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : Activity, ILocationListener
    {
        LocationManager locMgr;
        string tag = "Ж[ЭКО]";

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.RequestedOrientation = ScreenOrientation.Portrait;

            ////////////////////
            //
           // HttpClient client = new HttpClient();
           // string response = await client.GetStringAsync("http://fp.96.lt/soy4er/getMessagesMobile.php");






            ////////////
            locMgr = GetSystemService(Context.LocationService) as LocationManager;
            if (locMgr.AllProviders.Contains(LocationManager.NetworkProvider)
                    && locMgr.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, 2000, 1, this);
            }
            else
            {
                var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                StartActivity(intent);
                Toast.MakeText(this, "The Network Provider does not exist or is not enabled!", ToastLength.Long).Show();
            }
            //////////


            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.createPetitionButton);
            LinearLayout list = FindViewById<LinearLayout>(Resource.Id.ListPetition);

           // Button but = new Button();
           // list.AddChildrenForAccessibility(Button but);

            button.Click += OnButtonClicked;
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            string x = Variable.X.ToString(); x = x.Replace(",", "."); string y = Variable.Y.ToString(); y = y.Replace(",",".");
            request.RequestUri = new Uri("https://maps.google.com/maps/api/geocode/json?latlng=" + x + "," + y + "&sensor=false&language=ru&key=AIzaSyCZT_r6rCF01hNLO4s7_aH2ql5-Af3soe4");//key=AIzaSyCZT_r6rCF01hNLO4s7_aH2ql5-Af3soe4


            request.Method = HttpMethod.Get;
            //request.Headers.Add("Accept", "application/json");

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                string json = await responseContent.ReadAsStringAsync();
                RootObject root = JsonConvert.DeserializeObject<RootObject>(json);

                for (int i = 0; i < root.results.Count; i++)
                {
                    for (int j = 0; j < root.results[i].address_components.Count; j++)
                    {
                        if (root.results[i].address_components[j].types[0] == "street_number")
                            Variable.address = root.results[i].formatted_address;
                    }
                }

                Toast.MakeText(this, json, ToastLength.Long).Show();
            }
            else
            {
                HttpContent responseContent = response.Content;
                string json = await responseContent.ReadAsStringAsync();
                Roo root = JsonConvert.DeserializeObject<Roo>(json);
                /*for (int i = 0; i < root.results.Count; i++)
                {
                    for (int j = 0; j < root.results[i].address_components.Count; j++)
                    {
                        if (root.results[i].address_components[j].types[0] == "street_number")
                            Variable.address = root.results[i].formatted_address;
                    }
                }*/

                Toast.MakeText(this, json, ToastLength.Long).Show();
            }


            var intent = new Intent(this, typeof(CreatePetit));
            StartActivity(intent);
        }

        public class Roo
        {
            public string error_message { get; set; }
            public List<object> results { get; set; }
            public string status { get; set; }
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

            locMgr.RemoveUpdates(this);
            Log.Debug(tag, "Location updates paused because application is entering the background");
        }

        protected override void OnStop()
        {
            base.OnStop();
            Log.Debug(tag, "OnStop called");
        }

        //Изменение местоположения в фоне
        public void OnLocationChanged(Android.Locations.Location location)
        {
            Log.Debug(tag, "Location changed");
            Variable.X = location.Latitude;
            Variable.Y = location.Longitude;
            //latitudeY.Text = "    Y:  " + Variable.Y;
            //longitudeX.Text = "    X: " + Variable.X;
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

        /// <summary>
        /// Классы для json геокодера гугл карт
        /// </summary>
        public class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public List<string> types { get; set; }
        }

        public class Northeast
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Southwest
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Bounds
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }

        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Northeast2
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Southwest2
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Viewport
        {
            public Northeast2 northeast { get; set; }
            public Southwest2 southwest { get; set; }
        }

        public class Geometry
        {
            public Bounds bounds { get; set; }
            public Location location { get; set; }
            public string location_type { get; set; }
            public Viewport viewport { get; set; }
        }

        public class Result
        {
            public List<AddressComponent> address_components { get; set; }
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string place_id { get; set; }
            public List<string> types { get; set; }
        }

        public class RootObject
        {
            public List<Result> results { get; set; }
            public string status { get; set; }
        }
    }
}

