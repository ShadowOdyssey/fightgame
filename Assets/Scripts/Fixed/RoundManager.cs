using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RoundManager : MonoBehaviour
{
    #region Variables

    #region Scene Setup

    [Header("Scripts Setup")]
    [Tooltip("Audio System script should be attached here from Audio System game object in hierarchy")]
    public AudioSystem audioSystem;
    [Tooltip("Player Health Bar script should be attached here from Player Life Bar object inside UI game object in hierarchy")]
    public HealthBar playerHealthBar;
    [Tooltip("Enemy Health Bar script should be attached here from Enemy Life Bar object inside UI game object in hierarchy")]
    public HealthBar opponentHealthBar;
    [Tooltip("PlayerSystem will be loaded automatically when scene to start, so attach nothing here! Make sure characters and arenas area all disabled!")]
    public PlayerSystem playerSystem;
    [Tooltip("EnemySystem will be loaded automatically when scene to start, so attach nothing here! Make sure characters and arenas area all disabled!")]
    public EnemySystem enemySystem;
    public ServerSystem serverSystem;
    public FadeControl fadeSystem;
    public MenuSystem displaySystem;

    [Header("Arena Battlegrounds")]
    [Tooltip("Attach here Arena 1 object inside Battlegrounds object in hierarchy")]
    public GameObject arena1;
    [Tooltip("Attach here Arena 2 object inside Battlegrounds object in hierarchy")]
    public GameObject arena2;
    [Tooltip("Attach here Arena 3 object inside Battlegrounds object in hierarchy")]
    public GameObject arena3;
    [Tooltip("Attach here Arena 4 object inside Battlegrounds object in hierarchy")]
    public GameObject arena4;

    #endregion

    #region Characters Setup

    [Header("Player Characters")]
    [Tooltip("Attach here GabriellaPlayer object inside Player object in hierarchy")]
    public GameObject playerCharacter1;
    [Tooltip("Attach here MarcusPlayer object inside Player object in hierarchy")]
    public GameObject playerCharacter2;
    [Tooltip("Attach here SelenaPlayer object inside Player object in hierarchy")]
    public GameObject playerCharacter3;
    [Tooltip("Attach here BryanPlayer object inside Player object in hierarchy")]
    public GameObject playerCharacter4;
    [Tooltip("Attach here NunPlayer object inside Player object in hierarchy")]
    public GameObject playerCharacter5;
    [Tooltip("Attach here OliverPlayer object inside Player object in hierarchy")]
    public GameObject playerCharacter6;
    [Tooltip("Attach here OrionPlayer object inside Player object in hierarchy")]
    public GameObject playerCharacter7;
    [Tooltip("Attach here AriaPlayer object inside Player object in hierarchy")]
    public GameObject playerCharacter8;

    [Header("Enemy Characters")]
    [Tooltip("Attach here GabriellaEnemy object inside Enemy object in hierarchy")]
    public GameObject enemyCharacter1;
    [Tooltip("Attach here MarcusEnemy object inside Enemy object in hierarchy")]
    public GameObject enemyCharacter2;
    [Tooltip("Attach here SelenaEnemy object inside Enemy object in hierarchy")]
    public GameObject enemyCharacter3;
    [Tooltip("Attach here BryanEnemy object inside Enemy object in hierarchy")]
    public GameObject enemyCharacter4;
    [Tooltip("Attach here NunEnemy object inside Enemy object in hierarchy")]
    public GameObject enemyCharacter5;
    [Tooltip("Attach here OliverEnemy object inside Enemy object in hierarchy")]
    public GameObject enemyCharacter6;
    [Tooltip("Attach here OrionEnemy object inside Enemy object in hierarchy")]
    public GameObject enemyCharacter7;
    [Tooltip("Attach here AriaEnemy object inside Enemy object in hierarchy")]
    public GameObject enemyCharacter8;

    #endregion

    #region UI Setup

    [Header("Round UI Setup")]
    [Tooltip("Attach here Actual Time object inside UI object in hierarchy")]
    public TextMeshProUGUI actualTime;
    [Tooltip("Attach here Round Text object inside RoundText Background inside UI object in hierarchy")]
    public TextMeshProUGUI roundText;
    [Tooltip("Attach here Player Name object inside Player Life Bar inside UI object in hierarchy")]
    public TextMeshProUGUI playerNameText;
    [Tooltip("Attach here Enemy Name object inside Enemy Life Bar inside UI object in hierarchy")]
    public TextMeshProUGUI enemyNameText;
    [Tooltip("Attach here RoundText Background object inside UI object in hierarchy")]
    public GameObject roundTextBackground;

    [Header("Profile UI Setup")]
    [Tooltip("Attach here Player Profile object inside Player Life Bar inside UI object in hierarchy")]
    public Image playerProfile;
    [Tooltip("Attach here Enemy Profile object inside Enemy Life Bar inside UI object in hierarchy")]
    public Image enemyProfile;
    [Tooltip("Attach here Gabriella´s Profile 1 Image inside Background inside UI object in hierarchy")]
    public Image imageProfile1;
    [Tooltip("Attach here Marcus´s Profile 2 Image inside Background inside UI object in hierarchy")]
    public Image imageProfile2;
    [Tooltip("Attach here Selena´s Profile 3 Image inside Background inside UI object in hierarchy")]
    public Image imageProfile3;
    [Tooltip("Attach here Bryan´s Profile 4 Image inside Background inside UI object in hierarchy")]
    public Image imageProfile4;
    [Tooltip("Attach here Nun´s Profile 5 Image inside Background inside UI object in hierarchy")]
    public Image imageProfile5;
    [Tooltip("Attach here Oliver´s Profile 6 Image inside Background inside UI object in hierarchy")]
    public Image imageProfile6;
    [Tooltip("Attach here Orion´s Profile 7 Image inside Background inside UI object in hierarchy")]
    public Image imageProfile7;
    [Tooltip("Attach here Aria´s Profile 8 Image inside Background inside UI object in hierarchy")]
    public Image imageProfile8;

    [Header("Combo UI Setup")]
    [Tooltip("Attach here Player Combo 1 object inside UI object in hierarchy")]
    public GameObject playerCombo1;
    [Tooltip("Attach here Player Combo 2 object inside UI object in hierarchy")]
    public GameObject playerCombo2;
    [Tooltip("Attach here Player Combo 3 object inside UI object in hierarchy")]
    public GameObject playerCombo3;
    [Tooltip("Attach here Enemy Combo 1 object inside UI object in hierarchy")]
    public GameObject enemyCombo1;
    [Tooltip("Attach here Enemy Combo 2 object inside UI object in hierarchy")]
    public GameObject enemyCombo2;
    [Tooltip("Attach here Enemy Combo 3 object inside UI object in hierarchy")]
    public GameObject enemyCombo3;

    #endregion

    #region Round Setup

    [Header("Win UI Setup")]
    [Tooltip("Attach here Round 1 Win Player object inside Player Life Bar inside UI object in hierarchy")]
    public GameObject playerWonRound1;
    [Tooltip("Attach here Round 2 Win Player object inside Player Life Bar inside UI object in hierarchy")]
    public GameObject playerWonRound2;
    [Tooltip("Attach here Round 1 Win Enemy object inside Enemy Life Bar inside UI object in hierarchy")]
    public GameObject enemyWonRound1;
    [Tooltip("Attach here Round 2 Win Enemy object inside Enemy Life Bar inside UI object in hierarchy")]
    public GameObject enemyWonRound2;

    [Header("Round Setup")]
    [Tooltip("3 minutes time for each round is the standard value, change it to a desired time per round")]
    public int roundTime = 180;
    [Tooltip("Setup the quantity of damage round will deal on Player per second")]
    public int playerDamagePerSecond = 2;
    [Tooltip("Setup the quantity of damage round will deal on Enemy per second")]
    public int opponentDamagePerSecond = 1;

    #endregion

    #region Arena Setup

    [Header("Arena Setup")]
    [Tooltip("Attach here Directional Light in hiearchy")]
    public Light sceneLight;
    [Tooltip("Choice a color to use in Light when Arena 1 to be loaded")]
    public Color arena1Color;
    [Tooltip("Choice a color to use in Light when Arena 2 to be loaded")]
    public Color arena2Color;
    [Tooltip("Choice a color to use in Light when Arena 3 to be loaded")]
    public Color arena3Color;
    [Tooltip("Choice a color to use in Light when Arena 4 to be loaded")]
    public Color arena4Color;

    #endregion

    #region Multiplayer
    [Header("Multiplayer Setup")]
    public OpponentMultiplayer playerMultiplayer;
    public OpponentMultiplayer enemyMultiplayer;
    public int playerMultiplayerID = 0;
    public int enemyMultiplayerID = 0;
    public int playerMultiplayerProfile = 0;
    public int enemyMultiplayerProfile = 0;
    public string actualHost = "";
    public string enemyMultiplayerName = "";
    public bool wasPlayerInput = false;
    public bool wasEnemyInput = false;

    #endregion

    #region Hidden Variables

    [Header("Monitor - Dont change values")]
    [Tooltip("Current Stage loaded in the selection")]
    public int currentStage = 0;
    [Tooltip("Current Player character loaded in the selection")]
    public int currentPlayerCharacter = 0;
    [Tooltip("Current Enemy character loaded in the selection")]
    public int currentEnemyCharacter = 0;
    [Tooltip("Current round player is playing")]
    public int currentRound = 1;
    [Tooltip("Current combo hit sequence Player is doing")]
    public int playerTotalCombo = 0;
    [Tooltip("Current combo hit sequence Enemy is doing")]
    public int enemyTotalCombo = 0;
    [Tooltip("Trigger to check if round started or not")]
    public bool roundStarted = false;
    [Tooltip("Trigger to check if round finished or not")]
    public bool roundOver = false;
    [Tooltip("Trigger to check who won the round")]
    public bool wasDetermined = false;
    [Tooltip("Trigger to check if Player is doing a combo or not")]
    public bool isPlayerCombo = false;
    [Tooltip("Trigger to check if Enemy is doing a combo or not")]
    public bool isEnemyCombo = false;
    [Tooltip("Max health allowed to Player and Enemy")]
    private readonly int maxHealth = 100;
    [Tooltip("Round will check how many rounds has Player won")]
    private int timesPlayerWon = 0;
    [Tooltip("Round will check how many rounds has Enemy won")]
    private int timesEnemyWon = 0;
    [Tooltip("Current health of Player")]
    private int playerHealth = 0;
    [Tooltip("Current health of Enemy")]
    private int opponentHealth = 0;
    [Tooltip("Current combo timer for Player. When it reachs zero deactivate Player combo")]
    private float playerComboTime = 0f;
    [Tooltip("Current combo timer for Enemy. When it reachs zero deactivate Enemy combo")]
    private float enemyComboTime = 0f;
    [Tooltip("Time counter for round mechanics to work")]
    private float decreaseTime = 0f;
    [Tooltip("When enabled so Trainning system will be loaded in the scene, if disabled so load Singleplay system or Multiplayer system in the scene")]
    public bool isTrainingMode = false;
    [Tooltip("When enabled so Multiplayer system will be loaded in the scene, if disabled so load Singleplay system or Training system in the scene")]
    public bool isMultiplayer = false;
    [Tooltip("Trigger to start events and round mechanics in the current round")]
    private bool canDecrease = false;
    [Tooltip("When enabled means game saved the result of the fight to the next scene")]
    private bool dataSent = false;
    private bool fadeIsOut = false;
    private bool wasWaitShow = false;

    #endregion

    #endregion

    #region Loading Data

    public void Awake()
    {
        StopAllCoroutines(); // Stop all coroutines from old scenes
    }

    public void Start()
    {
        playerMultiplayerID = PlayerPrefs.GetInt("multiplayerPlayer");
        enemyMultiplayerID = PlayerPrefs.GetInt("multiplayerOpponent");
        playerMultiplayerProfile = PlayerPrefs.GetInt("multiplayerPlayerProfile");

        if (int.TryParse(PlayerPrefs.GetString("multiplayerOpponentProfile"), out int ni))
        {
            enemyMultiplayerProfile = ni;
        }

        actualHost = PlayerPrefs.GetString("whoWasTheHost");
        enemyMultiplayerName = PlayerPrefs.GetString("multiplayerOpponentName");

        CheckForMultiplayerOrTrainningMode();
        LoadArenaAndEnemyCharacter();

        if (isMultiplayer == false)
        {
            CheckCurrentArena();
        }
        
        CheckCurrentPlayerCharacter();
        CheckCurrentEnemyCharacter();
        LoadPlayerName();
        SetupCharactersHealth();
    }

    #endregion

    #region Real Time Check Operations
    
    private void Update()
    {
        RoundStarted();

        if (isTrainingMode == false)
        {
            CountRoundTime();
            CheckCharactersHealth();
            CheckRoundWinner();
        }

        CheckPlayerCombo();
        CheckEnemyCombo();
    }

    #endregion

    #region Setup Loaded Data

    private void CheckForMultiplayerOrTrainningMode()
    {
        if (PlayerPrefs.GetString("isMultiplayerActivated") == "no") // Check if is multiplayer game to the loaded scene
        {
            isMultiplayer = false;

            LoadPlayerCharacter();

            if (PlayerPrefs.GetString("isTraining") == "yes")
            {
                isTrainingMode = true;
            }
            else
            {
                isTrainingMode = false;
            }
        }
        else
        {
            isTrainingMode = false;
            isMultiplayer = true; // Game is multiplayer, so dont load stage number from Singleplay

            LoadMultiplayerCharacter();
        }
    }

    private void LoadPlayerCharacter()
    {
        if (PlayerPrefs.GetInt("playerCharacterSelected") != 0)
        {
            currentPlayerCharacter = PlayerPrefs.GetInt("playerCharacterSelected");

            //Debug.Log("Player selected character: " + currentPlayerCharacter);
        }
    }

    private void LoadMultiplayerCharacter()
    {
        //Debug.Log("############## LOADING ###############");

        if (actualHost == playerMultiplayerID.ToString())
        {
            // Loading Character Mesh Model
            currentPlayerCharacter = playerMultiplayerProfile; // I am the left side
            currentEnemyCharacter = enemyMultiplayerProfile; // Opponent is right side

            // Loading Names
            playerNameText.text = PlayerPrefs.GetString("playerName").ToUpper(); // Load my name at left side
            enemyNameText.text = PlayerPrefs.GetString("multiplayerOpponentName").ToUpper(); // Load opponent name at right side
        }
        else
        {
            // Loading Character Mesh Model
            currentPlayerCharacter = enemyMultiplayerProfile; // Opponent is left side
            currentEnemyCharacter = playerMultiplayerProfile; // I am the right side

            // Loading Names
            playerNameText.text = PlayerPrefs.GetString("multiplayerOpponentName").ToUpper(); // Load opponent name at left side
            enemyNameText.text = PlayerPrefs.GetString("playerName").ToUpper(); // Load my name at right side
        }
    }

    private void CheckCurrentPlayerCharacter()
    {
        if (isMultiplayer == false)
        {
            switch (currentPlayerCharacter) // Loading Player character
            {
                case 1: playerCharacter1.SetActive(true); playerSystem = GameObject.Find("GabriellaPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile1.sprite; break;
                case 2: playerCharacter2.SetActive(true); playerSystem = GameObject.Find("MarcusPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile2.sprite; break;
                case 3: playerCharacter3.SetActive(true); playerSystem = GameObject.Find("SelenaPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile3.sprite; break;
                case 4: playerCharacter4.SetActive(true); playerSystem = GameObject.Find("BryanPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile4.sprite; break;
                case 5: playerCharacter5.SetActive(true); playerSystem = GameObject.Find("NunPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile5.sprite; break;
                case 6: playerCharacter6.SetActive(true); playerSystem = GameObject.Find("OliverPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile6.sprite; break;
                case 7: playerCharacter7.SetActive(true); playerSystem = GameObject.Find("OrionPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile7.sprite; break;
                case 8: playerCharacter8.SetActive(true); playerSystem = GameObject.Find("AriaPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile8.sprite; break;
            }
        }
        else
        {
            switch (currentPlayerCharacter) // Loading Player character
            {
                case 1: playerCharacter1.SetActive(true); playerMultiplayer = GameObject.Find("GabriellaPlayer").GetComponent<OpponentMultiplayer>(); playerSystem = GameObject.Find("GabriellaPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile1.sprite; break;
                case 2: playerCharacter2.SetActive(true); playerMultiplayer = GameObject.Find("MarcusPlayer").GetComponent<OpponentMultiplayer>(); playerSystem = GameObject.Find("MarcusPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile2.sprite; break;
                case 3: playerCharacter3.SetActive(true); playerMultiplayer = GameObject.Find("SelenaPlayer").GetComponent<OpponentMultiplayer>(); playerSystem = GameObject.Find("SelenaPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile3.sprite; break;
                case 4: playerCharacter4.SetActive(true); playerMultiplayer = GameObject.Find("BryanPlayer").GetComponent<OpponentMultiplayer>(); playerSystem = GameObject.Find("BryanPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile4.sprite; break;
                case 5: playerCharacter5.SetActive(true); playerMultiplayer = GameObject.Find("NunPlayer").GetComponent<OpponentMultiplayer>(); playerSystem = GameObject.Find("NunPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile5.sprite; break;
                case 6: playerCharacter6.SetActive(true); playerMultiplayer = GameObject.Find("OliverPlayer").GetComponent<OpponentMultiplayer>(); playerSystem = GameObject.Find("OliverPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile6.sprite; break;
                case 7: playerCharacter7.SetActive(true); playerMultiplayer = GameObject.Find("OrionPlayer").GetComponent<OpponentMultiplayer>(); playerSystem = GameObject.Find("OrionPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile7.sprite; break;
                case 8: playerCharacter8.SetActive(true); playerMultiplayer = GameObject.Find("AriaPlayer").GetComponent<OpponentMultiplayer>(); playerSystem = GameObject.Find("AriaPlayer").GetComponent<PlayerSystem>(); playerProfile.sprite = imageProfile8.sprite; break;
            }
        }
    }

    private void CheckCurrentEnemyCharacter()
    {
        if (isMultiplayer == false)
        {
            switch (currentEnemyCharacter) // Loading Enemy character
            {
                case 1: enemyCharacter1.SetActive(true); enemySystem = GameObject.Find("GabriellaEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile1.sprite; break;
                case 2: enemyCharacter2.SetActive(true); enemySystem = GameObject.Find("MarcusEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile2.sprite; break;
                case 3: enemyCharacter3.SetActive(true); enemySystem = GameObject.Find("SelenaEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile3.sprite; break;
                case 4: enemyCharacter4.SetActive(true); enemySystem = GameObject.Find("BryanEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile4.sprite; break;
                case 5: enemyCharacter5.SetActive(true); enemySystem = GameObject.Find("NunEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile5.sprite; break;
                case 6: enemyCharacter6.SetActive(true); enemySystem = GameObject.Find("OliverEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile6.sprite; break;
                case 7: enemyCharacter7.SetActive(true); enemySystem = GameObject.Find("OrionEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile7.sprite; break;
                case 8: enemyCharacter8.SetActive(true); enemySystem = GameObject.Find("AriaEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile8.sprite; break;
            }
        }
        else
        {
            switch (currentEnemyCharacter) // Loading Enemy character
            {
                case 1: enemyCharacter1.SetActive(true); enemyMultiplayer = GameObject.Find("GabriellaEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("GabriellaEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile1.sprite; break;
                case 2: enemyCharacter2.SetActive(true); enemyMultiplayer = GameObject.Find("MarcusEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("MarcusEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile2.sprite; break;
                case 3: enemyCharacter3.SetActive(true); enemyMultiplayer = GameObject.Find("SelenaEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("SelenaEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile3.sprite; break;
                case 4: enemyCharacter4.SetActive(true); enemyMultiplayer = GameObject.Find("BryanEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("BryanEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile4.sprite; break;
                case 5: enemyCharacter5.SetActive(true); enemyMultiplayer = GameObject.Find("NunEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("NunEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile5.sprite; break;
                case 6: enemyCharacter6.SetActive(true); enemyMultiplayer = GameObject.Find("OliverEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("OliverEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile6.sprite; break;
                case 7: enemyCharacter7.SetActive(true); enemyMultiplayer = GameObject.Find("OrionEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("OrionEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile7.sprite; break;
                case 8: enemyCharacter8.SetActive(true); enemyMultiplayer = GameObject.Find("AriaEnemy").GetComponent<OpponentMultiplayer>(); enemySystem = GameObject.Find("AriaEnemy").GetComponent<EnemySystem>(); enemyProfile.sprite = imageProfile8.sprite; break;
            }

            CheckActualHost();
        }

        fadeSystem.StartFadeOut(currentPlayerCharacter, currentEnemyCharacter);
    }

    private void CheckActualHost()
    {
        if (actualHost == playerMultiplayerID.ToString())
        {
            playerMultiplayer.SetOpponentEnemy(enemySystem);
            playerMultiplayer.SetID(playerMultiplayerID, enemyMultiplayerID); // If i am the host so i am the player at left side
            playerSystem.RegisterInput();
            serverSystem.RegisterOpponentPlayer(playerMultiplayer);
            serverSystem.RegisterOpponentEnemy(enemyMultiplayer);
            serverSystem.RegisterHost(playerMultiplayerID);
            serverSystem.RegisterDuel(enemyMultiplayerID);
            wasPlayerInput = true;
        }

        if (actualHost != playerMultiplayerID.ToString())
        {
            enemyMultiplayer.SetOpponentPlayer(playerSystem);
            enemyMultiplayer.SetID(playerMultiplayerID, enemyMultiplayerID); // If i am not the host so i am the player at right side
            enemySystem.RegisterInput();
            serverSystem.RegisterOpponentPlayer(playerMultiplayer);
            serverSystem.RegisterOpponentEnemy(enemyMultiplayer);
            serverSystem.RegisterHost(enemyMultiplayerID);
            serverSystem.RegisterDuel(playerMultiplayerID);
            wasEnemyInput = true;
        }
    }

    private void LoadArenaAndEnemyCharacter()
    {
        if (isMultiplayer == false) // If game is SinglePlay load the correct current stage selected from Selection scene and also the correct AI character name
        {
            if (PlayerPrefs.GetInt("stageSelected") != 0)
            {
                currentStage = PlayerPrefs.GetInt("stageSelected");
            }

            if (PlayerPrefs.GetInt("enemyCharacterSelected") != 0)
            {
                currentEnemyCharacter = PlayerPrefs.GetInt("enemyCharacterSelected");

                //Debug.Log("Enemy selected character: " + currentEnemyCharacter);
            }

            switch (currentEnemyCharacter) // Load Singleplay Enemy name
            {
                case 1: enemyNameText.text = "GABRIELLA"; break;
                case 2: enemyNameText.text = "MARCUS"; break;
                case 3: enemyNameText.text = "SELENA"; break;
                case 4: enemyNameText.text = "BRYAN"; break;
                case 5: enemyNameText.text = "NUN"; break;
                case 6: enemyNameText.text = "OLIVER"; break;
                case 7: enemyNameText.text = "ORION"; break;
                case 8: enemyNameText.text = "ARIA"; break;
            }
        }
    }

    public void CheckCurrentArena()
    {
        switch (currentStage) // Loading stage
        {
            case 1: arena1.SetActive(true); sceneLight.color = arena1Color; audioSystem.PlayMusic(2); break; //Debug.Log("Arena 1 loaded"); 
            case 2: arena2.SetActive(true); sceneLight.color = arena2Color; audioSystem.PlayMusic(3); break; //Debug.Log("Arena 2 loaded");
            case 3: arena3.SetActive(true); sceneLight.color = arena3Color; audioSystem.PlayMusic(4); break; //Debug.Log("Arena 3 loaded");
            case 4: arena4.SetActive(true); sceneLight.color = arena4Color; audioSystem.PlayMusic(5); break; //Debug.Log("Arena 4 loaded");
        }
    }

    private void LoadPlayerName()
    {
        if (isMultiplayer == false)
        {
            if (PlayerPrefs.GetString("playerName") != "") // Load player name to show in the screen
            {
                playerNameText.text = PlayerPrefs.GetString("playerName").ToUpper();
            }
        }
    }

    private void SetupCharactersHealth()
    {
        // Set both characters' initial health to max health
        playerHealth = maxHealth;
        opponentHealth = maxHealth;

        // Set up the HealthBars with max health
        playerHealthBar.SetMaxHealth(maxHealth);
        opponentHealthBar.SetMaxHealth(maxHealth);

        // Start displaying round information
        ResetHealth();
    }

    #endregion

    #region Real Time Methods

    private void StartRound()
    {
        //Debug.Log("Characters can fight now!");

        roundText.text = "";

        if (roundOver == true)
        {
            roundOver = false;
        }

        roundTime = 180; // Reset timer for each round
                        
        //Debug.Log("Round " + currentRound + " started: Health reset.");

        if (wasDetermined == true)
        {
            wasDetermined = false;
        }

        // Start automatically decreasing health over the duration of the round
        canDecrease = true;
    }

    private void RoundStarted()
    {
        if (roundStarted == false && fadeIsOut == true)
        {
            //Debug.Log("Round started, show current round");

            if (isTrainingMode == false && isMultiplayer == false)
            {
                ShowRoundText("ROUND " + currentRound);
            }

            if (isTrainingMode == true && isMultiplayer == false)
            {
                ShowRoundText("TRAINING MODE");
            }

            actualTime.text = roundTime.ToString();

            wasDetermined = false;
            decreaseTime = 0f;
            ResetHealth();

            if (isMultiplayer == true)
            {
                if (wasWaitShow == false)
                {
                    if (playerSystem.multiplayerSystem.selected == true)
                    {
                        playerMultiplayer.UpdatePlayerLoaded("yes");
                    }

                    if (enemySystem.multiplayerSystem.selected == true)
                    {
                        enemyMultiplayer.UpdateEnemyLoaded("yes");
                    }

                    ShowRoundText("WAITING FOR OPPONENT TO START...");
                    wasWaitShow = true;
                }

                if (serverSystem.stageLoadedA == "yes" && serverSystem.stageLoadedB == "yes")
                {
                    roundOver = true;
                    ShowRoundText("ROUND " + currentRound);
                    roundStarted = true;
                    Invoke(nameof(DisableRoundText), 6f);
                }
            }
            else
            {
                roundOver = true;
                roundStarted = true;
                Invoke(nameof(DisableRoundText), 6f);
            }
        }
    }

    private void CountRoundTime()
    {
        if (canDecrease == true)
        {
            decreaseTime = decreaseTime + Time.deltaTime;

            if (decreaseTime > 1f && roundTime > 0)
            {
                roundTime = roundTime - 1;
                actualTime.text = roundTime.ToString();

                if (isTrainingMode == false)
                {
                    if (isMultiplayer == false)
                    {
                        ApplyDamageToOpponent(opponentDamagePerSecond);
                        ApplyDamageToPlayer(playerDamagePerSecond);
                    }
                    else
                    {
                        //ApplyDamageToOpponent(playerDamagePerSecond); // Disabled round damage in multiplayer
                        //ApplyDamageToPlayer(playerDamagePerSecond); // Disabled round damage in multiplayer
                    }
                }

                decreaseTime = 0f;
            }

            if (roundTime <= 0f)
            {
                //Debug.Log("Time Over! Check for winner!");

                roundTime = 0;
                roundOver = true;
                DetermineRoundWinner();
            }
        }
    }

    private void CheckCharactersHealth()
    {
        if (isMultiplayer == true)
        {
            if (serverSystem.actualPlayerHealth == 100 && roundOver == false && actualTime.text != "180" && playerHealthBar.slider.value != 100f ||
                serverSystem.actualEnemyHealth == 100 && roundOver == false && actualTime.text != "180" && opponentHealthBar.slider.value != 100f)
            {
                displaySystem.OpenPlayerLeftGameScreen();
            }

            if (serverSystem.actualPlayerHealth <= 0 && roundOver == false || serverSystem.actualEnemyHealth <= 0 && roundOver == false)
            {
                roundOver = true;
                DetermineRoundWinner();
            }
        }
        else
        {
            if (playerHealthBar.slider.value <= 0f && roundOver == false || opponentHealthBar.slider.value <= 0f && roundOver == false)
            {
                roundOver = true;
                DetermineRoundWinner();
            }
        }
    }

    private void DetermineRoundWinner()
    {
        if (wasDetermined == false)
        {
            canDecrease = false;

            if (isMultiplayer == false)
            {
                // Determine the winner of the round based on remaining health from round manager
                if (playerHealth > opponentHealth)
                {
                    playerSystem.StartVictoryAnimation();
                    enemySystem.StartDefeatAnimation();

                    if (playerWonRound1.activeInHierarchy == false)
                    {
                        playerWonRound1.SetActive(true);
                        timesPlayerWon = 1;
                    }
                    else
                    {
                        playerWonRound2.SetActive(true);
                        timesPlayerWon = 2;
                    }

                    ShowRoundText(playerNameText.text + " WINS ROUND " + currentRound);
                }

                if (opponentHealth > playerHealth)
                {
                    playerSystem.StartDefeatAnimation();
                    enemySystem.StartVictoryAnimation();

                    if (enemyWonRound1.activeInHierarchy == false)
                    {
                        enemyWonRound1.SetActive(true);
                        timesEnemyWon = 1;
                    }
                    else
                    {
                        enemyWonRound2.SetActive(true);
                        timesEnemyWon = 2;
                    }

                    ShowRoundText(enemyNameText.text + " WINS ROUND " + currentRound);
                }

                if (playerHealth <= 0f && opponentHealth <= 0f)
                {
                    playerSystem.StartDrawAnimation();
                    enemySystem.StartDrawAnimation();

                    if (enemyWonRound1.activeInHierarchy == false)
                    {
                        enemyWonRound1.SetActive(true);
                        timesEnemyWon = 1;
                    }
                    else
                    {
                        enemyWonRound2.SetActive(true);
                        timesEnemyWon = 2;
                    }

                    if (playerWonRound1.activeInHierarchy == false)
                    {
                        playerWonRound1.SetActive(true);
                        timesPlayerWon = 1;
                    }
                    else
                    {
                        playerWonRound2.SetActive(true);
                        timesPlayerWon = 2;
                    }

                    ShowRoundText("ROUND " + currentRound + " DRAW");
                }

                wasDetermined = true;
            }
            else
            {
                if (playerSystem.multiplayerSystem.selected == true)
                {
                    playerMultiplayer.UpdatePlayerLoaded("no");
                }

                if (enemySystem.multiplayerSystem.selected == true)
                {
                    enemyMultiplayer.UpdateEnemyLoaded("no");
                }

                // Determine the winner of the round based on remaining health from server
                if (serverSystem.actualPlayerHealth > serverSystem.actualEnemyHealth)
                {
                    playerSystem.StartVictoryAnimation();
                    enemySystem.StartDefeatAnimation();

                    if (playerWonRound1.activeInHierarchy == false)
                    {
                        playerWonRound1.SetActive(true);
                        timesPlayerWon = 1;
                    }
                    else
                    {
                        playerWonRound2.SetActive(true);
                        timesPlayerWon = 2;
                    }

                    ShowRoundText(playerNameText.text + " WINS ROUND " + currentRound);
                }

                if (serverSystem.actualEnemyHealth > serverSystem.actualPlayerHealth)
                {
                    playerSystem.StartDefeatAnimation();
                    enemySystem.StartVictoryAnimation();

                    if (enemyWonRound1.activeInHierarchy == false)
                    {
                        enemyWonRound1.SetActive(true);
                        timesEnemyWon = 1;
                    }
                    else
                    {
                        enemyWonRound2.SetActive(true);
                        timesEnemyWon = 2;
                    }

                    ShowRoundText(enemyNameText.text + " WINS ROUND " + currentRound);
                }

                if (serverSystem.actualPlayerHealth <= 0f && serverSystem.actualEnemyHealth <= 0f)
                {
                    playerSystem.StartDrawAnimation();
                    enemySystem.StartDrawAnimation();

                    if (enemyWonRound1.activeInHierarchy == false)
                    {
                        enemyWonRound1.SetActive(true);
                        timesEnemyWon = 1;
                    }
                    else
                    {
                        enemyWonRound2.SetActive(true);
                        timesEnemyWon = 2;
                    }

                    if (playerWonRound1.activeInHierarchy == false)
                    {
                        playerWonRound1.SetActive(true);
                        timesPlayerWon = 1;
                    }
                    else
                    {
                        playerWonRound2.SetActive(true);
                        timesPlayerWon = 2;
                    }

                    ShowRoundText("ROUND " + currentRound + " DRAW");
                }

                wasDetermined = true;
            }
        }
    }

    private void CheckRoundWinner()
    {
        if (wasDetermined == true)
        {
            if (timesPlayerWon == 1 && timesEnemyWon == 0 || timesEnemyWon == 1 && timesPlayerWon == 0)
            {
                currentRound = 2;
                Invoke(nameof(StartRoundAgain), 6f);
            }

            if (timesPlayerWon == 1 && timesEnemyWon == 1)
            {
                if (currentRound == 2)
                {
                    currentRound = 3;
                }

                if (currentRound == 1)
                {
                    currentRound = 2;
                }

                Invoke(nameof(StartRoundAgain), 6f);
            }

            if (timesPlayerWon == 2 && timesEnemyWon == 1 || timesEnemyWon == 2 && timesPlayerWon == 1 ||
                timesPlayerWon == 2 && timesEnemyWon == 0 || timesEnemyWon == 2 && timesPlayerWon == 0)
            {

                if (timesPlayerWon == 2)
                {
                    if (isMultiplayer == true)
                    {
                        playerMultiplayer.RegisterVictory();
                    }

                    ShowRoundText("FIGHT OVER! " + playerNameText.text + " WINS");
                }

                if (timesEnemyWon == 2)
                {
                    if (isMultiplayer == true)
                    {
                        enemyMultiplayer.RegisterVictory();
                    }

                    ShowRoundText("FIGHT OVER! " + enemyNameText.text + " WINS");
                }

                Invoke(nameof(FightEnded), 5f);
            }

            if (timesPlayerWon == 2 && timesEnemyWon == 2)
            {
                ShowRoundText("FIGHT OVER! DRAW! NO WINNERS");
                Invoke(nameof(FightEnded), 5f);
            }
        }
    }

    private void CheckPlayerCombo()
    {
        if (isPlayerCombo == true)
        {
            playerComboTime = playerComboTime + Time.deltaTime;

            if (playerComboTime > 3f)
            {
                PlayerFinishedCombo();
            }
        }
    }

    private void CheckEnemyCombo()
    {
        if (isEnemyCombo == true)
        {
            enemyComboTime = enemyComboTime + Time.deltaTime;

            if (enemyComboTime > 3f)
            {
                EnemyFinishedCombo();
            }
        }
    }

    #endregion

    #region Health Operations

    private void ResetHealth()
    {
        playerHealth = maxHealth;
        opponentHealth = maxHealth;

        if (isMultiplayer == false)
        {
            playerHealthBar.SetMaxHealth(maxHealth);
            opponentHealthBar.SetMaxHealth(maxHealth);
        }
        else
        {
            if (serverSystem.actualPlayerHealth != maxHealth)
            {
                serverSystem.actualPlayerHealth = maxHealth;
                playerHealthBar.SetMaxHealth(maxHealth);

                if (playerSystem.multiplayerSystem.selected == true)
                {
                    playerMultiplayer.UpdatePlayerLife(playerHealth.ToString());
                }
            }

            
            if (serverSystem.actualEnemyHealth != maxHealth)
            {
                serverSystem.actualEnemyHealth = maxHealth;
                opponentHealthBar.SetMaxHealth(maxHealth);

                if (enemySystem.multiplayerSystem.selected == true)
                {
                    enemyMultiplayer.UpdateEnemyLife(opponentHealth.ToString());
                }
            }
        }
    }

    public void MultiplayerPlayerDamage(int newDamage)
    {
        playerHealth = playerHealth - newDamage;
        playerMultiplayer.UpdatePlayerLife(playerHealth.ToString());
    }

    public void MultiplayerEnemyDamage(int newDamage)
    {
        opponentHealth = opponentHealth - newDamage;
        enemyMultiplayer.UpdateEnemyLife(opponentHealth.ToString());
    }

    public void UpdatePlayerHealth(int newHealth)
    {
        playerHealth = newHealth;
        playerHealthBar.SetHealth(playerHealth);
    }

    public void UpdateEnemyHealth(int newHealth)
    {
        opponentHealth = newHealth;
        opponentHealthBar.SetHealth(opponentHealth);
    }

    public void ApplyDamageToPlayer(int damage)
    {
        if (isTrainingMode == false)
        {
            playerHealth = playerHealth - damage;
            playerHealthBar.SetHealth(playerHealth);
        }
    }

    public void ApplyDamageToOpponent(int damage)
    {
        if (isTrainingMode == false)
        {
            opponentHealth = opponentHealth - damage;
            opponentHealthBar.SetHealth(opponentHealth);
        }
    }

    #endregion

    #region Combo Operations

    public void PlayerFinishedCombo()
    {
        if (playerTotalCombo < 3)
        {
            isPlayerCombo = false;
        }
        else
        {
            isPlayerCombo = false;
        }

        UpdatePlayerComboOnScreen();
        playerComboTime = 0f;
        playerTotalCombo = 0;
    }

    public void EnemyFinishedCombo()
    {
        if (enemyTotalCombo < 3)
        {
            isEnemyCombo = false;
        }
        else
        {
            isEnemyCombo = false;
        }

        UpdateEnemyComboOnScreen();
        enemyTotalCombo = 0;
        enemyComboTime = 0f;
    }
    
    public void PlayerStartCombo()
    {
        if (isPlayerCombo == false)
        {
            playerTotalCombo = 1;
            UpdatePlayerComboOnScreen();
            isPlayerCombo = true;
        }
    }

    public void EnemyStartCombo()
    {
        if (isEnemyCombo == false)
        {
            enemyTotalCombo = 1;
            UpdateEnemyComboOnScreen();
            isEnemyCombo = true;
        }
    }

    public void PlayerContinueCombo()
    {
        if (playerComboTime != 0f)
        {
            playerTotalCombo = playerTotalCombo + 1;
            UpdatePlayerComboOnScreen();
            playerComboTime = 0f;
        }
    }

    public void EnemyContinueCombo()
    {
        if (enemyComboTime != 0f)
        {
            enemyTotalCombo = enemyTotalCombo + 1;
            UpdateEnemyComboOnScreen();
            enemyComboTime = 0f;
        }
    }

    private void UpdatePlayerComboOnScreen()
    {
        // Show current Player combo in the screen using an animated gameObject that have Animator on it, just enable and disable it using SetActive() to make it to work

        if (playerTotalCombo == 1)
        {
            playerCombo1.SetActive(true);
        }

        if (playerTotalCombo == 2)
        {
            playerCombo2.SetActive(true);
        }

        if (playerTotalCombo == 3)
        {
            playerCombo3.SetActive(true);
        }

        // In all combos gameObjects will be disabled in the last frame of it´s animations
    }

    private void UpdateEnemyComboOnScreen()
    {
        // Show current Player combo in the screen using an animated gameObject that have Animator on it, just enable and disable it using SetActive() to make it to work

        if (enemyTotalCombo == 1)
        {
            enemyCombo1.SetActive(true);
        }

        if (enemyTotalCombo == 2)
        {
            enemyCombo2.SetActive(true);
        }

        if (enemyTotalCombo == 3)
        {
            enemyCombo3.SetActive(true);
        }

        // In all combos gameObjects will be disabled in the last frame of it´s animations
    }

    #endregion

    #region UI Operations

    public void DisableRoundText()
    {
        roundTextBackground.SetActive(false);
        StartRound();
    }

    public void ShowRoundText(string message)
    {
        roundText.text = message;
        roundTextBackground.SetActive(true);
    }

    #endregion

    #region Round Operations

    private void StartRoundAgain()
    {
        if (isMultiplayer == true)
        {
            wasWaitShow = false;
        }

        roundStarted = false;
    }

    private void FightEnded()
    {
        //Debug.Log("Fight ended! Return to scene of character selecting an opponent or go to main menu if Player lost the battle");

        if (timesPlayerWon == 2 && dataSent == false)
        {
            //Debug.Log("Player finished the game before? " + PlayerPrefs.GetString("playerFinishedGame"));

            if (isMultiplayer == false)
            {
                if (PlayerPrefs.GetString("playerFinishedGame") == "no")
                {
                    //Debug.Log("Player Won! Lets return at character selection with a character unblocked or show final video if player defeated last enemy!");

                    PlayerPrefs.SetString("playerUnlockedNewCharacter", "yes"); // Lets inform the another scene that Player was successfull to defeat last enemy
                    PlayerPrefs.SetInt("enemyCharacter", currentEnemyCharacter); // Lets inform the another scene about the current enemy character in the scene

                    //Debug.Log("Player Unlocked New Character value is: " + PlayerPrefs.GetString("playerUnlockedNewCharacter"));

                    Invoke(nameof(ReturnToSelection), 10f); // We use a delay here to make sure the data in Playerprefs will be registered safely to inform the next scene correctly
                }
                else
                {
                    //Debug.Log("Player won and already finished the game, return to selection character scene");

                    Invoke(nameof(ReturnToSelection), 10f); // We use a delay here to make sure the data in Playerprefs will be registered safely to inform the next scene correctly
                }
            }
            else
            {
                enemyMultiplayer.LeaveFight();
                playerMultiplayer.LeaveFight();
                Invoke(nameof(ReturnToLobby), 6f);
            }

            dataSent = true;
        }

        if (timesEnemyWon == 2 && dataSent == false)
        {
            //Debug.Log("Player finished the game before? " + PlayerPrefs.GetString("playerFinishedGame"));

            if (isMultiplayer == false)
            {
                if (PlayerPrefs.GetString("playerFinishedGame") == "no")
                {
                    //Debug.Log("Player lost! Return to character selection or load a defeat animation that loads main menu again in the end, it can be a video");

                    PlayerPrefs.SetString("playerUnlockedNewCharacter", "no"); // Lets inform the another scene that Player was not successfull to defeat last enemy
                    PlayerPrefs.SetInt("enemyCharacter", currentEnemyCharacter); // Lets inform the another scene about the current enemy character in the scene
                    Invoke(nameof(ReturnToMenu), 10f); // We use a delay here to make sure the data in Playerprefs will be registered safely to inform the next scene correctly
                }
                else
                {
                    //Debug.Log("Player lost but already finished the game");

                    Invoke(nameof(ReturnToMenu), 10f); // We use a delay here to make sure the data in Playerprefs will be registered safely to inform the next scene correctly
                }
            }
            else
            {
                enemyMultiplayer.LeaveFight();
                playerMultiplayer.LeaveFight();
                Invoke(nameof(ReturnToLobby), 6f);
            }

            dataSent = true;
        }

        if (timesPlayerWon == 2 && timesEnemyWon == 2 && isMultiplayer == false)
        {
            Invoke(nameof(ReturnToMenu), 10f); // We use a delay here to make sure the data in Playerprefs will be registered safely to inform the next scene correctly
        }

        if (timesPlayerWon == 2 && timesEnemyWon == 2 && isMultiplayer == true)
        {
            enemyMultiplayer.LeaveFight();
            playerMultiplayer.LeaveFight();
            Invoke(nameof(ReturnToLobby), 6f);
        }
    }

    private void ReturnToSelection()
    {
        SceneManager.LoadScene("SelectionCharacter");
    }

    public void ReturnToLobby()
    {
        SceneManager.LoadScene("ArcadeMode");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void FadeFinished()
    {
        fadeIsOut = true;
    }

    #endregion
}
