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
    [Activity(Label = "UpdateSubject")]
    public class UpdateSubject : Activity
    {
        private Spinner subjectSpinner;
        private EditText editSubj;
        private Button updateBtn, returnBtn;
        private Functions functions;
        private string selectedSubject, updatedSubject;
        private List<string> subjects;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.updateSubject);

            subjectSpinner = FindViewById<Spinner>(Resource.Id.subjectSpinner);
            editSubj = FindViewById<EditText>(Resource.Id.editSubj);
            updateBtn = FindViewById<Button>(Resource.Id.updateBtn);
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

            subjectSpinner.ItemSelected += (sender, e) =>
            {
                selectedSubject = functions.GetSelectedText(subjectSpinner, e.Position);
                editSubj.Text = selectedSubject;
            };

            updateBtn.Click += delegate
            {
                updatedSubject = editSubj.Text;
                if (selectedSubject.Equals("Select a Subject"))
                {
                    Toast.MakeText(this, "Choose an Subject.", ToastLength.Long).Show();
                }
                else if (string.IsNullOrWhiteSpace(updatedSubject))
                {
                    Toast.MakeText(this, "Textfield is Empty.", ToastLength.Short).Show();
                }
                else if (!selectedSubject.Equals(updatedSubject))
                {
                    functions.ConfirmationPopUp("Update Subject", "Are you sure you want to update the selected subject?", "Subject Updated.", "Discard Changes", () =>
                    {
                        Thread updateThread = new Thread(() =>
                        {
                            functions.UpdateData("subject", updatedSubject, selectedSubject, null);

                            RunOnUiThread(() =>
                            {
                                subjects.Clear();
                                subjects = functions.PopulateRequestedList(subjects, "subject", null);
                                subjectSpinner.Adapter = functions.DisplaySpinnerTexts(subjects);
                                subjectSpinner.SetSelection(subjects.IndexOf(updatedSubject));
                            });
                        });
                        updateThread.Start();
                    });
                }
                else if (selectedSubject.Equals(updatedSubject))
                {
                    Toast.MakeText(this, "No changes detected.", ToastLength.Short).Show();
                }
            };

            returnBtn.Click += delegate
            {
                functions.NextActivity();
            };
        }
    }
}