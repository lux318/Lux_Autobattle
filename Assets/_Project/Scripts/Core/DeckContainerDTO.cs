using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeckContainerDTO
{
    public DeckDTO deck;

    public DeckContainerDTO()
    {
        deck = new DeckDTO();
    }

}

[Serializable]
public class DeckDTO
{
    [SerializeField]
    private List<Card> cards;
    [SerializeField]
    private int diceResult;

    public List<Card> Cards { get => cards; set => cards = value; }
    public int DiceResult { get => diceResult; set => diceResult = value; }

    public DeckDTO()
    {
        cards = new List<Card>();
        diceResult = 0;
    }
}

[Serializable]
public class Card
{
    public int cardID;
    public int cardLevel;

    public Card (int cardID, int cardLevel)
    {
        this.cardID = cardID;
        this.cardLevel = cardLevel;
    }
}


//Per pescare json e crearlo:
//JsonUtility.FromJson<DeckContainerDTO>("pippo");
//DeckContainerDTO deck = null;
//JsonUtility.ToJson(deck);

//+ chiamata API
//string jsonText = "";
//        CallAPIManager.Instance.ProcessRequest(CallAPIManager.Instance.CompositeJSONURL(url), callback =>
//        {
//            if (callback != 0)
//            {
//                Debug.LogError("Error");
//                StopAllCoroutines();
//                return;
//            }
//        },
//        json =>
//        {
//            jsonText = json;
//        });

//        yield return new WaijsonTexttUntil(() =>  != "");
