using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerNameTxt;

    private void Start()
    {
        GetName();
    }
    public async void GetName()
    {
        string name = await AuthenticationManager.Instance.GetPlayerName();
        playerNameTxt.text = name.Split("#")[0];
    }

}
