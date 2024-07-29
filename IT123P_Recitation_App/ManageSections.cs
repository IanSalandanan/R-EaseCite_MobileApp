using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace IT123P_Recitation_App
{
    [Activity(Label = "ManageSections")]
    public class ManageSections : Activity
    {
        private Spinner sectionOpt, actionOpt;
        private Button proceedBtn, returnBtn, removeSecBtn, newSecBtn, updateSecBtn;
        private List<string> sections;
        private List<string> actions = new List<string> { "Select an Operation", "Add Student", "Remove Student", "Update Student" };
        private Functions functions;
        private string selectedSection, selectedAction;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.manageSections);

            sectionOpt = FindViewById<Spinner>(Resource.Id.sectionOpt);
            actionOpt = FindViewById<Spinner>(Resource.Id.actionOpt);
            proceedBtn = FindViewById<Button>(Resource.Id.proceedBtn);
            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);
            removeSecBtn = FindViewById<Button>(Resource.Id.removeSecBtn);
            newSecBtn = FindViewById<Button>(Resource.Id.newSecBtn);
            updateSecBtn = FindViewById<Button>(Resource.Id.updateSecBtn);


            functions = new Functions(this, typeof(MainActivity));
            sections = new List<string>(); // Initialize sections here 

            InitiateEvents();
        }

        public void InitiateEvents()
        {
            // Populate the sections list
            sections = functions.PopulateRequestedList(sections, "section", null);
            sections.Insert(0, "Select a Section");

            sectionOpt.Adapter = functions.DisplayItems(sections);
            sectionOpt.SetSelection(0);
            actionOpt.Adapter = functions.DisplayItems(actions);
            actionOpt.SetSelection(0);

            sectionOpt.ItemSelected += (sender, e) =>
            {
                selectedSection = functions.GetSelectedText(sectionOpt, e.Position);
            };

            actionOpt.ItemSelected += (sender, e) =>
            {
                selectedAction = functions.GetSelectedText(actionOpt, e.Position);
            };

            proceedBtn.Click += delegate
            {
                if (selectedSection.Equals("Select a Section") || selectedAction.Equals("Select an Operation"))
                {
                    Toast.MakeText(this, "Choose an Option.", ToastLength.Long).Show();
                }
                else
                {
                    Bundle extras = new Bundle();
                    extras.PutString("section", selectedSection);
                    functions = new Functions(this, Type.GetType($"IT123P_Recitation_App.{selectedAction.Replace(" ", "")}"));
                    functions.NextExtraActivity(extras);
                }
            };

            newSecBtn.Click += delegate
            {
                functions = new Functions(this, typeof(AddSection));
                functions.NextActivity();
            };

            removeSecBtn.Click += delegate
            {
                functions = new Functions(this, typeof(RemoveSection));
                functions.NextActivity();
            };

            updateSecBtn.Click += delegate
            {
                functions = new Functions(this, typeof(UpdateSection));
                functions.NextActivity();
            };

            returnBtn.Click += delegate
            {
                functions.NextActivity();
            };
        }
    }
}
