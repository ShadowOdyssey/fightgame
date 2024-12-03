using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DuelSystem : MonoBehaviour
{
    public LobbyManager lobbySystem;

    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI opponentNameText;
    public TextMeshProUGUI waitText;
    public TextMeshProUGUI timerText;

    public GameObject acceptButton;
    public GameObject declineButton;
    public GameObject declineHostButton;
    public GameObject waitMessage;

    public Image playerImage;
    public Image opponentImage;

    public Sprite loadImage;

    public Sprite gabriellaVersus;
    public Sprite marcusVersus;
    public Sprite selenaVersus;
    public Sprite bryanVersus;
    public Sprite nunVersus;
    public Sprite oliverVersus;
    public Sprite orionVersus;
    public Sprite ariaVersus;

    private readonly float waitTime = 60f;

    public string playerSession = "";
    public string opponentSession = "";
    private float countTime = 60f;
    
    private bool wasOpen = false;
    private bool isWaiting = false;

    public void Awake()
    {
        countTime = waitTime;
    }

    public void Start()
    {
        timerText.text = waitTime.ToString("00");
        playerImage.sprite = loadImage;
        opponentImage.sprite = loadImage;
    }

    public void Update()
    {
        if (isWaiting == true)
        {
            countTime = countTime - Time.deltaTime;

            timerText.text = countTime.ToString("00");

            if (countTime < 0f)
            {
                DeclineButton();
            }
        }
    }

    public void LoadVersusImages(int playerProfile, string opponentProfile)
    {
        switch (playerProfile)
        {
            case 1: playerImage.sprite = gabriellaVersus; //Debug.Log("Loaded current versus image - Player is: gabriellaVersus"); 
                break;
            case 2: playerImage.sprite = marcusVersus; //Debug.Log("Loaded current versus image - Player is: marcusVersus"); 
                break;
            case 3: playerImage.sprite = selenaVersus; //Debug.Log("Loaded current versus image - Player is: selenaVersus"); 
                break;
            case 4: playerImage.sprite = bryanVersus; //Debug.Log("Loaded current versus image - Player is: bryanVersus"); 
                break;
            case 5: playerImage.sprite = nunVersus; //Debug.Log("Loaded current versus image - Player is: nunVersus"); 
                break;
            case 6: playerImage.sprite = oliverVersus; //Debug.Log("Loaded current versus image - Player is: oliverVersus"); 
                break;
            case 7: playerImage.sprite = orionVersus; //Debug.Log("Loaded current versus image - Player is: orionVersus"); 
                break;
            case 8: playerImage.sprite = ariaVersus; //Debug.Log("Loaded current versus image - Player is: ariaVersus"); 
                break;
        }

        switch (opponentProfile)
        {
            case "1": opponentImage.sprite = gabriellaVersus; //Debug.Log("Loaded current versus image - Enemy is: gabriellaVersus"); 
                break;
            case "2": opponentImage.sprite = marcusVersus; //Debug.Log("Loaded current versus image - Enemy is: marcusVersus"); 
                break;
            case "3": opponentImage.sprite = selenaVersus; //Debug.Log("Loaded current versus image - Enemy is: selenaVersus"); 
                break;
            case "4": opponentImage.sprite = bryanVersus; //Debug.Log("Loaded current versus image - Enemy is: bryanVersus"); 
                break;
            case "5": opponentImage.sprite = nunVersus; //Debug.Log("Loaded current versus image - Enemy is: nunVersus"); 
                break;
            case "6": opponentImage.sprite = oliverVersus; //Debug.Log("Loaded current versus image - Enemy is: oliverVersus"); 
                break;
            case "7": opponentImage.sprite = orionVersus; //Debug.Log("Loaded current versus image - Enemy is: orionVersus"); 
                break;
            case "8": opponentImage.sprite = ariaVersus; //Debug.Log("Loaded current versus image - Enemy is: ariaVersus"); 
                break;
        }
    }

    public void UpdateSessions(string actualPlayerSesison, string actualOpponentSession)
    {
        if (wasOpen == false)
        {
            playerSession = actualPlayerSesison;
            opponentSession = actualOpponentSession;

            //Debug.Log("Loaded current sessions - Player is: " + playerSession + " and Opponent is: " + opponentSession);
        }
    }

    public void UpdateNames(string playerName, string opponentName)
    {
        if (wasOpen == false)
        {
            playerNameText.text = playerName;
            opponentNameText.text = opponentName;

            //Debug.Log("Loaded current names - Player is: " + playerNameText.text + " and Opponent is: " + opponentNameText.text);
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
        //Debug.Log("Loading versus screen as Host...");

        acceptButton.SetActive(false);
        declineButton.SetActive(false);
        declineHostButton.SetActive(true);
        waitText.text = "Waiting for opponent " + opponentNameText.text + " to accept...";
        waitMessage.SetActive(true);
        isWaiting = true;

        //Debug.Log("Loading versus screen as Host was loaded");
    }

    public void PlayerIsRequested()
    {
        //Debug.Log("Loading versus screen as Invited...");

        acceptButton.SetActive(true);
        declineButton.SetActive(true);
        declineHostButton.SetActive(false);
        waitMessage.SetActive(false);
        isWaiting = true;

        //Debug.Log("Loading versus screen as Invited was loaded");
    }

    public void AcceptButton()
    {
        isWaiting = false;
        lobbySystem.DuelAccepted();
    }

    public void DeclineButton()
    {
        playerNameText.text = "";
        opponentNameText.text = "";
        playerImage.sprite = loadImage;
        opponentImage.sprite = loadImage;
        playerSession = "";
        opponentSession = "";
        isWaiting = false;
        wasOpen = false;
        countTime = waitTime;
        timerText.text = waitTime.ToString("00");
        lobbySystem.DuelDeclined();
    }
}
