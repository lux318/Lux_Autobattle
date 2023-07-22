using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Text;

public class Player : NetworkBehaviour
{
    [HideInInspector]
    public struct DataCardsStructNetwork : INetworkSerializable
    {
        public string nameCard;
        public int hpCard;
        public int atkCard;
        public int speedCard;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T: IReaderWriter
        {
            serializer.SerializeValue(ref nameCard);
            serializer.SerializeValue(ref hpCard);
            serializer.SerializeValue(ref atkCard);
            serializer.SerializeValue(ref speedCard);
        }
    }

    [HideInInspector]
    public struct DataCardsStruct
    {
        public string nameCard;
        public int hpCard;
        public int atkCard;
        public int speedCard;

        public void DataCards(string _nameCard, int _hpCard, int _atkCard, int _speedCard)
        { 
            this.nameCard = _nameCard;
            this.hpCard = _hpCard;
            this.atkCard = _atkCard;
            this.speedCard = _speedCard;
        }

    }
    public static Player Instance { get; private set; }

    public event EventHandler<DataCardsStructNetwork> OnReady;
    public event EventHandler<DataCardsStruct> UIOnReady;


    private string namePlayer;

    public class OnReadyEventArgs: EventArgs
    {
        public int idCard;
        public string nameCard;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Instance = this;

        Debug.Log("Spawn By Network!");

    }


    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (BasicCard card in DeckManager.Instance.SelectedCards)
            {
                OnReady?.Invoke(Instance, new DataCardsStructNetwork
                {
                    nameCard = card.actualStats.cardName,
                    hpCard = card.actualStats.hp,
                    atkCard = card.actualStats.atk,
                    speedCard = card.actualStats.speed
                });
            }
        }
    }


    [ServerRpc]
    public void MessageServerRpc()
    {
        Debug.Log("TestServerRpc" + OwnerClientId);
    }
    

    public string GetNamePlayer() { return namePlayer; }

}
