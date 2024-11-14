using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public HealthBar playerHealthBar;         // Player's HealthBar component
    public HealthBar opponentHealthBar;       // Enemy's HealthBar component

    public bool roundStarted = false;

    private int currentRound = 1;
    private int maxHealth = 100;
    private int playerHealth;
    private int opponentHealth;
    private bool roundOver = false;

    public float textDisplayDuration = 2f;
    private Text roundText;
    private Text timerText;

    public float roundTime = 0f;  // 3-minute timer for each round

    void Start()
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
    }

    private void StartRound()
    {
        roundOver = false;
        roundTime = 180f; // Reset timer for each round
        //Debug.Log("Round " + currentRound + " started: Health reset.");

        // Start automatically decreasing health over the duration of the round
        StartCoroutine(DecreaseHealthOverTime());
    }

    IEnumerator DecreaseHealthOverTime()
    {
        float damageInterval = 1f; // How often to deal damage
        int playerDamagePerSecond = 5; // Example damage per second for player
        int opponentDamagePerSecond = 3; // Example damage per second for opponent

        while (!roundOver)
        {
            //ApplyDamageToOpponent(opponentDamagePerSecond);
            //ApplyDamageToPlayer(playerDamagePerSecond);

            yield return new WaitForSeconds(damageInterval);
        }
    }

    IEnumerator RoundCountdown()
    {
        while (roundTime > 0 && !roundOver)
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
            if (playerHealth <= 0 || opponentHealth <= 0)
            {
                roundOver = true;
            }
        }

        if (roundTime <= 0 && !roundOver)
        {
            roundOver = true;
            DetermineRoundWinner();
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
        // Determine the winner of the round based on remaining health
        if (playerHealth > opponentHealth)
        {
            DrawRoundText("Gabriella Wins Round " + currentRound + "!");
        }
        else if (opponentHealth > playerHealth)
        {
            DrawRoundText("Marcus Wins Round " + currentRound + "!");
        }
        else
        {
            DrawRoundText("Round " + currentRound + " Draw!");
        }
    }

    private void ResetHealth()
    {
        playerHealth = maxHealth;
        opponentHealth = maxHealth;

        playerHealthBar.SetMaxHealth(maxHealth);
        opponentHealthBar.SetMaxHealth(maxHealth);
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
}
