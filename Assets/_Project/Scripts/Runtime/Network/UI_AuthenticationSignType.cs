using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AuthenticationSignType : MonoBehaviour
{

    private string usernameValue;
    private string passwordValue;

    [SerializeField] private Button btnSignType;
    [SerializeField] private Button btnClose;
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;


    void Awake()
    {
        btnClose.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        gameObject.SetActive(false);
    }
    
    

    public void AddListenerSignIn()
    {
        btnSignType.GetComponentInChildren<TextMeshProUGUI>().text = "Sign In";

        btnSignType.onClick.AddListener(async () =>
        {
            usernameValue = usernameInput.text;
            passwordValue = passwordInput.text;
            await AuthenticationManager.Instance.SignInWithUsernamePasswordAsync(usernameValue, passwordValue);
            GameManager.Instance.ChangeScene("MenuScene");
        });
    }


    public void AddListenerSignUp()
    {
        btnSignType.GetComponentInChildren<TextMeshProUGUI>().text = "Sign Up";

        btnSignType.onClick.AddListener(async () =>
        {
            usernameValue = usernameInput.text;
            passwordValue = passwordInput.text;
            await AuthenticationManager.Instance.SignUpWithUsernamePasswordAsync(usernameValue, passwordValue);
            GameManager.Instance.ChangeScene("MenuScene");
        });
    }

}
