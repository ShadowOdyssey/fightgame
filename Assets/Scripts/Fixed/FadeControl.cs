using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeControl : MonoBehaviour
{
    public RoundManager roundSystem;

    public Animator fadeAnim;

    public Transform fadeCenter;
    public Transform fadeHidden;

    public Image playerImage;
    public Image enemyImage;

    public Sprite gabriellaPlayer;
    public Sprite marcusPlayer;
    public Sprite selenaPlayer;
    public Sprite bryanPlayer;
    public Sprite nunPlayer;
    public Sprite oliverPlayer;
    public Sprite orionPlayer;
    public Sprite ariaPlayer;

    public Sprite gabriellaEnemy;
    public Sprite marcusEnemy;
    public Sprite selenaEnemy;
    public Sprite bryanEnemy;
    public Sprite nunEnemy;
    public Sprite oliverEnemy;
    public Sprite orionEnemy;
    public Sprite ariaEnemy;

    public void StartFadeInSelection(int playerIndex, int enemyIndex)
    {
        switch (playerIndex)
        {
            case 1: playerImage.sprite = gabriellaPlayer; break;
            case 2: playerImage.sprite = marcusPlayer; break;
            case 3: playerImage.sprite = selenaPlayer; break;
            case 4: playerImage.sprite = bryanPlayer; break;
            case 5: playerImage.sprite = nunPlayer; break;
            case 6: playerImage.sprite = oliverPlayer; break;
            case 7: playerImage.sprite = orionPlayer; break;
            case 8: playerImage.sprite = ariaPlayer; break;
        }

        switch (enemyIndex)
        {
            case 1: enemyImage.sprite = gabriellaEnemy; break;
            case 2: enemyImage.sprite = marcusEnemy; break;
            case 3: enemyImage.sprite = selenaEnemy; break;
            case 4: enemyImage.sprite = bryanEnemy; break;
            case 5: enemyImage.sprite = nunEnemy; break;
            case 6: enemyImage.sprite = oliverEnemy; break;
            case 7: enemyImage.sprite = orionEnemy; break;
            case 8: enemyImage.sprite = ariaEnemy; break;
        }

        gameObject.transform.position = fadeCenter.position;
        fadeAnim.Play("FadeInSelection");
    }

    public void StartFadeOut(int playerIndex, int enemyIndex)
    {
        switch (playerIndex)
        {
            case 1: playerImage.sprite = gabriellaPlayer; break;
            case 2: playerImage.sprite = marcusPlayer; break;
            case 3: playerImage.sprite = selenaPlayer; break;
            case 4: playerImage.sprite = bryanPlayer; break;
            case 5: playerImage.sprite = nunPlayer; break;
            case 6: playerImage.sprite = oliverPlayer; break;
            case 7: playerImage.sprite = orionPlayer; break;
            case 8: playerImage.sprite = ariaPlayer; break;
        }

        switch (enemyIndex)
        {
            case 1: enemyImage.sprite = gabriellaEnemy; break;
            case 2: enemyImage.sprite = marcusEnemy; break;
            case 3: enemyImage.sprite = selenaEnemy; break;
            case 4: enemyImage.sprite = bryanEnemy; break;
            case 5: enemyImage.sprite = nunEnemy; break;
            case 6: enemyImage.sprite = oliverEnemy; break;
            case 7: enemyImage.sprite = orionEnemy; break;
            case 8: enemyImage.sprite = ariaEnemy; break;
        }

        gameObject.transform.position = fadeCenter.position;
        fadeAnim.Play("FadeOut");
    }

    public void FinishedFadeOut()
    {
        gameObject.transform.position = fadeHidden.position;
        roundSystem.FadeFinished();
    }

    public void FinishedFadeIn()
    {
        SceneManager.LoadScene("FightScene");
    }
}
