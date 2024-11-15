using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public HealthBar playerHealthBar;         // Player's HealthBar component
    public HealthBar opponentHealthBar;       // Enemy's HealthBar component

    public Text roundText;

    public int playerTotalCombo = 0;
    public int enemyTotalCombo = 0;
    public float textDisplayDuration = 2f;
    public float roundTime = 0f;  // 3-minute timer for each round
    public bool roundStarted = false;
    public bool roundOver = false;
    public bool wasDetermined = false;
    public bool isPlayerCombo = false;
    public bool isEnemyCombo = false;

    private PlayerSystem playerSystem;
    private EnemySystem enemySystem;

    private readonly int maxHealth = 100;
    private readonly int playerDamagePerSecond = 2; // Example damage per second for player
    private readonly int opponentDamagePerSecond = 1; // Example damage per second for opponent
    private readonly float damageInterval = 1f; // How often to deal damage

    private Text timerText;
    private int currentRound = 1;
    private int playerHealth = 0;
    private int opponentHealth = 0;
    private float comboTimeLimit = 3f;
    private float playerComboTime = 0f;
    private float enemyComboTime = 0f;

    private void Awake()
    {
        StopAllCoroutines(); // Stop all coroutines from old scenes

        playerSystem = GameObject.Find("Gabriella").GetComponent<PlayerSystem>();
        enemySystem = GameObject.Find("Marcus").GetComponent<EnemySystem>();
    }

    private void Start()
    {
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
        StartCoroutine(DisplayRoundText());
    }

    private void Update()
    {
        if (isPlayerCombo == true)
        {
            playerComboTime = playerComboTime + Time.deltaTime;

            if (playerComboTime > comboTimeLimit)
            {
                PlayerFinishedCombo();
                //DisablePlayerComboOnScreen(); // Disable current Player combo on screen
            }
        }

        if (isEnemyCombo == true)
        {
            enemyComboTime = enemyComboTime + Time.deltaTime;

            if (enemyComboTime > comboTimeLimit)
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

    IEnumerator DisplayRoundText()
    {
        while (currentRound <= 3)
        {
            DrawRoundText("Round " + currentRound);
            yield return new WaitForSeconds(textDisplayDuration);
            roundText.gameObject.SetActive(false);

            // Reset health for both players
            ResetHealth();

            StartRound();

            // Start the countdown and decrease health over time
            yield return StartCoroutine(RoundCountdown());
            currentRound++;
        }

        DrawRoundText("Fight Over!");
        Invoke(nameof(FightEnded), 15f);
    }

    private void StartRound()
    {
        roundText.text = "";
        roundOver = false;
        roundTime = 180f; // Reset timer for each round
                        
        //Debug.Log("Round " + currentRound + " started: Health reset.");

        if (wasDetermined == true)
        {
            wasDetermined = false;
        }

        // Start automatically decreasing health over the duration of the round
        StartCoroutine(DecreaseHealthOverTime());
    }

    IEnumerator DecreaseHealthOverTime()
    {
        while (roundOver == false)
        {
            ApplyDamageToOpponent(opponentDamagePerSecond);
            ApplyDamageToPlayer(playerDamagePerSecond);

            yield return new WaitForSeconds(damageInterval);
        }
    }

    IEnumerator RoundCountdown()
    {
        while (roundTime > 0f && roundOver == false)
        {
            //Debug.Log("Counting round time");

            if (roundStarted == false)
            {
                roundStarted = true;
            }

            roundTime -= Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;

            // Check if either player's health is zero
            if (playerHealth <= 0f || opponentHealth <= 0f)
            {
                roundOver = true;
                DetermineRoundWinner();
            }
        }

        if (roundTime <= 0f && roundOver == false)
        {
            roundTime = 0f;
            UpdateTimerDisplay();
            DetermineRoundWinner();
            roundOver = true;
        }

        yield return new WaitForSeconds(1);
    }

    private void UpdateTimerDisplay()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(roundTime) + "s";
    }

    void DrawRoundText(string message)
    {
        roundText.text = message;
        roundText.gameObject.SetActive(true);
    }

    private void DetermineRoundWinner()
    {
        Debug.Log("Determining winner");

        if (wasDetermined == false)
        {

            // Determine the winner of the round based on remaining health
            if (playerHealth > opponentHealth)
            {
                Debug.Log("Player Won");

                playerSystem.StartVictoryAnimation();
                enemySystem.StartDefeatAnimation();

                DrawRoundText("Player Wins Round " + currentRound + "!");
            }
            else if (opponentHealth > playerHealth)
            {
                Debug.Log("Enemy Won");

                playerSystem.StartDefeatAnimation();
                enemySystem.StartVictoryAnimation();

                DrawRoundText("Enemy Wins Round " + currentRound + "!");
            }
            else
            {
                Debug.Log("Nobody Won");

                playerSystem.StartDrawAnimation();
                enemySystem.StartDrawAnimation();

                DrawRoundText("Round " + currentRound + " Draw!");
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
            playerComboTime = 0f;
            playerTotalCombo = 0;
        }
        else
        {
            UpdatePlayerComboOnScreen();
            isPlayerCombo = false;
            playerComboTime = 0f;
            playerTotalCombo = 0;
        }
    }

    public void EnemyFinishedCombo()
    {
        if (enemyTotalCombo < 3)
        {
            isEnemyCombo = false;
            enemyTotalCombo = 0;
            enemyComboTime = 0f;
        }
        else
        {
            UpdateEnemyComboOnScreen();
            isEnemyCombo = false;
            enemyComboTime = 0f;
            enemyTotalCombo = 0;
        }
    }

    public void ApplyDamageToPlayer(int damage)
    {
        playerHealth -= damage;
        playerHealthBar.SetHealth(playerHealth);
        if (playerHealth <= 0) roundOver = true;
    }

    public void ApplyDamageToOpponent(int damage)
    {
        opponentHealth -= damage;
        opponentHealthBar.SetHealth(opponentHealth);
        if (opponentHealth <= 0) roundOver = true;
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

    private void RestartRound()
    {
        roundOver = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
