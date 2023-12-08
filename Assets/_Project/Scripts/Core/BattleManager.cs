using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BattleManager : Singleton<BattleManager>
{
    [Header("References")]
    [SerializeField]
    private GameObject battleCardPrefab;


    //Decks PUBLIC FOR DEBUG. MAKE PRIVATE
    public Queue<BattleCard> localPlayerQueue;
    public Queue<BattleCard> remotePlayerQueue;

    private CombatResult combatResult = CombatResult.None;
    private List<BasicCardScriptable> basicCardScriptables;

    public UnityEvent BattleStarted;
    public UnityEvent BattleEnded;


    //DEBUG
    public TextMeshProUGUI combatLabel;
    //


    public struct BattlePlayer
    {
        public BattleCard reference;
        public int currentHp;
        public int currentAtk;

        public BattlePlayer(BattleCard _card)
        {
            reference = _card;
            currentHp = _card.ActualStats.hp + _card.ActualBuff; //TODO CONSIDER LEVEL
            currentAtk = _card.ActualStats.atk + _card.ActualBuff; //TODO CONSIDER LEVEL
        }

        public void UpdateBuff()
        {
            //Todo consider ability to modify buff
        }
    }

    private enum CombatResult { None, P1Wins, P2Wins, Tie }

    #region SetUp Decks
    public void Init(DeckDTO localPlayerDeck, DeckDTO remotePlayerDeck)
    {
        Debug.Log("Init battle");
        combatLabel.text = "";

        //Store the list of all the cards
        basicCardScriptables = new List<BasicCardScriptable>();
        var scriptables = Resources.LoadAll("Cards");
        foreach (var scriptable in scriptables)
            basicCardScriptables.Add(scriptable as BasicCardScriptable);

        //Create local player deck queue
        localPlayerQueue = CreateDeckWithBuff(localPlayerDeck, "player1");

        //Create remote player deck queue
        remotePlayerQueue = CreateDeckWithBuff(remotePlayerDeck, "player2");

        //Start battle
        SetUpBattle();
    }

    //Create ordered deck and apply buff for level and dice roll
    public Queue<BattleCard> CreateDeckWithBuff(DeckDTO deck, string playerName)
    {
        Queue<BattleCard> queue = new Queue<BattleCard>();

        //Create deck queue
        foreach (var card in deck.Cards)
        {
            //Find the correspondents card
            BasicCardScriptable correctScriptableCard = null;
            foreach (var scriptableCard in basicCardScriptables)
            {
                if (card.cardID == scriptableCard.cardID)
                {
                    correctScriptableCard = scriptableCard;
                    break;
                }
            }

            //Create correspondent card
            var battleCard = Instantiate(battleCardPrefab).GetComponent<BattleCard>();  //TODO PUT THEM IN THE SPACE
            battleCard.Initialize(correctScriptableCard);

            //Move the card to the correct spot
            battleCard.MoveCard();

            //Buff correspondent card using level
            battleCard.BuffCardWithLevel(card.cardLevel);

            //Buff correspondent card using dice roll
            battleCard.BuffCardWithDiceRoll(card.cardDiceRoll);
            DiceResultFeedback(playerName, card.cardDiceRoll, battleCard.ActualStats.cardName);

            //Add card to the Queue
            queue.Enqueue(battleCard);
        }

        return queue;
    }

    private void DiceResultFeedback(string playerName, int diceRoll, string cardName)
    {
        combatLabel.text += $"{playerName} rolls {diceRoll} for {cardName}, ";
    }
    #endregion

    #region Battle
    public void SetUpBattle()
    {

        combatResult = CombatResult.None;
        BattleStarted?.Invoke();

        //combatLabel.text += $"Your Dice roll: {localDiceRoll}\n";
        combatLabel.text += "\nYour Deck: ";
        foreach (var card in localPlayerQueue)
            combatLabel.text += $"{card.ActualStats.cardName} ({card.ActualStats.hp + card.ActualBuff}hp, {card.ActualStats.atk + card.ActualBuff}atk) ";
        combatLabel.text += "\n";
        //combatLabel.text += $"Opponent Dice roll: {remoteDiceRoll}\n";
        combatLabel.text += "Opponent Deck: ";
        foreach (var card in remotePlayerQueue)
            combatLabel.text += $"{card.ActualStats.cardName} ({card.ActualStats.hp + card.ActualBuff}hp, {card.ActualStats.atk + card.ActualBuff}atk) ";
        combatLabel.text += "\n";

        StartCoroutine(CombatPhase());
    }

    public IEnumerator CombatPhase()
    {
        BattlePlayer player1 = new BattlePlayer();
        BattlePlayer player2 = new BattlePlayer();

        yield return null;

        while (true)
        {
            //Check if the battle ended in a tie
            if (player1.reference == null && player2.reference == null && localPlayerQueue.Count == 0 && remotePlayerQueue.Count == 0)
            {
                combatResult = CombatResult.Tie;
            }

            //Take first element of the queue
            yield return null;
            if (player1.reference == null && combatResult != CombatResult.Tie)
            {
                if (localPlayerQueue.Count == 0)
                    combatResult = CombatResult.P2Wins;
                else
                    player1 = GetPlayer(localPlayerQueue);
            }

            if (player2.reference == null && combatResult != CombatResult.Tie)
            {
                if (remotePlayerQueue.Count == 0)
                    combatResult = CombatResult.P1Wins;
                else
                    player2 = GetPlayer(remotePlayerQueue);
            }

            switch (combatResult)
            {
                case CombatResult.None:
                    combatLabel.text += $"--- Start combat between {player1.reference.ActualStats.cardName} ({player1.currentHp}hp) and {player2.reference.ActualStats.cardName} ({player2.currentHp}hp) \n";
                    //Made card battles
                    bool hasWinner = false;
                    while (hasWinner == false)
                    {
                        yield return null;

                        //Made cards battle simultaneously
                        hasWinner = Battle(player1, ref player2);
                        if (hasWinner)
                            Battle(player2, ref player1);
                        else
                            hasWinner = Battle(player2, ref player1);
                    }
                    CombatEnded(ref player1, ref player2);
                    break;

                case CombatResult.P1Wins:
                    combatLabel.text += $"P1 wins the battle";
                    Debug.Log("P1 win");
                    BattleEnded?.Invoke();
                    yield break;

                case CombatResult.P2Wins:
                    combatLabel.text += $"P2 wins the battle";
                    Debug.Log("P2 win");
                    BattleEnded?.Invoke();
                    yield break;
                case CombatResult.Tie:
                    combatLabel.text += $"Battle ended with a tie";
                    Debug.Log("Tie");
                    BattleEnded?.Invoke();
                    yield break;
            }
        }
    }


    public BattlePlayer GetPlayer(Queue<BattleCard> playersQueue)
    {
        var player1Current = playersQueue.Dequeue();
        var player = new BattlePlayer(player1Current);
        return player;
    }

    public bool Battle(BattlePlayer attack, ref BattlePlayer defense)
    {
        combatLabel.text += $"-> {attack.reference.ActualStats.cardName} hit for {attack.currentAtk} ";
        defense.currentHp -= attack.currentAtk;
        if (defense.currentHp <= 0)
        {
            return true;
        }
        return false;
    }

    public void CombatEnded(ref BattlePlayer player1, ref BattlePlayer player2)
    {
        combatLabel.text += $"\n";
        //Check the players defeated
        if (player1.currentHp <= 0)
        {
            combatLabel.text += $"-> {player1.reference.ActualStats.cardName} is defeated\n";
            Debug.Log(combatLabel.text);
            player1 = new BattlePlayer();
        }
        if (player2.currentHp <= 0)
        {
            combatLabel.text += $"-> {player2.reference.ActualStats.cardName} is defeated\n";
            Debug.Log(combatLabel.text);
            player2 = new BattlePlayer();
        }
    }
    #endregion

    //DEBUG
    public void Restart()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene(0);
    }
}
