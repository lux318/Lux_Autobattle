using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Economy;

public class CardZone : MonoBehaviour
{
    [SerializeField]
    private int zoneIndex;
    private DeckConstructionCard actualCard;
    private Button zoneButton;

    public int ZoneIndex { get => zoneIndex; }
    public DeckConstructionCard ActualCard { get => actualCard; set => actualCard = value; }

    public UnityEvent OnCardUpdated;

    private void Start()
    {
        DeckManager.Instance.OnCardSelected += CardSelected;
        DeckManager.Instance.OnCardMoved += OnCardMoved;

        zoneButton = GetComponentInChildren<Button>(true);
        zoneButton.onClick.AddListener(OnZoneClick);
        zoneButton.gameObject.SetActive(false);
    }

    private void CardSelected(DeckConstructionCard card)
    {
        if (card != null)
        {
            zoneButton.gameObject.SetActive(true);
            //Graphic stuff
        }
        else
        {
            zoneButton.gameObject.SetActive(false);
            //Graphic stuff
        }
    }

    private void OnCardMoved(DeckConstructionCard card)
    {
        if (this.actualCard == card)
        {
            this.actualCard = DeckManager.Instance.CardToSwitch;
            DeckManager.Instance.CardToSwitch = null;
            OnCardUpdated?.Invoke();
        }
    }

    public void UpdateGraphichs()
    {
        OnCardUpdated?.Invoke();
    }

    public void OnZoneClick()
    {

        var selectedCard = DeckManager.Instance.SelectedCard;

        //Check if the zone is free
        //if (actualCard != null)
        //{
        //if (actualCard.ActualStats.cardID == DeckManager.Instance.SelectedCard.ActualStats.cardID)
        //{
        //    Debug.Log("TODO Logica upgrade livello");
        //    DeckManager.Instance.SelectedCard = null;
        //    return;
        //}
        //else
        //        return;
        //}

        
        //Check if the card was already in another zone
        if (selectedCard.IsOwned)
        {
            if (actualCard != null)
            {
                //TODO aumento di livello con 2 carte uguali??
                DeckManager.Instance.CardToSwitch = actualCard;
            }

            //Remove card from previous zone to put it in the new one
            DeckManager.Instance.CardMoved();
            actualCard = selectedCard;
            OnCardUpdated?.Invoke();

            //Graphics stuff!!
            return;
        }

        if (actualCard == null)
        {
            if (Economy.Instance.actualEconomy < selectedCard.ActualStats.cost)
            {
                return;
            }
            Economy.Instance.OnBuyPressed(selectedCard.ActualStats.cost);
            actualCard = selectedCard;
            OnCardUpdated?.Invoke();
            //Graphics stuff!!
            return;
        }

    }
}
