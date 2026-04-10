using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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

    // Checklist:
    // 1. Is pauseAction assigned in the Inspector?
    // 2. Is the PlayerInput component present and set to the 'Player' action map at start?
    // 3. Is the PauseManager on the same GameObject as PlayerInput?
    // 4. Is the pause binding mapped to the correct key (e.g., Escape) in your Input Actions asset?

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SwapActionMap("Player"); // Switch back to the player action map when resuming the game 
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
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
        Time.timeScale = 0f; 
        isPaused = true;
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        if (menuListManager != null)
        {
            menuListManager.AddMenuToList(pauseMenuUI);
        }
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
