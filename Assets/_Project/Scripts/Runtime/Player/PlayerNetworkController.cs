using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using QFSW.QC;
using Unity.Services.Relay.Models;

public class PlayerNetworkController : NetworkBehaviour
{
    
    public static PlayerNetworkController Instance { get; private set; }
    public event EventHandler<DeckDTO> OnReady;
    private string namePlayer;
    private string jsonDeck;



	//Lo spawn fa partire in "automatico" l'invio dei dati
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Instance = this;
        namePlayer = "Iron Duck " + UnityEngine.Random.Range(0, 100);
        Debug.Log("Spawn By Network!");
        jsonDeck = DeckManager.Instance.GetJsonDeck();
		
		
		//Funzione automatica per fare le chiamate (commentare per disabilitare l'automazione)
        RpcNotifyServerSpawnServerRpc(NetworkManager.Singleton.LocalClientId); 

        //Automazione:
        //Se sei client, manda rpc a server che mandi i dati a tutti
    }
	

	//Funzione che dovrebbe chiamare un client ogni volta che spawna
    [ServerRpc(RequireOwnership = false)] 
    void RpcNotifyServerSpawnServerRpc(ulong clientId)
    {
        Debug.Log("Sono spawnato client " + clientId);
        if(IsOwner)
            StartSendInfo();
    }
    
	//Funzione del server
    private void StartSendInfo()
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            CallSendDeckInfoClientRpc();
            CallSendDeckInfoServerRpc(); //Add server rpc call
        }
    }


    [ClientRpc]
    private void CallSendDeckInfoClientRpc()
    {
        SendDeckInfo();
    }
    [ServerRpc]
    private void CallSendDeckInfoServerRpc()
    {
        SendDeckInfo();
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

        SetUpBattle(deckDto);
    }

	//Funzione che da l'errore di richiesta ownership 
    [ServerRpc(RequireOwnership = false)]
    private void SendDeckInfoServerRpc(string jsonData)
    {
        DeckContainerDTO deckDto = JsonUtility.FromJson<DeckContainerDTO>(jsonData);
        Debug.Log("Client sent this : " + deckDto);

        SetUpBattle(deckDto);
    }


    public string GetNamePlayer() { return namePlayer; }

    private void SetUpBattle(DeckContainerDTO remoteDTO)
    {
        DeckDTO localDeck = JsonUtility.FromJson<DeckContainerDTO>(jsonDeck).deck;
        DeckDTO remoteDeck = remoteDTO.deck;
        BattleManager.Instance.Init(localDeck, remoteDeck);
    }

}
