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
    [Activity(Label = "AddSection")]
    public class AddSection : Activity
    {
        private EditText addSubjTxt;
        private Button addBtn, retBtn;
        private Functions functions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.addSection);

            addSubjTxt = FindViewById<EditText>(Resource.Id.addSubj);
            addBtn = FindViewById<Button>(Resource.Id.addBtn);
            retBtn = FindViewById<Button>(Resource.Id.retBtn);

            functions = new Functions(this, typeof(ManageSections)); // Initialize the functions object

            InitiateEvents();
        }

        public void InitiateEvents()
        {
            addBtn.Click += delegate
            {
                string newSection = addSubjTxt.Text;

                if (string.IsNullOrEmpty(newSection))
                {
                    Toast.MakeText(this, "Please enter a section name.", ToastLength.Short).Show();
                    return;
                }

                functions.AddData("section", newSection, null);
                addSubjTxt.Text = string.Empty;

            };

            retBtn.Click += delegate
            {
                functions.NextActivity();
            };
        }
    }
}