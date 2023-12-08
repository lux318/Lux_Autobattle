using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckConstructionCard : BasicCard
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

    private bool isOwned;

    public bool IsOwned { get => isOwned; set => isOwned = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (cardButton == null)
            cardButton = GetComponentInChildren<Button>();
        cardButton.onClick.AddListener(OnSelect);

        DeckManager.Instance.OnCardSelected += OnCardSelected;
    }

    public override void Initialize(BasicCardScriptable card)
    {
        base.Initialize(card);
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
