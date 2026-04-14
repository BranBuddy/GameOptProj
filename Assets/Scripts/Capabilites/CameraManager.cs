/*
    This script is a singleton that allows other scripts to easily change focus of the central camera
*/

using Unity.Cinemachine;
using UnityEngine;
public class CameraManager : MonoBehaviour
{
    #region Singleton Pattern
    private static CameraManager _instance;
    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<CameraManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("CameraManager");
                    _instance = singletonObject.AddComponent<CameraManager>();
                }
            }
            return _instance;
        }
    }
    #endregion
    [SerializeField] private CinemachineCamera cam;
    private GameObject target;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameManager.Instance.currentPlayer; // Set the initial target to the current player
        cam.Follow = target.transform;
    }

    public void UpdateCameraTarget(GameObject newTarget)
    {
        target = newTarget;
        cam.Follow = target.transform;
    }
}
