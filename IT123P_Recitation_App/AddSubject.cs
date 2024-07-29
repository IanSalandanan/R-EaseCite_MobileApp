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
using System.Threading.Tasks;

namespace IT123P_Recitation_App
{
    [Activity(Label = "AddSubject")]
    public class AddSubject : Activity
    {
        private EditText newSub;
        private Button addBtn, returnBtn;
        private Functions functions;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.addSubject);

            newSub = FindViewById<EditText>(Resource.Id.newSub);
            addBtn = FindViewById<Button>(Resource.Id.addBtn);
            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);

            functions = new Functions(this, typeof(ManageSubjects));

            InitiateEvents();
        }

        public void InitiateEvents()
        {
            addBtn.Click += delegate
            {
                if (!string.IsNullOrEmpty(newSub.Text))
                {
                    Thread addThread = new Thread(() =>
                    {
                        functions.AddData("subject", newSub.Text, null);
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

            returnBtn.Click += delegate
            {
                functions.NextActivity();
            };
        }
    }
}