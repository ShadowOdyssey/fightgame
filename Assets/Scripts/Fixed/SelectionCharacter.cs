using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Add this line to resolve the IEnumerator error

public class SelectionCharacter : MonoBehaviour
{
    public Texture2D background;
    public Texture2D[] characterImages;
    public AudioClip introClip;
    public Texture2D lockIcon;

    // New textures for the panel display
    public Texture2D gabriellaTexture;
    public Texture2D vsIcon;
    public Texture2D marcusTexture;

    private readonly string[] characterNames = { "Gabriella", "Marcus", "Selena", "Bryan", "Nun", "Oliver", "Orion", "Aria" };

    private readonly int[] durabilityStats = { 7, 5, 9, 6, 8, 6, 8, 10 };
    private readonly int[] offenseStats = { 8, 9, 7, 6, 6, 9, 7, 10 };
    private readonly int[] controlEffectStats = { 6, 4, 8, 5, 7, 5, 9, 10 };
    private readonly int[] difficultyStats = { 4, 6, 8, 3, 5, 4, 5, 10 };

    private int currentIndex = 0;
    private AudioSource audioSource;
    private bool hasPlayedIntro = false;

    // Locking mechanism
    private readonly bool[] isUnlocked = { true, false, false, false, false, false, false, false }; // Only Gabriella is unlocked by default

    private bool showLockedMessage = false; // Indicates if the locked character message should be shown
    private float lockedMessageTimer = 0f; // Timer to control how long the message appears

