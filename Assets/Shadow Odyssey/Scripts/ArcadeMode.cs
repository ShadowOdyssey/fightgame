using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeMode : MonoBehaviour
{
    // Assign these in the Unity Editor
    public Texture2D exitButtonTexture;
    public Color buttonColor = new Color(0.8f, 0.8f, 0.8f);

    private bool showButtons = false;

    // Character selection variables
    public Texture2D[] characterImages; // Array to hold character images
    private string[] characterNames = { "Selena", "Aria", "Orion", "Marcus", "Gabriella", "Nun", "Bryan" }; // Character names
    private int[] durabilityStats = { 7, 5, 9, 6, 8, 6, 8 }; // Durability stats
    private int[] offenseStats = { 8, 9, 7, 6, 6, 9, 10 }; // Offense stats
    private int[] controlEffectStats = { 6, 4, 8, 5, 7, 5, 6 }; // Control effect stats
    private int[] difficultyStats = { 4, 6, 8, 3, 5, 4, 5 }; // Difficulty stats
    private int currentIndex = 0; // Current character index
    private bool characterSelected = false; // Track if a character has been selected

    // Sound effects
    public AudioClip chooseYourHeroSound; // Sound for choosing hero
    public AudioClip selectYourArenaSound; // Sound for selecting arena
    private AudioSource audioSource; // Reference to AudioSource component

    // Panels visibility
    private bool showDifficultySelectionPanel = false; // Variable for difficulty selection
    private bool showSettingsPanel = false;
    private bool showArenaSelectionPanel = false; // Variable for arena selection
    private bool showWaitingPanel = false; // Variable to show waiting panel

    // Arena selection variables
    public Texture2D[] arenaImages; // Array to hold arena images
    private string[] arenaNames = { "Town", "Castle", "Forest", "Mountain" }; // Arena names
    private int selectedArenaIndex = 0; // Current selected arena index
    private bool arenaSelected = false; // Track if an arena has been selected

    private void Start()
    {
        Cursor.visible = true;
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        // Play the "Choose Your Hero" sound at the start
        if (chooseYourHeroSound != null)
        {
            audioSource.PlayOneShot(chooseYourHeroSound);
        }
    }

    private void OnGUI()
    {
        // If waiting panel is active, display it and exit the rest of the UI
        if (showWaitingPanel)
        {
            DrawWaitingPanel();
            return;
        }

        // If the arena selection panel is not open, draw character selection UI
        if (!showArenaSelectionPanel)
        {
            DrawTitle();
            DrawCharacter();
            DrawHeroDetails();
            DrawNavigationButtons();
            DrawSelectHeroButton();
            DrawSelectYourArenaButton();
        }
        else
        {
            DrawArenaSelectionPanel(); // Show the arena selection panel
        }

        DrawExitButton();
    }

    private void DrawTitle()
    {
        if (!characterSelected)
        {
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 48,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };

            string titleText = "Select Your Hero";
            GUI.Label(new Rect(0, 20, Screen.width, 60), titleText, titleStyle);
        }
    }

    private void DrawExitButton()
    {
        float exitButtonWidth = 70f;
        float exitButtonHeight = 70f;
        float exitButtonX = Screen.width - exitButtonWidth - 20;
        float exitButtonY = 30;
        Rect exitButtonRect = new Rect(exitButtonX, exitButtonY, exitButtonWidth, exitButtonHeight);

        if (GUI.Button(exitButtonRect, exitButtonTexture, GUIStyle.none))
        {
            ReturnToMainMenu();
        }
    }

    private void DrawSelectYourArenaButton()
{
    float arenaButtonWidth = 200f;
    float arenaButtonHeight = 50f;
    float arenaButtonX = Screen.width - arenaButtonWidth - 90;
    float arenaButtonY = 40;
    Rect arenaButtonRect = new Rect(arenaButtonX, arenaButtonY, arenaButtonWidth, arenaButtonHeight);

    if (GUI.Button(arenaButtonRect, "Select Your Arena", GetBlackButtonStyle()))
    {
        showArenaSelectionPanel = true; // Show arena selection panel

        // Play the "Select Your Arena" sound
        if (selectYourArenaSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(selectYourArenaSound);
        }
    }
}


    private void DrawCharacter()
    {
        if (characterImages.Length > 0)
        {
            float charWidth = Screen.width * 0.4f;
            float charHeight = Screen.height * 0.7f;
            float charX = (Screen.width - charWidth) / 2;
            float charY = (Screen.height - charHeight) / 2 - 50;

            GUI.DrawTexture(new Rect(charX, charY, charWidth, charHeight), characterImages[currentIndex], ScaleMode.ScaleToFit);
        }
    }

    private void DrawHeroDetails()
    {
        float panelX = Screen.width * 0.09f;
        float panelY = Screen.height * 0.2f;
        float labelWidth = 500f;
        float labelHeight = 50f;

        if (!characterSelected)
        {
            float totalStatsHeight = CalculateStatsHeight() + 20;
            float totalStatsWidth = CalculateStatsWidth() + 100;

            GUIStyle blackBackgroundStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTexture(1, 1, new Color(0, 0, 0, 0.7f)) }
            };
            GUI.Box(new Rect(panelX - 20, panelY - 10, totalStatsWidth, totalStatsHeight), GUIContent.none, blackBackgroundStyle);

            GUIStyle nameStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 40,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };
            GUI.Label(new Rect(panelX, panelY, labelWidth, labelHeight), characterNames[currentIndex], nameStyle);

            DrawStatBars(panelX, panelY + 50);
        }
    }

    private float CalculateStatsHeight()
    {
        float statHeight = 40 + 30;
        return statHeight * 4;
    }

    private float CalculateStatsWidth()
    {
        float maxLabelWidth = 0f;
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 24 };

        foreach (string stat in new[] { "Durability", "Offense", "Control Effect", "Difficulty" })
        {
            float labelWidth = labelStyle.CalcSize(new GUIContent(stat)).x;
            if (labelWidth > maxLabelWidth)
            {
                maxLabelWidth = labelWidth;
            }
        }

        return maxLabelWidth + 150 + 250;
    }

    private void DrawStatBars(float x, float y)
    {
        float barWidth = 150f;
        float barHeight = 20f;

        GUIStyle statLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 24,
            normal = { textColor = Color.white }
        };

        GUI.Label(new Rect(x, y, 150, 30), "Durability", statLabelStyle);
        GUI.HorizontalScrollbar(new Rect(x + 150, y + 5, barWidth, barHeight), 0, durabilityStats[currentIndex], 0, 10);

        GUI.Label(new Rect(x, y + 40, 150, 30), "Offense", statLabelStyle);
        GUI.HorizontalScrollbar(new Rect(x + 150, y + 45, barWidth, barHeight), 0, offenseStats[currentIndex], 0, 10);

        GUI.Label(new Rect(x, y + 80, 150, 30), "Control Effect", statLabelStyle);
        GUI.HorizontalScrollbar(new Rect(x + 150, y + 85, barWidth, barHeight), 0, controlEffectStats[currentIndex], 0, 10);

        GUI.Label(new Rect(x, y + 120, 150, 30), "Difficulty", statLabelStyle);
        GUI.HorizontalScrollbar(new Rect(x + 150, y + 125, barWidth, barHeight), 0, difficultyStats[currentIndex], 0, 10);
    }

    private void DrawNavigationButtons()
    {
        if (!characterSelected)
        {
            if (GUI.Button(new Rect(Screen.width * 0.2f - 50, Screen.height * 0.7f, 100, 100), "←", GetBlackButtonStyle()))
            {
                currentIndex = (currentIndex - 1 + characterImages.Length) % characterImages.Length;
            }

            if (GUI.Button(new Rect(Screen.width * 0.8f - 50, Screen.height * 0.7f, 100, 100), "→", GetBlackButtonStyle()))
            {
                currentIndex = (currentIndex + 1) % characterImages.Length;
            }
        }
    }

    private void DrawSelectHeroButton()
    {
        if (!characterSelected)
        {
            if (GUI.Button(new Rect(Screen.width * 0.5f - 50, Screen.height * 0.9f, 150, 50), "Select Hero", GetBlackButtonStyle()))
            {
                characterSelected = true; // Mark character as selected
                showArenaSelectionPanel = true; // Show arena selection panel
            }
        }
    }

    private void DrawArenaSelectionPanel()
{
    float panelWidth = Screen.width * 0.8f;
    float panelHeight = Screen.height * 0.8f;
    float panelX = (Screen.width - panelWidth) / 2;
    float panelY = (Screen.height - panelHeight) / 2;

    // Background box for the arena selection panel
    GUIStyle panelStyle = new GUIStyle(GUI.skin.box)
    {
        normal = { background = MakeTexture(1, 1, new Color(0, 0, 0, 0.8f)) }
    };
    GUI.Box(new Rect(panelX, panelY, panelWidth, panelHeight), GUIContent.none, panelStyle);

    // Title for the arena selection
    GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
    {
        fontSize = 36,
        fontStyle = FontStyle.Bold,
        alignment = TextAnchor.MiddleCenter,
        normal = { textColor = Color.white }
    };
    GUI.Label(new Rect(panelX, panelY + 20, panelWidth, 40), "Select Your Arena", titleStyle);

    // Draw arena images and buttons
    for (int i = 0; i < arenaImages.Length; i++)
    {
        float buttonWidth = panelWidth * 0.5f; // Width of arena button
        float buttonHeight = panelHeight * 0.4f; // Increased height for arena button
        float buttonX = panelX + (i % 2) * (buttonWidth + 20) + 30; // Two columns
        float buttonY = panelY + (i / 2) * (buttonHeight + 20) + 80; // Position vertically

        // Button for each arena
        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), arenaImages[i], GUIStyle.none))
        {
            selectedArenaIndex = i; // Store the selected arena index
            arenaSelected = true; // Mark arena as selected
            showWaitingPanel = true; // Show waiting panel after arena selection
            Debug.Log("Arena Selected: " + arenaNames[i]);
        }

        // Label for arena names
        GUI.Label(new Rect(buttonX, buttonY + buttonHeight + 5, buttonWidth, 30), arenaNames[i], titleStyle);
    }

    // Back button to return to character selection
    if (GUI.Button(new Rect(panelX + panelWidth - 100, panelY + panelHeight - 60, 80, 30), "Back"))
    {
        showArenaSelectionPanel = false; // Hide arena selection panel
        characterSelected = false; // Reset character selection
    }
}

  private void DrawWaitingPanel()
{
    float panelWidth = Screen.width * 0.6f;
    float panelHeight = Screen.height * 0.4f;
    float panelX = (Screen.width - panelWidth) / 2;
    float panelY = (Screen.height - panelHeight) / 2;

    // Background box for the waiting panel
    GUIStyle panelStyle = new GUIStyle(GUI.skin.box)
    {
        normal = { background = MakeTexture(1, 1, new Color(0, 0, 0, 0.8f)) }
    };
    GUI.Box(new Rect(panelX, panelY, panelWidth, panelHeight), GUIContent.none, panelStyle);

    // Title for the waiting panel
    GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
    {
        fontSize = 36, // Increased font size
        fontStyle = FontStyle.Bold,
        alignment = TextAnchor.MiddleCenter,
        normal = { textColor = Color.white }
    };

    // Calculate the position for the label to center it
    float labelWidth = titleStyle.CalcSize(new GUIContent("Waiting for the opponent..")).x;
    float labelHeight = titleStyle.CalcSize(new GUIContent("Waiting for the opponent..")).y;

    // Center the label within the panel
    float labelX = panelX + (panelWidth - labelWidth) / 2;
    float labelY = panelY + (panelHeight - labelHeight) / 2;

    GUI.Label(new Rect(labelX, labelY, labelWidth, labelHeight), "Waiting for the opponent..", titleStyle);
}


    private GUIStyle GetBlackButtonStyle()
{
    GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
    {
        normal = { textColor = Color.white },
        fontSize = 20,
        fontStyle = FontStyle.Bold
    };
    buttonStyle.normal.background = MakeTexture(1, 1, Color.black); // Set background to black
    return buttonStyle;
}

    private Texture2D MakeTexture(int width, int height, Color color)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] colors = new Color[width * height];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }

        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your actual main menu scene name
    }
}
