using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasicCard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameLabel;
    [SerializeField]
    private TextMeshProUGUI hpLabel;
    [SerializeField]
    private TextMeshProUGUI atkLabel;
    [SerializeField]
    private TextMeshProUGUI speedLabel;

    private BasicCardScriptable actualStats;

    public void Initialize(BasicCardScriptable card)
    {
        actualStats = card;
        nameLabel.text = card.cardName;
        hpLabel.text = card.hp.ToString();
        atkLabel.text = card.atk.ToString();
        speedLabel.text = card.speed.ToString();
    }

    //To add selected behaviour
}
