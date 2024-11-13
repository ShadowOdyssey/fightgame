using UnityEngine;
using UnityEngine.SceneManagement; // Namespace for scene management

public class BattleGroundSelection : MonoBehaviour
{
    [Header("Audio Elements")]
    public AudioClip voiceClip; // The voice recording to play
    private AudioSource audioSource; // AudioSource component to play the clip

    [Header("UI Elements")]
    public Texture2D[] battlegroundImages; // Array to store the 4 background images
    public Texture2D backgroundTexture;    // Background image for the selection screen
    public Texture2D checkmarkTexture;     // Texture for checkmark or selection indicator
    public Texture2D selectButtonTexture;  // Texture for the "Select" button
    public Texture2D exitButtonImage;      // Texture for the exit button
    public Texture2D checkboxTexture;      // Texture for the checkbox circle
    public Texture2D checkboxSelectedTexture; // Texture for the filled checkbox circle

    [Header("Scene Names")]
    [Tooltip("Ensure these match exactly with your scene names in the Build Settings.")]
    public string[] battlegroundSceneNames = { "Battleground1", "Battleground2", "Battleground3", "Battleground4" }; // 4 scenes

    // Array for arena names
    public string[] arenaNames = { "Town", "Castle", "Forest", "Mountain" }; // Add arena names

    private int selectedArenaIndex = -1;   // Track which arena the player selects
    public GameObject selectionPanel;      // The panel that shows the selection options

    private bool selectionOpen = true;     // Controls whether the selection panel is open

    void Start()
    {
        // Set up the AudioSource
        audioSource = gameObject.AddComponent<AudioSource>(); // Add an AudioSource component to the GameObject
        audioSource.clip = voiceClip; // Assign the voice clip to the AudioSource

        // Play the voice clip
        if (voiceClip != null)
        {
            audioSource.Play(); // Play the voice recording
        }
        else
        {
            Debug.LogError("Voice clip is not assigned in the Inspector.");
        }

        // Validate battlegroundImages array
        if (battlegroundImages.Length != 4)
        {
            Debug.LogError("Please assign exactly 4 battleground images in the Inspector!");
        }

        // Validate battlegroundSceneNames array
        if (battlegroundSceneNames.Length != battlegroundImages.Length)
        {
            Debug.LogError("The number of battleground scene names must match the number of battleground images.");
        }

        // Validate arenaNames array
        if (arenaNames.Length != battlegroundImages.Length)
        {
            Debug.LogError("The number of arena names must match the number of battleground images.");
        }

        if (selectionPanel != null)
        {
            selectionPanel.SetActive(true); // Show the selection panel at start
        }
        else
        {
            Debug.LogError("Selection Panel is not assigned in the Inspector.");
        }
    }

    void OnGUI()
    {
        // Draw the full-screen background image
        if (backgroundTexture != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture, ScaleMode.StretchToFill);
        }

        // Draw the selection panel if it's open
        if (selectionOpen)
        {
            DrawSelectionPanel();
        }

        // Show the "Select" button once a battleground is selected
        if (selectedArenaIndex >= 0)
        {
            DrawSelectButton();
        }

