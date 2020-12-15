using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase;
using System;
using UnityEngine.SceneManagement;
using System.Reflection;

public class FirebaseInit : MonoBehaviour
{
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
               var app = Firebase.FirebaseApp.DefaultInstance;
                InitializeFirebase();
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
               Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });


    }

    // Update is called once per frame
    void InitializeFirebase()
    {
 
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        FirebaseAnalytics.SetMinimumSessionDuration(new TimeSpan(0, 0, 10));
        FirebaseAnalytics.SetCurrentScreen("Main menu", "main screen");
    }
}
