using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerReady : NetworkBehaviour
{


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Player.Instance.OnReady += Test_OnReady;
    }

    private void Test_OnReady(object sender, Player.OnReadyEventArgs e)
    {
        SendInfoServerRpc(e.nameCard, e.idCard);
    }

    [ServerRpc]
    private void SendInfoServerRpc(string nameCard, int idCard)
    {
        Debug.Log("User: " + OwnerClientId + " Id Card: " + idCard + " Name Card: " + nameCard);
    }
}