    // New variables for the VS panel display with countdown
    private bool showVsPanel = false;
    private readonly int countdownDuration = 5; // Countdown duration in seconds
    private int currentCountdown;
    private Coroutine countdownCoroutine; // Store the reference to the countdown coroutine


    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = introClip;
        PlayIntroSound();
    }

    void OnGUI()
    {
        if (background != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background, ScaleMode.StretchToFill);
        }

        DrawBackButton();
        DrawCharacter();
        DrawHeroDetails();
        DrawNavigationButtons();
        DrawActionButtons();

        if (showLockedMessage)
        {
            DrawLockedMessage();
        }

        if (showVsPanel)
        {
            DrawVsPanel();
            DrawCountdown();
        }
    }

    void DrawBackButton()
    {
        float buttonWidth = 120f; // Increased width
        float buttonHeight = 50f; // Increased height
        float buttonX = 20f;
        float buttonY = 20f;

        GUIStyle backButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 28, // Increased font size
            normal = { textColor = Color.white, background = MakeTexture(1, 1, Color.black) }
        };

        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "←", backButtonStyle))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void DrawCharacter()
    {
        if (characterImages.Length > 0)
        {
            float charWidth = Screen.width * 0.5f; // Increased width
            float charHeight = Screen.height * 0.75f; // Increased height
            float charX = (Screen.width - charWidth) / 2;
            float charY = (Screen.height - charHeight) / 2 - 50;

            GUI.DrawTexture(new Rect(charX, charY, charWidth, charHeight), characterImages[currentIndex], ScaleMode.ScaleToFit);

            if (!isUnlocked[currentIndex] && lockIcon != null)
            {
                float lockIconSize = 100f; // Increased lock icon size
                GUI.DrawTexture(new Rect(charX + (charWidth / 2) - (lockIconSize / 2), charY + (charHeight / 2) - (lockIconSize / 2), lockIconSize, lockIconSize), lockIcon, ScaleMode.ScaleToFit);
            }
        }
    }

    void DrawHeroDetails()
    {
        float panelX = Screen.width * 0.1f; // Increased margin
        float panelY = Screen.height * 0.25f; // Adjusted panel position
        float labelWidth = 600f; // Increased label width
        float labelHeight = 100f; // Increased label height

        float totalStatsHeight = CalculateStatsHeight() + 200; // Adjusted height for spacing
        float totalStatsWidth = CalculateStatsWidth() + 120; // Adjusted width for spacing

        GUIStyle blackBackgroundStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = MakeTexture(1, 1, new Color(0, 0, 0, 0.7f)) }
        };
        GUI.Box(new Rect(panelX - 20, panelY - 20, totalStatsWidth, totalStatsHeight), GUIContent.none, blackBackgroundStyle);

        GUIStyle nameStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 50, // Increased font size
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };
        GUI.Label(new Rect(panelX, panelY, labelWidth, labelHeight), characterNames[currentIndex], nameStyle);

        if (isUnlocked[currentIndex])
        {
            DrawStatBars(panelX, panelY + 70); // Adjusted for spacing
        }
    }

    void DrawNavigationButtons()
    {
        float buttonSize = 80f; // Increased button size
        float panelWidth = Screen.width * 0.35f;
        float panelX = (Screen.width - panelWidth) / 2;
        float panelY = (Screen.height - (Screen.height * 0.5f)) / 2;

        GUIStyle navigationButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 40, // Increased font size
            normal = { textColor = Color.white, background = MakeTexture(1, 1, Color.black) }
        };

        if (GUI.Button(new Rect(panelX - buttonSize - 20, panelY + (Screen.height * 0.5f) - (buttonSize / 2), buttonSize, buttonSize), "<", navigationButtonStyle))
        {
            ShowPreviousCharacter();
        }

        if (GUI.Button(new Rect(panelX + panelWidth + 20, panelY + (Screen.height * 0.5f) - (buttonSize / 2), buttonSize, buttonSize), ">", navigationButtonStyle))
        {
            ShowNextCharacter();
        }
    }

    void DrawActionButtons()
    {
        float buttonWidth = 250f; // Increased button width
        float buttonHeight = 60f; // Increased button height
        float buttonX = (Screen.width - buttonWidth) / 2;
        float buttonY = Screen.height - buttonHeight - 30;

        GUIStyle selectButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 50, // Increased font size for the select button
            normal = { textColor = Color.white, background = MakeTexture(1, 1, Color.black) }
        };

        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Select", selectButtonStyle))
        {
            if (isUnlocked[currentIndex])
            {
                Debug.Log("Character Selected: " + characterNames[currentIndex]);

                if (characterNames[currentIndex] == "Gabriella")
                {
                    PlayerPrefs.SetInt("playerCharacterSelected", 1); // Select Gabriella as player
                    PlayerPrefs.SetInt("enemyCharacterSelected", 2); // Select Marcus as opponent
                    PlayerPrefs.SetInt("stageSelected", 1); // Load Village Arena

                    ShowVsPanel();
                }
            }
            else
            {
                showLockedMessage = true; // Show the lock message
                lockedMessageTimer = 3f; // Set timer to 3 seconds
            }
        }
    }

    void ShowVsPanel()
    {
        showVsPanel = true;
        currentCountdown = countdownDuration; // Initialize countdown
        StartCoroutine(CountdownAndLoadScene());
    }

    // Coroutine for countdown and scene load
    

    IEnumerator CountdownAndLoadScene()
   {
       while (currentCountdown > 0)
      {
          yield return new WaitForSeconds(1f);
         currentCountdown--;
      }

       if (showVsPanel) // Only load the scene if the VS panel is still active
      {
        SceneManager.LoadScene("FightScene");
      }
    }


   
    void DrawVsPanel()
{
    float panelWidth = 2100f; // Increased panel width
    float panelHeight = 1200f; // Increased panel height
    float panelX = (Screen.width - panelWidth) / 2;
    float panelY = (Screen.height - panelHeight) / 2;

    GUIStyle blackBackgroundStyle = new GUIStyle(GUI.skin.box)
    {
        normal = { background = MakeTexture(1, 1, new Color(0, 0, 0, 0.8f)) }
    };
    GUI.Box(new Rect(panelX, panelY, panelWidth, panelHeight), GUIContent.none, blackBackgroundStyle);

    float characterTextureWidth = 1000f; // Width for character textures
    float characterTextureHeight = 1000f; // Height for character textures

    float vsIconWidth = 600f; // Adjusted width for VS icon
    float vsIconHeight = 600f; // Adjusted height for VS icon

    float gabriellaX = panelX + 50f;
    float iconX = panelX + (panelWidth / 2) - (vsIconWidth / 2);
    float marcusX = panelX + panelWidth - characterTextureWidth - 50f;

    GUI.DrawTexture(new Rect(gabriellaX, panelY + (panelHeight - characterTextureHeight) / 2, characterTextureWidth, characterTextureHeight), gabriellaTexture, ScaleMode.ScaleToFit);
    GUI.DrawTexture(new Rect(iconX, panelY + (panelHeight - vsIconHeight) / 2, vsIconWidth, vsIconHeight), vsIcon, ScaleMode.ScaleToFit);
    GUI.DrawTexture(new Rect(marcusX, panelY + (panelHeight - characterTextureHeight) / 2, characterTextureWidth, characterTextureHeight), marcusTexture, ScaleMode.ScaleToFit);

    DrawCancelButton(panelX, panelY, panelWidth); // Pass panelWidth as an argument
}

