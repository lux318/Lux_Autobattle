using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using static LobbyManager;
using Unity.Services.Authentication;
using QFSW.QC;
using Unity.Services.Core;
using System;

public class LobbyMatchMakerManager : MonoBehaviour
{
    public static LobbyMatchMakerManager Instance { get; private set; }

    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string RELAY_CODE = "RELAY_CODE";


    private Lobby joinedLobby;
    private float lobbyUpdateTimer;
    private float heartBeatTimer;

    private string playerName;

    private bool isGameStared = false;

    private async void Start()
    {
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile("Matteo");

        await UnityServices.InitializeAsync(initializationOptions);

        if(!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public void Play()
    {
        CheckForLobbies();
    }

    private void Update()
    {
        HandleLobbyHeartBeat();
        HandleLobbyPollForUpdates();
        HandleStartGame();
    }

    private void HandleStartGame()
    {
        if(IsLobbyHost() && joinedLobby.AvailableSlots == 0 && !isGameStared)
        {
            StartGame();
        }
    }

    private async void HandleLobbyHeartBeat()
    {
        if (IsLobbyHost())
        {
            heartBeatTimer = Time.deltaTime;
            if (heartBeatTimer < 0f)
            {
                float heartBeatTimerMax = 15f;
                heartBeatTimer = heartBeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    public async void CreateLobby()
    {
        Debug.Log("Create Lobby");
        try
        {
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {RELAY_CODE, new DataObject(DataObject.VisibilityOptions.Member, "0")}
                }
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync("LobbyCustom", 2, options);

            joinedLobby = lobby;

            Debug.Log("Lobby created: " + lobby.Id);
            //OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }


    public async void JoinLobby(Lobby lobby)
    {
        try
        {
            Player player = GetPlayer();
            joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions { Player = player });

            if(joinedLobby.AvailableSlots == 0)
                StartGame();
            Debug.Log("Joined Lobby");
            //OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }


    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                //OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                if (!IsPlayerInLobby())
                {
                    //OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
                    joinedLobby = null;
                }

                if (joinedLobby.Data[RELAY_CODE].Value != "0")
                {
                    if(!IsLobbyHost())
                    {
                        Relay.Instance.JoinRelay(joinedLobby.Data[RELAY_CODE].Value);
                        Debug.Log("Joined");
                    }
                    joinedLobby = null;
                }
            }
        }
    }


    private Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
            { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
        });
    }

    private bool IsPlayerInLobby()
    {
        if (joinedLobby != null && joinedLobby.Players != null)
        {
            foreach (Player player in joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    // This player is in this lobby
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }


    public bool IsLobbyFull()
    {
        return joinedLobby != null && joinedLobby.AvailableSlots == 0;
    }


    public async void StartGame()
    {
        if (IsLobbyHost())
        {
            try
            {
                Debug.Log("Start");
                isGameStared = true;
                string relayCode = await Relay.Instance.CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        {RELAY_CODE, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });

                joinedLobby = lobby;
            } catch (LobbyServiceException e)
            {
                Debug.LogError(e.Message);
                isGameStared=false;
            }
        }
    }


    public async void CheckForLobbies()
    {

        QueryLobbiesOptions query = new QueryLobbiesOptions
        {
            Filters = new List<QueryFilter>
            {
                new QueryFilter (
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            }
        };

        QueryResponse response = await LobbyService.Instance.QueryLobbiesAsync(query);
        List<Lobby> lobbies = response.Results;

        if (lobbies.Count > 0)
        {
            foreach (Lobby l in lobbies)
            {
                JoinLobby(l);
                return;
            }

            CreateLobby();
        }
        else
            CreateLobby();
    }

}
