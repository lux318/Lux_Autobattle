using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystemManager : Singleton<SkillSystemManager>
{
    public void InitSkill(BattleCard card, SkillScriptable skill)
    {
        switch (skill.skillTrigger)
        {
            case When.OnPlay:
                BattleManager.Instance.OnCardPlayed += card.Perform;
                break;
            case When.WhileOnField:
                //BattleManager.Instance.OnBattleStart += card.Perform;
                break;
            case When.OnDeath:
                BattleManager.Instance.OnCardDeath += card.Perform;
                break;
            case When.OnAttackDone:
                BattleManager.Instance.OnCardAttack += card.Perform;
                break;
            case When.OnAttackReceived:
                BattleManager.Instance.OnCardIsAttacked += card.Perform;
                break;
        }
    }

    public void PerformSkill(BattleCard card, SkillScriptable skill)
    {
        //TODO ASPETTARE CHE SKILL ABBIANO FINITO. AWAIT/SIMILI?
        //Chiamare funzioni in base a valori
        Debug.LogError($"{card.ActualStats.cardName} perform skill: {skill.skillName}");
    }

   //Calculate targets for the skill
    public List<BattleCard> FindTargets(Target skillTarget)
    {
        List<BattleCard> targets = new List<BattleCard>();

        switch (skillTarget)
        {
            case Target.HimSelf:
                break;
            case Target.BackAlly:
                break;
            case Target.FrontAlly:
                break;
            case Target.AllOthers:
                break;
            case Target.FrontAndBack:
                break;
            case Target.RandomEnemy:
                break;
            case Target.RandomOtherAlly:
                break;
        }
        return targets;
    }

    //Calculate skill data
    public float CalculateData(SkillScriptable skill)
    {
        float result = 0;

        switch (skill.hmType)
        {
            case HowMuchType.Dice:
                //Lanciare il relativo dado e scegliere come usare il valore
                break;
            case HowMuchType.Percentage:
                result = skill.percentageValue; //Da capire come convertirlo
                break;
            case HowMuchType.Value:
                result = skill.value;
                break;
        }

        return result;
    }

    //Apply skill effects
    public void ApplyEffect(What skillEffect, List<BattleCard> skillTargets, int value)
    {
        switch (skillEffect)
        {
            case What.DamageSkill:
                break;
            case What.BuffSkill:
                break;
            case What.Dodge:
                break;
            case What.RepeatAction:
                break;
        }
    }

    //e duration? Dove gestisco la situa "buff temporanei"? Aggiungere conteggio nella battle card?
}
