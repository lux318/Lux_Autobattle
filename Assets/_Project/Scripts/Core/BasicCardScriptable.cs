using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Basic Card")]
public class BasicCardScriptable : ScriptableObject
{
    public string cardName;
    public int hp;
    public int atk;
    public int speed;
}
