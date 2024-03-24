using System;
using UnityEditor;
using UnityEngine;

public class PlayerClientController : MonoBehaviour
{
    private static PlayerClientController instance;
    public static PlayerClientController Instance => instance;
    private Session session;

    private void Awake()
    {
        
        session = new Session();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        //Controllo se ho salvato una sessione 

        if(true)
        {
            //Get Session somewhere
            session.SetOldSession();
            ReloadFromSession();
        }
        else
        { 
            session = new Session();
        }

    }


    public void ReloadFromSession()
    {
        if(session != null)
        {
            Debug.Log("Session reload");
            Debug.Log("Life: "+ session.GetLife());
            Debug.Log("Wins: " + session.GetWins());
            string temp = "{\"deck\":{\"cards\":[{\"cardID\":1,\"cardLevel\":1,\"cardDiceRoll\":9,\"cardPosition\":0},{\"cardID\":0,\"cardLevel\":1,\"cardDiceRoll\":9,\"cardPosition\":1},{\"cardID\":0,\"cardLevel\":1,\"cardDiceRoll\":12,\"cardPosition\":2}]}}";
            session.SetTeam(temp);
            Debug.Log("Team: " + session.GetTeam());
        }
    }

    public void ReloadFromSessionClean()
    {
        if (session != null)
        {
            Debug.Log("Session reload");
            Debug.Log("Life: " + session.GetLife());
            Debug.Log("Wins: " + session.GetWins());
            string temp = "{\"deck\":{\"cards\":[{\"cardID\":1,\"cardLevel\":1,\"cardDiceRoll\":9,\"cardPosition\":0},{\"cardID\":0,\"cardLevel\":1,\"cardDiceRoll\":9,\"cardPosition\":1},{\"cardID\":0,\"cardLevel\":1,\"cardDiceRoll\":12,\"cardPosition\":2}]}}";
            session.SetTeam(temp);
            Debug.Log("Team: " + session.GetTeam());
        }
    }

    public void ResetSession()
    {
        session = new Session();
    }

    public Session GetSession()
    {
        return session;
    }
}


public class Session
{
    bool bNewSession;
    string jsonTeam;
    int wins;
    int life;
    
    public Session()
    {
        bNewSession = true;
        wins = 0;
        life = 8;
        jsonTeam = string.Empty;
    }

    public void SetTeam(string team)
    {
        jsonTeam = team;
    }

    public string GetTeam()
    {
        return jsonTeam;
    }

    public void AddWins(int win)
    {
        wins += win;
    }

    public int GetWins()
    {
        return wins;
    }

    public void SetLife(int _life)
    {
        life = _life;
    }

    public void AddLose(int counter)
    {
        life -= counter;
    }

    public int GetLife ()
    { return life; }


    public void SetNewSession()
    {
        bNewSession = true;
    }

    public void SetOldSession()
    {
        bNewSession = false;
    }

    public bool GetNewSession()
    {
        return bNewSession;
    }
}