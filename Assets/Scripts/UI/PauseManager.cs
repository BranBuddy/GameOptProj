
using UnityEngine;
using UnityEngine.InputSystem;



public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeReference] private InputActionReference pauseAction;
    private bool isPaused = false;
    private MenuListManager menuListManager;

    private void OnEnable()
    {
        Debug.Log("[PauseManager] OnEnable called. pauseAction assigned: " + (pauseAction != null));
        if (pauseAction != null)
            pauseAction.action.performed += TogglePause;
    }
    
    private void Start()
    {
        // Ensure the pause menu is hidden at the start of the game
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }


    private void OnDisable()
    {
        Debug.Log("[PauseManager] OnDisable called.");
        if (pauseAction != null)
            pauseAction.action.performed -= TogglePause;
    }

    private void Awake()
    {
        menuListManager = GetComponent<MenuListManager>();
    }


    private void TogglePause(InputAction.CallbackContext context)
    {
        Debug.Log("[PauseManager] TogglePause called. CurrentPlayer: " + GameManager.Instance.currentPlayer + ", This: " + this.gameObject);
        Debug.Log("[PauseManager] Pause action performed");

        if (!isPaused)
        {
            Debug.Log("[PauseManager] Pausing game.");
            PauseGame();
        }
        else
        {
            Debug.Log("[PauseManager] Resuming game.");
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SwapActionMap("Player");
        pauseMenuUI.SetActive(false); 
        if (menuListManager != null)
        {
            menuListManager.GoBackToPreviousMenu();
            menuListManager.DisableBackMenuAction();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    
    }

    private void SwapActionMap(string actionMapName)
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap(actionMapName);
        }
    }

}
