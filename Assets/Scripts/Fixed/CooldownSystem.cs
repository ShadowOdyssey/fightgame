using UnityEngine;
using UnityEngine.UI;

public class CooldownSystem : MonoBehaviour
{
    public RoundManager roundSystem;

    public Image cooldown1Image;
    public Image cooldown2Image;
    public Image cooldown3Image;

    private readonly float cooldown1Time = 3f;
    private readonly float cooldown2Time = 9f;
    private readonly float cooldown3Time = 15f;

    private PlayerSystem playerSystem;

    private bool isCooldown1 = false;
    private bool isCooldown2 = false;
    private bool isCooldown3 = false;

    public void Start()
    {
        playerSystem = roundSystem.playerSystem; // Load it after Round Manager to load it

        cooldown1Image.fillAmount = 0f;
        cooldown2Image.fillAmount = 0f;
        cooldown3Image.fillAmount = 0f;
    }

    private void Update()
    {
        if (roundSystem.isMultiplayer == false)
        {
            if (isCooldown1 == true)
            {
                cooldown1Image.fillAmount = cooldown1Image.fillAmount - 1 / cooldown1Time * Time.deltaTime;

                if (cooldown1Image.fillAmount <= 0f)
                {
                    cooldown1Image.fillAmount = 0f;
                    playerSystem.Cooldown1Finished();
                    isCooldown1 = false;
                }
            }

            if (isCooldown2 == true)
            {
                cooldown2Image.fillAmount = cooldown2Image.fillAmount - 1 / cooldown2Time * Time.deltaTime;

                if (cooldown2Image.fillAmount <= 0f)
                {
                    cooldown2Image.fillAmount = 0f;
                    playerSystem.Cooldown2Finished();
                    isCooldown2 = false;
                }
            }

            if (isCooldown3 == true)
            {
                cooldown3Image.fillAmount = cooldown3Image.fillAmount - 1 / cooldown3Time * Time.deltaTime;

                if (cooldown3Image.fillAmount <= 0f)
                {
                    cooldown3Image.fillAmount = 0f;
                    playerSystem.Cooldown3Finished();
                    isCooldown3 = false;
                }
            }
        }
    }

    public void ActivateCooldown1()
    {
        if (isCooldown1 == false)
        {
            cooldown1Image.fillAmount = 1f;
            isCooldown1 = true;
        }
    }

    public void ActivateCooldown2()
    {
        if (isCooldown2 == false)
        {
            cooldown2Image.fillAmount = 1f;
            isCooldown2 = true;
        }
    }

    public void ActivateCooldown3()
    {
        if (isCooldown3 == false)
        {
            cooldown3Image.fillAmount = 1f;
            isCooldown3 = true;
        }
    }

    public void ResetAllCooldowns()
    {
        cooldown1Image.fillAmount = 0f;
        cooldown2Image.fillAmount = 0f;
        cooldown3Image.fillAmount = 0f;
        isCooldown1 = false;
        isCooldown2 = false;
        isCooldown3 = false;
    }
}
