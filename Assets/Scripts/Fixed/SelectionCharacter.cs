using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectionCharacter : MonoBehaviour
{
    #region Variables

    #region Scene Setup

    [Header("Scene Setup")]
    public Texture2D background;
    public Texture2D[] characterImages;
    public AudioClip[] heroIntroClips; // Array for hero-specific intro sounds
    public Texture2D lockIcon;

    // New textures for the panel display
    public Texture2D gabriellaTexture;
    public Texture2D vsIcon;
    public Texture2D marcusTexture;

    #endregion

    #region Hidden Variables

    private AudioSource audioSource;
    private Coroutine countdownCoroutine; // Store the reference to the countdown coroutine
    private readonly int[] durabilityStats = { 7, 5, 9, 6, 8, 6, 8, 10 };
    private readonly int[] offenseStats = { 8, 9, 7, 6, 6, 9, 7, 10 };
    private readonly int[] controlEffectStats = { 6, 4, 8, 5, 7, 5, 9, 10 };
    private readonly int[] difficultyStats = { 4, 6, 8, 3, 5, 4, 5, 10 };
    private readonly int countdownDuration = 5; // Countdown duration in seconds
    private readonly string[] characterNames = { "Gabriella", "Marcus", "Selena", "Bryan", "Nun", "Oliver", "Orion", "Aria" };
    private int currentIndex = 0;
    private int lastPlayedIndex = -1; // To track which hero's intro was last played
    private int currentCountdown;
    private bool[] isUnlocked = { true, false, false, false, false, false, false, false }; // Only Gabriella is unlocked by default
    private bool showLockedMessage = false; // Indicates if the locked character message should be shown
    private bool showVsPanel = false;
    private float lockedMessageTimer = 0f; // Timer to control how long the message appears

    #endregion

    #endregion

    #region Loading Components

    private void Awake() // Always loads components in Awake in the main script of the scene
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    #endregion

    #region Loading Data

    private void Start()
    {
        CheckIfPlayerWonLastBattle(); // Check if player is returning to Selection Character scene after to battle some IA
        PlayHeroIntro(); // Play the first character's intro at start
    }

    #endregion

    #region Load GUI

    private void OnGUI()
    {
        if (background != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background, ScaleMode.StretchToFill);
        }

        DrawSelectHeroText(); // Call the function to display the "Select a Hero" text
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

    #endregion

    #region Coroutines always first after core system

    IEnumerator CountdownAndLoadScene()
    {
        while (currentCountdown > 0)
        {
            yield return new WaitForSeconds(1f);
            currentCountdown--;
        }

        if (showVsPanel)
        {
            SceneManager.LoadScene("FightScene");
        }
    }

    #endregion

    #region Setup Data Loaded

    private void CheckIfPlayerWonLastBattle()
    {
        if (PlayerPrefs.GetString("playerUnlockedNewCharacter") == "yes") // Check if player won the last fight
        {
            // PLayer Won
            switch (PlayerPrefs.GetInt("enemyCharacter"))
            {
                default:
                    isUnlocked[0] = true; isUnlocked[1] = false; isUnlocked[2] = false; isUnlocked[3] = false; isUnlocked[4] = false; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player won against Gabriella

                case 1:
                    isUnlocked[0] = true; isUnlocked[1] = false; isUnlocked[2] = false; isUnlocked[3] = false; isUnlocked[4] = false; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player won against Gabriella

                case 2:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = false; isUnlocked[3] = false; isUnlocked[4] = false; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player won against Marcus

                case 3:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = true; isUnlocked[3] = false; isUnlocked[4] = false; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player won against Selena

                case 4:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = true; isUnlocked[3] = true; isUnlocked[4] = false; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player won against Bryan

                case 5:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = true; isUnlocked[3] = true; isUnlocked[4] = true; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player won against Nun

                case 6:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = true; isUnlocked[3] = true; isUnlocked[4] = true; isUnlocked[5] = true; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player won against Oliver

                case 7:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = true; isUnlocked[3] = true; isUnlocked[4] = true; isUnlocked[5] = true; isUnlocked[6] = true; isUnlocked[0] = false;
                    break; // Player won against Orion

                case 8:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = true; isUnlocked[3] = true; isUnlocked[4] = true; isUnlocked[5] = true; isUnlocked[6] = true; isUnlocked[0] = true;
                    break; // Player won against Aria
            }
        }

        if (PlayerPrefs.GetString("playerUnlockedNewCharacter") == "no") // Check if player lost the last fight
        {
            // Player Lost
            switch (PlayerPrefs.GetInt("enemyCharacter"))
            {
                default:
                    isUnlocked[0] = true; isUnlocked[1] = false; isUnlocked[2] = false; isUnlocked[3] = false; isUnlocked[4] = false; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player lost against Gabriella

                case 1:
                    isUnlocked[0] = true; isUnlocked[1] = false; isUnlocked[2] = false; isUnlocked[3] = false; isUnlocked[4] = false; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player lost against Gabriella

                case 2:
                    isUnlocked[0] = true; isUnlocked[1] = false; isUnlocked[2] = false; isUnlocked[3] = false; isUnlocked[4] = false; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player lost against Marcus

                case 3:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = false; isUnlocked[3] = false; isUnlocked[4] = false; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player lost against Selena

                case 4:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = true; isUnlocked[3] = false; isUnlocked[4] = false; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player lost against Bryan

                case 5:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = true; isUnlocked[3] = true; isUnlocked[4] = false; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player lost against Nun

                case 6:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = true; isUnlocked[3] = true; isUnlocked[4] = true; isUnlocked[5] = false; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player lost against Oliver

                case 7:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = true; isUnlocked[3] = true; isUnlocked[4] = true; isUnlocked[5] = true; isUnlocked[6] = false; isUnlocked[0] = false;
                    break; // Player lost against Orion

                case 8:
                    isUnlocked[0] = true; isUnlocked[1] = true; isUnlocked[2] = true; isUnlocked[3] = true; isUnlocked[4] = true; isUnlocked[5] = true; isUnlocked[6] = true; isUnlocked[0] = false;
                    break; // Player lost against Aria
            }

            PlayerPrefs.SetString("playerUnlockedNewCharacter", ""); // Reset the value to make it disponible to another fight
            PlayerPrefs.SetInt("enemyCharacter", 0); // Reset the value to make it disponible to another fight
        }
    }

    private void PlayHeroIntro()
    {
        if (heroIntroClips.Length > currentIndex && heroIntroClips[currentIndex] != null && currentIndex != lastPlayedIndex)
        {
            audioSource.Stop(); // Stop any currently playing sound
            audioSource.clip = heroIntroClips[currentIndex];
            audioSource.Play();
            lastPlayedIndex = currentIndex;
        }
    }

    #endregion

    #region Setup GUI Loaded

    private void DrawSelectHeroText()
    {
        float textWidth = 600f;
        float textHeight = 150f;
        float textX = (Screen.width - textWidth) / 2;  // Centers the text horizontally
        float textY = 50f;  // Positions the text near the top

        GUIStyle selectHeroStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 60, // Increased the font size for better visibility
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white },
            alignment = TextAnchor.MiddleCenter, // Centers the text horizontally
        };

        // Adding a text shadow effect for better contrast and readability
        selectHeroStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(textX + 5, textY + 5, textWidth, textHeight), "Select a Hero", selectHeroStyle); // Shadow
        selectHeroStyle.normal.textColor = Color.green;
        GUI.Label(new Rect(textX, textY, textWidth, textHeight), "Select a Hero", selectHeroStyle); // Main text
    }

    private void DrawBackButton()
    {
        float buttonWidth = 120f;
        float buttonHeight = 50f;
        float buttonX = 20f;
        float buttonY = 20f;

        GUIStyle backButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 28,
            normal = { textColor = Color.white, background = MakeTexture(1, 1, Color.black) }
        };

        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "←", backButtonStyle))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void DrawCharacter()
    {
        if (characterImages.Length > 0)
        {
            float charWidth = Screen.width * 0.5f;
            float charHeight = Screen.height * 0.75f;
            float charX = (Screen.width - charWidth) / 2;
            float charY = (Screen.height - charHeight) / 2 - 50;

            GUI.DrawTexture(new Rect(charX, charY, charWidth, charHeight), characterImages[currentIndex], ScaleMode.ScaleToFit);

            if (!isUnlocked[currentIndex] && lockIcon != null)
            {
                float lockIconSize = 100f;
                GUI.DrawTexture(new Rect(charX + (charWidth / 2) - (lockIconSize / 2), charY + (charHeight / 2) - (lockIconSize / 2), lockIconSize, lockIconSize), lockIcon, ScaleMode.ScaleToFit);
            }
        }
    }

    private void DrawHeroDetails()
    {
        float panelX = Screen.width * 0.1f;
        float panelY = Screen.height * 0.25f;
        float labelWidth = 600f;
        float labelHeight = 100f;

        float totalStatsHeight = CalculateStatsHeight() + 200;
        float totalStatsWidth = CalculateStatsWidth() + 120;

        GUIStyle blackBackgroundStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = MakeTexture(1, 1, new Color(0, 0, 0, 0.7f)) }
        };
        GUI.Box(new Rect(panelX - 20, panelY - 20, totalStatsWidth, totalStatsHeight), GUIContent.none, blackBackgroundStyle);

        GUIStyle nameStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 50,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };
        GUI.Label(new Rect(panelX, panelY, labelWidth, labelHeight), characterNames[currentIndex], nameStyle);

        if (isUnlocked[currentIndex])
        {
            DrawStatBars(panelX, panelY + 70);
        }
    }

    private void DrawNavigationButtons()
    {
        float buttonSize = 80f;
        float panelWidth = Screen.width * 0.35f;
        float panelX = (Screen.width - panelWidth) / 2;
        float panelY = (Screen.height - (Screen.height * 0.5f)) / 2;

        GUIStyle navigationButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 40,
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

    private void DrawActionButtons()
    {
        float buttonWidth = 250f;
        float buttonHeight = 60f;
        float buttonX = (Screen.width - buttonWidth) / 2;
        float buttonY = Screen.height - buttonHeight - 30;

        GUIStyle selectButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 50,
            normal = { textColor = Color.white, background = MakeTexture(1, 1, Color.black) }
        };

        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Select", selectButtonStyle))
        {
            if (isUnlocked[currentIndex])
            {
                Debug.Log("Character Selected: " + characterNames[currentIndex]);

                if (characterNames[currentIndex] == "Gabriella")
                {
                    PlayerPrefs.SetInt("playerCharacterSelected", 3); // Select Gabriella as player
                    PlayerPrefs.SetInt("enemyCharacterSelected", 8); // Select Marcus as opponent
                    PlayerPrefs.SetInt("stageSelected", 4); // Load Village Arena

                    ShowVsPanel();
                }
            }
            else
            {
                showLockedMessage = true;
                lockedMessageTimer = 3f;
            }
        }
    }

    private void ShowVsPanel()
    {
        showVsPanel = true;
        currentCountdown = countdownDuration;
        StartCoroutine(CountdownAndLoadScene());
    }

    private void DrawVsPanel()
    {
        float panelWidth = 2100f;
        float panelHeight = 1200f;
        float panelX = (Screen.width - panelWidth) / 2;
        float panelY = (Screen.height - panelHeight) / 2;

        GUIStyle blackBackgroundStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = MakeTexture(1, 1, new Color(0, 0, 0, 0.8f)) }
        };
        GUI.Box(new Rect(panelX, panelY, panelWidth, panelHeight), GUIContent.none, blackBackgroundStyle);

        float characterTextureWidth = 1000f;
        float characterTextureHeight = 1000f;

        float vsIconWidth = 600f;
        float vsIconHeight = 600f;

        float gabriellaX = panelX + 50f;
        float iconX = panelX + (panelWidth / 2) - (vsIconWidth / 2);
        float marcusX = panelX + panelWidth - characterTextureWidth - 50f;

        GUI.DrawTexture(new Rect(gabriellaX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), gabriellaTexture, ScaleMode.ScaleToFit);
        GUI.DrawTexture(new Rect(iconX, panelY + (panelHeight / 2) - (vsIconHeight / 2), vsIconWidth, vsIconHeight), vsIcon, ScaleMode.ScaleToFit);
        GUI.DrawTexture(new Rect(marcusX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), marcusTexture, ScaleMode.ScaleToFit);
    }

    private void DrawCountdown()
    {
        float labelWidth = 400f;
        float labelHeight = 200f;
        float labelX = (Screen.width - labelWidth) / 2;
        float labelY = Screen.height - labelHeight - 50;

        GUIStyle countdownStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 100,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };

        GUI.Label(new Rect(labelX, labelY, labelWidth, labelHeight), currentCountdown.ToString(), countdownStyle);
    }

    #endregion

    #region GUI Operations

    private void ShowPreviousCharacter()
    {
        currentIndex = (currentIndex > 0) ? currentIndex - 1 : characterImages.Length - 1;
        PlayHeroIntro();
    }

    private void ShowNextCharacter()
    {
        currentIndex = (currentIndex + 1) % characterImages.Length;
        PlayHeroIntro();
    }

    private void DrawLockedMessage()
    {
        if (lockedMessageTimer > 0)
        {
            lockedMessageTimer -= Time.deltaTime;
        }
        else
        {
            showLockedMessage = false;
        }

        float messageWidth = 500f;
        float messageHeight = 150f;
        float messageX = (Screen.width - messageWidth) / 2;
        float messageY = Screen.height - messageHeight - 100;

        GUIStyle lockedMessageStyle = new GUIStyle(GUI.skin.box)
        {
            fontSize = 30,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white, background = MakeTexture(1, 1, new Color(0, 0, 0, 0.8f)) }
        };

        GUI.Box(new Rect(messageX, messageY, messageWidth, messageHeight), "Character is Locked!", lockedMessageStyle);
    }

    private void DrawStatBars(float x, float y)
    {
        DrawStatBar("Durability", durabilityStats[currentIndex], x, y);
        DrawStatBar("Offense", offenseStats[currentIndex], x, y + 50);
        DrawStatBar("Control Effect", controlEffectStats[currentIndex], x, y + 100);
        DrawStatBar("Difficulty", difficultyStats[currentIndex], x, y + 150);
    }

    private void DrawStatBar(string label, int value, float x, float y)
    {
        float barWidth = 400f;
        float barHeight = 20f;
        float labelWidth = 150f;

        GUIStyle statLabelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 30,
            normal = { textColor = Color.white }
        };
        GUI.Label(new Rect(x, y, labelWidth, barHeight), label, statLabelStyle);

        GUIStyle filledBarStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = MakeTexture(1, 1, Color.green) }
        };
        GUI.Box(new Rect(x + labelWidth, y + 10, (barWidth / 10) * value, barHeight), GUIContent.none, filledBarStyle);

        GUIStyle emptyBarStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = MakeTexture(1, 1, Color.gray) }
        };
        GUI.Box(new Rect(x + labelWidth + ((barWidth / 10) * value), y + 10, barWidth - ((barWidth / 10) * value), barHeight), GUIContent.none, emptyBarStyle);
    }

    private float CalculateStatsHeight()
    {
        return (50 * 4);
    }

    private float CalculateStatsWidth()
    {
        return 500;
    }

    private Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    #endregion
}
