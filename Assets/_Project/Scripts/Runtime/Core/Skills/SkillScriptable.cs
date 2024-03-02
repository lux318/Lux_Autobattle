using System;
using UnityEngine;

public enum When { OnPlay, WhileOnField, OnDeath, OnAttackDone, OnAttackReceived } //Capire con Arrigo differenza tra mentre è in squadra e mentre è in vita. Intanto messo solo uno dei due. While on filed ha per forza bisogno di una fine in qualche modo!! SENZA AVERE FINE; NON IMPLEMENTABILE WHILE ON BOARD
public enum Target { HimSelf, BackAlly, FrontAlly, AllOthers, FrontAndBack, RandomEnemy, RandomAlly }
public enum What { DamageSkill, BuffSkill, Dodge, RepeatAction }
public enum HowMuchType { dice, percentage, value}
public enum Dices { d4, d6, d8, d20 }

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class SkillScriptable : ScriptableObject
{
    public string skillName;
    [Tooltip("When the skill needs to be triggered")]
    public When skillTrigger;
    [Tooltip("Skill's target")]
    public Target skillTarget;
    [Tooltip("What the skill does")]
    public What skillEffect;
    [Tooltip("Duration of the skill in rounds")]
    public int skillRoundDuration;

    //How much section
    public HowMuchType type;
    public Dices dice;
    public int dicesNumber;
    public int percentageValue;
    public float value;
}
