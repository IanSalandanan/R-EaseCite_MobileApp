using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Threading;
using System.Collections.Generic;

namespace IT123P_Recitation_App
{
    [Activity(Label = "RemoveSection")]
    public class RemoveSection : Activity
    {
        Spinner sectionSelect;
        Button removeBtn, returnBtn;
        List<string> sections;
        Functions functions;
        string selectedSection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.removeSection);

            sectionSelect = FindViewById<Spinner>(Resource.Id.sectionSpinner);
            removeBtn = FindViewById<Button>(Resource.Id.removeButton);
            returnBtn = FindViewById<Button>(Resource.Id.returnButton);

            functions = new Functions(this, typeof(ManageSections));
            sections = new List<string>();

            InitiateEvents();
        }

        private void InitiateEvents()
        {
            sections = functions.PopulateRequestedList(sections, "section", null);

            sections.Insert(0, "Select a Section");
            sectionSelect.Adapter = functions.DisplayItems(sections);
            sectionSelect.SetSelection(0);

            sectionSelect.ItemSelected += (sender, e) =>
            {
                selectedSection = functions.GetSelectedText(sectionSelect, e.Position);

            };

            removeBtn.Click += delegate
            {


                if (selectedSection.Equals("Select a Section"))
                {
                    Toast.MakeText(this, "Choose a Section.", ToastLength.Long).Show();
                }
                else
                {
                    functions.ConfirmationPopUp("Remove Section", "Are you sure you want to remove the selected section?", "Section Removed.", "Discard Changes", () =>
                    {
                        Thread removeThread = new Thread(() =>
                        {
                            functions.RemoveData("section", selectedSection, null);
                            RunOnUiThread(() =>
                            {
                                sections.Remove(selectedSection);
                                sectionSelect.Adapter = functions.DisplayItems(sections);
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
