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
    [Activity(Label = "RemoveQuestion")]
    public class RemoveQuestion : Activity
    {
        private Spinner questionsSpinner;
        private Button removeBtn, returnBtn;
        private string selectedSubject, selectedQuestion;
        private Functions functions;
        private List<string> questions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.removeQuestions);

            questionsSpinner = FindViewById<Spinner>(Resource.Id.questionsSpinner);
            removeBtn = FindViewById<Button>(Resource.Id.removeBtn);
            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);

            functions = new Functions(this, typeof(ManageSubjects));
            questions = new List<string>();

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

            questions = functions.PopulateRequestedList(questions, "question", selectedSubject);
            questions.Insert(0, "Select a Question");
            questionsSpinner.Adapter = functions.DisplayItems(questions);
            questionsSpinner.SetSelection(0);

            questionsSpinner.ItemSelected += (sender, e) =>
            {
                selectedQuestion = functions.GetSelectedText(questionsSpinner, e.Position);
            };

            removeBtn.Click += delegate
            {
                if (selectedQuestion.Equals("Select a Question"))
                {
                    Toast.MakeText(this, "Choose an Question.", ToastLength.Long).Show();
                }
                else
                {
                    functions.ConfirmationPopUp("Remove Question", "Are you sure you want to remove the selected question?", "Question Removed.", "Discard Changes", () =>
                    {
                        Thread removeThread = new Thread(() =>
                        {
                            functions.RemoveData("question", selectedQuestion, selectedSubject);
                            RunOnUiThread(() =>
                            {
                                questions.Remove(selectedQuestion);
                                questionsSpinner.Adapter = functions.DisplaySpinnerTexts(questions);
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