void DrawCancelButton(float panelX, float panelY, float panelWidth)
{
    float buttonSize = 80f; // Size of the cancel button
    float buttonX = panelX + panelWidth - buttonSize - 20f; // Position to the top right of the panel
    float buttonY = panelY + 20f; // Position from the top of the panel

    GUIStyle cancelButtonStyle = new GUIStyle(GUI.skin.button)
    {
        fontSize = 32, // Font size for the button
        normal = { textColor = Color.white, background = MakeTexture(1, 1, Color.red) } // Red background for visibility
    };

    if (GUI.Button(new Rect(buttonX, buttonY, buttonSize, buttonSize), "X", cancelButtonStyle))
    {
        CancelSelection(); // Call the method to handle the cancel action
    }
}



    void CancelSelection()
    {
      if (countdownCoroutine != null)
     {
        StopCoroutine(countdownCoroutine); // Stop the countdown coroutine
        countdownCoroutine = null;
     }
       showVsPanel = false; // Close the VS panel
    }




    void DrawCountdown()
    {
        float countdownWidth = 150f;
        float countdownHeight = 100f;
        float countdownX = (Screen.width - countdownWidth) / 2;
        float countdownY = Screen.height * 0.85f;

        GUIStyle countdownStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 100, // Large font size for countdown
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white }
        };
        GUI.Label(new Rect(countdownX, countdownY, countdownWidth, countdownHeight), currentCountdown.ToString(), countdownStyle);
    }

    void PlayIntroSound()
    {
        if (!hasPlayedIntro)
        {
            audioSource.Play();
            hasPlayedIntro = true;
        }
    }

    void ShowPreviousCharacter()
    {
        currentIndex = (currentIndex > 0) ? currentIndex - 1 : characterImages.Length - 1;
    }

    void ShowNextCharacter()
    {
        currentIndex = (currentIndex + 1) % characterImages.Length;
    }

    Texture2D MakeTexture(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    void DrawLockedMessage()
    {
        float messageWidth = 800f; // Increased message width
        float messageHeight = 200f; // Increased message height
        float messageX = (Screen.width - messageWidth) / 2;
        float messageY = Screen.height * 0.75f;

        GUIStyle lockedMessageStyle = new GUIStyle(GUI.skin.box)
        {
            fontSize = 40, // Larger font size
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white, background = MakeTexture(1, 1, Color.red) }
        };

        GUI.Box(new Rect(messageX, messageY, messageWidth, messageHeight), "Defeat to Unlock!", lockedMessageStyle);

        lockedMessageTimer -= Time.deltaTime;
        if (lockedMessageTimer <= 1f)
        {
            showLockedMessage = false; // Hide message after timer runs out
        }
    }

    void DrawStatBars(float x, float y)
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 30, // Increased font size for stats
            normal = { textColor = Color.white }
        };

        GUI.Label(new Rect(x, y, 200, 50), "Durability:", labelStyle);
        DrawStatBar(x + 220, y, durabilityStats[currentIndex]);

        GUI.Label(new Rect(x, y + 60, 200, 50), "Offense:", labelStyle);
        DrawStatBar(x + 220, y + 60, offenseStats[currentIndex]);

        GUI.Label(new Rect(x, y + 120, 200, 50), "Control Effect:", labelStyle);
        DrawStatBar(x + 220, y + 120, controlEffectStats[currentIndex]);

        GUI.Label(new Rect(x, y + 180, 200, 50), "Difficulty:", labelStyle);
        DrawStatBar(x + 220, y + 180, difficultyStats[currentIndex]);
    }

    void DrawStatBar(float x, float y, int stat)
    {
        GUIStyle barStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = MakeTexture(1, 1, Color.green) }
        };
        GUI.Box(new Rect(x, y, stat * 30, 40), GUIContent.none, barStyle);
    }

    float CalculateStatsHeight()
    {
        return 4 * 40 + 3 * 10; // Total height for stats and spacing
    }

    float CalculateStatsWidth()
    {
        return 200 + 300; // Label width + bar width
    }
}
