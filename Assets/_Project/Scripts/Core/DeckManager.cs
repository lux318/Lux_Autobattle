using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField]
    private int maxCardsCount;

    private List<BasicCard> selectedCards;

    public UnityEvent LastCardAdded;
    public UnityEvent LastCardRemoved;

    public List<BasicCard> SelectedCards { get => selectedCards; }

    private void Start()
    {
        selectedCards= new List<BasicCard>();
    }

    public void AddCard(BasicCard card)
    {
        selectedCards.Add(card);
        if (maxCardsCount == selectedCards.Count)
            LastCardAdded?.Invoke();
    }

    public void RemoveCard(BasicCard card)
    {
        selectedCards.Remove(card);
        if (maxCardsCount - 1 == selectedCards.Count)
            LastCardRemoved?.Invoke();
    }

    public void ClearCards()
    {
        selectedCards.Clear();
    }

    public bool HasReachMaxLength()
    {
        return selectedCards.Count == maxCardsCount;
    }

    ////////////////////////////////// USE DTO ////////////////////////////////////////
    public void SubmitDeck()
    {
        DeckContainerDTO deckContainerDTo = new DeckContainerDTO();

        deckContainerDTo.deck.DiceResult = 20;
        foreach (var card in selectedCards)
        {
            deckContainerDTo.deck.Cards.Add(new Card(card.actualStats.cardID, 1)); //TODO add level
        }

        string json = JsonUtility.ToJson(deckContainerDTo);
        Debug.Log(json);
        //Store the json somewhere
    }
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
            deckManager.SubmitDeck();
    }
}
#endif
