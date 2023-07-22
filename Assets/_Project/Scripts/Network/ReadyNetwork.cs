using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ReadyNetwork : NetworkBehaviour
{

    [SerializeField] private TextMeshProUGUI text;


    private void Start()
    {
        ButtonPublisher buttonPublisher = FindObjectOfType<ButtonPublisher>();
        buttonPublisher.OnButtonPressed += Testing_OnButtonPressed;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            ReadyServerRpc("Hello World");
        }
    }

    private void Testing_OnButtonPressed(object sender, string e)
    {
        ReadyServerRpc(e);
    }

    [ServerRpc]
    public void ReadyServerRpc(string message)
    {
        text.text = message;
        Debug.Log("TestServerRpc" + OwnerClientId);
    }
}
