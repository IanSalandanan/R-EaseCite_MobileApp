using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Threading;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

namespace IT123P_Recitation_App
{
    [Activity(Label = "RemoveStudent")]
    public class RemoveStudent : Activity
    {
        Spinner studentSelect;
        Button removeBtn, returnBtn;
        List<string> students;
        Functions functions;
        string selectedStudent, selectedSection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.removeStudent);

            studentSelect = FindViewById<Spinner>(Resource.Id.studentSpinner);
            removeBtn = FindViewById<Button>(Resource.Id.removeButton);
            returnBtn = FindViewById<Button>(Resource.Id.returnButton);

            functions = new Functions(this, typeof(ManageSections));
            students = new List<string>();

            InitiateEvents();
        }

        private void InitiateEvents()
        {
            Bundle extras = Intent.Extras;

            if (extras != null)
            {
                selectedSection = extras.GetString("section");
            }
            else
            {
                Toast.MakeText(this, "bundle_error", ToastLength.Long).Show();
            }

            students = functions.PopulateRequestedList(students, "student", selectedSection);
            students.Insert(0, "Select a Student");
            studentSelect.Adapter = functions.DisplayItems(students);
            studentSelect.SetSelection(0);

            studentSelect.ItemSelected += (sender, e) =>
            {
                selectedStudent = functions.GetSelectedText(studentSelect, e.Position);
            };

            removeBtn.Click += delegate
            {
                if (selectedStudent.Equals("Select a Student"))
                {
                    Toast.MakeText(this, "Choose a Student.", ToastLength.Long).Show();
                }
                else
                {
                    functions.ConfirmationPopUp("Remove Student", "Are you sure you want to remove the selected student?", "Student Removed.", "Discard Changes", () =>
                    {
                        Thread removeThread = new Thread(() =>
                        {
                            functions.RemoveData("student", selectedStudent, selectedSection);
                            RunOnUiThread(() =>
                            {
                                students.Remove(selectedStudent);
                                studentSelect.Adapter = functions.DisplayItems(students);
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
