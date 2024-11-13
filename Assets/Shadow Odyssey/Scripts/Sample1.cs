using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sample1 : MonoBehaviour
{
    public Texture2D _settingsButtonIcon;
    public Texture2D _exitButtonIcon;
    public GameObject settingsPanel;

    private bool settingsOpen = false;

    // Health bar properties
    public Texture2D healthBarBackgroundTexture;
    public Texture2D healthBarForegroundTexture;

    private float playerMaxHealth = 100f;
    private float playerCurrentHealth;

    private float opponentMaxHealth = 100f;
    private float opponentCurrentHealth;

    void Start()
    {
        Cursor.visible = true;
        settingsPanel.SetActive(false); // Settings panel is initially hidden

        // Initialize player and opponent health
        playerCurrentHealth = playerMaxHealth;
        opponentCurrentHealth = opponentMaxHealth;
    }

    void OnGUI()
    {
        // Settings and Exit buttons
        float exitButtonWidth = Screen.width * 0.08f;
        float exitButtonHeight = Screen.height * 0.08f;
        float exitButtonX = Screen.width - exitButtonWidth - 20;
        float exitButtonY = 20;
        Rect exitButtonRect = new Rect(exitButtonX, exitButtonY, exitButtonWidth, exitButtonHeight);
        if (GUI.Button(exitButtonRect, _exitButtonIcon, GUIStyle.none))
        {
            ExitButtonClicked();
        }

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

        // Draw the health bars for the player and the opponent
        DrawHealthBars();
    }

    void DrawHealthBars()
    {
        // Player health bar
        float healthBarWidth = Screen.width * 0.25f; // 25% of screen width
        float healthBarHeight = Screen.height * 0.05f; // 5% of screen height
        float healthBarX = 20f;
        float healthBarY = 20f;

        // Background bar (Player)
        GUI.DrawTexture(new Rect(healthBarX, healthBarY, healthBarWidth, healthBarHeight), healthBarBackgroundTexture);

        // Foreground bar (Player)
        float playerHealthPercentage = playerCurrentHealth / playerMaxHealth;
        GUI.DrawTexture(new Rect(healthBarX, healthBarY, healthBarWidth * playerHealthPercentage, healthBarHeight), healthBarForegroundTexture);

        // Opponent health bar
        float opponentHealthBarX = Screen.width - healthBarWidth - 20f;

        // Background bar (Opponent)
        GUI.DrawTexture(new Rect(opponentHealthBarX, healthBarY, healthBarWidth, healthBarHeight), healthBarBackgroundTexture);

        // Foreground bar (Opponent)
        float opponentHealthPercentage = opponentCurrentHealth / opponentMaxHealth;
        GUI.DrawTexture(new Rect(opponentHealthBarX, healthBarY, healthBarWidth * opponentHealthPercentage, healthBarHeight), healthBarForegroundTexture);
    }

    // Reduce player health
    public void TakeDamagePlayer(float damage)
    {
        playerCurrentHealth -= damage;
        playerCurrentHealth = Mathf.Clamp(playerCurrentHealth, 0, playerMaxHealth);
    }

    // Reduce opponent health
    public void TakeDamageOpponent(float damage)
    {
        opponentCurrentHealth -= damage;
        opponentCurrentHealth = Mathf.Clamp(opponentCurrentHealth, 0, opponentMaxHealth);
    }

    void ExitButtonClicked()
    {
        SceneManager.LoadScene("StartScene");
    }

    void Update()
    {
        // Example: Reduce health with keys (you can replace this with actual gameplay logic)
        if (Input.GetKeyDown(KeyCode.P)) // Press 'P' to deal damage to player
        {
            TakeDamagePlayer(10f);
        }
        if (Input.GetKeyDown(KeyCode.O)) // Press 'O' to deal damage to opponent
        {
            TakeDamageOpponent(10f);
        }
    }
}
