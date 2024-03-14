using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AuthenticationMenu : MonoBehaviour
{
    [SerializeField] 
    private Button btnAnonymousSignIn;
    [SerializeField] 
    private Button btnUserPwdSignIn;
    [SerializeField] 
    private Button btnUserPwdSignUp;

    [SerializeField]
    private GameObject panelUserPwd;

    // Start is called before the first frame update
    void Start()
    {
        btnAnonymousSignIn.onClick.AddListener(async () => 
        {
            await AuthenticationManager.Instance.SignInAnonymouslyAsync();
            GameManager.Instance.ChangeScene("MenuScene");
        });

        btnUserPwdSignIn.onClick.AddListener(() => OpenModalSignType(true));
        btnUserPwdSignUp.onClick.AddListener(() => OpenModalSignType(false));
    }

    void OpenModalSignType(bool isSignIn)
    {
        panelUserPwd.SetActive(true);
        //Sign In metodo per gestire listener sign up

        if(isSignIn)
            panelUserPwd.GetComponent<UI_AuthenticationSignType>().AddListenerSignIn();
        else
            panelUserPwd.GetComponent<UI_AuthenticationSignType>().AddListenerSignUp();
    }
}