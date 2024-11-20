using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public AudioSource currentAudio; // Added the AudioSource component here to make Sound On and Sound Off option to work - Felipe

    public Texture2D _playButtonImage; // Play button image
    public Texture2D _settingsButtonImage; // Settings button image
    public Texture2D _exitButtonImage; // Exit button image
    public Texture2D[] settingsBackgroundImages; // Array to hold the settings panel background images

    private bool settingsOpen = false;
    private bool isSoundOn = true; // Flag to track sound state
    private int currentSettingsBackgroundIndex = 0;
    private string labelText = "Input your name!";
    private string inputText = "";

    // Fading variables
    private readonly float fadeDuration = 10f; // Duration of fade in seconds
    private float fadeTimer = 10f;
    private bool isFading = false;
    private Color fadeColor = Color.clear;

    public void Start()
    {
        Cursor.visible = true;

        Debug.Log(PlayerPrefs.GetString("playerName"));

        if (PlayerPrefs.GetString("playerName") != "")
        {
            inputText = PlayerPrefs.GetString("playerName");
            labelText = "Welcome Back " + PlayerPrefs.GetString("playerName");
        }
    }

    private void Update()
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

    private void OnGUI()
    {
        float buttonWidth = Screen.width * 0.3f; // Width for the play button
        float buttonHeight = Screen.height * 0.15f; // Height for the play button

        // Draw background image for the whole scene
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), settingsBackgroundImages[currentSettingsBackgroundIndex]);

        // Draw Input Field
        GUIStyle inputStyle = new GUIStyle(GUI.skin.textField);
        inputStyle.fontSize = 60; // Adjust the font size as needed
        inputStyle.normal.textColor = Color.black; // Set text color to black
        inputStyle.normal.background = MakeTex(2, 2, Color.white); // Set background color to white

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 60; // Adjust the font size as needed

        Rect inputFieldRect = new Rect(700, 400, Screen.width * 0.4f, Screen.height * 0.06f);
        inputText = GUI.TextField(inputFieldRect, inputText, inputStyle);
        Rect labelFieldRect = new Rect(1000, 300, Screen.width * 0.4f, Screen.height * 0.06f);
        GUI.Label(labelFieldRect, labelText, labelStyle);
        

        // Exit button
        float exitButtonWidth = Screen.width * 0.08f; // Smaller exit button width
        float exitButtonHeight = Screen.height * 0.08f; // Smaller exit button height
        float exitButtonX = Screen.width - exitButtonWidth - 20; // Positioning
        float exitButtonY = 20;
        Rect exitButtonRect = new Rect(exitButtonX, exitButtonY, exitButtonWidth, exitButtonHeight);
        if (GUI.Button(exitButtonRect, _exitButtonImage, GUIStyle.none))
        {
            ExitButtonClicked();
        }

        // Settings button
        float settingsButtonWidth = exitButtonWidth; // Same width as the exit button
        float settingsButtonHeight = exitButtonHeight; // Same height as the exit button
        float settingsButtonX = exitButtonX - settingsButtonWidth - 20; // Adjusted to reduce the gap
        float settingsButtonY = exitButtonY;
        Rect settingsButtonRect = new Rect(settingsButtonX, settingsButtonY, settingsButtonWidth, settingsButtonHeight);
        if (GUI.Button(settingsButtonRect, _settingsButtonImage, GUIStyle.none))
        {
            settingsOpen = !settingsOpen; // Toggle settings state
        }

        // Centered Play button with further right adjustment
        Rect playButtonRect = new Rect(1100, 500, Screen.width * 0.8f, Screen.height * 0.1f);
        if (GUI.Button(playButtonRect, _playButtonImage, GUIStyle.none))
        {
            if (inputText == "")
            {
                labelText = "Please input a name!";
            }
            
            if (inputText != "")
            {
                PlayerPrefs.SetString("playerName", inputText);
                labelText = "Loading...";
                Invoke(nameof(StartButtonClicked), 5f); // Load Main Menu after 5 seconds to give time to persist data correctly
            }
        }

        // Draw settings panel when settingsOpen is true
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
            if (alpha < 1.0f)
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

    private static Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private void DrawSettingsPanel()
    {
        float panelWidth = Screen.width * 0.7f; // Increased panel width
        float panelHeight = Screen.height * 0.8f; // Increased panel height
        float panelX = Screen.width / 2 - panelWidth / 2; // Center panel
        float panelY = Screen.height / 2 - panelHeight / 2; // Center panel

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
            DisplayButtonClicked(); // Load 1BattleGroundSelection when Display is clicked
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        // Implement logic to toggle sound on and off
        if (isSoundOn)
        {
            Debug.Log("Sound On");
            currentAudio.Play(); // Assigned AudioSource component to play the music - Felipe
        }
        else
        {
            Debug.Log("No Sound");
            currentAudio.Stop();// Assigned AudioSource component to stop to play the music - Felipe
        }
    }

    private void StartButtonClicked()
    {
        // Load the next scene after clicking the Start button
        SceneManager.LoadScene("MainMenu");
    }

    private void ExitButtonClicked()
    {
        SceneManager.LoadScene("OverviewStory");
    }

    private void DisplayButtonClicked()
    {
        // Load the 1BattleGroundSelection scene when the Display button is clicked
        SceneManager.LoadScene("1BattleGroundSelection");
    }
}
