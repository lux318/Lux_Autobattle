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
    }

    //private void Test_OnReady(object sender, PlayerLocal. e)
    //{
    //    //Aggiungi un check
    //    DeckContainerDTO playerDeck = new DeckContainerDTO();
    //    playerDeck.deck.Cards.Add()
    //}

    //private void Test_OnReady(object sender, Player.OnReadyEventArgs e)
    //{
    //    SendInfoServerRpc(e.nameCard, e.idCard);
    //}
}