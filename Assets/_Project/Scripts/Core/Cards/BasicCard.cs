using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicCard : MonoBehaviour
{
    [Header("Label References")]
    [SerializeField]
    protected TextMeshProUGUI nameLabel;
    [SerializeField]
    protected TextMeshProUGUI hpLabel;
    [SerializeField]
    protected TextMeshProUGUI atkLabel;
    [SerializeField]
    protected Image sprite;

    //Card settings
    protected BasicCardScriptable actualStats;

    public BasicCardScriptable ActualStats { get => actualStats; }


    public virtual void Initialize(BasicCardScriptable card)
    {
        if (card == null)
        {
            Debug.LogError("The card is null!");
            return;
        }

        //Initialize data
        actualStats = card;
        nameLabel.text = card.cardName;
        hpLabel.text = card.hp.ToString();
        atkLabel.text = card.atk.ToString();
        sprite.sprite = card.sprite;
    }
}
