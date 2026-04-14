/*
    Handles game settings UI, including resolution, display mode, and audio sliders for music and SFX.
*/

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class Settings : MonoBehaviour
{
    [Header("Settings Components")]
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] availableResolutions;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;



    private void Start()
    {
        Debug.Log($"[Settings] Start: resolutionDropdown is {(resolutionDropdown == null ? "NULL" : "ASSIGNED")}.");
        Debug.Log($"[Settings] Start: displayModeDropdown is {(displayModeDropdown == null ? "NULL" : "ASSIGNED")}.");
        Debug.Log($"[Settings] Start: musicSlider is {(musicSlider == null ? "NULL" : "ASSIGNED")}.");
        Debug.Log($"[Settings] Start: sfxSlider is {(sfxSlider == null ? "NULL" : "ASSIGNED")}.");
        PopulateDisplayModeDropdown();
        SetUpResolutionDropdown();
    }

    private void SetDefaultSettings()
    {
        displayModeDropdown.value = 0; // Default to Fullscreen
        resolutionDropdown.value = 0; // Default to the first resolution option
        musicSlider.value = 0.5f; // Default music volume
        sfxSlider.value = 0.5f; // Default SFX volume
    }

    #region Display Mode
    public void SetDisplayMode(int displayMode)
    {
        switch (displayMode)
        {
            case 0: // Fullscreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2: // Borderless
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }

        PlayerPrefs.SetInt("DisplayMode", displayMode);
        Debug.Log($"[Settings] Display mode changed to: {Screen.fullScreenMode}");
        StartCoroutine(LogResolutionAndDisplayModeAfterDelay());
    }

    private void PopulateDisplayModeDropdown()
    {
        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(new System.Collections.Generic.List<string> { "Fullscreen", "Windowed", "Borderless" });
    }
    #endregion

    #region Resolution
    private void SetUpResolutionDropdown()
    {
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + " x " + availableResolutions[i].height;
            resolutionOptions.Add(option);

            if (availableResolutions[i].width == Screen.width && availableResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution selectedResolution = availableResolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        Debug.Log($"[Settings] Requested resolution set to: {selectedResolution.width}x{selectedResolution.height}");
        StartCoroutine(LogResolutionAndDisplayModeAfterDelay());
        
    }

    private System.Collections.IEnumerator LogResolutionAndDisplayModeAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // Wait longer for Unity to apply changes
        Debug.Log($"[Settings] Actual Screen.width/height: {Screen.width}x{Screen.height}");
        Debug.Log($"[Settings] Actual Screen.fullScreenMode: {Screen.fullScreenMode}");
        Debug.Log($"[Settings] Actual Screen.currentResolution: {Screen.currentResolution.width}x{Screen.currentResolution.height} @{Screen.currentResolution.refreshRateRatio.value}Hz");
    }
    #endregion

    #region Volume Sliders
    public void SetMusicVolume(float volume)
    {
       SoundManager.Instance.musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SoundManager.Instance.sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    #endregion
}