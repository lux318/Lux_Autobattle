using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    private Button playButton;
    [SerializeField] 
    private TextMeshProUGUI playerNameTxt;

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            StartCoroutine(LoadSceneAsync());
        });
        GetName();
    }
    public async void GetName()
    {
        string name = await AuthenticationManager.Instance.GetPlayerName();
        if (name != null)
            playerNameTxt.text = name.Split("#")[0];
        else
            playerNameTxt.text = "Player";
    }


    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
