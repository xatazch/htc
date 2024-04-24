using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance;

    public UnityEvent SignIn;

    private  void Awake()
    {
        Instance = this;

        Login();
    }


    private async void Login()
    {
        try
        {
            LogManager.Instance.AddLog("Authenticating...");
            InitializationOptions options = new InitializationOptions();
            await UnityServices.InitializeAsync(options);

            LogManager.Instance.AddLog("Checking if user is signed in");
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                LogManager.Instance.AddLog("User Not signed in. Trying to sign in.");
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                LogManager.Instance.AddLog("Signed in as user " + AuthenticationService.Instance.PlayerId);
            }

            LogManager.Instance.AddLog("Invoke Login");
            SignIn.Invoke();
        }
        catch (Exception ex)
        {
            LogManager.Instance.AddLog("Error: " + ex.Message.ToString());
        }
    }
}
