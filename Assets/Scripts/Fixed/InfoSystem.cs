using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoSystem : MonoBehaviour
{
    public RoundManager roundSystem;

    private float disableTimer = 3f;
    private bool canDisable = false;

    private void Start()
    {
        Time.timeScale = 0.003f;
        canDisable = true;
    }

    private void Update()
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
