using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace IT123P_Recitation_App
{
    [Activity(Label = "QuestionRound")]
    public class QuestionRound : Activity
    {
        private ListView participantsView, questionsView;
        private Button pickPBtn, pickQBtn, nextRoundBtn, quitRoundBtn;
        private Functions functions;

        private List<string> students, selectedStudents, questions, selectedQuestions;
        private ArrayList partiBlockList, questBlockList;
        private string selectedSubject, selectedSection;
        private int studCount;
        private Bundle extras;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.questionRound);

            participantsView = FindViewById<ListView>(Resource.Id.participantsView);
            questionsView = FindViewById<ListView>(Resource.Id.questionsView);
            pickPBtn = FindViewById<Button>(Resource.Id.pickPBtn);
            pickQBtn = FindViewById<Button>(Resource.Id.pickQBtn);
            nextRoundBtn = FindViewById<Button>(Resource.Id.nextRoundBtn);
            quitRoundBtn = FindViewById<Button>(Resource.Id.quitRoundBtn);

            partiBlockList = new ArrayList();
            questBlockList = new ArrayList();

            students = new List<string>();
            questions = new List<string>();

            selectedStudents = new List<string>();
            selectedQuestions = new List<string>();

            functions = new Functions(this, typeof(SetRound));
            
            InitiateEvents();
        }
        public void InitiateEvents()
        {
            extras = Intent.Extras;

            if (extras != null)
            {
                selectedSubject = extras.GetString("subject");
                selectedSection = extras.GetString("section");
                studCount = extras.GetInt("studCount");
            }
            else
            {
                Toast.MakeText(this, "bundle_error", ToastLength.Long).Show();
            }

            questions = functions.PopulateRequestedList(questions, "question", selectedSubject);
            students = functions.PopulateRequestedList(students, "student", selectedSection);

            questionsView.ItemClick += (sender, e) =>
            {
                string questToView = functions.GetViewItem(questionsView, e.Position);
                functions.ViewQuestion("View Question", questToView, "RETURN");
            };

            pickPBtn.Click += delegate
            {
                selectedStudents = functions.PickRandomItems(students, partiBlockList, studCount, "Students");
                participantsView.Adapter = functions.DisplaySpinnerTexts(selectedStudents);
            };

            pickQBtn.Click += delegate
            {
                selectedQuestions = functions.PickRandomItems(questions, questBlockList, 1, "Questions");
                questionsView.Adapter = functions.DisplaySpinnerTexts(selectedQuestions);
            };

            nextRoundBtn.Click += delegate
            {
                functions.BlockItems(partiBlockList, selectedStudents);
                functions.BlockItems(questBlockList, selectedQuestions);
                selectedStudents = functions.PickRandomItems(students, partiBlockList, studCount, "Students");
                participantsView.Adapter = functions.DisplayItems(selectedStudents);
                selectedQuestions = functions.PickRandomItems(questions, questBlockList, 1, "Questions");
                questionsView.Adapter = functions.DisplayItems(selectedQuestions);
            };

            quitRoundBtn.Click += delegate
            {
                functions.ConfirmationPopUp("Quit Round", "Are you sure you want to quit?", "Round Terminated", "Continue", () =>
                {
                    functions.NextActivity();
                });
            };
        }
    }
}