using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System.Threading;
using static Android.Media.TV.TvContract.Channels;

namespace IT123P_Recitation_App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button startBtn, subjectsBtn, sectionsBtn, exitBtn;
        private Functions functions;
        private ImageView logo;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.lobby);

            startBtn = FindViewById<Button>(Resource.Id.startBtn);
            subjectsBtn = FindViewById<Button>(Resource.Id.subjectsBtn);
            sectionsBtn = FindViewById<Button>(Resource.Id.sectionsBtn);
            exitBtn = FindViewById<Button>(Resource.Id.exitBtn);
            logo = FindViewById<ImageView>(Resource.Id.logo);

            logo.SetImageResource(Resource.Drawable.IT123PIcon);

            functions = new Functions(this, typeof(SetRound));
            InitiateEvents();
        }
        protected void InitiateEvents()
        {
            startBtn.Click += delegate { functions.NextActivity(); };
            subjectsBtn.Click += delegate { functions = new Functions(this, typeof(ManageSubjects)); functions.NextActivity(); };
            sectionsBtn.Click += delegate { functions = new Functions(this, typeof(ManageSections)); functions.NextActivity(); };
            exitBtn.Click += delegate 
            {
                functions.ConfirmationPopUp("Exit Application", "Are you sure you want to exit?", "UI Thread Disposed", "Glad to have you back!", 
                () => { FinishAffinity(); });
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}