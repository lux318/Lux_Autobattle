using System;
using UnityEditor;
using UnityEngine;

public class PlayerClientController : MonoBehaviour
{
    public static PlayerClientController Instance { get; private set; }

    private ulong clientId;
    private string namePlayer;

    public event EventHandler<InfoClientArgs> OnSpawnClient;

    private void Awake()
    {
        Instance = this;
    }

    public class InfoClientArgs : EventArgs
    {
        public ulong id;
        public string namePlayer;
    }


    public void setIdAndNamePlayer(ulong _clientId, string _namePlayer)
    {
        clientId = _clientId;
        namePlayer = _namePlayer;

    }
}
