using TMPro;
using UnityEngine;

public class InteractionUIManager : MonoBehaviour
{
    #region Singleton Pattern
    private static InteractionUIManager _instance;
    public static InteractionUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<InteractionUIManager>();
                if (_instance == null)
                {
                    Debug.LogError("No InteractionUIManager found in the scene. Please add one to your UI canvas and assign the interactionText field.");
                }
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private string defaultInteractionText = "Press [E] to interact with ";

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

        interactionText.text = "";
        interactionText.gameObject.SetActive(false);
    }

    public void ShowInteractionText(string text)
    {
        interactionText.text = defaultInteractionText + text;
        interactionText.gameObject.SetActive(true);
    }

    public void HideInteractionText()
    {
        interactionText.text = "";
        interactionText.gameObject.SetActive(false);
    }
}
