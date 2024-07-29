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
    [Activity(Label = "AddQuestion")]
    public class AddQuestion : Activity
    {
        private EditText newQuest;
        private Button addBtn, returnBtn;
        private string selectedSubject;
        private Functions functions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.addQuestions);

            newQuest = FindViewById<EditText>(Resource.Id.newQuest);
            addBtn = FindViewById<Button>(Resource.Id.addBtn);
            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);

            functions = new Functions(this, typeof(ManageSubjects));

            InitiateEvents();
        }

        public void InitiateEvents()
        {
            Bundle extras = Intent.Extras;

            if (extras != null)
            {
                selectedSubject = extras.GetString("subject");
            }
            else
            {
                Toast.MakeText(this, "bundle_error", ToastLength.Long).Show();
            }

            addBtn.Click += delegate
            {
                if (!string.IsNullOrEmpty(newQuest.Text))
                {
                    Thread addThread = new Thread(() =>
                    {
                        functions.AddData("question", newQuest.Text, selectedSubject);
                    }); addThread.Start();
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, "Textfield is Empty.", ToastLength.Short).Show();
                    });
                }
            };
            returnBtn.Click += delegate
            {
                functions.NextActivity();
            };
        }
    }
}