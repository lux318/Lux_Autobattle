using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Text;
using QFSW.QC;
using static LobbyManager;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;

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
		
        PlayerClientController.Instance.setIdAndNamePlayer(NetworkManager.Singleton.LocalClientId, namePlayer);
		
		//Funzione automatica per fare le chiamate (commentare per disabilitare l'automazione)
        RpcNotifyServerSpawnServerRpc(NetworkManager.Singleton.LocalClientId); 
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
        }
    }


    [ClientRpc]
    private void CallSendDeckInfoClientRpc()
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
    }

	//Funzione che da l'errore di richiesta ownership 
    [ServerRpc(RequireOwnership = false)]
    private void SendDeckInfoServerRpc(string jsonData)
    {
        DeckContainerDTO deckDto = JsonUtility.FromJson<DeckContainerDTO>(jsonData);
        Debug.Log("Client sent this : " + deckDto);
    }


    public string GetNamePlayer() { return namePlayer; }

}
