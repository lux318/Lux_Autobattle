using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicCard : MonoBehaviour
{
    [Header("Select Behaviour")]
    [SerializeField]
    private Image bgImage;
    [SerializeField]
    private Color unselectedColor = Color.black;
    [SerializeField]
    private Color selectedColor = Color.yellow;
    [SerializeField]
    private Button selectButton;
    [SerializeField]
    private Button deselectButton;

    [Header("Label References")]
    [SerializeField]
    private TextMeshProUGUI nameLabel;
    [SerializeField]
    private TextMeshProUGUI hpLabel;
    [SerializeField]
    private TextMeshProUGUI atkLabel;
    [SerializeField]
    private TextMeshProUGUI speedLabel;

    public BasicCardScriptable actualStats;

    private void Start()
    {
        selectButton.onClick.AddListener(OnSelect);
        deselectButton.onClick.AddListener(OnDeselect);
    }

    public void Initialize(BasicCardScriptable card)
    {
        //Initialize data
        actualStats = card;
        nameLabel.text = card.cardName;
        hpLabel.text = card.hp.ToString();
        atkLabel.text = card.atk.ToString();
        speedLabel.text = card.speed.ToString();

        //Reset graphic
        selectButton.gameObject.SetActive(true);
        deselectButton.gameObject.SetActive(false);
        bgImage.color = unselectedColor;
    }

    public void OnSelect()
    {
        //Change bg color
        bgImage.color = selectedColor;
        //Switch buttons
        selectButton.gameObject.SetActive(false);
        deselectButton.gameObject.SetActive(true);
        //Add to deck manager list
        DeckManager.Instance.AddCard(this);
    }

    public void OnDeselect()
    {
        //Change bg color
        bgImage.color = unselectedColor;
        //Switch buttons
        deselectButton.gameObject.SetActive(false);
        selectButton.gameObject.SetActive(true);
        //Remove from deck manager list
        DeckManager.Instance.RemoveCard(this);
    }
}
