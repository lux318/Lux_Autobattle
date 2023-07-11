using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
}
