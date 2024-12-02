using UnityEngine;

public class InfoSystem : MonoBehaviour
{
    public RoundManager roundSystem;

    private float disableTimer = 20f;
    private bool canDisable = false;

    public void Start()
    {
        Time.timeScale = 0.01f;
        canDisable = true;
    }

    public void Update()
    {
        if (canDisable == true)
        {
            disableTimer = disableTimer - Time.timeScale;

            if (disableTimer < 0f)
            {
                disableTimer = 0f;
                canDisable = false;
                Time.timeScale = 1f;
                roundSystem.playerSystem.completedTutorial = false;
                gameObject.SetActive(false);
            }
        }
    }
}