        // Draw Exit Button
        DrawExitButton();
    }

    void DrawSelectionPanel()
    {
        // Adjust panel size for 4 images instead of 5
        float panelWidth = Screen.width * 0.8f;    // 80% of screen width
        float panelHeight = Screen.height * 0.6f;  // 60% of screen height
        float panelX = Screen.width / 2 - panelWidth / 2; // Center the panel horizontally
        float panelY = Screen.height / 2 - panelHeight / 2; // Center the panel vertically

        // Begin the selection panel area
        GUILayout.BeginArea(new Rect(panelX, panelY, panelWidth, panelHeight), GUI.skin.box);

        // Centered title with larger font size
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 50; // Make the font larger
        GUILayout.Label("Select Your Arena", titleStyle, GUILayout.ExpandWidth(true));

        // Spacer
        GUILayout.Space(30);

        // Begin horizontal layout for battleground buttons
        GUILayout.BeginHorizontal();

        // Loop through the battlegrounds and create a clickable image box for each one
        for (int i = 0; i < battlegroundImages.Length; i++)
        {
            // Adjust button size and padding for 4 images
            float buttonWidth = (panelWidth - 40) / 4;  // Adjust width for 4 images (20 padding on each side)
            float buttonHeight = panelHeight - 160;     // Adjust height for bigger images

            // Create a vertical layout for each battleground
            GUILayout.BeginVertical(GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight));

            // Display arena name above the image
            GUIStyle nameStyle = new GUIStyle(GUI.skin.label);
            nameStyle.alignment = TextAnchor.MiddleCenter;
            nameStyle.fontSize = 30; // Increased font size for better readability
            GUILayout.Label(arenaNames[i], nameStyle); // Draw arena name

            // Create a button with the battleground image
            if (GUILayout.Button(new GUIContent(battlegroundImages[i]), GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                selectedArenaIndex = i; // Set selected arena index
            }

            // Explicitly position and draw the checkbox
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            Rect checkboxRect = new Rect(panelX + i * (buttonWidth + 20) + buttonWidth / 2 - 25, panelY + buttonHeight + 35, 50, 50);

            // Draw checkbox based on selection
            if (selectedArenaIndex == i)
            {
                if (checkboxSelectedTexture != null)
                {
                    GUI.DrawTexture(checkboxRect, checkboxSelectedTexture); // Draw filled checkbox
                }
                else
                {
                    Debug.LogError("Selected checkbox texture is missing.");
                }
            }
            else
            {
                if (checkboxTexture != null)
                {
                    GUI.DrawTexture(checkboxRect, checkboxTexture); // Draw empty checkbox
                }
                else
                {
                    Debug.LogError("Unselected checkbox texture is missing.");
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            // Spacer between buttons
            if (i < battlegroundImages.Length - 1)
            {
                GUILayout.Space(20); // Adjust spacing between buttons
            }
        }

        // End horizontal layout
        GUILayout.EndHorizontal();

        // End the selection panel area
        GUILayout.EndArea();
    }

    void DrawSelectButton()
    {
        // Increase size and adjust position for "Select" button
        float buttonWidth = 400; // Larger size for select button
        float buttonHeight = 80; // Increased size for select button
        float buttonX = Screen.width / 2 - buttonWidth / 2; // Center the button horizontally
        float buttonY = Screen.height - 250; // Position the button near the bottom of the screen

        // Draw the button background
        GUI.color = Color.black; // Set button color to black
        GUI.DrawTexture(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), Texture2D.whiteTexture); // White texture for the button background
        GUI.color = Color.white; // Reset the color to default

        // Create a style for the button text
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.label);
        buttonStyle.normal.textColor = Color.white; // Set text color to white
        buttonStyle.fontSize = 40; // Set font size
        buttonStyle.alignment = TextAnchor.MiddleCenter; // Center the text

        // Create a rect for the button with the same size
        Rect buttonRect = new Rect(buttonX, buttonY, buttonWidth, buttonHeight);

        // Detect click on the button
        if (GUI.Button(buttonRect, "", GUIStyle.none)) // Use an empty button style to make it clickable
        {
            // Removed LoadSelectedArena call, no functionality for selecting an arena
        }

        // Draw the "Select" text on top of the button
        GUI.Label(buttonRect, "Select", buttonStyle); // Use GUI.Label to draw centered text
    }

    void DrawExitButton()
    {
        // Draw the exit button with the same size and position as the CharacterSelection scene
        float exitButtonWidth = 65f; // Same width as in CharacterSelection
        float exitButtonHeight = 65f; // Same height as in CharacterSelection
        float exitButtonX = 100f; // Increased horizontal offset to match CharacterSelection
        float exitButtonY = 20f;  // Vertical offset

        Rect exitButtonRect = new Rect(exitButtonX, exitButtonY, exitButtonWidth, exitButtonHeight);

        // Check if a custom exit button texture is available
        if (exitButtonImage != null)
        {
            // Draw the exit button image
            GUI.DrawTexture(exitButtonRect, exitButtonImage);

            // Check if the button is clicked
            if (GUI.Button(exitButtonRect, "", GUIStyle.none))
            {
                ExitButtonClicked();
            }
        }
        else
        {
            // Draw a default button if no texture is assigned
            if (GUI.Button(exitButtonRect, "Exit"))
            {
                ExitButtonClicked();
            }
        }
    }

    void ExitButtonClicked()
    {
        // Load the MainMenu scene when exit button is clicked
        Debug.Log("Exit button clicked!"); // Added a debug log for feedback
        SceneManager.LoadScene("MainMenu"); // Load the MainMenu scene
    }
}
