using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField]
    public List<CardZone> zones;
    public Dictionary<int,DeckConstructionCard> selectedCards;
    private string jsonDeck;
    private DeckConstructionCard selectedCard;
    private DeckConstructionCard cardToSwitch;

    public DeckConstructionCard SelectedCard
    {
        get => selectedCard;
        set
        {
            selectedCard = value;
            OnCardSelected?.Invoke(selectedCard);
        }
    }

    public DeckConstructionCard CardToSwitch
    {
        get => cardToSwitch;
        set => cardToSwitch = value;
    }

    public delegate void CardDelegate(DeckConstructionCard card);
    public delegate void IDCardDelegate(int cardId, int cardPosition);
    public event IDCardDelegate OnDeckCreating;
    public event CardDelegate OnCardSelected;
    public event CardDelegate OnCardMoved;
    public event CardDelegate OnCardPlaced;
    public event CardDelegate OnCardSold;

    //Debug Event
    public UnityEvent deckSubmitted;


    private void Start()
    {
        zones = new List<CardZone>(zones.OrderBy((t) => t.ZoneIndex).ToList());
        SelectedCard = null;
        CardToSwitch = null;

        Economy.Instance.OnEconomyBuy += Events_OnEconomyBuyEventArgs;

        if(!PlayerClientController.Instance.GetSession().GetNewSession())
        {
            LoadJsonDeck(PlayerClientController.Instance.GetSession().GetTeam());
            //LoadJsonDeck("{\"deck\":{\"cards\":[{\"cardID\":0,\"cardLevel\":1,\"cardDiceRoll\":11},{\"cardID\":0,\"cardLevel\":1,\"cardDiceRoll\":16},{\"cardID\":2,\"cardLevel\":1,\"cardDiceRoll\":3}]}}");
        }
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            LoadJsonDeck("{\"deck\":{\"cards\":[{\"cardID\":1,\"cardLevel\":1,\"cardDiceRoll\":9,\"cardPosition\":0},{\"cardID\":0,\"cardLevel\":1,\"cardDiceRoll\":9,\"cardPosition\":1},{\"cardID\":0,\"cardLevel\":1,\"cardDiceRoll\":12,\"cardPosition\":2}]}}");
        }
    }
    public void AddCardInZone(DeckConstructionCard card, int zonePosition)
    {
        
        foreach (var zone in zones)
        {
            if (zone.ZoneIndex == zonePosition)
            {
                zone.ActualCard = card;
                zone.UpdateGraphichs();
            }
        }
    }

    private void LoadJsonDeck(string jsonDeck)
    {
        DeckContainerDTO deckDTO = JsonConvert.DeserializeObject<DeckContainerDTO>(jsonDeck);
        foreach (Card card in deckDTO.deck.Cards)
        {
            OnDeckCreating?.Invoke(card.cardID, card.cardPosition);
        }
        
    }

    private void Events_OnEconomyBuyEventArgs(object sender, Economy.OnEconomySpentEventArgs e)
    {
        CardPlaced();
    }

    #region DeckCreation
    public void CardMoved()
    {
        OnCardMoved?.Invoke(selectedCard);
        SelectedCard = null;
    }

    public bool CardPlaced()
    {
        if (selectedCard != null)
        {
            selectedCard.IsOwned = true;
            OnCardPlaced?.Invoke(selectedCard);
            SelectedCard = null;

            //TODO add money check

            return true;
        }
        return false;
    }

    public void CardSold()
    {
        OnCardSold?.Invoke(selectedCard);
        Destroy(SelectedCard);
        SelectedCard = null;
    }

    //Put the selected cards in the deck in the correct order
    public void CreateOrderedDeck()
    {
        selectedCards = new Dictionary<int, DeckConstructionCard>();
        foreach (var zone in zones)
        {
            if (zone.ActualCard != null)
                selectedCards.Add(zone.ZoneIndex, zone.ActualCard);
        }
        
        if (selectedCards.Count > 0)
            SubmitDeck();
    }

    ////////////////////////////////// USE DTO ////////////////////////////////////////
    private void SubmitDeck()
    {
        DeckContainerDTO deckContainerDTo = new DeckContainerDTO();

        //Randomize seed
        Random.InitState((int)System.DateTime.Now.Ticks);

        foreach (var card in selectedCards)
        {
            deckContainerDTo.deck.Cards.Add(new Card(card.Value.ActualStats.cardID, 1, Random.Range(1, 20), card.Key)); //TODO add level
        }

        jsonDeck = JsonUtility.ToJson(deckContainerDTo);
        PlayerClientController.Instance.GetSession().SetTeam(jsonDeck);
        Debug.Log(jsonDeck);


        //Debug event
        deckSubmitted?.Invoke();

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