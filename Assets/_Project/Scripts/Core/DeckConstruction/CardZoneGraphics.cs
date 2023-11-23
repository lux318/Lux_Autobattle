using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardZoneGraphics : MonoBehaviour
{
    private CardZone cardZone;

    private void Start()
    {
        cardZone = GetComponent<CardZone>();
        cardZone.OnCardUpdated.AddListener(OnZoneChanged);
        OnZoneChanged();
    }

    private void OnZoneChanged()
    {
        if (cardZone.ActualCard == null)
        {
            
        }
        else
        {
            cardZone.ActualCard.gameObject.transform.SetParent(transform);
            cardZone.ActualCard.gameObject.transform.localPosition = Vector3.zero;
            cardZone.ActualCard.gameObject.transform.SetAsFirstSibling();
        }
    }
}
