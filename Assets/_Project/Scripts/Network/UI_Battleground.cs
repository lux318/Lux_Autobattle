using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Battleground : MonoBehaviour
{
    [SerializeField]
    private GameObject basicCardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Player.Instance.UIOnReady += Test_UIOnReady;
    }

    private void Test_UIOnReady(object sender, Player.DataCardsStruct e)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
