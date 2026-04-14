/*
    Central script used to get which character is currently in use, update UI, and handle win condition.
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Singleton Pattern
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    public int coinCount;
    public List<GameObject> players = new List<GameObject>();
    public GameObject playerToStartAs;
    public GameObject currentPlayer;
    public TextMeshProUGUI coinCountText, playerOneDeathCountText, playerTwoDeathCountText;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        TryResolveStartingPlayer();
    }

    private void TryResolveStartingPlayer()
    {
        if (players.Contains(playerToStartAs))
        {
            currentPlayer = playerToStartAs;
        }
        else
        {
            Debug.LogWarning("Player to start as is not set correctly. Defaulting to first player in the list.");
            currentPlayer = players[0];
        }
    }

    public void IncreaseCoinCount(int amount)
    {
        coinCount += amount;
        coinCountText.text = coinCount.ToString();
    }

    public void IncreasePlayerDeathCount(GameObject player)
    {
        if (player == players[0])
        {
            int currentCount = int.Parse(playerOneDeathCountText.text);
            playerOneDeathCountText.text = (currentCount + 1).ToString();
        }
        else if (player == players[1])
        {
            int currentCount = int.Parse(playerTwoDeathCountText.text);
            playerTwoDeathCountText.text = (currentCount + 1).ToString();
        }
    }

    public void CompleteGame()
    {
        SoundManager.Instance.musicSource.Stop(); // Stop the current music
        SceneManager.LoadScene("WinScene"); 
        SoundManager.Instance.musicSource.clip = SoundManager.Instance.winMusic; // Set the win music to play
        SoundManager.Instance.musicSource.Play(); // Play the win music
    }
}
