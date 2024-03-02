using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCard : BasicCard
{
    private int actualBuff;

    public int ActualBuff { get => actualBuff; }

    public override void Initialize(BasicCardScriptable card)
    {
        base.Initialize(card);
        SkillSystemManager.Instance.InitSkill(this, actualSkill);
        actualBuff = 0;
    }

    //Battle card deve avere metodi per buffare carta in base a livello e dado
    public void BuffCardWithLevel(int level)
    {
        Debug.Log("Todo buff with level");
        //actualBuff += Calcolo in base al livello
    }

    public void BuffCardWithDiceRoll(int diceResult)
    {
        //Togliere interi? Capire cdome usare buff

        //Calculate buff
        actualBuff += Mathf.FloorToInt(diceResult / 4);
        Debug.Log($"{actualStats.cardName} has a buff of {actualBuff}");
    }

    public void BuffCardWithAbility()
    {
        //Calculate buff
        //actualBuff += //Aggiungere buff
        //Debug.Log($"{actualStats.cardName} has a buff of {actualBuff}");
    }

    public void MoveCard()
    {
        Debug.Log("Todo move card in the correct spot");
    }

    //Perform skill
    public void Perform(BattleCard card)
    {
        if (card != this)
            return;
        SkillSystemManager.Instance.PerformSkill(this, actualSkill);
    }
}
