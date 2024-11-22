using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Texture2D playButton;
    public Texture2D characterButton;
    public Texture2D trainingModeButton;
    public Texture2D arcadeButton;
    public Texture2D settingsButton;
    public Texture2D _settingsButtonIcon;
    public Texture2D _exitButtonIcon;
    public Texture2D[] settingsBackgroundImages;
    public AudioClip backgroundMusic;

    private AudioSource audioSource;
    private bool isSoundOn = true;
    private int currentSettingsBackgroundIndex = 0;

    private float fadeDuration = 10f;
    private float fadeTimer = 10f;
    private bool isFading = false;
    private Color fadeColor = Color.clear;

    void Start()
    {
        PlayerPrefs.SetString("isMultiplayerActivade", "no");

        Cursor.visible = true;

        // Initialize audio source and set mute based on isSoundOn
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // Only play music if sound is on
        audioSource.mute = !isSoundOn;
        audioSource.Play();
    }

    private void OnGUI()
    {
        float buttonWidth = Screen.width * 0.2f;
        float buttonHeight = Screen.height * 0.1f;
        float buttonGap = 10.0f;

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), settingsBackgroundImages[currentSettingsBackgroundIndex]);

        // Exit Button
        float exitButtonWidth = Screen.width * 0.08f;
        float exitButtonHeight = Screen.height * 0.08f;
        float exitButtonX = Screen.width - exitButtonWidth - 20;
        float exitButtonY = 20;
        Rect exitButtonRect = new Rect(exitButtonX, exitButtonY, exitButtonWidth, exitButtonHeight);
        if (GUI.Button(exitButtonRect, _exitButtonIcon, GUIStyle.none))
        {
            ExitButtonClicked();
        }

        GUI.color = Color.white;

        // Calculate panel dimensions and position
        float panelWidth = buttonWidth + 40;
        float panelHeight = buttonHeight * 4 + buttonGap * 3;
        float panelX = 100;
        float panelY = Screen.height / 2 - panelHeight / 2;

        GUI.Box(new Rect(panelX, panelY, panelWidth, panelHeight), GUIContent.none, GUI.skin.box);

        float centerX = Screen.width / 2 - buttonWidth / 2;

        // Play Button
        Rect playButtonRect = new Rect(centerX, panelY, buttonWidth, buttonHeight);
        if (GUI.Button(playButtonRect, GUIContent.none, GUIStyle.none))
        {

            PlayerPrefs.SetString("isMultiplayerActivade", "no");
            PlayerPrefs.SetString("isTraining", "no");
            SceneManager.LoadScene("SelectionCharacter");
        }
        GUI.DrawTexture(playButtonRect, playButton, ScaleMode.ScaleToFit);

        // Character Button
        Rect characterButtonRect = new Rect(centerX, playButtonRect.yMax + buttonGap, buttonWidth, buttonHeight);
        if (GUI.Button(characterButtonRect, GUIContent.none, GUIStyle.none))
        {
            LoadScene("CharactersViewing");
        }
        GUI.DrawTexture(characterButtonRect, characterButton, ScaleMode.ScaleToFit);

        // Training Mode Button
        Rect trainingModeButtonRect = new Rect(centerX, characterButtonRect.yMax + buttonGap, buttonWidth, buttonHeight);
        if (GUI.Button(trainingModeButtonRect, GUIContent.none, GUIStyle.none))
        {
            PlayerPrefs.SetString("isTraining", "yes");
            LoadScene("SelectionCharacter");
        }
        GUI.DrawTexture(trainingModeButtonRect, trainingModeButton, ScaleMode.ScaleToFit);

        // Arcade Button
        Rect arcadeButtonRect = new Rect(centerX, trainingModeButtonRect.yMax + buttonGap, buttonWidth, buttonHeight);
        if (GUI.Button(arcadeButtonRect, GUIContent.none, GUIStyle.none))
        {
            LoadScene("ArcadeMode");
        }
        GUI.DrawTexture(arcadeButtonRect, arcadeButton, ScaleMode.ScaleToFit);

        // Apply fading effect
        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);

            if (isFading && alpha < 1.0f)
            {
                fadeColor.a = alpha;
                GUI.color = fadeColor;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), settingsBackgroundImages[currentSettingsBackgroundIndex]);
                GUI.color = Color.white;
            }
            else
            {
                isFading = false;
                fadeTimer = 0.0f;
            }
        }
    }

    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        audioSource.mute = !isSoundOn;  // Toggle mute based on sound setting
        Debug.Log(isSoundOn ? "Sound On" : "No Sound");
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void ExitButtonClicked()
    {
        SceneManager.LoadScene("StartScene");
    }

    void Update()
    {
        if (settingsBackgroundImages.Length > 1)
        {
            float interval = 5.0f;
            if (Time.time > interval)
            {
                currentSettingsBackgroundIndex = (currentSettingsBackgroundIndex + 1) % settingsBackgroundImages.Length;
                isFading = true;
                fadeColor = Color.clear;
                interval = Time.time + 5.0f;
            }
        }
    }
}
