using QFSW.QC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class AuthenticationManager : Singleton<AuthenticationManager>
{

    private const string DEV_ENV = "develop";

    protected override async void Awake()
    {
        base.Awake();
        var options = new InitializationOptions();

        options.SetEnvironmentName(DEV_ENV);
        try
        {
            await UnityServices.InitializeAsync();
            if(UnityServices.State == ServicesInitializationState.Initialized)
                await SignInCachedUserAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        DontDestroyOnLoad(gameObject);
    }
    public async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");
            await UpdatePlayerName(username);
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }


    public async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public async Task UpdatePlayerName(string username)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            Debug.Log("Player name aggiornato");
        } catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        } catch(RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    public async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    async Task SignInCachedUserAsync()
    {
        // Nessun utente nella cache
        if (!AuthenticationService.Instance.SessionTokenExists)
        {
            return;
        }

        // Sign in Anonymously
        // Invocherà l'utente in cache
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            GameManager.Instance.ChangeScene("MenuScene");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }


    public async Task<String> GetPlayerName()
    {
        
        if (AuthenticationService.Instance.IsSignedIn)
        {
            try
            {
                return await AuthenticationService.Instance.GetPlayerNameAsync();
            } catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            } catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }

        return null;
    }

    [Command]
    void CheckStates()
    {
        Debug.Log($"Is SignedIn: {AuthenticationService.Instance.IsSignedIn}");
        Debug.Log($"Is Authorized: {AuthenticationService.Instance.IsAuthorized}");
        Debug.Log($"Is Expired: {AuthenticationService.Instance.IsExpired}");
    }
}