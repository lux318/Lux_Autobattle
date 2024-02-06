using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Economy;

public class UI_Economy : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI uiValue;


    // Start is called before the first frame update
    void Start()
    {
        Economy.Instance.OnEconomyInit += Events_OnEconomyUpdateEventArgs;
        Economy.Instance.OnEconomyRoll += Events_OnEconomyUpdateEventArgs;
        Economy.Instance.OnEconomyBuy += Events_OnEconomyUpdateEventArgs;
    }



    private void Events_OnEconomyUpdateEventArgs(object sender, OnEconomySpentEventArgs e)
    {
        UpdateValue(e.updateEconomy);
    }

    private void UpdateValue(int economy)
    {
        uiValue.text = economy.ToString();
    }
}