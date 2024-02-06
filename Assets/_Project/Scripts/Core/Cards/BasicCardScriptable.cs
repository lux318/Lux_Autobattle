using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Basic Card")]
public class BasicCardScriptable : ScriptableObject
{
    public int cardID;
    public string cardName;
    public Sprite sprite;
    public int hp;
    public int atk;
    public int cost;
    public int rarity;
    //public Ability ability
}
