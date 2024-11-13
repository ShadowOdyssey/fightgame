using UnityEngine;
using UnityEngine.SceneManagement;

public class FightingSingle : MonoBehaviour
{
    public Texture2D _settingsButtonImage; // Settings button image
    public Texture2D _exitButtonImage; // Exit button image
    public Texture2D[] backgroundImages; // Array to hold background images
    public GameObject character; // Reference to the character GameObject

    // Health variables for each player
    private int player1MaxHealth = 100;
    private int player2MaxHealth = 100;
    private int player1CurrentHealth;
    private int player2CurrentHealth;

    private bool settingsOpen = false;
    private bool isSoundOn = true; // Flag to track sound state
    private int currentBackgroundIndex = 0;

    void Start()
    {
        // Initialize current health for each player
        player1CurrentHealth = player1MaxHealth;
        player2CurrentHealth = player2MaxHealth;
    }

    void OnGUI()
    {
        // Draw larger health bars for player 1 and player 2
        DrawHealthBar(50, 70, player1CurrentHealth, player1MaxHealth, Color.green); // Player 1 health bar in green
        DrawHealthBar(Screen.width - 450, 70, player2CurrentHealth, player2MaxHealth, Color.red); // Player 2 health bar in red

        // Draw settings and exit buttons with textures
        DrawSettingsButton();
        DrawExitButton();

        // Draw settings panel if settings are open
        if (settingsOpen)
        {
            DrawSettingsPanel();
        }
    }

    void DrawHealthBar(float x, float y, int currentHealth, int maxHealth, Color healthColor)
    {
        float barWidth = 600; // Increased width
        float barHeight = 70; // Increased height

        // Calculate the percentage of health remaining
        float healthPercentage = (float)currentHealth / maxHealth;

        // Draw the background of the health bar
        GUI.Box(new Rect(x, y, barWidth, barHeight), "");

        // Set the color for the filled part of the health bar
        GUI.color = healthColor;

        // Draw the filled part of the health bar using solid color
        Rect healthRect = new Rect(x, y, barWidth * healthPercentage, barHeight);
        GUI.DrawTexture(healthRect, Texture2D.whiteTexture);

        // Reset GUI color to avoid affecting other GUI elements
        GUI.color = Color.white;
    }

    void DrawSettingsButton()
    {
        float buttonWidth = 80;
        float buttonHeight = 80;
        float buttonX = Screen.width - buttonWidth - 30; // Adjusted position to the left
        float buttonY = 10;

        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), _settingsButtonImage, GUIStyle.none))
        {
            settingsOpen = !settingsOpen; // Toggle settings panel visibility
        }
    }

    void DrawExitButton()
    {
        float buttonWidth = 80;
        float buttonHeight = 80;
        float buttonX = Screen.width - buttonWidth - 30; // Adjusted position to the left
        float buttonY = 100;

        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), _exitButtonImage, GUIStyle.none))
        {
            ExitButtonClicked();
        }
    }

    void DrawSettingsPanel()
    {
        // Significantly enlarged panel size for better visibility
        float panelWidth = 900;
        float panelHeight = 700;
        float panelX = Screen.width / 2 - panelWidth / 2;
        float panelY = Screen.height / 2 - panelHeight / 2;

        // Use a large font size for better readability
        GUIStyle largeFont = new GUIStyle(GUI.skin.label);
        largeFont.fontSize = 30;

        // Create a custom GUIStyle for buttons with a larger font size
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 30;

        GUILayout.BeginArea(new Rect(panelX, panelY, panelWidth, panelHeight), GUI.skin.box);
        GUILayout.Label("Settings", largeFont);

        // Pause Button
        if (GUILayout.Button("Pause", buttonStyle, GUILayout.Height(100)))
        {
            PauseGame();
        }

        // Sound Toggle
        GUILayout.BeginHorizontal();
        GUILayout.Label("Sound:", largeFont);
        isSoundOn = GUILayout.Toggle(isSoundOn, isSoundOn ? "On" : "Off", largeFont);
        GUILayout.EndHorizontal();

        // Background Image Selection
        GUILayout.BeginHorizontal();
        GUILayout.Label("Background:", largeFont);
        if (GUILayout.Button("Next", buttonStyle, GUILayout.Height(50)))
        {
            currentBackgroundIndex = (currentBackgroundIndex + 1) % backgroundImages.Length;
            ChangeBackgroundImage();
        }
        GUILayout.EndHorizontal();

        // Back to Main Menu Button
        if (GUILayout.Button("Back to Main Menu", buttonStyle, GUILayout.Height(100)))
        {
            BackToMainMenu();
        }

        // Exit Button
        if (GUILayout.Button("Exit", buttonStyle, GUILayout.Height(100)))
        {
            ExitButtonClicked();
        }

        GUILayout.EndArea();
    }

    void ChangeBackgroundImage()
    {
        // Change the background image
        // Assuming there's a GUI element responsible for rendering the background image
        // Change the background image to the one at the current index in the backgroundImages array
        // For example:
        // backgroundGUIElement.texture = backgroundImages[currentBackgroundIndex];
    }

    void ExitButtonClicked()
    {
        // Placeholder for exit button functionality
        Debug.Log("Exit button clicked");
        SceneManager.LoadScene("SinglePlayer");
    }

    void BackToMainMenu()
    {
        // Placeholder for back to main menu functionality
        Debug.Log("Back to main menu button clicked");
        SceneManager.LoadScene("MainMenu"); // Assumes you have a scene named "MainMenu"
    }

    void PauseGame()
    {
        // Placeholder for pause game functionality
        Debug.Log("Pause button clicked");
        // Implement game pause logic here
        Time.timeScale = Time.timeScale == 1 ? 0 : 1; // Toggle pause
    }

    void MoveCharacterLeft()
    {
        // Move the character to the left
        character.transform.Translate(Vector3.left * Time.deltaTime);
    }

    void MoveCharacterRight()
    {
        // Move the character to the right
        character.transform.Translate(Vector3.right * Time.deltaTime);
    }

    void MoveCharacterUp()
    {
        // Move the character up
        character.transform.Translate(Vector3.up * Time.deltaTime);
    }

    void MoveCharacterDown()
    {
        // Move the character down
        character.transform.Translate(Vector3.down * Time.deltaTime);
    }
}
