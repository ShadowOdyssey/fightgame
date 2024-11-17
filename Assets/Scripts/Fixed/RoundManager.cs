using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundManager : MonoBehaviour
{
    [Header("Scripts Setup")]
    [Tooltip("Player Health Bar script should be attached here from Player Life Bar object inside UI game object in hierarchy")]
    public HealthBar playerHealthBar;
    [Tooltip("Enemy Health Bar script should be attached here from Enemy Life Bar object inside UI game object in hierarchy")]
    public HealthBar opponentHealthBar;
    [Tooltip("PlayerSystem will be loaded automatically when scene to start, so attach nothing here! Make sure characters and arenas area all disabled!")]
    public PlayerSystem playerSystem;
    [Tooltip("EnemySystem will be loaded automatically when scene to start, so attach nothing here! Make sure characters and arenas area all disabled!")]
    public EnemySystem enemySystem;

    [Header("Arena Battlegrounds")]
    [Tooltip("Attach here Arena 1 object inside Battlegrounds object in hierarchy")]
    public GameObject arena1;
    [Tooltip("Attach here Arena 2 object inside Battlegrounds object in hierarchy")]
    public GameObject arena2;
    [Tooltip("Attach here Arena 3 object inside Battlegrounds object in hierarchy")]
    public GameObject arena3;
    [Tooltip("Attach here Arena 4 object inside Battlegrounds object in hierarchy")]
    public GameObject arena4;

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

    [Header("Round Setup")]
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
    [Tooltip("When enabled so Multiplayer system will be loaded in the scene, if disabled so load Singleplay system in the scene")]
    private bool isMultiplayer = false;
    [Tooltip("Trigger to start events and round mechanics in the current round")]
    private bool canDecrease = false;

    private void Awake()
    {
        StopAllCoroutines(); // Stop all coroutines from old scenes

        // DEBUG ONLY - WILL BE REMOVED LATER
        PlayerPrefs.SetInt("playerCharacterSelected", 1); // Select a player character - Just for Debug it will be removed later
        PlayerPrefs.SetInt("enemyCharacterSelected", 7); // Select an enemy character - Just for Debug it will be removed later
        PlayerPrefs.SetInt("stageSelected", 4); // Select an arena - Just for Debug it will be removed later


        if (PlayerPrefs.GetInt("playerCharacterSelected") != 0)
        {
            currentPlayerCharacter = PlayerPrefs.GetInt("playerCharacterSelected");
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("isMultiplayer") != "yes") // Check if is multiplayer game to the loaded scene
        {
            isMultiplayer = false;
        }
        else
        {
            isMultiplayer = true; // Game is multiplayer, so dont load stage number from Singleplay
        }

        if (isMultiplayer == true) // If game is Singleplay load the correct enemy name based in the current stage
        {
            // Wait for server to give the name of the opponent and the selected character in Lobby
            //enemyNameText.text = MultiplayerEnemyName().ToUpper();
            //currentEnemyCharacter = MultiplayerEnemyCharacter();
            //currentStage = MultiplayerStage();
        }
        else
        {
            if (PlayerPrefs.GetInt("stageSelected") != 0)
            {
                currentStage = PlayerPrefs.GetInt("stageSelected");
            }

            if (PlayerPrefs.GetInt("enemyCharacterSelected") != 0)
            {
                currentEnemyCharacter = PlayerPrefs.GetInt("enemyCharacterSelected");
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

        switch (currentStage) // Loading stage
        {
            case 1: arena1.SetActive(true); sceneLight.color = arena1Color; break;
            case 2: arena2.SetActive(true); sceneLight.color = arena2Color; break;
            case 3: arena3.SetActive(true); sceneLight.color = arena3Color; break;
            case 4: arena4.SetActive(true); sceneLight.color = arena4Color; break;
        }

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

        if (PlayerPrefs.GetString("playerName") != "") // Load player name to show in the screen
        {
            playerNameText.text = PlayerPrefs.GetString("playerName").ToUpper();
        }

        // Set both characters' initial health to max health
        playerHealth = maxHealth;
        opponentHealth = maxHealth;

        // Set up the HealthBars with max health
        playerHealthBar.SetMaxHealth(maxHealth);
        opponentHealthBar.SetMaxHealth(maxHealth);

        // Start displaying round information
        ResetHealth();
    }

    private void Update()
    {
        if (roundStarted == false)
        {
            //Debug.Log("Round started, show current round");
            DrawRoundText("ROUND " + currentRound);
            actualTime.text = roundTime.ToString();
            roundStarted = true;
            roundOver = true;
            wasDetermined = false;
            decreaseTime = 0f;
            ResetHealth();
            Invoke(nameof(DisableRoundText), 6f);
        }

        if (canDecrease == true)
        {
            decreaseTime = decreaseTime + Time.deltaTime;

            if (decreaseTime > 1f && roundTime > 0)
            {
                roundTime = roundTime - 1;
                actualTime.text = roundTime.ToString();
                ApplyDamageToOpponent(opponentDamagePerSecond);
                ApplyDamageToPlayer(playerDamagePerSecond);
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

        if (playerHealthBar.slider.value <= 0f && roundOver == false || opponentHealthBar.slider.value <= 0f && roundOver == false)
        {
            //Debug.Log("Character died! Round Over! Check for winner!");

            roundOver = true;
            DetermineRoundWinner();
        }

        if (wasDetermined == true)
        {
            if (timesPlayerWon == 1 && timesEnemyWon == 0 || timesEnemyWon == 1 && timesPlayerWon == 0)
            {
                currentRound = 2;
                Invoke(nameof(StartRoundAgain), 6f);
            }

            if (timesPlayerWon == 1 && timesEnemyWon == 1)
            {
                currentRound = 3;
                Invoke(nameof(StartRoundAgain), 6f);
            }

            if (timesPlayerWon == 2 || timesEnemyWon == 2)
            {

                if (timesPlayerWon == 2)
                {
                    DrawRoundText("FIGHT OVER! " + playerNameText.text + " WINS");
                }

                if (timesEnemyWon == 2)
                {
                    DrawRoundText("FIGHT OVER! " + enemyNameText.text + " WINS");
                }

                Invoke(nameof(FightEnded), 15f);
            }
        }

        if (isPlayerCombo == true)
        {
            playerComboTime = playerComboTime + Time.deltaTime;

            if (playerComboTime > 3f)
            {
                PlayerFinishedCombo();
                //DisablePlayerComboOnScreen(); // Disable current Player combo on screen
            }
        }

        if (isEnemyCombo == true)
        {
            enemyComboTime = enemyComboTime + Time.deltaTime;

            if (enemyComboTime > 3f)
            {
                EnemyFinishedCombo();
                //DisableEnemyComboOnScreen(); // Disable current Enemy combo on screen
            }
        }
    }

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

    private void DetermineRoundWinner()
    {
        if (wasDetermined == false)
        {
            //Debug.Log("Determining winner");

            canDecrease = false;

            // Determine the winner of the round based on remaining health
            if (playerHealth > opponentHealth)
            {
                //Debug.Log("Player Won");

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

                DrawRoundText(playerNameText.text + " WINS ROUND " + currentRound);
            }
            else if (opponentHealth > playerHealth)
            {
                //Debug.Log("Enemy Won");

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

                DrawRoundText(enemyNameText.text + " WINS ROUND " + currentRound);
            }
            else
            {
                //Debug.Log("Nobody Won");

                playerSystem.StartDrawAnimation();
                enemySystem.StartDrawAnimation();

                DrawRoundText("ROUND " + currentRound + " DRAW");
            }

            wasDetermined = true;
        }
    }

    private void ResetHealth()
    {
        playerHealth = maxHealth;
        opponentHealth = maxHealth;

        playerHealthBar.SetMaxHealth(maxHealth);
        opponentHealthBar.SetMaxHealth(maxHealth);
    }

    public void PlayerFinishedCombo()
    {
        if (playerTotalCombo < 3)
        {
            isPlayerCombo = false;
        }
        else
        {
            UpdatePlayerComboOnScreen();
            isPlayerCombo = false;
        }

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
            UpdateEnemyComboOnScreen();
            isEnemyCombo = false;
        }

        enemyTotalCombo = 0;
        enemyComboTime = 0f;
    }

    public void ApplyDamageToPlayer(int damage)
    {
        playerHealth -= damage;
        playerHealthBar.SetHealth(playerHealth);
    }

    public void ApplyDamageToOpponent(int damage)
    {
        opponentHealth -= damage;
        opponentHealthBar.SetHealth(opponentHealth);
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

    private void DisableRoundText()
    {
        roundTextBackground.SetActive(false);
        StartRound();
    }

    private void DrawRoundText(string message)
    {
        roundText.text = message;
        roundTextBackground.SetActive(true);
    }

    private void StartRoundAgain()
    {
        roundStarted = false;
    }

    private void UpdatePlayerComboOnScreen()
    {
        // Show current Player combo in the screen
    }

    private void UpdateEnemyComboOnScreen()
    {
        // Show current Enemy combo in the screen
    }

    private void FightEnded()
    {
        Debug.Log("Fight ended! Return to scene of character selecting an opponent or go to main menu if Player lost the battle");
    }
}
