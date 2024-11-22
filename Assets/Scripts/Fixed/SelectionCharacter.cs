using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectionCharacter : MonoBehaviour
{
    #region Variables

    #region Scene Setup

    [Header("Debug Menu")] // To remove it before to release the game - Just for debug purposes
    public bool resetStats = false; // To enable it will reset all player progress

    [Header("Scene Setup")]
    public Texture2D background;
    public Texture2D[] characterImages;
    public AudioClip[] heroIntroClips; // Array for hero-specific intro sounds
    public Texture2D lockIcon;

    [Header("Versus Panel Setup")]
    public Texture2D vsIcon;

    [Header("Player Textures Versus Setup")]
    public Texture2D gabriellaPlayer;
    public Texture2D marcusPlayer;
    public Texture2D selenaPlayer;
    public Texture2D bryanPlayer;
    public Texture2D nunPlayer;
    public Texture2D oliverPlayer;
    public Texture2D orionPlayer;
    public Texture2D ariaPlayer;

    [Header("Enemy Textures Versus Setup")]
    public Texture2D gabriellaEnemy;
    public Texture2D marcusEnemy;
    public Texture2D selenaEnemy;
    public Texture2D bryanEnemy;
    public Texture2D nunEnemy;
    public Texture2D oliverEnemy;
    public Texture2D orionEnemy;
    public Texture2D ariaEnemy;

    #endregion

    #region Hidden Variables

    private AudioSource audioSource;
    [SerializeField] private AudioClip selectArenaSound;
    private Coroutine countdownCoroutine; // Store the reference to the countdown coroutine
    private readonly int[] durabilityStats = { 7, 5, 9, 6, 8, 6, 8, 10 };
    private readonly int[] offenseStats = { 8, 9, 7, 6, 6, 9, 7, 10 };
    private readonly int[] controlEffectStats = { 6, 4, 8, 5, 7, 5, 9, 10 };
    private readonly int[] difficultyStats = { 4, 6, 8, 3, 5, 4, 5, 10 };
    private readonly int countdownDuration = 5; // Countdown duration in seconds
    private readonly string[] characterNames = { "Gabriella", "Marcus", "Selena", "Bryan", "Nun", "Oliver", "Orion", "Aria" };
    private int playerIndex = 0;
    private int enemyIndex = 0;
    private int randomizeCharacter = 0;
    private int randomizeArena = 0;
    private int lastPlayedIndex = -1; // To track which hero's intro was last played
    private int currentCountdown;
    private float lockedMessageTimer = 0f; // Timer to control how long the message appears
    private bool[] isUnlocked = { true, false, false, false, false, false, false, false }; // Only Gabriella is unlocked by default
    private bool showLockedMessage = false; // Indicates if the locked character message should be shown
    private bool showVsPanel = false;
    private bool canRender = false;
    private bool showArenaPanel = false; // Flag to control the visibility of the arena panel
    public Texture2D[] arenaImages; // Array to hold arena textures
    public string[] arenaNames;     // Optional: Array to hold arena names
    private int selectedArenaIndex = -1; // No arena selected initially

    private bool wasArenaSelected = false;

    #endregion

    #endregion

    #region Loading Components

    public void Awake() // Always loads components in Awake in the main script of the scene
    {
        StopAllCoroutines(); // Stop all coroutines from old scenes

        if (resetStats == true)
        {
            // Just for Debug purposes, it will be removed later - Use this 3 lines to reset the progress stats from Player
            PlayerPrefs.SetString("playerUnlockedNewCharacter", ""); // Reset the value to make it disponible to next fight
            PlayerPrefs.SetInt("enemyCharacter", 0); // Reset the value to make it disponible to next fight
            PlayerPrefs.SetString("currentProgress", ""); // Reset the value to make it disponible to next fight
            PlayerPrefs.SetString("playerFinishedGame", ""); // Reset the value to make it disponible to next fight
        }

        if (PlayerPrefs.GetString("playerFinishedGame") == "")
        {
            PlayerPrefs.SetString("playerFinishedGame", "no");
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    #endregion

    #region Loading Data

    public void Start()
    {
        CheckIfPlayerFinishedGame();
        CheckPlayerReturn(); // Check if player is returning to Selection Character scene after to battle some IA
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
         DrawArenaButton();
        DrawArenaPanel();
        DrawNavigationButtons();
        DrawActionButtons();

        if (showLockedMessage == true)
        {
            DrawLockedMessage();
        }

        if (showVsPanel == true)
        {
            DrawVsPanel();
            DrawCountdown();
        }
    }

    #endregion

    #region Coroutines should be always first method after core system methods

    IEnumerator CountdownAndLoadScene()
    {
        while (currentCountdown > 0)
        {
            yield return new WaitForSeconds(1f);
            currentCountdown--;
        }

        if (showVsPanel == true)
        {
            SceneManager.LoadScene("FightScene");
        }
    }

    #endregion

    #region Setup Data Loaded

    private void CheckIfPlayerFinishedGame()
    {
        if (PlayerPrefs.GetString("currentProgress") == "true, true, true, true, true, true, true, true" || PlayerPrefs.GetString("isTraining") == "yes")
        {
            PlayerPrefs.SetString("playerFinishedGame", "yes");

            //Debug.Log("Player finished the game");
        }
        else
        {
            PlayerPrefs.SetString("playerFinishedGame", "no");

            //Debug.Log("Player dont finished the game yet");
        }
    }

    private void CheckPlayerReturn()
    {
        if (PlayerPrefs.GetString("playerUnlockedNewCharacter") != "") // Just for debug, it will be removed later
        {
            //Debug.Log("Has player unlocked new character? " + PlayerPrefs.GetString("playerUnlockedNewCharacter"));
            //Debug.Log("Last enemy player fought: " + PlayerPrefs.GetInt("enemyCharacter"));

            //Debug.Log("Player returned from a battle, so lets apply the result from the last fight");

            if (PlayerPrefs.GetString("playerFinishedGame") == "no")
            {
                CheckIfPlayerWon();
                CheckIfPlayerLost();
            }

            PlayerPrefs.SetString("playerUnlockedNewCharacter", "");
        }
        else
        {
            //Debug.Log("Player was open game now so lets load the last progress if Player played the game before");

            LoadProgress();
        }
    }

    private void PlayHeroIntro()
    {
        if (heroIntroClips.Length > playerIndex && heroIntroClips[playerIndex] != null && playerIndex != lastPlayedIndex)
        {
            audioSource.Stop(); // Stop any currently playing sound
            audioSource.clip = heroIntroClips[playerIndex];
            audioSource.Play();
            lastPlayedIndex = playerIndex;
        }
    }

    private void UnlockCharacter(bool gabriella, bool marcus, bool selena, bool bryan, bool nun, bool oliver, bool orion, bool aria)
    {
        bool[] newUnlocked = { gabriella, marcus, selena, bryan, nun, oliver, orion, aria};
        isUnlocked = newUnlocked;
    }

    private void CheckIfPlayerWon()
    {
        //Debug.Log("Checking if player won last battle...");

        if (PlayerPrefs.GetString("playerUnlockedNewCharacter") == "yes") // Check if player won the last fight
        {
            //Debug.Log("Player unlocked new character because won last battle and advanced in progress also");

            // PLayer Won
            switch (PlayerPrefs.GetInt("enemyCharacter")) // Current enemy character from last battle will determine the progress of the player
            {
                default: UnlockCharacter(true, false, false, false, false, false, false, false); SaveProgress(1); break; // Only Gabriella unlocked by default
                case 2: UnlockCharacter(true, true, false, false, false, false, false, false); SaveProgress(2); break; // Player won against Marcus
                case 3: UnlockCharacter(true, true, true, false, false, false, false, false); SaveProgress(3); break; // Player won against Selena
                case 4: UnlockCharacter(true, true, true, true, false, false, false, false); SaveProgress(4); break; // Player won against Bryan
                case 5: UnlockCharacter(true, true, true, true, true, false, false, false); SaveProgress(5); break; // Player won against Nun
                case 6: UnlockCharacter(true, true, true, true, true, true, false, false); SaveProgress(6); break; // Player won against Oliver
                case 7: UnlockCharacter(true, true, true, true, true, true, true, false); SaveProgress(7); break; // Player won against Orion
                case 8: UnlockCharacter(true, true, true, true, true, true, true, true); SaveProgress(8); break; // Player won against Aria
            }
        }
    }

    private void CheckIfPlayerLost()
    {
        //Debug.Log("Checking if player lost last battle...");

        if (PlayerPrefs.GetString("playerUnlockedNewCharacter") == "no") // Check if player lost the last fight
        {
            //Debug.Log("Player lost last battle, so dont unlock a new character based in the last progress");

            // Player Lost
            switch (PlayerPrefs.GetInt("enemyCharacter")) // Current enemy character from last battle determines the not progress of the player
            {
                default: UnlockCharacter(true, false, false, false, false, false, false, false); SaveProgress(1); break; // Only Gabriella unlocked by default
                case 2: UnlockCharacter(true, false, false, false, false, false, false, false); SaveProgress(1); break; // Player lost against Marcus
                case 3: UnlockCharacter(true, true, false, false, false, false, false, false); SaveProgress(2); break; // Player lost against Selena
                case 4: UnlockCharacter(true, true, true, false, false, false, false, false); SaveProgress(3); break; // Player lost against Bryan
                case 5: UnlockCharacter(true, true, true, true, false, false, false, false); SaveProgress(4); break; // Player lost against Nun
                case 6: UnlockCharacter(true, true, true, true, true, false, false, false); SaveProgress(5); break; // Player lost against Oliver
                case 7: UnlockCharacter(true, true, true, true, true, true, false, false); SaveProgress(6); break; // Player lost against Orion
                case 8: UnlockCharacter(true, true, true, true, true, true, true, false); SaveProgress(7); break; // Player lost against Aria
            }

            PlayerPrefs.SetString("playerUnlockedNewCharacter", ""); // Reset the value to make it disponible to another fight
            PlayerPrefs.SetInt("enemyCharacter", 0); // Reset the value to make it disponible to another fight
        }
    }

    private void LoadProgress()
    {
        //Debug.Log("Loading current player progress");

        if (PlayerPrefs.GetString("currentProgress") == "")
        {
            //Debug.Log("Load default value of unlocked characters because player have no progress saved before!");

            UnlockCharacter(true, false, false, false, false, false, false, false);
        }

        if (PlayerPrefs.GetString("currentProgress") == "true, false, false, false, false, false, false, false")
        {
            //Debug.Log("Player have unlocked Gabriella by default");

            UnlockCharacter(true, false, false, false, false, false, false, false);
        }

        if (PlayerPrefs.GetString("currentProgress") == "true, true, false, false, false, false, false, false")
        {
            //Debug.Log("Player have unlocked Marcus before");

            UnlockCharacter(true, true, false, false, false, false, false, false);
        }

        if (PlayerPrefs.GetString("currentProgress") == "true, true, true, false, false, false, false, false")
        {
            //Debug.Log("Player have unlocked Selena before");

            UnlockCharacter(true, true, true, false, false, false, false, false);
        }

        if (PlayerPrefs.GetString("currentProgress") == "true, true, true, true, false, false, false, false")
        {
            //Debug.Log("Player have unlocked Bryan before");

            UnlockCharacter(true, true, true, true, false, false, false, false);
        }

        if (PlayerPrefs.GetString("currentProgress") == "true, true, true, true, true, false, false, false")
        {
            //Debug.Log("Player have unlocked Nun before");

            UnlockCharacter(true, true, true, true, true, false, false, false);
        }

        if (PlayerPrefs.GetString("currentProgress") == "true, true, true, true, true, true, false, false")
        {
            //Debug.Log("Player have unlocked Oliver before");

            UnlockCharacter(true, true, true, true, true, true, false, false);
        }

        if (PlayerPrefs.GetString("currentProgress") == "true, true, true, true, true, true, true, false")
        {
            //Debug.Log("Player have unlocked Orion before");

            UnlockCharacter(true, true, true, true, true, true, true, false);
        }

        if (PlayerPrefs.GetString("currentProgress") == "true, true, true, true, true, true, true, true" || PlayerPrefs.GetString("isTraining") == "yes")
        {
            //Debug.Log("Player have unlocked Aria before");

            UnlockCharacter(true, true, true, true, true, true, true, true);
            PlayerPrefs.SetString("playerFinishedGame", "yes");
        }

        //Debug.Log(isUnlocked[0].ToString() + isUnlocked[1].ToString() + isUnlocked[2].ToString() + isUnlocked[3].ToString() + isUnlocked[4].ToString() + isUnlocked[5].ToString() + isUnlocked[6].ToString() + isUnlocked[7].ToString());
    }

    private void SaveProgress(int progressIndex)
    {
        //Debug.Log("Updating progress! Actual progress is: " + progressIndex);

        switch (progressIndex)
        {
            case 1: PlayerPrefs.SetString("currentProgress", "true, false, false, false, false, false, false, false"); break;
            case 2: PlayerPrefs.SetString("currentProgress", "true, true, false, false, false, false, false, false"); break;
            case 3: PlayerPrefs.SetString("currentProgress", "true, true, true, false, false, false, false, false"); break;
            case 4: PlayerPrefs.SetString("currentProgress", "true, true, true, true, false, false, false, false"); break;
            case 5: PlayerPrefs.SetString("currentProgress", "true, true, true, true, true, false, false, false"); break;
            case 6: PlayerPrefs.SetString("currentProgress", "true, true, true, true, true, true, false, false"); break;
            case 7: PlayerPrefs.SetString("currentProgress", "true, true, true, true, true, true, true, false"); break;
            case 8: PlayerPrefs.SetString("currentProgress", "true, true, true, true, true, true, true, true"); PlayerPrefs.SetString("playerFinishedGame", "yes"); break;
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

            GUI.DrawTexture(new Rect(charX, charY, charWidth, charHeight), characterImages[playerIndex], ScaleMode.ScaleToFit);

            if (isUnlocked[playerIndex] == false && lockIcon != null)
            {
                float lockIconSize = 100f;
                GUI.DrawTexture(new Rect(charX + (charWidth / 2) - (lockIconSize / 2), charY + (charHeight / 2) - (lockIconSize / 2), lockIconSize, lockIconSize), lockIcon, ScaleMode.ScaleToFit);
            }
        }
    }

    private void DrawHeroDetails()
    {
        float panelX = Screen.width * 0.1f;
        float panelY = Screen.height * 0.30f;
        float labelWidth = 600f;
        float labelHeight = 200f;

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
        GUI.Label(new Rect(panelX, panelY, labelWidth, labelHeight), characterNames[playerIndex], nameStyle);

        if (isUnlocked[playerIndex] == true)
        {
            DrawStatBars(panelX, panelY + 70);
        }
    }

    private void DrawArenaButton()
{
    float buttonWidth = 400f;
    float buttonHeight = 80f;
    float buttonX = Screen.width - buttonWidth - 20f; // Top-right corner
    float buttonY = 20f;

    GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
    {
        fontSize = 35,
        normal = { textColor = Color.white, background = MakeTexture(1, 1, Color.black) }
    };

    if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Select Arena", buttonStyle))
    {
        showArenaPanel = !showArenaPanel; // Toggle panel visibility
    }
}

private void DrawArenaPanel()
{
    if (!showArenaPanel) return;

    float panelWidth = Screen.width * 0.9f; // Adjust the panel width
    float panelHeight = Screen.height * 0.8f; // Adjust the panel height
    float panelX = (Screen.width - panelWidth) / 2;
    float panelY = (Screen.height - panelHeight) / 2;

    // Panel Background Style
    GUIStyle panelStyle = new GUIStyle(GUI.skin.box)
    {
        normal = { background = MakeTexture(1, 1, new Color(0, 0, 0, 0.6f)) }
    };
    GUI.Box(new Rect(panelX, panelY, panelWidth, panelHeight), GUIContent.none, panelStyle);

    // Add "Select Arena" Title
    GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
    {
        fontSize = 45,
        fontStyle = FontStyle.Bold,
        normal = { textColor = Color.white },
        alignment = TextAnchor.UpperCenter
    };

    GUI.Label(new Rect(panelX, panelY + 20f, panelWidth, 50f), "Select Arena", titleStyle);

    // Arena Image Dimensions
    float imageWidth = 500f; // Width of each image
    float imageHeight = 500f; // Height of each image
    float spacing = 40f; // Spacing between images

    float startX = panelX + 50f; // Starting X position for images
    float startY = panelY + 100f; // Start below the title

    for (int i = 0; i < arenaImages.Length; i++)
    {
        float imageX = startX + (i * (imageWidth + spacing)); // Calculate X position for each image
        float imageY = startY;

        // Draw the Arena Image Button
        if (GUI.Button(new Rect(imageX, imageY, imageWidth, imageHeight), GUIContent.none))
        {
            selectedArenaIndex = i; // Select the clicked arena

            if (wasArenaSelected == false)
            {
                wasArenaSelected = true;
            }

            Debug.Log("Selected Arena: " + arenaNames[selectedArenaIndex]);
        }

        // Draw the Arena Image
        GUI.DrawTexture(new Rect(imageX, imageY, imageWidth, imageHeight), arenaImages[i], ScaleMode.ScaleToFit);

        // Checkmark for Selected Arena
        if (selectedArenaIndex == i)
        {
            GUIStyle checkmarkStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 60,
                normal = { textColor = Color.green },
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(imageX + imageWidth - 70f, imageY + 20f, 50f, 50f), "✔", checkmarkStyle);
        }
    }

    // Render Arena Names after the panel and images
    for (int i = 0; i < arenaImages.Length; i++)
    {
        float imageX = startX + (i * (imageWidth + spacing)); // Calculate X position for each image
        float imageY = startY;

        // Add Arena Name Label
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 35,
            normal = { textColor = Color.white },
            alignment = TextAnchor.UpperCenter
        };

        GUI.Label(new Rect(imageX, imageY + imageHeight + 30, imageWidth, 40), arenaNames[i], labelStyle);
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
            if (isUnlocked[playerIndex] == true && wasArenaSelected == true)
            {
                //Debug.Log("Character Selected: " + characterNames[currentIndex]);

                SelectedGabriella();
                SelectedMarcus();
                SelectedSelena();
                SelectedBryan();
                SelectedNun();
                SelectedOliver();
                SelectedOrion();
                SelectedAria();

                ShowVsPanel();
            }
            else
            {
                if (wasArenaSelected == false)
                {
                    // Warn about not selected arena
                }

                showLockedMessage = true;
                lockedMessageTimer = 3f;
            }
        }
    }

    private void ShowVsPanel()
    {
        showVsPanel = true;
        currentCountdown = countdownDuration;

        if (canRender == false)
        {
            canRender = true;
            StartCoroutine(CountdownAndLoadScene());
        }
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

        if (playerIndex == 0)
        {
            //Debug.Log("Loading Gabriella Player Versus Image");

            GUI.DrawTexture(new Rect(gabriellaX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), gabriellaPlayer, ScaleMode.ScaleToFit);
        }

        if (playerIndex == 1)
        {
            //Debug.Log("Loading Marcus Player Versus Image");

            GUI.DrawTexture(new Rect(gabriellaX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), marcusPlayer, ScaleMode.ScaleToFit);
        }

        if (playerIndex == 2)
        {
            //Debug.Log("Loading Selena Player Versus Image");

            GUI.DrawTexture(new Rect(gabriellaX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), selenaPlayer, ScaleMode.ScaleToFit);
        }

        if (playerIndex == 3)
        {
            //Debug.Log("Loading Bryan Player Versus Image");

            GUI.DrawTexture(new Rect(gabriellaX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), bryanPlayer, ScaleMode.ScaleToFit);
        }

        if (playerIndex == 4)
        {
            //Debug.Log("Loading Nun Player Versus Image");

            GUI.DrawTexture(new Rect(gabriellaX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), nunPlayer, ScaleMode.ScaleToFit);
        }

        if (playerIndex == 5)
        {
            //Debug.Log("Loading Oliver Player Versus Image");

            GUI.DrawTexture(new Rect(gabriellaX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), oliverPlayer, ScaleMode.ScaleToFit);
        }

        if (playerIndex == 6)
        {
            //Debug.Log("Loading Orion Player Versus Image");

            GUI.DrawTexture(new Rect(gabriellaX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), orionPlayer, ScaleMode.ScaleToFit);
        }

        if (playerIndex == 7)
        {
            //Debug.Log("Loading Aria Player Versus Image");

            GUI.DrawTexture(new Rect(gabriellaX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), ariaPlayer, ScaleMode.ScaleToFit);
        }

        GUI.DrawTexture(new Rect(iconX, panelY + (panelHeight / 2) - (vsIconHeight / 2), vsIconWidth, vsIconHeight), vsIcon, ScaleMode.ScaleToFit);

        if (enemyIndex == 1)
        {
            //Debug.Log("Loading Gabriella Enemy Versus Image");

            GUI.DrawTexture(new Rect(marcusX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), gabriellaEnemy, ScaleMode.ScaleToFit);
        }

        if (enemyIndex == 2)
        {
            //Debug.Log("Loading Marcus Enemy Versus Image");

            GUI.DrawTexture(new Rect(marcusX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), marcusEnemy, ScaleMode.ScaleToFit);
        }

        if (enemyIndex == 3)
        {
            //Debug.Log("Loading Selena Enemy Versus Image");

            GUI.DrawTexture(new Rect(marcusX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), selenaEnemy, ScaleMode.ScaleToFit);
        }

        if (enemyIndex == 4)
        {
            //Debug.Log("Loading Bryan Enemy Versus Image");

            GUI.DrawTexture(new Rect(marcusX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), bryanEnemy, ScaleMode.ScaleToFit);
        }

        if (enemyIndex == 5)
        {
            //Debug.Log("Loading Nun Enemy Versus Image");

            GUI.DrawTexture(new Rect(marcusX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), nunEnemy, ScaleMode.ScaleToFit);
        }

        if (enemyIndex == 6)
        {
            //Debug.Log("Loading Oliver Enemy Versus Image");

            GUI.DrawTexture(new Rect(marcusX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), oliverEnemy, ScaleMode.ScaleToFit);
        }

        if (enemyIndex == 7)
        {
            //Debug.Log("Loading Orion Enemy Versus Image");

            GUI.DrawTexture(new Rect(marcusX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), orionEnemy, ScaleMode.ScaleToFit);
        }

        if (enemyIndex == 8)
        {
            //Debug.Log("Loading Aria Enemy Versus Image");

            GUI.DrawTexture(new Rect(marcusX, panelY + (panelHeight / 2) - (characterTextureHeight / 2), characterTextureWidth, characterTextureHeight), ariaEnemy, ScaleMode.ScaleToFit);
        }
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
        playerIndex = (playerIndex > 0) ? playerIndex - 1 : characterImages.Length - 1;
        PlayHeroIntro();
    }

    private void ShowNextCharacter()
    {
        playerIndex = (playerIndex + 1) % characterImages.Length;
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
        DrawStatBar("Durability", durabilityStats[playerIndex], x, y);
        DrawStatBar("Offense", offenseStats[playerIndex], x, y + 50);
        DrawStatBar("Control Effect", controlEffectStats[playerIndex], x, y + 100);
        DrawStatBar("Difficulty", difficultyStats[playerIndex], x, y + 150);
    }

    private void DrawStatBar(string label, int value, float x, float y)
    {
        float barWidth = 400f;
        float barHeight = 35f;
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

    #region Next Fight Operations

    private void SelectedGabriella()
    {
        if (characterNames[playerIndex] == "Gabriella")
        {
            if (isUnlocked[0] == true && isUnlocked[1] == false && isUnlocked[2] == false && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 2, selectedArenaIndex + 1); // Gabriella vs Marcus
                enemyIndex = 2;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == false && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 3, selectedArenaIndex + 1); // Gabriella vs Selena
                enemyIndex = 3;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 4, selectedArenaIndex + 1); // Gabriella vs Bryan
                enemyIndex = 4;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 5, selectedArenaIndex + 1); // Gabriella vs Nun
                enemyIndex = 5;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 6, selectedArenaIndex + 1); // Gabriella vs Oliver
                enemyIndex = 6;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 7, selectedArenaIndex + 1); // Gabriella vs Orion
                enemyIndex = 7;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(1, 8, selectedArenaIndex + 1); // Gabriella vs Aria
                enemyIndex = 8;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == true)
            {
                randomizeCharacter = Random.Range(1, 9); // Randomize next Enemy character
                randomizeArena = Random.Range(1, 5);

                if (randomizeCharacter == 9)
                {
                    randomizeCharacter = Random.Range(1, 8); // Randomize again to let it more unpredictible
                }

                if (randomizeArena == 5)
                {
                    randomizeArena = Random.Range(1, 4); // Randomize again to let it more unpredictible
                }

                enemyIndex = randomizeCharacter;

                SetupNextFight(1, randomizeCharacter, selectedArenaIndex + 1); // Next enemy character and arena selected by random against Gabriella

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedMarcus()
    {
        if (characterNames[playerIndex] == "Marcus")
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == false && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(2, 3, selectedArenaIndex + 1); // Marcus vs Selena
                enemyIndex = 3;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(2, 4, selectedArenaIndex + 1); // Marcus vs Bryan
                enemyIndex = 4;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(2, 5, selectedArenaIndex + 1); // Marcus vs Nun
                enemyIndex = 5;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(2, 6, selectedArenaIndex + 1); // Marcus vs Oliver
                enemyIndex = 6;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(2, 7, selectedArenaIndex + 1); // Marcus vs Orion
                enemyIndex = 7;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(2, 8, selectedArenaIndex + 1); // Marcus vs Aria
                enemyIndex = 8;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == true)
            {
                randomizeCharacter = Random.Range(1, 9); // Randomize next Enemy character
                randomizeArena = Random.Range(1, 5);

                if (randomizeCharacter == 9)
                {
                    randomizeCharacter = Random.Range(1, 8); // Randomize again to let it more unpredictible
                }

                if (randomizeArena == 5)
                {
                    randomizeArena = Random.Range(1, 4); // Randomize again to let it more unpredictible
                }

                enemyIndex = randomizeCharacter;

                SetupNextFight(2, randomizeCharacter, selectedArenaIndex + 1); // Next enemy character and arena selected by random against Marcus

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedSelena()
    {
        if (characterNames[playerIndex] == "Selena")
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(3, 4, selectedArenaIndex + 1); // Selena vs Bryan
                enemyIndex = 4;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(3, 5, selectedArenaIndex + 1); // Selena vs Nun
                enemyIndex = 5;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(3, 6, selectedArenaIndex + 1); // Selena vs Oliver
                enemyIndex = 6;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(3, 7, selectedArenaIndex + 1); // Selena vs Orion
                enemyIndex = 7;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(3, 8, selectedArenaIndex + 1); // Selena vs Aria
                enemyIndex = 8;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == true)
            {
                randomizeCharacter = Random.Range(1, 9); // Randomize next Enemy character
                randomizeArena = Random.Range(1, 5);

                if (randomizeCharacter == 9)
                {
                    randomizeCharacter = Random.Range(1, 8); // Randomize again to let it more unpredictible
                }

                if (randomizeArena == 5)
                {
                    randomizeArena = Random.Range(1, 4); // Randomize again to let it more unpredictible
                }

                enemyIndex = randomizeCharacter;

                SetupNextFight(3, randomizeCharacter, selectedArenaIndex + 1); // Next enemy character and arena selected by random against Selena

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedBryan()
    {
        if (characterNames[playerIndex] == "Bryan")
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(4, 5, selectedArenaIndex + 1); // Bryan vs Nun
                enemyIndex = 5;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(4, 6, selectedArenaIndex + 1); // Bryan vs Oliver
                enemyIndex = 6;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(4, 7, selectedArenaIndex + 1); // Bryan vs Orion
                enemyIndex = 7;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(4, 8, selectedArenaIndex + 1); // Bryan vs Aria
                enemyIndex = 8;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == true)
            {
                randomizeCharacter = Random.Range(1, 9); // Randomize next Enemy character
                randomizeArena = Random.Range(1, 5);

                if (randomizeCharacter == 9)
                {
                    randomizeCharacter = Random.Range(1, 8); // Randomize again to let it more unpredictible
                }

                if (randomizeArena == 5)
                {
                    randomizeArena = Random.Range(1, 4); // Randomize again to let it more unpredictible
                }

                enemyIndex = randomizeCharacter;

                SetupNextFight(4, randomizeCharacter, selectedArenaIndex + 1); // Next enemy character and arena selected by random against Bryan

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedNun()
    {
        if (characterNames[playerIndex] == "Nun")
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(5, 6, selectedArenaIndex + 1); // Nun vs Oliver
                enemyIndex = 6;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(5, 7, selectedArenaIndex + 1); // Nun vs Orion
                enemyIndex = 7;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(5, 8, selectedArenaIndex + 1); // Nun vs Aria
                enemyIndex = 8;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == true)
            {
                randomizeCharacter = Random.Range(1, 9); // Randomize next Enemy character
                randomizeArena = Random.Range(1, 5);

                if (randomizeCharacter == 9)
                {
                    randomizeCharacter = Random.Range(1, 8); // Randomize again to let it more unpredictible
                }

                if (randomizeArena == 5)
                {
                    randomizeArena = Random.Range(1, 4); // Randomize again to let it more unpredictible
                }

                enemyIndex = randomizeCharacter;

                SetupNextFight(5, randomizeCharacter, selectedArenaIndex + 1); // Next enemy character and arena selected by random against Nun

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedOliver()
    {
        if (characterNames[playerIndex] == "Oliver")
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(6, 7, selectedArenaIndex + 1); // Oliver vs Orion
                enemyIndex = 7;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(6, 8, selectedArenaIndex + 1); // Oliver vs Aria
                enemyIndex = 8;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == true)
            {
                randomizeCharacter = Random.Range(1, 9); // Randomize next Enemy character
                randomizeArena = Random.Range(1, 5);

                if (randomizeCharacter == 9)
                {
                    randomizeCharacter = Random.Range(1, 8); // Randomize again to let it more unpredictible
                }

                if (randomizeArena == 5)
                {
                    randomizeArena = Random.Range(1, 4); // Randomize again to let it more unpredictible
                }

                enemyIndex = randomizeCharacter;

                SetupNextFight(6, randomizeCharacter, selectedArenaIndex + 1); // Next enemy character and arena selected by random against Oliver

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedOrion()
    {
        if (characterNames[playerIndex] == "Orion")
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(7, 8, selectedArenaIndex + 1); // Orion vs Aria
                enemyIndex = 7;
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == true)
            {
                randomizeCharacter = Random.Range(1, 9); // Randomize next Enemy character
                randomizeArena = Random.Range(1, 5);

                if (randomizeCharacter == 9)
                {
                    randomizeCharacter = Random.Range(1, 8); // Randomize again to let it more unpredictible
                }

                if (randomizeArena == 5)
                {
                    randomizeArena = Random.Range(1, 4); // Randomize again to let it more unpredictible
                }

                enemyIndex = randomizeCharacter;

                SetupNextFight(7, randomizeCharacter, selectedArenaIndex + 1); // Next enemy character and arena selected by random against Orion

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedAria()
    {
        if (characterNames[playerIndex] == "Aria")
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == true)
            {
                randomizeCharacter = Random.Range(1, 9); // Randomize next Enemy character
                randomizeArena = Random.Range(1, 5);

                if (randomizeCharacter == 9)
                {
                    randomizeCharacter = Random.Range(1, 8); // Randomize again to let it more unpredictible
                }

                if (randomizeArena == 5)
                {
                    randomizeArena = Random.Range(1, 4); // Randomize again to let it more unpredictible
                }

                enemyIndex = randomizeCharacter;

                SetupNextFight(8, randomizeCharacter, selectedArenaIndex + 1); // Next enemy character and arena selected by random against Aria

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SetupNextFight(int playerCharacter, int enemyCharacter, int selectedArena)
    {
        PlayerPrefs.SetInt("playerCharacterSelected", playerCharacter); // Load selected Player character
        PlayerPrefs.SetInt("enemyCharacterSelected", enemyCharacter); // Load selected Enemy character
        PlayerPrefs.SetInt("stageSelected", selectedArena); // Load selected Arena battleground
    }

    #endregion
}
