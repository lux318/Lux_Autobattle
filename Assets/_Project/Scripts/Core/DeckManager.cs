using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField]
    private List<CardZone> zones;
    private List<BasicCard> selectedCards;
    private string jsonDeck;
    private BasicCard selectedCard;
    private BasicCard cardToSwitch;

    public BasicCard SelectedCard
    {
        get => selectedCard;
        set
        {
            selectedCard = value;
            OnCardSelected?.Invoke(selectedCard);
        }
    }

    public BasicCard CardToSwitch
    {
        get => cardToSwitch;
        set => cardToSwitch = value;
    }

    public delegate void CardDelegate(BasicCard card);
    public event CardDelegate OnCardSelected;
    public event CardDelegate OnCardMoved;
    public event CardDelegate OnCardPlaced;


    private void Start()
    {
        zones = new List<CardZone>(zones.OrderBy((t) => t.ZoneIndex).ToList());
        SelectedCard = null;
        CardToSwitch = null;
    }

    #region DeckCreation
    public void CardMoved()
    {
        OnCardMoved?.Invoke(selectedCard);
        SelectedCard = null;
    }

    public bool CardPlaced()
    {
        selectedCard.IsOwned = true;
        OnCardPlaced?.Invoke(selectedCard);
        SelectedCard = null;

        //TODO add money check
        return true;
    }

    //Put the selected cards in the deck in the correct order
    public void CreateOrderedDeck()
    {
        selectedCards = new List<BasicCard>();
        foreach (var zone in zones)
        {
            if (zone.ActualCard != null)
                selectedCards.Add(zone.ActualCard);
        }
        
        if (selectedCards.Count > 0)
            SubmitDeck();
    }

    ////////////////////////////////// USE DTO ////////////////////////////////////////
    private void SubmitDeck()
    {
        DeckContainerDTO deckContainerDTo = new DeckContainerDTO();

        deckContainerDTo.deck.DiceResult = 20;
        foreach (var card in selectedCards)
        {
            deckContainerDTo.deck.Cards.Add(new Card(card.ActualStats.cardID, 1)); //TODO add level
        }

        jsonDeck = JsonUtility.ToJson(deckContainerDTo);
        Debug.Log(jsonDeck);

        //Start the matchmaking
        LobbyMatchMakerManager.Instance.CheckForLobbies();
    }

    public string GetJsonDeck()
    {
        return jsonDeck;
    }
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(DeckManager))]
public class DeckManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DeckManager deckManager = (DeckManager)target;

        if (GUILayout.Button("Submit deck"))
            deckManager.CreateOrderedDeck();
    }
}
#endif
