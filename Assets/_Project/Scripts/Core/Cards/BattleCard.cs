using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCard : BasicCard
{

    //Battle card deve avere metodi per buffare carta in base a livello e dado
    public void BuffCardWithLevel(int level)
    {
        Debug.Log("Todo buff with level");
        //actualStats.atk += level;
    }

    public void BuffCardWithDiceRoll(int diceResult)
    {
        Debug.Log("Todo buff with dice roll");
    }

    public void MoveCard()
    {
        Debug.Log("Todo move card in the correct spot");
    }
}
