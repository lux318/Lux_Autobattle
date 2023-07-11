using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Text;

public class Player : NetworkBehaviour
{


    public static Player Instance { get; private set; }

    public event EventHandler<OnReadyEventArgs> OnReady;

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
            OnReady?.Invoke(Instance, new OnReadyEventArgs{
                idCard = 99,
                nameCard = "Barbaro"
            });
        }

        if (Input.GetKeyDown(KeyCode.A))
            MovementServerAuth();
    }


    void MovementServerAuth()
    {
        
        MovementServerRPC();
    }


    [ServerRpc(RequireOwnership = false)]
    void MovementServerRPC()
    {
        transform.position = transform.position - new Vector3(1, 0, 0);
    }



    [ServerRpc]
    public void MessageServerRpc()
    {
        Debug.Log("TestServerRpc" + OwnerClientId);
    }
    

    public string GetNamePlayer() { return namePlayer; }

}
