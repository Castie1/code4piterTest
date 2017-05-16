using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Util;
using Android.Provider;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Java.IO;
using Android.Graphics;
using Environment = Android.OS.Environment;

//using Uri = Android.Net.Uri;



namespace GeoCod
{
    public static class App
    {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }

    [Activity(Label = "CreatePetition", Theme = "@android:style/Theme.NoTitleBar")]
    public class CreatePetit : Activity
    {
        //Button button;
        EditText textAddress;
        EditText message;
        ImageButton buttonCam;

        ImageView imageview1;
        Button button1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestedOrientation = ScreenOrientation.Portrait;

            // Create your application here
            SetContentView(Resource.Layout.CreatePetition);

            button1 = FindViewById<Button>(Resource.Id.buttonSend);
            textAddress = FindViewById<EditText>(Resource.Id.editText1);
            message = FindViewById<EditText>(Resource.Id.text);

            textAddress.Text = Variable.address;

            button1.Clickable = false;

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                buttonCam = FindViewById<ImageButton>(Resource.Id.imageButton1);
                imageview1 = FindViewById<ImageView>(Resource.Id.imageView1);
                buttonCam.Click += OnButtonCamClicked;
            }

            button1.Click += OnButtonClicked;

        }

      
        void OnButtonCamClicked(object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));

            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));

            StartActivityForResult(intent, 0);

        }
    

     async  void OnButtonClicked(object sender, System.EventArgs e)
         {           
           // if(Variable.img != null)
           // {
                Variable.address = textAddress.Text;
                Variable.message = message.Text;
                string coord = Variable.X.ToString() + "," + Variable.Y.ToString();


                //  System.IO.Stream stream = new System.IO.MemoryStream();
                // stream.Write(ImageToByte(Variable.img));
              /*  System.IO.MemoryStream stream = new System.IO.MemoryStream();
                Variable.img.Compress(Bitmap.CompressFormat.Png, 0, stream);
                 byte[] bitmapData = stream.ToArray();
                
                

               HttpContent content = new StreamContent(stream);

                HttpClient client = new HttpClient();

                HttpRequestMessage req = new HttpRequestMessage();
                req.Headers.Add("Authorization", "2pp72KtHx13pOEf3BD5B");
                req.RequestUri = new Uri("https://api.imageban.ru/v1");
                req.Method = HttpMethod.Post;
                req.Content = content;

                // HttpResponseMessage response = await client.PostAsync("http://fp.96.lt/soy4er/uploadimg.php/?data=", content);
                HttpResponseMessage response = await client.SendAsync(req);
                Toast.MakeText(this, response.Content.ToString(), ToastLength.Long).Show();*/
                Toast.MakeText(this, coord, ToastLength.Long).Show();
         /*   }
           // Toast.MakeText(this, Variable.address, ToastLength.Long).Show();
            else
                Toast.MakeText(this, "Загрузите фотографию", ToastLength.Long).Show();*/

        }


        protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = imageview1.Height;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null)
            {
                Variable.img = App.bitmap;
                imageview1.SetImageBitmap (App.bitmap);
                App.bitmap = null;
            }

            // Dispose of the Java side bitmap.
            GC.Collect();
        }
        private void CreateDirectoryForPictures()
        {
            App._dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "CameraAppDemo");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

      
    }
}