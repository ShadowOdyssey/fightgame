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
    private bool wasLoaded = false;
    private string actualName = "";
    private string actualReady = "";
    private string actualStatus = "";
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

        wasLoaded = true;
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

        //Debug.Log("Player " + gameObject.name + " updated session! Session value is: " + actualSession);
    }

    public void UpdateProfile(int newProfile)
    {
        actualProfile = newProfile;

        if (wasLoaded == true)
        {
            LoadProfile();
        }

        //Debug.Log("Player " + gameObject.name + " updated profile! Profile value is: " + actualProfile);
    }

    public void UpdateWins(int totalWins)
    {
        actualWins = totalWins;
        totalWinsText.text = totalWins.ToString();

        //Debug.Log("Player " + gameObject.name + " updated wins! Wins value is: " + actualWins);
    }

    public void UpdateName(string newName)
    {
        actualName = newName;
        playerNameText.text = actualName;

        //Debug.Log("Player " + gameObject.name + " updated name! Name value is: " + actualName);
    }

    public void UpdateReady(string newReady)
    {
        actualReady = newReady;

        if (actualReady == "no")
        {
            actualReady = "LOBBY";
            buttonFightText.color = Color.white;
            buttonFightText.text = actualReady;
            canFight = false;
        }

        if (actualReady == "yes")
        {
            actualReady = "FIGHT";
            buttonFightText.color = Color.green;
            buttonFightText.text = actualReady;
            canFight = true;
        }

        //Debug.Log("Player " + gameObject.name + " updated ready! Ready value is: " + actualReady);
    }

    public void UpdateStatus(string newStatus)
    {
        actualStatus = newStatus;

        if (actualStatus == "offline")
        {
            lobbySystem.RemovePlayer(actualSession, actualName, uniqueHash);
            Destroy(gameObject);
        }

        //Debug.Log("Player " + gameObject.name + " updated new status! Status: " + newStatus);
    }

    public void RegisterUnique(string newHash)
    {
        uniqueHash = newHash;
    }
}
