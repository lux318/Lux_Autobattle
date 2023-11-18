using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Text;
using QFSW.QC;

public class PlayerLocal : NetworkBehaviour
{
    
    public static PlayerLocal Instance { get; private set; }

    public event EventHandler<DeckDTO> OnReady;
    
    private string namePlayer;

    private string jsonDeck;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Instance = this;
        Debug.Log("Spawn By Network!");
        jsonDeck = DeckManager.Instance.GetJsonDeck();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    [Command]
    public void SendDeckInfo()
    {
        if (IsServer)
        {
            SendDeckInfoClientRpc(jsonDeck);
        }
        else
        {
            SendDeckInfoServerRpc(jsonDeck);
        }
    }


    [ClientRpc]
    private void SendDeckInfoClientRpc(string jsonData)
    {
        if (IsOwner) return;
        DeckContainerDTO deckDto = JsonUtility.FromJson<DeckContainerDTO>(jsonData);
        Debug.Log("Server sent this : " + deckDto.ToString());
    }

    [ServerRpc]
    private void SendDeckInfoServerRpc(string jsonData)
    {
        DeckContainerDTO deckDto = JsonUtility.FromJson<DeckContainerDTO>(jsonData);
        Debug.Log("Client sent this : " + deckDto);
    }


    public string GetNamePlayer() { return namePlayer; }

}
