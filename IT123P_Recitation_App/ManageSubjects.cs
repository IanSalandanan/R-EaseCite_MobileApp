using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.View.Menu;
using Java.Nio.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IT123P_Recitation_App
{
    [Activity(Label = "ManageQuestions")]
    public class ManageSubjects : Activity
    {
        private Spinner subjectOpt, activityOpt;
        private Button proceedBtn, newSubBtn, removeSubBtn, updateBtn ,returnBtn;
        private List<string> subjects;
        private List<string> options = new List<string> {"Select an Operation","Add Question", "Remove Question", "Update Question"};
        private Functions functions;
        private string selectedSubject, selectedOpt;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.manageSubjects);

            subjectOpt = FindViewById<Spinner>(Resource.Id.subjectOpt);
            activityOpt = FindViewById<Spinner>(Resource.Id.activityOpt);
            proceedBtn = FindViewById<Button>(Resource.Id.proceedBtn);
            newSubBtn = FindViewById<Button>(Resource.Id.newSubBtn);
            removeSubBtn = FindViewById<Button>(Resource.Id.removeSubBtn);
            updateBtn = FindViewById<Button>(Resource.Id.updateBtn);
            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);

            functions = new Functions(this, typeof(MainActivity));
            subjects = new List<string>();

            InitiateEvents();
        }

        public void InitiateEvents()
        {
            subjects.Insert(0,"Select a Subject");
            subjects = functions.PopulateRequestedList(subjects, "subject", null);
            subjectOpt.Adapter = functions.DisplayItems(subjects);
            subjectOpt.SetSelection(0);
            activityOpt.Adapter = functions.DisplayItems(options);
            activityOpt.SetSelection(0);

            subjectOpt.ItemSelected += (sender, e) =>
            {
                selectedSubject = functions.GetSelectedText(subjectOpt, e.Position);
            };

            activityOpt.ItemSelected += (sender, e) =>
            {
                selectedOpt = functions.GetSelectedText(activityOpt, e.Position);
            };

            proceedBtn.Click += delegate
            {
                if (selectedSubject.Equals("Select a Subject") || selectedOpt.Equals("Select an Operation"))
                {
                    Toast.MakeText(this, "Choose an Option.", ToastLength.Long).Show();
                }
                else
                {
                    Bundle extras = new Bundle();
                    extras.PutString("subject", selectedSubject);
                    functions = new Functions(this, Type.GetType($"IT123P_Recitation_App.{selectedOpt.Replace(" ", "")}"));
                    functions.NextExtraActivity(extras);
                }
            };

            newSubBtn.Click += delegate
            {
                functions = new Functions(this, typeof(AddSubject));
                functions.NextActivity();
            };

            removeSubBtn.Click += delegate
            {
                functions = new Functions(this, typeof(RemoveSubject));
                functions.NextActivity();
            };

            updateBtn.Click += delegate
            {
                functions = new Functions(this, typeof(UpdateSubject));
                functions.NextActivity();
            };

            returnBtn.Click += delegate
            {
                functions.NextActivity();
            };
        }
    }
}