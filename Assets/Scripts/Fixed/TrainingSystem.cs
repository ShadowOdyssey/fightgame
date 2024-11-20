using UnityEngine;

public class TrainingSystem : MonoBehaviour
{
    public RoundManager roundSystem;

    public Animator infoAnim;

    public GameObject infoForward;
    public GameObject infoBackward;
    public GameObject infoAttack1;
    public GameObject infoAttack2;
    public GameObject infoAttack3;
    public GameObject infoHit;
    public GameObject infoDefense;

    public int actualInfoIndex = 0;

    public bool resetStats = false;

    public void Start()
    {
        if (resetStats == true)
        {
            PlayerPrefs.SetString("tutorialComplete", ""); // Debug only
            Invoke(nameof(StartInfo), 5f); // Debug only
        }

        if (roundSystem.isTrainingMode == true && PlayerPrefs.GetString("tutorialComplete") != "yes" && resetStats == false)
        {
            Invoke(nameof(StartInfo), 5f);
        }
    }

    public void SelectInfo()
    {
        switch (actualInfoIndex)
        {
            case 1: infoForward.SetActive(false); roundSystem.ShowRoundText("MOVE FORWARD COMPLETED"); Invoke(nameof(InfoBackward), 5f); break;
            case 2: infoBackward.SetActive(false); roundSystem.ShowRoundText("MOVE BACKWARD COMPLETED"); Invoke(nameof(InfoAttack1), 5f); break;
            case 3: infoAttack1.SetActive(false); roundSystem.ShowRoundText("ATTACK 1 COMPLETED"); Invoke(nameof(InfoAttack2), 5f); break;
            case 4: infoAttack2.SetActive(false); roundSystem.ShowRoundText("ATTACK 2 COMPLETED"); Invoke(nameof(InfoAttack3), 5f); break;
            case 5: infoAttack3.SetActive(false); roundSystem.ShowRoundText("ATTACK 3 COMPLETED"); Invoke(nameof(InfoHit), 5f); break;
            case 6: infoHit.SetActive(false); roundSystem.ShowRoundText("GET HIT COMPLETED"); Invoke(nameof(InfoDefense), 5f); break;
            case 7: infoDefense.SetActive(false); roundSystem.ShowRoundText("MOVE DEFENSE COMPLETED"); Invoke(nameof(InfoComplete), 5f); break;
        }
    }

    private void StartInfo()
    {
        actualInfoIndex = 1;
        infoForward.SetActive(true);
        infoAnim.Play("Info1");
        roundSystem.ShowRoundText("MOVE FORWARD");
    }

    public void InfoBackward()
    {
        actualInfoIndex = 2;
        infoBackward.SetActive(true);
        infoAnim.Play("Info2");
        roundSystem.ShowRoundText("MOVE BACKWARD");
    }

    public void InfoAttack1()
    {
        actualInfoIndex = 3;
        infoAttack1.SetActive(true);
        infoAnim.Play("Info3");
        roundSystem.ShowRoundText("USE ATTACK 1");
    }

    public void InfoAttack2()
    {
        actualInfoIndex = 4;
        infoAttack2.SetActive(true);
        infoAnim.Play("Info4");
        roundSystem.ShowRoundText("USE ATTACK 2");
    }

    public void InfoAttack3()
    {
        actualInfoIndex = 5;
        infoAttack3.SetActive(true);
        infoAnim.Play("Info5");
        roundSystem.ShowRoundText("USE ATTACK 3");
    }

    public void InfoHit()
    {
        actualInfoIndex = 6;
        infoHit.SetActive(true);
        infoAnim.Play("Info6");
        roundSystem.ShowRoundText("GET HIT BY OPPONENT");
    }

    public void InfoDefense()
    {
        actualInfoIndex = 7;
        infoDefense.SetActive(true);
        infoAnim.Play("Info7");
        roundSystem.ShowRoundText("AVOID AN OPPONENT ATTACK");
    }

    public void InfoComplete()
    {
        actualInfoIndex = 8;
        roundSystem.ShowRoundText("TUTORIAL COMPLETED");
        PlayerPrefs.SetString("tutorialComplete", "yes");
        Invoke(nameof(CloseRoundText), 5f);
    }

    private void CloseRoundText()
    {
        roundSystem.DisableRoundText();
    }
}
