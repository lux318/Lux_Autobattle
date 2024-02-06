using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Economy : Singleton<Economy>
{
    public event EventHandler<OnEconomySpentEventArgs> OnEconomyRoll;

    public event EventHandler<OnEconomySpentEventArgs> OnEconomyBuy;

    public event EventHandler<OnEconomySpentEventArgs> OnEconomyInit;

    public class OnEconomySpentEventArgs : EventArgs
    {
        public int updateEconomy;
    }

    [SerializeField]
    private const int MAX_ECONOMY = 10;

    public int actualEconomy = 0;

    [SerializeField]
    private int rollCost = 2;

    [SerializeField]
    private int sellCost = 1;


    // Start is called before the first frame update
    public void Start()
    {
        actualEconomy = MAX_ECONOMY;
        OnEconomyInit?.Invoke(this, new OnEconomySpentEventArgs { updateEconomy = actualEconomy });
    }

    public void OnRollPressed()
    {
        if(actualEconomy >= rollCost)
        {
            actualEconomy -= rollCost;
            OnEconomyRoll?.Invoke(this, new OnEconomySpentEventArgs { updateEconomy = actualEconomy});
        }
    }

    public void OnBuyPressed(int costCard)
    {
        if(actualEconomy >= costCard)
        {
            actualEconomy -= costCard;
            OnEconomyBuy?.Invoke(this, new OnEconomySpentEventArgs { updateEconomy = actualEconomy });
        }
    }

    public void OnSellPressed()
    {
        actualEconomy += sellCost;
        OnEconomyBuy?.Invoke(this, new OnEconomySpentEventArgs { updateEconomy = actualEconomy });
    }
}