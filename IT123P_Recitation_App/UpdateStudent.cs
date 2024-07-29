using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IT123P_Recitation_App
{
    [Activity(Label = "UpdateStudent")]
    public class UpdateStudent : Activity
    {
        Spinner studentSelect;
        EditText editStudentName;
        Button updateBtn, returnBtn;
        List<string> students;
        Functions functions;
        string selectedStudent, updatedStudent, selectedSection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.updateStudent);

            studentSelect = FindViewById<Spinner>(Resource.Id.studentSpinner);
            editStudentName = FindViewById<EditText>(Resource.Id.updateStudentInput);
            updateBtn = FindViewById<Button>(Resource.Id.updateButton);
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
                return;
            }

            students = functions.PopulateRequestedList(students, "student", selectedSection);
            students.Insert(0, "Select a Student");
            studentSelect.Adapter = functions.DisplayItems(students);
            studentSelect.SetSelection(0);

            studentSelect.ItemSelected += (sender, e) =>
            {
                selectedStudent = functions.GetSelectedText(studentSelect, e.Position);
                editStudentName.Text = selectedStudent;
            };

            updateBtn.Click += delegate
            {
                updatedStudent = editStudentName.Text;
                if (selectedStudent.Equals("Select a Student"))
                {
                    Toast.MakeText(this, "Choose a Student.", ToastLength.Long).Show();
                }
                else if (string.IsNullOrWhiteSpace(updatedStudent))
                {
                    Toast.MakeText(this, "Textfield is Empty.", ToastLength.Short).Show();
                }
                else if (!selectedStudent.Equals(updatedStudent))
                {
                    functions.ConfirmationPopUp("Update Student", "Are you sure you want to update the selected student?", "Student Updated.", "Discard Changes", () =>
                    {
                        Thread updateThread = new Thread(() =>
                        {
                            functions.UpdateData("student", updatedStudent, selectedStudent, selectedSection);
                            RunOnUiThread(() =>
                            {
                                students.Clear();
                                students = functions.PopulateRequestedList(students, "student", selectedSection);
                                students.Insert(0, "Select a Student");
                                studentSelect.Adapter = functions.DisplayItems(students);
                                studentSelect.SetSelection(students.IndexOf(updatedStudent));
                            });
                        });
                        updateThread.Start();
                    });
                }
                else if (selectedStudent.Equals(updatedStudent))
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

