using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsBattle : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel; // The panel with settings options
    [SerializeField] private Button settingsButton; // The main settings button
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    private bool isGamePaused = false;

    void Start()
    {
        // Initially, hide the settings panel
        settingsPanel.SetActive(false);

        // Load saved music volume preference
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        // Attach listeners
        settingsButton.onClick.AddListener(ToggleSettingsPanel);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);

        // Initially, the resume button should be inactive
        resumeButton.gameObject.SetActive(false);
    }

    public void ToggleSettingsPanel()
    {
        // Toggle the panel's active state
        bool isActive = settingsPanel.activeSelf;
        settingsPanel.SetActive(!isActive);

        // If the game is paused, resume it when closing the panel
        if (isGamePaused && !isActive)
        {
            ResumeGame();
        }
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        AudioListener.volume = volume;
    }

    private void PauseGame()
    {
        Time.timeScale = 0; // Pause the game
        isGamePaused = true;
        pauseButton.gameObject.SetActive(false); // Hide pause button
        resumeButton.gameObject.SetActive(true); // Show resume button
    }

    private void ResumeGame()
    {
        Time.timeScale = 1; // Resume the game
        isGamePaused = false;
        pauseButton.gameObject.SetActive(true); // Show pause button
        resumeButton.gameObject.SetActive(false); // Hide resume button
    }

    private void QuitGame()
    {
        Application.Quit(); // Quits the game (works only in a built game)
    }
}
