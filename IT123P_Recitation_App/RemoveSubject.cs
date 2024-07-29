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
using static System.Collections.Specialized.BitVector32;

namespace IT123P_Recitation_App
{
    [Activity(Label = "RemoveSubject")]
    public class RemoveSubject : Activity
    {
        private Spinner subjectSpinner;
        private Button removeBtn, returnBtn;
        private List<string> subjects;
        private Functions functions;
        private string selectedSubject;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.removeSubject);

            subjectSpinner = FindViewById<Spinner>(Resource.Id.subjectSpinner);
            removeBtn = FindViewById<Button>(Resource.Id.removeBtn);
            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);
            
            functions = new Functions(this, typeof(ManageSubjects));
            subjects = new List<string>();

            InitiateEvents();
        }

        public void InitiateEvents()
        {
            subjects = functions.PopulateRequestedList(subjects, "subject", null);
            subjectSpinner.Adapter = functions.DisplaySpinnerTexts(subjects);

            subjects.Insert(0, "Select a Subject");
            subjectSpinner.Adapter = functions.DisplayItems(subjects);
            subjectSpinner.SetSelection(0);

            subjectSpinner.ItemSelected += (selected, e) =>
            {
                selectedSubject = functions.GetSelectedText(subjectSpinner, e.Position);
            };

            removeBtn.Click += delegate
            {
                if (selectedSubject.Equals("Select a Subject"))
                {
                    Toast.MakeText(this, "Choose an Subject.", ToastLength.Long).Show();
                }
                else
                {
                    functions.ConfirmationPopUp("Remove Subject", "Are you sure you want to remove the selected subject?", "Subject Removed.", "Discard Changes", () =>
                    {
                        Thread removeThread = new Thread(() =>
                        {
                            functions.RemoveData("subject", selectedSubject, null);
                            RunOnUiThread(() =>
                            {
                                subjects.Remove(selectedSubject);
                                subjectSpinner.Adapter = functions.DisplaySpinnerTexts(subjects);
                            });
                        });
                        removeThread.Start();
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