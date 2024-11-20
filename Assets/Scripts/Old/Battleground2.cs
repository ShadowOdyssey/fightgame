using UnityEngine;
using UnityEngine.SceneManagement;

public class Battleground2 : MonoBehaviour
{
    // Assign these in the Unity Editor
    public Texture2D exitButtonTexture;
    public Texture2D settingsButtonTexture;

    private bool showSettingsPanel = false;

    public void Start()
    {
        Cursor.visible = true; // Ensure the cursor is visible
    }

    private void OnGUI()
    {
        // Draw the exit button
        DrawExitButton();

        // Draw the settings button
        DrawSettingsButton();

        // Draw the settings panel if it's open
        if (showSettingsPanel)
        {
            DrawSettingsPanel();
        }
    }

    private void DrawExitButton()
    {
        float exitButtonWidth = 40f;
        float exitButtonHeight = 40f;
        float exitButtonX = Screen.width - exitButtonWidth - 20;
        float exitButtonY = 20;
        Rect exitButtonRect = new Rect(exitButtonX, exitButtonY, exitButtonWidth, exitButtonHeight);

        if (GUI.Button(exitButtonRect, exitButtonTexture, GUIStyle.none))
        {
            ReturnToMainMenu(); // Load Main Menu when exit button is clicked
        }
    }

    private void DrawSettingsButton()
    {
        float settingsButtonWidth = 40f;
        float settingsButtonHeight = 40f;
        float settingsButtonX = Screen.width - settingsButtonWidth - 70; // Position to the left of the exit button
        Rect settingsButtonRect = new Rect(settingsButtonX, 20, settingsButtonWidth, settingsButtonHeight);

        if (GUI.Button(settingsButtonRect, settingsButtonTexture, GUIStyle.none))
        {
            showSettingsPanel = !showSettingsPanel; // Toggle settings panel visibility
        }
    }

    private void DrawSettingsPanel()
    {
        float panelWidth = 250f;
        float panelHeight = 180f;
        float panelX = Screen.width / 2 - panelWidth / 2;
        float panelY = Screen.height / 2 - panelHeight / 2;

        GUI.Box(new Rect(panelX, panelY, panelWidth, panelHeight), "Settings");

        GUIStyle settingsButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 16
        };

        if (GUI.Button(new Rect(panelX + 20, panelY + 40, 210, 40), "Sound", settingsButtonStyle))
        {
            ToggleSound();
        }

        if (GUI.Button(new Rect(panelX + 20, panelY + 100, 210, 40), "Display", settingsButtonStyle))
        {
            Debug.Log("Display Settings clicked.");
        }

        // Close button for settings panel
        if (GUI.Button(new Rect(panelX + panelWidth - 30, panelY + 10, 20, 20), "X"))
        {
            showSettingsPanel = false; // Close settings panel
        }
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Load the Main Menu scene when exit button is clicked
    }

    private void ToggleSound()
    {
        Debug.Log("Sound toggled."); // Add functionality to toggle sound here
    }
}
