using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;

namespace GeoCod
{

    [Activity(Label = "GeoCod", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

             SetContentView (Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.createPetitionButton);

            button.Click += OnButtonClicked;
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CreatePetit));
            StartActivity(intent);
        }
    }
}

