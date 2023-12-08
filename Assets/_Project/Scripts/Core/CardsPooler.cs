using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class CardsPooler : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private List<BasicCardScriptable> basicCardScriptables;
    [SerializeField]
    private GameObject basicCardPrefab;
    [SerializeField]
    private Transform cardsParent;

    [Header("Settings")]
    [SerializeField]
    private int initialPoolSize;

    [Header("Bullet Pooling")]
    [SerializeField]
    private IObjectPool<BasicCard> bulletPool;
    [SerializeField]
    private int poolCapacity = 10;
    [SerializeField]
    private int poolMaxSize = 10000;
    private List<BasicCard> activeCards;


    private void Start()
    {
        activeCards = new List<BasicCard>();

        basicCardScriptables = new List<BasicCardScriptable>();
        var scriptables = Resources.LoadAll("Cards");
        foreach (var scriptable in scriptables)
            basicCardScriptables.Add(scriptable as BasicCardScriptable);

        InitializePool();
    }

    public void RandomizePool()
    {
        //Clear card selection
        DeckManager.Instance.SelectedCard = null;

        //Clear actual active cards list
        var cardsTopRemove = new List<BasicCard>();
        foreach (var activeCard in activeCards)
        {
            if (activeCard.transform.parent == cardsParent)
                bulletPool.Release(activeCard);
            else
                cardsTopRemove.Add(activeCard);
        }
        
        //Remove selected cards from the list
        foreach (var card in cardsTopRemove)
        {
            activeCards.Remove(card);
        }

        activeCards.Clear();

        //Create needed cards and initialize those using random scriptable
        Random.InitState((int)System.DateTime.Now.Ticks);
        for (int i = 0; i < initialPoolSize; i++)
        {
            int randomIndex = Random.Range(0, basicCardScriptables.Count);
            BasicCardScriptable cardScriptable = basicCardScriptables[randomIndex];
            var card = bulletPool.Get();
            card.Initialize(cardScriptable);
        }
    }

    #region CardsPooling
    //Initialize pool
    public void InitializePool()
    {
        bulletPool = new ObjectPool<BasicCard>(CreateCard, OnGetCard, OnReleaseCard, OnDestroyCard, true, poolCapacity, poolMaxSize);
    }

    private BasicCard CreateCard()
    {
        BasicCard card = Instantiate(basicCardPrefab, cardsParent).GetComponent<BasicCard>();
        card.gameObject.SetActive(false);
        return card;
    }

    //Called when I get a bullet from the pull
    private void OnGetCard(BasicCard card)
    {
        card.gameObject.SetActive(true);
        activeCards.Add(card);
    }

    //Called when the instance is returned to the pool
    private void OnReleaseCard(BasicCard card)
    {
        card.gameObject.SetActive(false);
    }

    //Called when the element could not be returned to the pool due to the pool reaching the maximum size
    private void OnDestroyCard(BasicCard card)
    {
        Destroy(card.gameObject);
    }
    #endregion
}
