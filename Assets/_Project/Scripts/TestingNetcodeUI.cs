using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] Button startHostBtn;
    [SerializeField] Button startServerBtn;
    [SerializeField] Button startClientBtn;
    [SerializeField] TMP_InputField inputUserName;


    private void Awake()
    {
        startHostBtn.onClick.AddListener(() =>
        {
            Debug.Log("HOST");
            NetworkManager.Singleton.StartHost();
            Hide();
        });

        startServerBtn.onClick.AddListener(() =>
        {
            Debug.Log("Server");
            NetworkManager.Singleton.StartServer();
            Hide();
        });

        startClientBtn.onClick.AddListener(() =>
        {
            Debug.Log("Client");
            NetworkManager.Singleton.StartClient();
            Hide();
        });

    }


    void Hide()
    {
        gameObject.SetActive(false);
    }
}
