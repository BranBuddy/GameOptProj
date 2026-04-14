/*
    Handles events for the main menu like quitting, starting game, etc.
*/


using UnityEngine;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private int mainSceneIndex = 1; // Index of the main game scene to load
    [SerializeField] private int loadingSceneIndex = 2; // Index of the loading screen scene to load
    [SerializeField] private int winSceneIndex = 3; // Index of the win scene to load
    public bool isInMainMenu = true;

    private void Start()
    {
        if (isInMainMenu)
        {
            PlayMainMenuMusic();
        }
    }

    public void PlayGame()
    {
        SoundManager.Instance.musicSource.Stop(); // Stop the main menu music
        SceneLoader.Instance.StartGame(mainSceneIndex); // Load the loading screen scene
        SoundManager.Instance.musicSource.clip = SoundManager.Instance.levelMusic; // Set the level music to play after loading
        SoundManager.Instance.musicSource.Play(); // Play the level music
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }

    private void PlayMainMenuMusic()
    {
        SoundManager.Instance.musicSource.clip = SoundManager.Instance.mainMenuMusic;
        SoundManager.Instance.musicSource.Play();
    }

    public void GoToMain()
    {
        SoundManager.Instance.musicSource.Stop(); // Stop the current music
        SceneLoader.Instance.LoadScene(0); // Load the main menu scene
        SoundManager.Instance.musicSource.clip = SoundManager.Instance.mainMenuMusic; // Set the main menu music to play
        SoundManager.Instance.musicSource.Play(); // Play the main menu music
    }

}
