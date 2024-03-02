using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    public static LobbyCreateUI Instance { get; private set; }



    [SerializeField] private Button lobbyCreateButton;
    [SerializeField] private Button lobbyNameButton;
    [SerializeField] private Button lobbyStatusButton;
    [SerializeField] private Button lobbyMaxPlayersButton;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyStatusText;
    [SerializeField] private TextMeshProUGUI lobbyPlayersText;
    [SerializeField] private int maxPlayers = 2;


  

    private string lobbyName;
    private int numberPlayers;
    private bool isPrivateLobby;

    private void Awake()
    {
        Instance = this;

        lobbyCreateButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.CreateLobby(lobbyName, numberPlayers, isPrivateLobby);
            Hide();
        });


        //Change Name Lobby
        lobbyNameButton.onClick.AddListener(() =>
        {
            UI_Blocker.Show_Static();
            UI_InputWindow.Show_Static("Lobby Name", "", "ABCDEFGIJKLMNOPQRSTUVXYWZabcdefghijklmnopqrstuvxywz1234567890-_ ", 20, () =>
            {
                // Cancel
                UI_Blocker.Hide_Static();
            }, (string nameText) =>
            {
                // Ok
                UI_Blocker.Hide_Static();
                lobbyName = nameText;
                UpdateText();
            });
        });

        lobbyStatusButton.onClick.AddListener(() =>
        {
            isPrivateLobby = !isPrivateLobby;
            UpdateText();
        });

        lobbyMaxPlayersButton.onClick.AddListener(() =>
        {
            UI_Blocker.Show_Static();
            UI_InputWindow.Show_Static("Max Players", maxPlayers, () => {
                // Cancel
                UI_Blocker.Hide_Static();
            }, (int numberPlayers) => {
                // Ok
                this.numberPlayers = numberPlayers;
                UI_Blocker.Hide_Static();
            });
        });

        Hide();
    }

    private void UpdateText()
    {
        lobbyNameText.text = lobbyName;
        lobbyPlayersText.text = maxPlayers.ToString();
        lobbyStatusText.text = isPrivateLobby ? "Private" : "Public";
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        lobbyName = "My Lobby";
        isPrivateLobby = false;
        numberPlayers = maxPlayers;

        UpdateText();
    }
}