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
    [Activity(Label = "UpdateQuestion")]
    public class UpdateQuestion : Activity
    {
        private Spinner questionsSpinner;
        private EditText editQuest;
        private Button updateBtn, returnBtn;
        private Functions functions;
        private string selectedSubject, selectedQuestion, updatedQuestion;
        private List<string> questions;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.updateQuestions);

            questionsSpinner = FindViewById<Spinner>(Resource.Id.questionsSpinner);
            editQuest = FindViewById<EditText>(Resource.Id.editQuest);
            updateBtn = FindViewById<Button>(Resource.Id.updateBtn);
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
                editQuest.Text = selectedQuestion;
            };

            updateBtn.Click += delegate
            {
                updatedQuestion = editQuest.Text;
                if (selectedQuestion.Equals("Select a Question"))
                {
                    Toast.MakeText(this, "Choose an Question.", ToastLength.Long).Show();
                }
                else if (string.IsNullOrWhiteSpace(updatedQuestion))
                {
                    Toast.MakeText(this, "Textfield is Empty.", ToastLength.Short).Show();
                }
                else if (!selectedQuestion.Equals(updatedQuestion))
                {
                    functions.ConfirmationPopUp("Update Question", "Are you sure you want to update the selected question?", "Question Updated.", "Discard Changes", () =>
                    {
                        Thread updateThread = new Thread(() =>
                        {
                            functions.UpdateData("question", updatedQuestion, selectedQuestion, selectedSubject);
                            RunOnUiThread(() =>
                            {
                                questions.Clear();
                                questions = functions.PopulateRequestedList(questions, "question", selectedSubject);
                                questionsSpinner.Adapter = functions.DisplaySpinnerTexts(questions);
                                questionsSpinner.SetSelection(questions.IndexOf(updatedQuestion));
                            });
                        });
                        updateThread.Start();
                    });
                }
                else if (selectedQuestion.Equals(updatedQuestion))
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



