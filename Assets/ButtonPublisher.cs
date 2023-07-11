using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ButtonPublisher : MonoBehaviour
{

    public event EventHandler<string> OnButtonPressed;
    // Start is called before the first frame update

    public string message;

    public void OnClick()
    {
        OnButtonPressed?.Invoke(this, "Ciao Sono il client");
    }
}
