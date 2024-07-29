using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IT123P_Recitation_App
{
    [Activity(Label = "AddStudent")]
    public class AddStudent : Activity
    {
        private EditText addStudtxt;
        private Button addBtn, retBtn;
        private Functions functions;
        private string selectedSection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.addStudent);

            addStudtxt = FindViewById<EditText>(Resource.Id.addStud);
            addBtn = FindViewById<Button>(Resource.Id.addBtn);
            retBtn = FindViewById<Button>(Resource.Id.retBtn);

            functions = new Functions(this, typeof(ManageSections));

            InitiateEvents();
        }

        public void InitiateEvents()
        {
            Bundle extras = Intent.Extras;

            if (extras != null)
            {
                selectedSection = extras.GetString("section");
            }
            else
            {
                Toast.MakeText(this, "bundle_error", ToastLength.Long).Show();
                selectedSection = string.Empty;
            }

            addBtn.Click += delegate
            {
                if (!string.IsNullOrEmpty(addStudtxt.Text))
                {
                    Toast.MakeText(this, $"{addStudtxt.Text} - {selectedSection}", ToastLength.Long).Show();
                    Thread addThread = new Thread(() =>
                    {
                        functions.AddData("student", addStudtxt.Text, selectedSection);
                    });
                    addThread.Start();
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, "Textfield is Empty.", ToastLength.Short).Show();
                    });
                }
            };

            retBtn.Click += delegate
            {
                functions.NextActivity();
            };
        }
    }
}
