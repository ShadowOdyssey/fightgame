using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Texture2D _singlePlayerButtonImage;
    public Texture2D _trainingModeButtonImage;
    public Texture2D _leaderboardsButtonImage;
    public Texture2D _settingsButtonIcon;
    public Texture2D _exitButtonIcon;
    public Texture2D[] settingsBackgroundImages;
    public GameObject settingsPanel;

    private bool settingsOpen = false;
    private bool isSoundOn = true;
    private int currentSettingsBackgroundIndex = 0;

    // Fading variables
    private float fadeDuration = 10f; // Duration of fade in seconds
    private float fadeTimer = 10f;
    private bool isFading = false;
    private Color fadeColor = Color.clear;

    void Start()
    {
        Cursor.visible = true;
        settingsPanel.SetActive(false); // Settings panel is initially hidden
    }

    void OnGUI()
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

        // Settings Button
        float settingsButtonWidth = exitButtonWidth;
        float settingsButtonHeight = exitButtonHeight;
        float settingsButtonX = exitButtonX - settingsButtonWidth - 20;
        float settingsButtonY = exitButtonY;
        Rect settingsButtonRect = new Rect(settingsButtonX, settingsButtonY, settingsButtonWidth, settingsButtonHeight);
        if (GUI.Button(settingsButtonRect, _settingsButtonIcon, GUIStyle.none))
        {
            settingsOpen = !settingsOpen;
            settingsPanel.SetActive(settingsOpen); // Toggle settings panel visibility
        }

        GUI.color = Color.white;

        // Calculate panel dimensions and position
        float panelWidth = buttonWidth + 40;
        float panelHeight = buttonHeight * 3 + buttonGap * 2;
        float panelX = 100;
        float panelY = Screen.height / 2 - panelHeight / 2;

        GUI.Box(new Rect(panelX, panelY, panelWidth, panelHeight), GUIContent.none, GUI.skin.box);

        // Centering buttons
        float centerX = Screen.width / 2 - buttonWidth / 2;

        // Single Player Button
        Rect singlePlayerButtonRect = new Rect(centerX, panelY, buttonWidth, buttonHeight);
        if (GUI.Button(singlePlayerButtonRect, GUIContent.none, GUIStyle.none))
        {
            SceneManager.LoadScene("SinglePlayer");
        }
        GUI.DrawTexture(singlePlayerButtonRect, _singlePlayerButtonImage, ScaleMode.ScaleToFit);

        // Training Mode Button
        Rect trainingModeButtonRect = new Rect(centerX, singlePlayerButtonRect.yMax + buttonGap, buttonWidth, buttonHeight);
        if (GUI.Button(trainingModeButtonRect, GUIContent.none, GUIStyle.none))
        {
            LoadScene("TrainingMode");
        }
        GUI.DrawTexture(trainingModeButtonRect, _trainingModeButtonImage, ScaleMode.ScaleToFit);

        // Leaderboards Button
        Rect leaderboardsButtonRect = new Rect(centerX, trainingModeButtonRect.yMax + buttonGap, buttonWidth, buttonHeight);
        if (GUI.Button(leaderboardsButtonRect, GUIContent.none, GUIStyle.none))
        {
            LoadScene("Leaderboards");
        }
        GUI.DrawTexture(leaderboardsButtonRect, _leaderboardsButtonImage, ScaleMode.ScaleToFit);

        if (settingsOpen)
        {
            DrawSettingsPanel();
        }

        // Apply fading effect
        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);

            // Fade in
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

    void DrawSettingsPanel()
    {
        float panelWidth = Screen.width * 0.7f; // Increased panel width
        float panelHeight = Screen.height * 0.8f; // Increased panel height
        float panelX = Screen.width / 2 - panelWidth / 2;
        float panelY = Screen.height / 2 - panelHeight / 2;

        // Create a custom GUIStyle for the buttons with a larger font size
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 30;

        GUILayout.BeginArea(new Rect(panelX, panelY, panelWidth, panelHeight), GUI.skin.box);
        GUILayout.Label("Settings", GUILayout.ExpandWidth(true));

        // Sound Button
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(isSoundOn ? "Sound On" : "No Sound", buttonStyle, GUILayout.ExpandWidth(true), GUILayout.Height(80))) // Increased height
        {
            ToggleSound(); // Toggle sound when clicked
        }
        GUILayout.EndHorizontal();

        // Display Button
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Display", buttonStyle, GUILayout.ExpandWidth(true), GUILayout.Height(80))) // Increased height
        {
            // Display button action
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
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
        // Automatically cycle through the settings background images
        if (settingsBackgroundImages.Length > 1 && !settingsOpen)
        {
            float interval = 5.0f; // Change image every 5 seconds
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
