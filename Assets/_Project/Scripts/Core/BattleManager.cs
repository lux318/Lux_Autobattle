using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    [Header("Debug AI battle")]
    //Eventually to move in a dedicated AI script
    public BasicCardScriptable[] aiDeck;
    public BasicCard cardPrefab;
    public List<BasicCard> aiCards;

    public TextMeshProUGUI combatLabel;

    private CombatResult combatResult = CombatResult.None;

    public UnityEvent BattleStarted;
    public UnityEvent BattleEnded;

    public struct BattlePlayer
    {
        public BasicCard reference;
        public int currentHp;

        public BattlePlayer(BasicCard _card, int _currentHp)
        {
            reference = _card;
            currentHp = _currentHp;
        }
    }

    private enum CombatResult { None, P1Wins, P2Wins }

    private void Start()
    {
        //JUST FOR TEST STUFF. TO DO: PASS THE OTHER DECK
        aiCards = new List<BasicCard>();
        //foreach (var card in aiDeck)
        //{
        //    var basicCard = Instantiate(cardPrefab);
        //    basicCard.Initialize(card);
        //    basicCard.gameObject.SetActive(false);
        //    aiCards.Add(basicCard);
        //}
    }

    //public void SetUpBattle()
    //{

    //    combatResult = CombatResult.None;
    //    BattleStarted?.Invoke();

    //    combatLabel.text = "";
    //    combatLabel.text += "Your Deck: ";
    //    foreach (var card in DeckManager.Instance.SelectedCards)
    //        combatLabel.text += $"{card.actualStats.cardName} ({card.actualStats.hp}hp, {card.actualStats.atk}atk, {card.actualStats.speed}spe) ";
    //    combatLabel.text += "\n";
    //    combatLabel.text += "AI Deck: ";
    //    foreach (var card in aiDeck)
    //        combatLabel.text += $"{card.cardName}({card.hp}hp, {card.atk}atk, {card.speed}spe) ";
    //    combatLabel.text += "\n";

    //    StartCoroutine(CombatPhase());
    //}

    //public IEnumerator CombatPhase()
    //{
    //    //JUST FOR TEST STUFF
    //    Queue<BasicCard> playerQueue = new Queue<BasicCard>();
    //    foreach (var playerCard in DeckManager.Instance.SelectedCards)
    //        playerQueue.Enqueue(playerCard);

    //    Queue<BasicCard> aiQueue = new Queue<BasicCard>();
    //    foreach (var aiCard in aiCards)
    //        aiQueue.Enqueue(aiCard);
    //    //

    //    BattlePlayer player1 = new BattlePlayer();
    //    BattlePlayer player2 = new BattlePlayer();

    //    while (true)
    //    {
    //        //Take first elemnt of the queue
    //        yield return null;
    //        if (player1.reference == null)
    //        {
    //            if (playerQueue.Count == 0)
    //                combatResult = CombatResult.P2Wins;
    //            else
    //                player1 = GetPlayer(playerQueue);
    //        }

    //        if (player2.reference == null)
    //        {
    //            if (aiQueue.Count == 0)
    //                combatResult = CombatResult.P1Wins;
    //            else
    //                player2 = GetPlayer(aiQueue);
    //        }

    //        switch (combatResult)
    //        {
    //            case CombatResult.None:
    //                combatLabel.text += $"--- Start combat between {player1.reference.actualStats.cardName} ({player1.currentHp}hp) and {player2.reference.actualStats.cardName} ({player2.currentHp}hp) \n";
    //                Debug.Log(combatLabel.text);
    //                //Made card battles
    //                bool hasWinner = false;
    //                while (hasWinner == false)
    //                {
    //                    yield return null;
    //                    //Determine 1st with speed
    //                    if (player1.reference.actualStats.speed >= player2.reference.actualStats.speed) //TODO Calculate speed tie
    //                    {
    //                        hasWinner = Battle(player1, ref player2);
    //                        if (!hasWinner)
    //                            hasWinner = Battle(player2, ref player1);
    //                    }
    //                    else
    //                    {
    //                        hasWinner = Battle(player2, ref player1);
    //                        if (!hasWinner)
    //                            hasWinner = Battle(player1, ref player2);
    //                    }
    //                }
    //                break;

    //            case CombatResult.P1Wins:
    //                combatLabel.text += $"P1 wins the battle";
    //                Debug.Log("P1 win");
    //                BattleEnded?.Invoke();
    //                yield break;

    //            case CombatResult.P2Wins:
    //                combatLabel.text += $"P2 wins the battle";
    //                Debug.Log("P2 win");
    //                BattleEnded?.Invoke();
    //                yield break;
    //        }
    //    }
    //}

    public BattlePlayer GetPlayer(Queue<BasicCard> playersQueue)
    {
        var player1Current = playersQueue.Dequeue();
        var player = new BattlePlayer(player1Current, player1Current.ActualStats.hp);
        return player;
    }

    public bool Battle(BattlePlayer attack, ref BattlePlayer defense)
    {
        combatLabel.text += $"-> {attack.reference.ActualStats.cardName} hit for {attack.reference.ActualStats.atk} ";
        Debug.Log(combatLabel.text);
        defense.currentHp -= attack.reference.ActualStats.atk;
        if (defense.currentHp <= 0)
        {
            combatLabel.text += $"-> {defense.reference.ActualStats.cardName} is defeated\n";
            Debug.Log(combatLabel.text);
            defense = new BattlePlayer();
            return true;
        }
        return false;
    }
}
