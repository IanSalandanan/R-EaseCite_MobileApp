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
    [Activity(Label = "SetRound")]
    public class SetRound : Activity
    {
        private Spinner spinner1, spinner2;
        private EditText numParticipants;
        private Button proceedBtn, returnBtn;
        private Functions functions;

        private string selectedSubject, selectedSection;
        private int studCount;
        private List<string> subjects, sections;
        private Bundle extras;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.setRound);

            spinner1 = FindViewById<Spinner>(Resource.Id.spinner1);
            spinner2 = FindViewById<Spinner>(Resource.Id.spinner2);
            numParticipants = FindViewById<EditText>(Resource.Id.numParticipants);
            proceedBtn = FindViewById<Button>(Resource.Id.proceedBtn);
            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);

            subjects = new List<string>();
            sections = new List<string>();

            functions = new Functions(this, typeof(QuestionRound));
            InitiateEvents();
        }

        protected void InitiateEvents()
        {
            subjects = functions.PopulateRequestedList(subjects, "subject", null);
            sections = functions.PopulateRequestedList(sections, "section", null);

            subjects.Insert(0, "Select a Subject");
            sections.Insert(0, "Select a Section");

            spinner1.Adapter = functions.DisplayItems(subjects);
            spinner1.SetSelection(0);
            spinner2.Adapter = functions.DisplayItems(sections);
            spinner2.SetSelection(0);

            spinner1.ItemSelected += (sender, e) =>
            {
                selectedSubject = functions.GetSelectedText(spinner1, e.Position);
            };

            spinner2.ItemSelected += (sender, e) =>
            {
                selectedSection = functions.GetSelectedText(spinner2, e.Position);
            };

            proceedBtn.Click += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(numParticipants.Text) && int.TryParse(numParticipants.Text, out studCount))
                {
                    if (selectedSubject.Equals("Select a Subject") || selectedSection.Equals("Select a Section"))
                    {
                        Toast.MakeText(this, "Choose an Option.", ToastLength.Long).Show();
                    }
                    else
                    {
                        if (studCount > 0)
                        {
                            extras = new Bundle();
                            extras.PutString("subject", selectedSubject);
                            extras.PutString("section", selectedSection);
                            extras.PutInt("studCount", studCount);

                            functions.NextExtraActivity(extras);
                        }
                        else
                        {
                            Toast.MakeText(this, "Number not Applicable.", ToastLength.Long).Show();
                        }
                    }
                }
                else
                {
                    Toast.MakeText(this, "Please enter a valid number.", ToastLength.Long).Show();
                }
            };

            returnBtn.Click += delegate 
            {

                functions = new Functions(this, typeof(MainActivity));
                functions.NextActivity(); 


            };
        }
    }
}