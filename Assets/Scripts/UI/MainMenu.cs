using UnityEngine;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private int mainSceneIndex = 1; // Index of the main game scene to load
    [SerializeField] private int loadingSceneIndex = 2; // Index of the loading screen scene to load

    private void Start()
    {
        PlayMainMenuMusic();
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


}
