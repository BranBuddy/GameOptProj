using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public IEnumerator DisableThenDestroy(GameObject obj)
    {
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds before destroying the object
        Destroy(this.gameObject);
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
    }
}
