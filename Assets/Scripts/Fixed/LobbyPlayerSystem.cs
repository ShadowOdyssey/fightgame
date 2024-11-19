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

    private void Start()
    {
        lobbySystem = GameObject.Find("Lobby Manager").GetComponent<LobbyManager>();
    }
}
