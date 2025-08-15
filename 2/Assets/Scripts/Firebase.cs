using UnityEngine;
using Firebase.Extensions;
using Firebase;
using System;
using Firebase.Analytics;

public class Firevase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
    public void LogMeButton()
    {
        FirebaseAnalytics.LogEvent("Log_me_button_pressed");
    }
    public void PressNumberButton(int number)
    {
        FirebaseAnalytics.LogEvent("Press_Number_button_pressed", new Parameter("ButtonNumber", number));
    }
}
