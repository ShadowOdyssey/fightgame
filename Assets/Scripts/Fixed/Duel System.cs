using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DuelSystem : MonoBehaviour
{
    public LobbyManager lobbySystem;

    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI opponentNameText;
    public TextMeshProUGUI waitText;

    public GameObject acceptButton;
    public GameObject declineButton;
    public GameObject waitMessage;

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

    private readonly float waitTime = 20f;

    private int playerSession;
    private int opponentSession;
    private float countTime = 20f;
    
    private bool wasOpen = false;
    private bool isWaiting = false;

    public void Update()
    {
        if (isWaiting == true)
        {
            countTime = countTime - Time.deltaTime;

            if (countTime < 0f)
            {
                DeclineButton();
            }
        }
    }

    public void LoadVersusImages(int playerProfile, int opponentProfile)
    {
        if (wasOpen == false)
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
        if (wasOpen == false)
        {
            playerSession = actualPlayerSesison;
            opponentSession = actualOpponentSession;
        }
    }

    public void UpdateNames(string playerName, string opponentName)
    {
        if (wasOpen == false)
        {
            playerNameText.text = playerName;
            opponentNameText.text = opponentName;
        }
    }

    public void OpenDuel(int playerIndex)
    {
        if (wasOpen == false)
        {
            if (playerIndex == 1)
            {
                PlayerIsHost();
            }

            if (playerIndex == 2)
            {
                PlayerIsRequested();
            }

            wasOpen = true;
        }
    }

    public void PlayerIsHost()
    {
        acceptButton.SetActive(false);
        declineButton.SetActive(false);
        waitText.text = "Waiting for opponent " + opponentNameText.text + " to accept...";
        waitMessage.SetActive(true);
        isWaiting = true;
    }

    public void PlayerIsRequested()
    {
        acceptButton.SetActive(true);
        declineButton.SetActive(true);
        waitMessage.SetActive(false);
        isWaiting = true;
    }

    public void AcceptButton()
    {
        isWaiting = false;
        lobbySystem.DuelAccepted(playerSession, opponentSession);
    }

    public void DeclineButton()
    {
        playerNameText.text = "";
        opponentNameText.text = "";
        playerImage.sprite = null;
        opponentImage.sprite = null;
        playerSession = 0;
        opponentSession = 0;
        isWaiting = false;
        wasOpen = false;
        countTime = waitTime;
        gameObject.SetActive(false);
        lobbySystem.DuelDeclined(playerSession, opponentSession);
    }
}
