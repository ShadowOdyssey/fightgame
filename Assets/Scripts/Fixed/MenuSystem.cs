using UnityEngine;
using TMPro;

public class MenuSystem : MonoBehaviour
{
    public RoundManager roundSystem;
    public ServerSystem multiplayerSystem;

    public GameObject menuScreen;
    public GameObject playerLeftScreen;

    public TextMeshProUGUI soundText;
    public TextMeshProUGUI returnText;

    private bool soundOn = true;

    public void Start()
    {
        if (roundSystem.isMultiplayer == true)
        {
            returnText.text = "LOBBY";
        }
        else
        {
            returnText.text = "MAIN MENU";
        }
    }

    public void SoundButton()
    {
        if (soundOn == true)
        {
            roundSystem.audioSystem.ZeroVolume();
            soundText.text = "SOUND OFF";
            soundOn = false;
        }
        else
        {
            roundSystem.audioSystem.ZeroVolume();
            soundText.text = "SOUND ON";
            soundOn = true;
        }
    }

    public void ReturnButton()
    {
        if (roundSystem.isMultiplayer == true)
        {
            ReturnToLobby();
        }
        else
        {
            roundSystem.ReturnToMenu();
        }
    }

    public void ReturnToMainMenu()
    {
        if (multiplayerSystem.playerMultiplayer.selected == true)
        {
            multiplayerSystem.playerMultiplayer.LeaveFight();
        }

        if (multiplayerSystem.enemyMultiplayer.selected == true)
        {
            multiplayerSystem.enemyMultiplayer.LeaveFight();
        }

        roundSystem.ReturnToMenu();
    }

    public void ReturnToLobby()
    {
        if (multiplayerSystem.playerMultiplayer.selected == true)
        {
            multiplayerSystem.playerMultiplayer.LeaveFight();
        }

        if (multiplayerSystem.enemyMultiplayer.selected == true)
        {
            multiplayerSystem.enemyMultiplayer.LeaveFight();
        }

        roundSystem.ReturnToLobby();
    }

    public void OpenPlayerLeftGameScreen()
    {
        playerLeftScreen.SetActive(true);
    }

    public void OpenMenuScreen()
    {
        menuScreen.SetActive(true);
    }

    public void CloseMenuScreen()
    {
        menuScreen.SetActive(false);
    }

    public void QuitGame()
    {
        if (multiplayerSystem.playerMultiplayer.selected == true)
        {
            multiplayerSystem.playerMultiplayer.LeaveFight();
        }

        if (multiplayerSystem.enemyMultiplayer.selected == true)
        {
            multiplayerSystem.enemyMultiplayer.LeaveFight();
        }

        Application.Quit();
    }
}
