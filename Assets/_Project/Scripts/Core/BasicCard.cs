using JetBrains.Annotations;
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
    private Button cardButton;

    [Header("Label References")]
    [SerializeField]
    private TextMeshProUGUI nameLabel;
    [SerializeField]
    private TextMeshProUGUI hpLabel;
    [SerializeField]
    private TextMeshProUGUI atkLabel;
    [SerializeField]
    private Image sprite;

    //Card settings
    private BasicCardScriptable actualStats;
    private bool isOwned;

    public BasicCardScriptable ActualStats { get => actualStats; }
    public bool IsOwned { get => isOwned; set => isOwned = value; }

    private void Start()
    {
        if (cardButton == null)
            cardButton = GetComponentInChildren<Button>();
        cardButton.onClick.AddListener(OnSelect);

        DeckManager.Instance.OnCardSelected += OnCardSelected;
    }

    public void Initialize(BasicCardScriptable card)
    {
        //Initialize data
        actualStats = card;
        nameLabel.text = card.cardName;
        hpLabel.text = card.hp.ToString();
        atkLabel.text = card.atk.ToString();
        sprite.sprite = card.sprite;
        isOwned = false;

        //Reset graphic
        bgImage.color = unselectedColor;
    }

    public void OnSelect()
    {
        if (DeckManager.Instance.SelectedCard == this)
        {
            DeckManager.Instance.SelectedCard = null;

            //Graphic feedbacks
            bgImage.color = unselectedColor;
            //
        }
        else
        {
            DeckManager.Instance.SelectedCard = this;

            //Graphic feedbacks
            bgImage.color = selectedColor;
            //
        }
    }

    public void OnCardSelected(BasicCard card)
    {
        if (card != this)
        {
            //Graphic feedbacks
            bgImage.color = unselectedColor;
            //
        }

    }
}
