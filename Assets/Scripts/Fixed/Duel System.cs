using UnityEngine;
using UnityEngine.UI;

public class DuelSystem : MonoBehaviour
{
    public Image playerImage;
    public Image opponentImage;

    public Sprite gabriellaVersus;
    public Sprite marcusVersus;
    public Sprite selenaVersus;
    public Sprite bryanVersus;
    public Sprite nunVersus;
    public Sprite oliverVersus;
    public Sprite orionVersus;
    public Sprite ariaVersus;

    private int playerSession;
    private int opponentSession;

    private bool wasOpen = false;

    public void LoadVersusImages(int playerProfile, int opponentProfile)
    {
        if (wasOpen == true)
        {
            switch (playerProfile)
            {
                case 1: playerImage.sprite = gabriellaVersus; break;
                case 2: playerImage.sprite = marcusVersus; break;
                case 3: playerImage.sprite = selenaVersus; break;
                case 4: playerImage.sprite = bryanVersus; break;
                case 5: playerImage.sprite = nunVersus; break;
                case 6: playerImage.sprite = oliverVersus; break;
                case 7: playerImage.sprite = orionVersus; break;
                case 8: playerImage.sprite = ariaVersus; break;
            }

            switch (opponentProfile)
            {
                case 1: opponentImage.sprite = gabriellaVersus; break;
                case 2: opponentImage.sprite = marcusVersus; break;
                case 3: opponentImage.sprite = selenaVersus; break;
                case 4: opponentImage.sprite = bryanVersus; break;
                case 5: opponentImage.sprite = nunVersus; break;
                case 6: opponentImage.sprite = oliverVersus; break;
                case 7: opponentImage.sprite = orionVersus; break;
                case 8: opponentImage.sprite = ariaVersus; break;
            }
        }
    }

    public void UpdateSessions(int actualPlayerSesison, int actualOpponentSession)
    {
        if (wasOpen == true)
        {
            playerSession = actualPlayerSesison;
            opponentSession = actualOpponentSession;
        }
    }

    public void AcceptButton()
    {

    }

    public void DeclineButton()
    {
        playerImage.sprite = null;
        opponentImage.sprite = null;
        playerSession = 0;
        opponentSession = 0;
        wasOpen = false;
        gameObject.SetActive(false);
    }
}
