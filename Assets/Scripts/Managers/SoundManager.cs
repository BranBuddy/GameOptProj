/*
    Manager to easily change volumes and what music/sfx is playing
*/


using UnityEngine;

public class SoundManager : MonoBehaviour
{
     #region Singleton Pattern
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SoundManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("SoundManager");
                    _instance = singletonObject.AddComponent<SoundManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

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

    [SerializeField] internal AudioSource musicSource;
    [SerializeField] internal AudioSource sfxSource;
    [SerializeField] internal AudioClip mainMenuMusic;
    [SerializeField] internal AudioClip levelMusic;
    [SerializeField] internal AudioClip winMusic;
}
