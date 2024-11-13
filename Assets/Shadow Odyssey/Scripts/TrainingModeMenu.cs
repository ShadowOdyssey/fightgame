using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingModeMenu : MonoBehaviour
{
    // Assign these in the Unity Editor
    public Texture2D exitButtonTexture; // Removed menuButtonTexture and settingsButtonTexture
    public Color buttonColor = new Color(0.8f, 0.8f, 0.8f);

    private bool showButtons = false;

    // Character selection variables
    public Texture2D[] characterImages; // Array to hold character images
    private string[] characterNames = { "Selena", "Aria", "Orion", "Marcus", "Gabriella", "Nun" }; // Character names
    private int[] durabilityStats = { 7, 5, 9, 6, 8, 6 }; // Durability stats
    private int[] offenseStats = { 8, 9, 7, 6, 6, 9 }; // Offense stats
    private int[] controlEffectStats = { 6, 4, 8, 5, 7, 5 }; // Control effect stats
    private int[] difficultyStats = { 4, 6, 8, 3, 5, 4 }; // Difficulty stats
    private int currentIndex = 0; // Current character index
    private AudioSource audioSource;

    // Booleans to track panel visibility
    private bool showTrainingPanel = false;
    private bool showOpponentPanel = false;
    private bool showMovelistPanel = false;
    private bool showFreePracticePanel = false;

    private void Start()
    {
        Cursor.visible = true;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnGUI()
    {
        // Draw the title
        DrawTitle(); // Call to draw the title text

        // Draw the character in the center
        DrawCharacter();

        // Draw hero details on the left (name and stats)
        DrawHeroDetails();

        // Navigation buttons
        DrawNavigationButtons();

        // Select button
        DrawActionButtons();

        // Draw the exit button
        DrawExitButton();
    }

    private void DrawTitle()
    {
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 48,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white }
        };
        GUI.Label(new Rect(0, 20, Screen.width, 60), "SELECT YOUR HERO", titleStyle);
    }

    private void DrawExitButton()
    {
        // Exit Button
        float exitButtonWidth = 70f;
        float exitButtonHeight =70f;
        float exitButtonX = Screen.width - exitButtonWidth - 20;
        float exitButtonY = 30;
        Rect exitButtonRect = new Rect(exitButtonX, exitButtonY, exitButtonWidth, exitButtonHeight);

        if (GUI.Button(exitButtonRect, exitButtonTexture, GUIStyle.none))
        {
            ReturnToMainMenu(); // Change this to point to MainMenu
        }
    }

    private void DrawCharacter()
    {
        // Increase the character size
        if (characterImages.Length > 0)
        {
            float charWidth = Screen.width * 0.4f; // Character width
            float charHeight = Screen.height * 0.7f; // Character height
            float charX = (Screen.width - charWidth) / 2;
            float charY = (Screen.height - charHeight) / 2 - 50; // Adjust Y for better positioning

            GUI.DrawTexture(new Rect(charX, charY, charWidth, charHeight), characterImages[currentIndex], ScaleMode.ScaleToFit);
        }
    }

    private void DrawHeroDetails()
    {
        float panelX = Screen.width * 0.09f;
        float panelY = Screen.height * 0.2f;
        float labelWidth = 500f;
        float labelHeight = 50f;

        // Calculate total height for the stats
        float totalStatsHeight = CalculateStatsHeight() + 20; // Add some padding
        float totalStatsWidth = CalculateStatsWidth() + 100; // Add more padding for width

        // Black background for stats
        GUIStyle blackBackgroundStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = MakeTexture(1, 1, new Color(0, 0, 0, 0.7f)) } // Darker background for better contrast
        };
        GUI.Box(new Rect(panelX - 20, panelY - 10, totalStatsWidth, totalStatsHeight), GUIContent.none, blackBackgroundStyle); // Adjust height dynamically

        // Hero name
        GUIStyle nameStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 40, // Larger font size for name
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };
        GUI.Label(new Rect(panelX, panelY, labelWidth, labelHeight), characterNames[currentIndex], nameStyle);

        // Draw hero stats
        DrawStatBars(panelX, panelY + 50); // Adjusted Y for stats
    }

    float CalculateStatsHeight()
    {
        // Each stat has a height of 40 + 30 for the label; adjust if necessary
        float statHeight = 40 + 30; 
        return statHeight * 4; // 4 stats
    }

    float CalculateStatsWidth()
    {
        // Adjust based on longest label width and bar width
        float maxLabelWidth = 0f;
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 24 // Match the stat label font size
        };

        // Calculate max width for stat labels
        foreach (string stat in new[] { "Durability", "Offense", "Control Effect", "Difficulty" })
        {
            float labelWidth = labelStyle.CalcSize(new GUIContent(stat)).x;
            if (labelWidth > maxLabelWidth)
            {
                maxLabelWidth = labelWidth;
            }
        }

        // Total width is the max label width + the width of the bars
        return maxLabelWidth + 150 + 250; // 150 for bar labels and 250 for the progress bar
    }

    void DrawStatBars(float x, float y)
    {
        float barWidth = 150f; // Larger bars
        float barHeight = 20f; // Taller bars

        // Styles
        GUIStyle statLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 24, // Larger font size for stats
            normal = { textColor = Color.white }
        };

        // Durability
        GUI.Label(new Rect(x, y, 150, 30), "Durability", statLabelStyle);
        GUI.HorizontalScrollbar(new Rect(x + 150, y + 5, barWidth, barHeight), 0, durabilityStats[currentIndex], 0, 10);

        // Offense
        GUI.Label(new Rect(x, y + 40, 150, 30), "Offense", statLabelStyle);
        GUI.HorizontalScrollbar(new Rect(x + 150, y + 45, barWidth, barHeight), 0, offenseStats[currentIndex], 0, 10);

        // Control Effect
        GUI.Label(new Rect(x, y + 80, 150, 30), "Control Effect", statLabelStyle);
        GUI.HorizontalScrollbar(new Rect(x + 150, y + 85, barWidth, barHeight), 0, controlEffectStats[currentIndex], 0, 10);

        // Difficulty
        GUI.Label(new Rect(x, y + 120, 150, 30), "Difficulty", statLabelStyle);
        GUI.HorizontalScrollbar(new Rect(x + 150, y + 125, barWidth, barHeight), 0, difficultyStats[currentIndex], 0, 10);
    }

    private void DrawNavigationButtons()
    {
        float buttonSize = 60f; // Size of the navigation buttons
        float charWidth = Screen.width * 0.4f; // Width of the character image
        float charHeight = Screen.height * 0.7f; // Height of the character image
        float charX = (Screen.width - charWidth) / 2; // X position of the character image
        float charY = (Screen.height - charHeight) / 2 - 50; // Y position of the character image

        // Adjust Y position for buttons to be below the character image
        float buttonY = charY + charHeight + 20; // 20 pixels below the character image

        // Left button (Previous character)
        if (GUI.Button(new Rect(charX - buttonSize - 20, buttonY, buttonSize, buttonSize), "<", GetBlackButtonStyle()))
        {
            currentIndex = (currentIndex - 1 + characterImages.Length) % characterImages.Length; // Previous character
        }

        // Right button (Next character)
        if (GUI.Button(new Rect(charX + charWidth + 20, buttonY, buttonSize, buttonSize), ">", GetBlackButtonStyle()))
        {
            currentIndex = (currentIndex + 1) % characterImages.Length; // Next character
        }
    }

    private void DrawActionButtons()
    {
        // Action buttons (Select, Return to Main Menu, etc.)
        float buttonWidth = 210f;
        float buttonHeight = 70f;
        float buttonY = Screen.height - buttonHeight - 20; // Position near the bottom
        float buttonX = (Screen.width - buttonWidth) / 2; // Centered

        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Select", GetBlackButtonStyle()))
        {
            // Implement character selection logic here
            Debug.Log("Character Selected: " + characterNames[currentIndex]);
        }

        // Add more action buttons as needed...
    }

    private GUIStyle GetBlackButtonStyle()
    {
        GUIStyle blackButtonStyle = new GUIStyle(GUI.skin.button)
        {
            normal = { background = MakeTexture(1, 1, Color.black), textColor = Color.white },
            hover = { background = MakeTexture(1, 1, new Color(0.5f, 0.5f, 0.5f)), textColor = Color.white },
            active = { background = MakeTexture(1, 1, new Color(0.7f, 0.7f, 0.7f)), textColor = Color.white },
            alignment = TextAnchor.MiddleCenter,
            fontSize = 24
        };
        return blackButtonStyle;
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Adjust to the name of your main menu scene
    }

    private Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}
