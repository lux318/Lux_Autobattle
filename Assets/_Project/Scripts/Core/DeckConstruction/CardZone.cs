using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardZone : MonoBehaviour
{
    [SerializeField]
    private int zoneIndex;
    private BasicCard actualCard;
    private Button zoneButton;

    public int ZoneIndex { get => zoneIndex; }
    public BasicCard ActualCard { get => actualCard; }

    public UnityEvent OnCardUpdated;

    private void Start()
    {
        DeckManager.Instance.OnCardSelected += CardSelected;
        DeckManager.Instance.OnCardMoved += OnCardMoved;
        zoneButton = GetComponentInChildren<Button>(true);
        zoneButton.onClick.AddListener(OnZoneClick);
        zoneButton.gameObject.SetActive(false);
    }

    private void CardSelected(BasicCard card)
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

    private void OnCardMoved(BasicCard card)
    {
        if (this.actualCard == card)
        {
            this.actualCard = DeckManager.Instance.CardToSwitch;
            DeckManager.Instance.CardToSwitch = null;
            OnCardUpdated?.Invoke();
        }
    }

    private void OnZoneClick()
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
            if (DeckManager.Instance.CardPlaced())
            {
                actualCard = selectedCard;
                OnCardUpdated?.Invoke();

                //Graphics stuff!!
                return;
            }
        }
    }
}
