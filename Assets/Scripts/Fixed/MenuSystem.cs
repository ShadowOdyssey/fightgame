using UnityEngine;
using TMPro;

public class MenuSystem : MonoBehaviour
{
    public RoundManager roundSystem;

    public GameObject menuScreen;

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
            roundSystem.ReturnToLobby();
        }
        else
        {
            roundSystem.ReturnToMenu();
        }
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
        Application.Quit();
    }
}
