using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerReady : NetworkBehaviour
{
    public BasicCard cardPrefab;

    void Start()
    {
        if (IsClient)
        {
            Debug.Log("Hello client");
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Client id: " + clientId);
        }
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        PlayerLocal.Instance.OnReady += Test_OnReady;
    }

    private void Test_OnReady(object sender, PlayerLocal.DataCardsStructNetwork e)
    {
        //Aggiungi un check
        SendInfoServerRpc(e);
    }

    //private void Test_OnReady(object sender, Player.OnReadyEventArgs e)
    //{
    //    SendInfoServerRpc(e.nameCard, e.idCard);
    //}

    [ServerRpc]
    private void SendInfoServerRpc(PlayerLocal.DataCardsStructNetwork e)
    { 
        BasicCardScriptable basicCardScriptable = new BasicCardScriptable();
        basicCardScriptable.cardName = e.nameCard;
        basicCardScriptable.hp = e.hpCard;
        basicCardScriptable.atk = e.atkCard;
        basicCardScriptable.speed = e.speedCard;
        var basicCard = Instantiate(cardPrefab);
        basicCard.Initialize(basicCardScriptable);
        basicCard.gameObject.SetActive(false);
        FindObjectOfType<BattleManager>().aiCards.Add(basicCard);
        Debug.Log(String.Format("User: " + OwnerClientId + " Name Card: {0} \n Hp Card {1} \n Atk Card {2} \n Speed Card {3}", e.nameCard, e.hpCard, e.atkCard, e.speedCard));
    }
}