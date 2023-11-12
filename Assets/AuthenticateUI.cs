using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour
{
    [SerializeField] private Button authButton;
    [SerializeField] private InputField inputNamePlayer;
    // Start is called before the first frame update
    private void Start()
    {
        authButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.Authenticate();
            gameObject.SetActive(false);
        });
    }
}
