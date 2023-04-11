using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Pool;

public class CardsPooler : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private BasicCardScriptable[] basicCardScriptables;
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
        InitializePool();
    }

    public void RandomizePool()
    {
        //Clear actual active cards list keeping selected cards
        foreach (var activeCard in activeCards)
        {
            if (!DeckManager.Instance.SelectedCards.Contains(activeCard))
                bulletPool.Release(activeCard);
        }
        activeCards.Clear();

        //Add to the list again the cards that we keep
        foreach (var selectedCard in DeckManager.Instance.SelectedCards)
            activeCards.Add(selectedCard);

        //Create needed cards and initialize those using random scriptable
        Random.InitState((int)System.DateTime.Now.Ticks);
        int count = initialPoolSize - activeCards.Count;
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, basicCardScriptables.Length);
            BasicCardScriptable cardScriptable = basicCardScriptables[randomIndex];
            var card = bulletPool.Get();
            card.Initialize(cardScriptable);
        }
    }

    public void ClearPool()
    {
        foreach (var activeCard in activeCards)
        {
            bulletPool.Release(activeCard);
        }
        activeCards.Clear();
        DeckManager.Instance.ClearCards();
    }


    #region BulletPooling
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
