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

    //Region con tutti i possibili target

    //Region con tutti i possibili What. Qui verranno usati how much e duration?
}
