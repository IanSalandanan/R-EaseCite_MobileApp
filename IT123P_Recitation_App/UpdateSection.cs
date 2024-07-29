using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.Threading;

namespace IT123P_Recitation_App
{
    [Activity(Label = "UpdateSection")]
    public class UpdateSection : Activity
    {
        private Spinner sectionSpinner;
        private EditText editSection;
        private Button updateBtn, returnBtn;
        private Functions functions;
        private string selectedSection, updatedSection;
        private List<string> sections;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.updateSection);

            sectionSpinner = FindViewById<Spinner>(Resource.Id.sectionSpinner);
            editSection = FindViewById<EditText>(Resource.Id.updateSectionInput);
            updateBtn = FindViewById<Button>(Resource.Id.updateButton);
            returnBtn = FindViewById<Button>(Resource.Id.returnButton);

            functions = new Functions(this, typeof(ManageSections));
            sections = new List<string>();

            InitiateEvents();
        }

        public void InitiateEvents()
        {
            sections = functions.PopulateRequestedList(sections, "section", null);
            sections.Insert(0, "Select a Section");
            sectionSpinner.Adapter = functions.DisplayItems(sections);
            sectionSpinner.SetSelection(0);

            sectionSpinner.ItemSelected += (sender, e) =>
            {
                selectedSection = functions.GetSelectedText(sectionSpinner, e.Position);
                editSection.Text = selectedSection;
            };

            updateBtn.Click += delegate
            {
                updatedSection = editSection.Text;
                if (selectedSection.Equals("Select a Section"))
                {
                    Toast.MakeText(this, "Choose a Section.", ToastLength.Long).Show();
                }
                else if (string.IsNullOrWhiteSpace(updatedSection))
                {
                    Toast.MakeText(this, "Textfield is Empty.", ToastLength.Short).Show();
                }
                else if (!selectedSection.Equals(updatedSection))
                {
                    functions.ConfirmationPopUp("Update Section", "Are you sure you want to update the selected section?", "Section Updated.", "Discard Changes", () =>
                    {
                        Thread updateThread = new Thread(() =>
                        {
                            functions.UpdateData("section", updatedSection, selectedSection, null);

                            RunOnUiThread(() =>
                            {
                                sections.Clear();
                                sections = functions.PopulateRequestedList(sections, "section", null);
                                sections.Insert(0, "Select a Section");
                                sectionSpinner.Adapter = functions.DisplayItems(sections);
                                sectionSpinner.SetSelection(sections.IndexOf(updatedSection));
                            });
                        });
                        updateThread.Start();
                    });
                }
                else if (selectedSection.Equals(updatedSection))
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
