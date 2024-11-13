using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScene : MonoBehaviour
{
    public Texture2D _playButtonImage; // Play button image
    public Texture2D _settingsButtonImage; // Settings button image
    public Texture2D _exitButtonImage; // Exit button image
    public Texture2D[] settingsBackgroundImages; // Array to hold the settings panel background images

    public GameObject settingsPanel; // Reference to the settings panel GameObject

    private bool settingsOpen = false;
    private bool isSoundOn = true; // Flag to track sound state
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
        float buttonWidth = Screen.width * 0.5f; // Increased width for the play button
        float buttonHeight = Screen.height * 0.3f; // Increased height for the play button

        // Draw background image for the whole scene
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), settingsBackgroundImages[currentSettingsBackgroundIndex]);

        // Exit button
        float exitButtonWidth = Screen.width * 0.08f; // Smaller exit button width
        float exitButtonHeight = Screen.height * 0.08f; // Smaller exit button height
        float exitButtonX = Screen.width - exitButtonWidth - 20;
        float exitButtonY = 20;
        Rect exitButtonRect = new Rect(exitButtonX, exitButtonY, exitButtonWidth, exitButtonHeight);
        if (GUI.Button(exitButtonRect, _exitButtonImage, GUIStyle.none))
        {
            ExitButtonClicked();
        }

        // Settings button
        float settingsButtonWidth = exitButtonWidth;
        float settingsButtonHeight = exitButtonHeight;
        float settingsButtonX = exitButtonX - settingsButtonWidth - 20; // Adjusted to reduce the gap
        float settingsButtonY = exitButtonY;
        Rect settingsButtonRect = new Rect(settingsButtonX, settingsButtonY, settingsButtonWidth, settingsButtonHeight);
        if (GUI.Button(settingsButtonRect, _settingsButtonImage, GUIStyle.none))
        {
            settingsOpen = !settingsOpen;
            settingsPanel.SetActive(settingsOpen); // Toggle settings panel visibility
        }

        // Play button
        float playButtonOffsetX = 450f; // Centered horizontally
        float playButtonOffsetY = 100f; // Centered vertically
        Rect playButtonRect = new Rect((Screen.width - buttonWidth) / 2 + playButtonOffsetX, (Screen.height - buttonHeight) / 2 + playButtonOffsetY, buttonWidth, buttonHeight);
        if (GUI.Button(playButtonRect, _playButtonImage, GUIStyle.none))
        {
            StartButtonClicked();
        }

        // Settings panel
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
        // Here you can implement logic to toggle sound on and off
        if (isSoundOn)
        {
            Debug.Log("Sound On");
        }
        else
        {
            Debug.Log("No Sound");
        }
    }

    void StartButtonClicked()
    {
        // Here you can load the next scene after clicking the Start button
        SceneManager.LoadScene("MainMenu");
    }

    void ExitButtonClicked()
    {
        SceneManager.LoadScene("OverviewStory");
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
