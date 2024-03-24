using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Economy;

public class UI_SessionStatus : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lifeValue;
    [SerializeField]
    private TextMeshProUGUI winsValue;

    Session session;
    // Start is called before the first frame update
    void Start()
    {
        BattleManager.Instance.BattleEnded.AddListener(() =>
        {
            UpdateLife();
            UpdateWins();
        });
        session = PlayerClientController.Instance.GetSession();
        if(session != null )
        {
            UpdateLife();
            UpdateWins();
        }
    }

    private void UpdateLife()
    {
        lifeValue.text = session.GetLife().ToString();
    }

    private void UpdateWins()
    {
        winsValue.text = session.GetWins().ToString();
    }
}
