using System;
using CocosSharp;
using Microsoft.Xna.Framework;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;


namespace Box2DTest.Android
{
    [Activity(
        Label = "Box2DTest",
        AlwaysRetainTaskState = true,
        Icon = "@drawable/icon",
        Theme = "@android:style/Theme.NoTitleBar",
        ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleInstance,
        MainLauncher = true,
        ConfigurationChanges =  ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
    ]
    public class MainActivity : AndroidGameActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var application = new CCApplication();
            application.ApplicationDelegate = new Box2DTestClass();
            SetContentView(application.AndroidContentView);
            application.StartGame();
        }
    }
}

