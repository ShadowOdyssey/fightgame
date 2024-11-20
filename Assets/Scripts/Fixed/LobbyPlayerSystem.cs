using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyPlayerSystem : MonoBehaviour
{
    public Image imageProfile;

    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI totalWinsText;
    public TextMeshProUGUI buttonFightText;

    private LobbyManager lobbySystem;
    private int actualSession = 0;
    private int actualProfile = 0;
    private int actualWins = 0;
    private bool canFight = false;
    private string playerName = "";
    private string playerReady = "";
    private string playerStatus = "";
    private string uniqueHash = "";

    public void Start()
    {
        lobbySystem = GameObject.Find("Lobby Manager").GetComponent<LobbyManager>();

        if (actualProfile == 0)
        {
            imageProfile.sprite = lobbySystem.noImage;
        }
        else
        {
            LoadProfile();
        }
    }

    public void LoadProfile()
    {
        switch (actualProfile)
        {
            case 1: imageProfile.sprite = lobbySystem.gabriellaProfile; break;
            case 2: imageProfile.sprite = lobbySystem.marcusProfile; break;
            case 3: imageProfile.sprite = lobbySystem.selenaProfile; break;
            case 4: imageProfile.sprite = lobbySystem.bryanProfile; break;
            case 5: imageProfile.sprite = lobbySystem.nunProfile; break;
            case 6: imageProfile.sprite = lobbySystem.oliverProfile; break;
            case 7: imageProfile.sprite = lobbySystem.orionProfile; break;
            case 8: imageProfile.sprite = lobbySystem.ariaProfile; break;
        }
    }

    public void UpdateSession(int newSession)
    {
        actualSession = newSession;
    }

    public void UpdateProfile(int newProfile)
    {
        actualProfile = newProfile;
        LoadProfile();
    }

    public void UpdateWins(int totalWins)
    {
        actualWins = totalWins;
        totalWinsText.text = totalWins.ToString();
    }

    public void UpdateName(string newName)
    {
        playerName = newName;
        playerNameText.text = playerName;
    }

    public void UpdateReady(string newReady)
    {
        playerReady = newReady;

        if (playerReady == "no")
        {
            canFight = false;
            playerReady = "LOBBY";
            buttonFightText.color = Color.white;
            buttonFightText.text = playerReady;
        }

        if (playerReady == "yes")
        {
            canFight = true;
            playerReady = "FIGHT";
            buttonFightText.color = Color.green;
            buttonFightText.text = playerReady;
        }
    }

    public void UpdateStatus(string newStatus)
    {
        playerStatus = newStatus;

        if (playerStatus == "offline")
        {
            lobbySystem.RemoveUnique(uniqueHash);
            Destroy(gameObject);
        }
    }

    public void RegisterUnique(string newHash)
    {
        uniqueHash = newHash;
    }
}
