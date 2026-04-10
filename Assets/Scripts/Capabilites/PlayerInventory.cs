using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    #region Singleton Pattern
    private static PlayerInventory _instance;
    public static PlayerInventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<PlayerInventory>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("PlayerInventory");
                    _instance = singletonObject.AddComponent<PlayerInventory>();
                }
            }
            return _instance;
        }
    }
    #endregion

    public List<string> items = new List<string>();
    public List<string> checkpoints = new List<string>();
    // Universal event for checkpoint updates
    public UnityEvent<string> OnCheckpointCollected = new UnityEvent<string>();

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
    }

}
