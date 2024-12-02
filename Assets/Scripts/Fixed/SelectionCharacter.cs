using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectionCharacter : MonoBehaviour
{
    #region Variables

    #region Scene Setup

    [Header("Debug Menu")] // To remove it before to release the game - Just for debug purposes
    public bool resetStats = false; // To enable it will reset all player progress

    [Header("Fade Setup")]
    public FadeControl fadeSystem;

    [Header("Scene Setup")]
    public AudioClip[] heroIntroClips; // Array for hero-specific intro sounds
    public GameObject arenaScreen;
    public GameObject sceneMessageScreen;
    public GameObject lockedScreen;

    [Header("UI Setup")]
    public TextMeshProUGUI arenaSelection;
    public TextMeshProUGUI heroStatus;
    public TextMeshProUGUI sceneMessage;

    [Header("Attributes Setup")]
    public Slider durabilityValue;
    public Slider offenseValue;
    public Slider controlValue;
    public Slider difficultyValue;

    [Header("Hero Models")]
    public GameObject gabriella;
    public GameObject marcus;
    public GameObject selena;
    public GameObject bryan;
    public GameObject nun;
    public GameObject oliver;
    public GameObject orion;
    public GameObject aria;

    #endregion

    #region Hidden Variables

    private AudioSource audioSource;
    private readonly int[] durabilityStats = { 7, 5, 9, 6, 8, 6, 8, 10 };
    private readonly int[] offenseStats = { 8, 9, 7, 6, 6, 9, 7, 10 };
    private readonly int[] controlEffectStats = { 6, 4, 8, 5, 7, 5, 9, 10 };
    private readonly int[] difficultyStats = { 4, 6, 8, 3, 5, 4, 5, 10 };
    private int playerIndex = 1;
    private int randomizeCharacter = 0;
    private int randomizeArena = 0;
    private float lockedMessageTimer = 0f; // Timer to control how long the message appears
    private bool[] isUnlocked = { true, false, false, false, false, false, false, false }; // Only Gabriella is unlocked by default
    private bool showLockedMessage = false; // Indicates if the locked character message should be shown
    private int selectedArenaIndex = 0; // No arena selected initially
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
        CheckIfPlayerFinishedGame(); // Check if player is returning to Selection Character scene after to finish singleplay
        CheckPlayerReturn(); // Check if player is returning to Selection Character scene after to battle some IA
        ShowAttributes();
        PlayHeroIntro(); // Play the first character's intro at start
    }

    #endregion

    #region Real Time Operations

    public void Update()
    {
        if (showLockedMessage == true)
        {
            lockedMessageTimer = lockedMessageTimer - Time.deltaTime;

            if (lockedMessageTimer < 0f)
            {
                lockedMessageTimer = 0f;
                sceneMessageScreen.SetActive(false);
                showLockedMessage = false;
            }
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
        audioSource.loop = false;
        audioSource.clip = heroIntroClips[playerIndex - 1];
        audioSource.Play();
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

    #region Next Fight Operations

    private void SelectedGabriella()
    {
        if (playerIndex == 1)
        {
            if (isUnlocked[0] == true && isUnlocked[1] == false && isUnlocked[2] == false && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 2, selectedArenaIndex); // Gabriella vs Marcus
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == false && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 3, selectedArenaIndex); // Gabriella vs Selena
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 4, selectedArenaIndex); // Gabriella vs Bryan
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 5, selectedArenaIndex); // Gabriella vs Nun
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 6, selectedArenaIndex); // Gabriella vs Oliver
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(1, 7, selectedArenaIndex); // Gabriella vs Orion
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(1, 8, selectedArenaIndex); // Gabriella vs Aria
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

                SetupNextFight(1, randomizeCharacter, selectedArenaIndex); // Next enemy character and arena selected by random against Gabriella

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedMarcus()
    {
        if (playerIndex == 2)
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == false && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(2, 3, selectedArenaIndex); // Marcus vs Selena
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(2, 4, selectedArenaIndex); // Marcus vs Bryan
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(2, 5, selectedArenaIndex); // Marcus vs Nun
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(2, 6, selectedArenaIndex); // Marcus vs Oliver
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(2, 7, selectedArenaIndex); // Marcus vs Orion
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(2, 8, selectedArenaIndex); // Marcus vs Aria
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

                SetupNextFight(2, randomizeCharacter, selectedArenaIndex); // Next enemy character and arena selected by random against Marcus

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedSelena()
    {
        if (playerIndex == 3)
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == false && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(3, 4, selectedArenaIndex); // Selena vs Bryan
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(3, 5, selectedArenaIndex); // Selena vs Nun
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(3, 6, selectedArenaIndex); // Selena vs Oliver
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(3, 7, selectedArenaIndex); // Selena vs Orion
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(3, 8, selectedArenaIndex); // Selena vs Aria
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

                SetupNextFight(3, randomizeCharacter, selectedArenaIndex); // Next enemy character and arena selected by random against Selena

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedBryan()
    {
        if (playerIndex == 4)
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == false && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(4, 5, selectedArenaIndex); // Bryan vs Nun
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(4, 6, selectedArenaIndex); // Bryan vs Oliver
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(4, 7, selectedArenaIndex); // Bryan vs Orion
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(4, 8, selectedArenaIndex); // Bryan vs Aria
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

                SetupNextFight(4, randomizeCharacter, selectedArenaIndex); // Next enemy character and arena selected by random against Bryan

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedNun()
    {
        if (playerIndex == 5)
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == false && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(5, 6, selectedArenaIndex); // Nun vs Oliver
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(5, 7, selectedArenaIndex); // Nun vs Orion
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(5, 8, selectedArenaIndex); // Nun vs Aria
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

                SetupNextFight(5, randomizeCharacter, selectedArenaIndex); // Next enemy character and arena selected by random against Nun

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedOliver()
    {
        if (playerIndex == 6)
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == false && isUnlocked[7] == false)
            {
                SetupNextFight(6, 7, selectedArenaIndex); // Oliver vs Orion
            }

            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(6, 8, selectedArenaIndex); // Oliver vs Aria
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

                SetupNextFight(6, randomizeCharacter, selectedArenaIndex); // Next enemy character and arena selected by random against Oliver

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedOrion()
    {
        if (playerIndex == 7)
        {
            if (isUnlocked[0] == true && isUnlocked[1] == true && isUnlocked[2] == true && isUnlocked[3] == true && isUnlocked[4] == true && isUnlocked[5] == true && isUnlocked[6] == true && isUnlocked[7] == false)
            {
                SetupNextFight(7, 8, selectedArenaIndex); // Orion vs Aria
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

                SetupNextFight(7, randomizeCharacter, selectedArenaIndex); // Next enemy character and arena selected by random against Orion

                randomizeCharacter = 0; // Reset values to be used in the next fight
                randomizeArena = 0; // Reset values to be used in the next fight
            }
        }
    }

    private void SelectedAria()
    {
        if (playerIndex == 8)
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

                SetupNextFight(8, randomizeCharacter, selectedArenaIndex); // Next enemy character and arena selected by random against Aria

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

    #region Scene Operations

    public void SelectHero()
    {
        if (isUnlocked[playerIndex - 1] == true && wasArenaSelected == true)
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

        if (isUnlocked[playerIndex - 1] == true && wasArenaSelected == false)
        {
            lockedMessageTimer = 5f;
            sceneMessage.text = "SELECT AN ARENA TO CONTINUE";
            sceneMessageScreen.SetActive(true);
            showLockedMessage = true;
        }

        if (isUnlocked[playerIndex - 1] == false)
        {
            lockedMessageTimer = 5f;
            sceneMessage.text = "SELECT UNLOCKED HERO TO CONTINUE";
            sceneMessageScreen.SetActive(true);
            showLockedMessage = true;
        }
    }

    public void ShowCharacterModel()
    {
        HiddeAllModels();

        switch (playerIndex)
        {
            case 1: gabriella.SetActive(true); break;
            case 2: marcus.SetActive(true); break;
            case 3: selena.SetActive(true); break;
            case 4: bryan.SetActive(true); break;
            case 5: nun.SetActive(true); break;
            case 6: oliver.SetActive(true); break;
            case 7: orion.SetActive(true); break;
            case 8: aria.SetActive(true); break;
        }

        ShowAttributes();

        if (isUnlocked[playerIndex - 1] == false)
        {
            lockedScreen.SetActive(true);
            heroStatus.text = "HERO LOCKED";
        }
        else
        {
            lockedScreen.SetActive(false);
            heroStatus.text = "HERO UNLOCKED";
        }

        PlayHeroIntro();
    }

    private void ShowAttributes()
    {
        durabilityValue.value = durabilityStats[playerIndex - 1];
        offenseValue.value = offenseStats[playerIndex - 1];
        controlValue.value = controlEffectStats[playerIndex - 1];
        difficultyValue.value = difficultyStats[playerIndex - 1];
    }

    private void HiddeAllModels()
    {
        gabriella.SetActive(false);
        marcus.SetActive(false);
        selena.SetActive(false);
        bryan.SetActive(false);
        nun.SetActive(false);
        oliver.SetActive(false);
        orion.SetActive(false);
        aria.SetActive(false);
    }

    public void SelectArena(int arenaIndex)
    {
        selectedArenaIndex = arenaIndex; // Select the clicked arena

        switch (selectedArenaIndex)
        {
            case 1: arenaSelection.text = "VILLAGE SELECTED"; break;
            case 2: arenaSelection.text = "CASTLE SELECTED"; break;
            case 3: arenaSelection.text = "FOREST SELECTED"; break;
            case 4: arenaSelection.text = "DESERT SELECTED"; break;
        }

        if (wasArenaSelected == false)
        {
            wasArenaSelected = true;
        }

        arenaScreen.SetActive(false);

        //Debug.Log("Selected Arena: " + selectedArenaIndex);
    }

    public void ShowPreviousCharacter()
    {
        playerIndex = playerIndex - 1;

        if (playerIndex <= 0)
        {
            playerIndex = 8;
        }

        ShowCharacterModel();
    }

    public void ShowNextCharacter()
    {
        playerIndex = playerIndex + 1;

        if (playerIndex >= 9)
        {
            playerIndex = 1;
        }

        ShowCharacterModel();
    }

    public void ShowArenaSelection()
    {
        arenaScreen.SetActive(true);
    }

    private void ShowVsPanel()
    {
        fadeSystem.StartFadeInSelection(PlayerPrefs.GetInt("playerCharacterSelected"), PlayerPrefs.GetInt("enemyCharacterSelected"));
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
