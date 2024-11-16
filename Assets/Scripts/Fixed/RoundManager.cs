using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public HealthBar playerHealthBar;         // Player's HealthBar component
    public HealthBar opponentHealthBar;       // Enemy's HealthBar component

    public PlayerSystem playerSystem;
    public EnemySystem enemySystem;

    public GameObject playerWonRound1;
    public GameObject playerWonRound2;
    public GameObject enemyWonRound1;
    public GameObject enemyWonRound2;

    public GameObject arena1;
    public GameObject arena2;

    public GameObject playerCharacter1;
    public GameObject playerCharacter2;
    public GameObject playerCharacter3;
    public GameObject playerCharacter4;
    public GameObject playerCharacter5;
    public GameObject playerCharacter6;
    public GameObject playerCharacter7;
    public GameObject playerCharacter8;

    public GameObject enemyCharacter1;
    public GameObject enemyCharacter2;
    public GameObject enemyCharacter3;
    public GameObject enemyCharacter4;
    public GameObject enemyCharacter5;
    public GameObject enemyCharacter6;
    public GameObject enemyCharacter7;
    public GameObject enemyCharacter8;

    public Text roundText;

    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI enemyNameText;

    public int currentStage = 0;
    public int currentPlayerCharacter = 0;
    public int currentEnemyCharacter = 0;
    public int currentRound = 1;
    public int playerTotalCombo = 0;
    public int enemyTotalCombo = 0;
    public int roundTime = 180;  // 3-minute timer for each round
    public bool roundStarted = false;
    public bool roundOver = false;
    public bool wasDetermined = false;
    public bool isPlayerCombo = false;
    public bool isEnemyCombo = false;

    private readonly int maxHealth = 100;
    private readonly int playerDamagePerSecond = 2; // Example damage per second for player
    private readonly int opponentDamagePerSecond = 1; // Example damage per second for opponent
    private readonly float damageInterval = 1f; // How often to deal damage

    private Text timerText;
    private int combatStage = 0;
    private int timesPlayerWon = 0;
    private int timesEnemyWon = 0;
    private int playerHealth = 0;
    private int opponentHealth = 0;
    private float playerComboTime = 0f;
    private float enemyComboTime = 0f;
    private float decreaseTime = 0f;
    private bool isMultiplayer = false;
    private bool canDecrease = false;

    private void Awake()
    {
        // Round Manager should load the correct light based in the current arena

        PlayerPrefs.SetInt("playerCharacterSelected", 1); // Select Gabriella as player
        PlayerPrefs.SetInt("enemyCharacterSelected", 2); // Select Marcus as opponent
        PlayerPrefs.SetInt("stageSelected", 1); // Load Village Arena


        if (PlayerPrefs.GetInt("stageSelected") != 0)
        {
            currentStage = PlayerPrefs.GetInt("stageSelected");

            switch (currentStage)
            {
                case 1: arena1.SetActive(true); break;
                case 2: arena2.SetActive(true); break;
            }
        }

        if (PlayerPrefs.GetInt("playerCharacterSelected") != 0)
        {
            currentPlayerCharacter = PlayerPrefs.GetInt("playerCharacterSelected");
        }

        if (PlayerPrefs.GetInt("enemyCharacterSelected") != 0)
        {
            currentEnemyCharacter = PlayerPrefs.GetInt("enemyCharacterSelected");
        }

        StopAllCoroutines(); // Stop all coroutines from old scenes
    }

    private void Start()
    {
        switch (currentPlayerCharacter) // 1 = Gabriella 2 = Marcus 3 = Selena 4 = Bryan 5 = Nun 6 = Oliver 7 = Orion and 8 = Aria
        {
            case 1: playerCharacter1.SetActive(true); playerSystem = GameObject.Find("GabriellaPlayer").GetComponent<PlayerSystem>(); break;
            case 2: playerCharacter2.SetActive(true); playerSystem = GameObject.Find("MarcusPlayer").GetComponent<PlayerSystem>(); break;
            case 3: playerCharacter3.SetActive(true); playerSystem = GameObject.Find("SelenaPLayer").GetComponent<PlayerSystem>(); break;
            case 4: playerCharacter4.SetActive(true); playerSystem = GameObject.Find("BryanPlayer").GetComponent<PlayerSystem>(); break;
            case 5: playerCharacter5.SetActive(true); playerSystem = GameObject.Find("NunPlayer").GetComponent<PlayerSystem>(); break;
            case 6: playerCharacter6.SetActive(true); playerSystem = GameObject.Find("OliverPlayer").GetComponent<PlayerSystem>(); break;
            case 7: playerCharacter7.SetActive(true); playerSystem = GameObject.Find("OrionPlayer").GetComponent<PlayerSystem>(); break;
            case 8: playerCharacter8.SetActive(true); playerSystem = GameObject.Find("AriaPlayer").GetComponent<PlayerSystem>(); break;
        }

        switch (currentEnemyCharacter) // 1 = Gabriella 2 = Marcus 3 = Selena 4 = Bryan 5 = Nun 6 = Oliver 7 = Orion and 8 = Aria
        {
            case 1: enemyCharacter1.SetActive(true); enemySystem = GameObject.Find("GabriellaEnemy").GetComponent<EnemySystem>(); break;
            case 2: enemyCharacter2.SetActive(true); enemySystem = GameObject.Find("MarcusEnemy").GetComponent<EnemySystem>(); break;
            case 3: enemyCharacter3.SetActive(true); enemySystem = GameObject.Find("SelenaEnemy").GetComponent<EnemySystem>(); break;
            case 4: enemyCharacter4.SetActive(true); enemySystem = GameObject.Find("BryanEnemy").GetComponent<EnemySystem>(); break;
            case 5: enemyCharacter5.SetActive(true); enemySystem = GameObject.Find("NunEnemy").GetComponent<EnemySystem>(); break;
            case 6: enemyCharacter6.SetActive(true); enemySystem = GameObject.Find("OliverEnemy").GetComponent<EnemySystem>(); break;
            case 7: enemyCharacter7.SetActive(true); enemySystem = GameObject.Find("OrionEnemy").GetComponent<EnemySystem>(); break;
            case 8: enemyCharacter8.SetActive(true); enemySystem = GameObject.Find("AriaEnemy").GetComponent<EnemySystem>(); break;
        }

        if (PlayerPrefs.GetString("playerName") != "") // Load player name to show in the screen
        {
            playerNameText.text = PlayerPrefs.GetString("playerName").ToUpper();
        }

        if (PlayerPrefs.GetString("isMultiplayer") != "yes") // Check if is multiplayer game to the loaded scene
        {
            isMultiplayer = false;
            combatStage = PlayerPrefs.GetInt("combatStage"); // Game is not multiplayer so load the current stage in Singleplay
        }
        else
        {
            isMultiplayer = true; // Game is multiplayer, so dont load stage number from Singleplay
        }

        if (isMultiplayer == false) // If game is Singleplay load the correct enemy name based in the current stage
        {
            switch (combatStage)
            {
                default: enemyNameText.text = "MARCUS"; break; // Just for debug, this line will be removed soon
                case 1: enemyNameText.text = "MARCUS"; break; // Player is in the Stage 1 of Singleplay
                case 2: enemyNameText.text = "SELENA"; break; // Player is in the Stage 2 of Singleplay
                case 3: enemyNameText.text = "BRYAN"; break; // Player is in the Stage 3 of Singleplay
                case 4: enemyNameText.text = "NUN"; break; // Player is in the Stage 4 of Singleplay
                case 5: enemyNameText.text = "OLIVER"; break; // Player is in the Stage 5 of Singleplay
                case 6: enemyNameText.text = "ORION"; break; // Player is in the Stage 6 of Singleplay
                case 7: enemyNameText.text = "ARIA"; break; // Player is in the Stage 7 of Singleplay
            }
        }
        else
        {
            // Wait for server to give the name of the opponent
        }

        // Set both characters' initial health to max health
        playerHealth = maxHealth;
        opponentHealth = maxHealth;

        // Set up the HealthBars with max health
        playerHealthBar.SetMaxHealth(maxHealth);
        opponentHealthBar.SetMaxHealth(maxHealth);

        // Set up the Round text UI and timer UI
        SetupRoundTextUI();
        SetupTimerUI();

        // Start displaying round information
        ResetHealth();
    }

    private void Update()
    {
        if (roundStarted == false)
        {
            //Debug.Log("Round started, show current round");
            DrawRoundText("ROUND " + currentRound);
            timerText.text = "Time: " + roundTime.ToString() + "s";
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

            if (decreaseTime > 1f)
            {
                roundTime = roundTime - 1;
                timerText.text = "Time: " + roundTime.ToString() + "s";
                ApplyDamageToOpponent(opponentDamagePerSecond);
                ApplyDamageToPlayer(playerDamagePerSecond);
                decreaseTime = 0f;
            }

            if (roundTime < 0f)
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

    private void SetupRoundTextUI()
    {
        GameObject canvasGO = new GameObject("RoundCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        GameObject roundTextGO = new GameObject("RoundText");
        roundTextGO.transform.SetParent(canvasGO.transform);
        roundText = roundTextGO.AddComponent<Text>();

        roundText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        roundText.fontSize = 60;
        roundText.alignment = TextAnchor.MiddleCenter;
        roundText.color = new Color(1.0f, 0.84f, 0.0f);
        roundText.text = "Round " + currentRound;

        RectTransform rectTransform = roundText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(500, 150);
        rectTransform.anchoredPosition = new Vector2(0, 200);
    }

    private void SetupTimerUI()
    {
        GameObject timerTextGO = new GameObject("TimerText");
        timerTextGO.transform.SetParent(roundText.transform.parent);
        timerText = timerTextGO.AddComponent<Text>();

        timerText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        timerText.fontSize = 40;
        timerText.alignment = TextAnchor.MiddleCenter;
        timerText.color = Color.red; // Set text color to red
        timerText.fontStyle = FontStyle.Bold; // Set font style to bold

        RectTransform timerRect = timerText.GetComponent<RectTransform>();
        timerRect.sizeDelta = new Vector2(200, 100);
        timerRect.anchoredPosition = new Vector2(0, 480); // Adjusted position for upper middle of the screen
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

                DrawRoundText(playerNameText.text + " WINS ROUND " + currentRound + "!");
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

                DrawRoundText(enemyNameText.text + " WINS ROUND " + currentRound + "!");
            }
            else
            {
                //Debug.Log("Nobody Won");

                playerSystem.StartDrawAnimation();
                enemySystem.StartDrawAnimation();

                DrawRoundText("ROUND " + currentRound + " DRAW!");
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
        roundText.gameObject.SetActive(false);
        StartRound();
    }

    private void DrawRoundText(string message)
    {
        roundText.text = message;
        roundText.gameObject.SetActive(true);
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
