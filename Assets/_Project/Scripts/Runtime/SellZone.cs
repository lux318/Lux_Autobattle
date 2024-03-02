using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellZone : MonoBehaviour
{
    [SerializeField]
    private Button zoneButton;

    private void Start()
    {
        DeckManager.Instance.OnCardSelected += CardSelected;
        DeckManager.Instance.OnCardSold += CardSold;
        zoneButton.onClick.AddListener(OnZoneClick);
    }
    private void CardSelected(DeckConstructionCard card)
    {
        if (card != null && DeckManager.Instance.SelectedCard.IsOwned)
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


    private void CardSold(DeckConstructionCard card)
    {
        if(card != null)
        {
            Destroy(card.gameObject);
        }
    }

    private void OnZoneClick()
    {
        DeckManager.Instance.CardSold();
        Economy.Instance.OnSellPressed();
    }
}
