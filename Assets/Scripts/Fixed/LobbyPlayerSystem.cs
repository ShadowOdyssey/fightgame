using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyPlayerSystem : MonoBehaviour
{
    public Image imageProfile;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI totalWins;
    public TextMeshProUGUI buttonFightText;

    private LobbyManager lobbySystem;
    private int actualProfile = 0;

    private void Start()
    {
        lobbySystem = GameObject.Find("Lobby Manager").GetComponent<LobbyManager>();
        LoadProfile();
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

    public void UpdateName(string newName)
    {
        playerName.text = newName;
    }

    public void UpdateWins(string actualWins)
    {
        totalWins.text = actualWins;
    }

    public void UpdateStatus(string newStatus)
    {
        buttonFightText.text = newStatus;
    }

    public void UpdateProfile(int newProfile)
    {
        actualProfile = newProfile;
    }
}
