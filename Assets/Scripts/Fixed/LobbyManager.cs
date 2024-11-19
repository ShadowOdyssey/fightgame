using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security.Permissions;

public class LobbyManager : MonoBehaviour
{
    #region Variables

    #region Components Setup

    [Header("Lobby Setup")]
    public LobbyAudioSystem lobbyAudioSystem;
    public GameObject playerLobbyPrefab;

    [Header("Texts Setup")]
    public TextMeshProUGUI serverMessage;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI selectButtonText;

    [Header("Attributes Values")]
    public Slider durabilityValue;
    public Slider offenseValue;
    public Slider controlEffectValue;
    public Slider difficultyValue;

    #endregion

    #region Image Setup

    [Header("Image Displayer")]
    public Image characterSelectedImage;

    [Header("Select Images")]
    public Sprite gabriellaImage;
    public Sprite marcusImage;
    public Sprite selenaImage;
    public Sprite bryanImage;
    public Sprite nunImage;
    public Sprite oliverImage;
    public Sprite orionImage;
    public Sprite ariaImage;

    [Header("Profile Images")]
    public Sprite gabriellaProfile;
    public Sprite marcusProfile;
    public Sprite selenaProfile;
    public Sprite bryanProfile;
    public Sprite nunProfile;
    public Sprite oliverProfile;
    public Sprite orionProfile;
    public Sprite ariaProfile;

    #endregion

    #region Hidden Variables

    [Header("Monitor")]
    private int currentCharacterSelected = 1;
    private bool selectedCharacter = false;
    private bool joinedLobby = false;
    private bool loadedLobby = false;

    #endregion

    #endregion

    #region Load Components

    private void Awake()
    {
        PlayerPrefs.SetInt("selectedMultiplayerPlayerCharacter", currentCharacterSelected);
    }

    #endregion

    #region Setup Components Loaded

    private void Start()
    {
        lobbyAudioSystem.PlayIntro(currentCharacterSelected);
    }

    #endregion

    #region Selec Character Methods

    public void SelectButton()
    {
        if (selectedCharacter == false && currentCharacterSelected == PlayerPrefs.GetInt("selectedMultiplayerPlayerCharacter"))
        {
            ShowMessage(1);
            ButtonIsNotSelected();
            SaveCurrentSelection();
        }
        else
        {
            ShowMessage(2);
            ButtonIsNotSelected();
            ResetSelection();
        }

        if (currentCharacterSelected != PlayerPrefs.GetInt("selectedMultiplayerPlayerCharacter"))
        {
            ShowMessage(3);
            ButtonIsSelected();
            SaveCurrentSelection();
        }
    }   

    private void CheckCharacterImage()
    {
        switch (currentCharacterSelected)
        {
            case 1: characterSelectedImage.sprite = gabriellaImage; characterName.text = "GABRIELLA"; break;
            case 2: characterSelectedImage.sprite = marcusImage; characterName.text = "MARCUS"; break;
            case 3: characterSelectedImage.sprite = selenaImage; characterName.text = "SELENA"; break;
            case 4: characterSelectedImage.sprite = bryanImage; characterName.text = "BRYAN"; break;
            case 5: characterSelectedImage.sprite = nunImage; characterName.text = "NUN"; break;
            case 6: characterSelectedImage.sprite = oliverImage; characterName.text = "OLIVER"; break;
            case 7: characterSelectedImage.sprite = orionImage; characterName.text = "ORION"; break;
            case 8: characterSelectedImage.sprite = ariaImage; characterName.text = "ARIA"; break;
        }

        lobbyAudioSystem.PlayIntro(currentCharacterSelected);
    }

    public void PreviousCharacterToSelect()
    {
        currentCharacterSelected = currentCharacterSelected - 1;

        if (currentCharacterSelected <= 0)
        {
            currentCharacterSelected = 8;
        }

        if (currentCharacterSelected != PlayerPrefs.GetInt("selectedMultiplayerPlayerCharacter"))
        {
            ButtonIsNotSelected();
        }
        else
        {
            if (selectedCharacter == true)
            {
                ButtonIsSelected();
            }
        }

        CheckCharacterImage();
    }

    public void NextCharacterToSelect()
    {
        currentCharacterSelected = currentCharacterSelected + 1;

        if (currentCharacterSelected >= 9)
        {
            currentCharacterSelected = 1;
        }

        if (currentCharacterSelected != PlayerPrefs.GetInt("selectedMultiplayerPlayerCharacter"))
        {
            ButtonIsNotSelected();
        }
        else
        {
            if (selectedCharacter == true)
            {
                ButtonIsSelected();
            }
        }

        CheckCharacterImage();
    }

    #endregion

    #region Server Message Methods

    private void ShowMessage(int messageIndex)
    {
        switch (messageIndex)
        {
            case 1: serverMessage.text = "Character selected! Now you can fight any player in the lobby!"; break;
            case 2: serverMessage.text = "Character unselected! Select another before to join a fight in the lobby!"; break;
            case 3: serverMessage.text = "Changed character selected to a new one! Now you can fight any player in the lobby!"; break;
            case 4: serverMessage.text = "Server Message: Please select a character before to click in Fight button. You should have a character selected before to join a fight with a player in the lobby!"; break;
        }
    }

    #endregion

    #region Lobby Methods

    public void FightButton()
    {
        if (selectedCharacter == false)
        {
            ShowMessage(4);
            lobbyAudioSystem.PlaySelectHero();
        }
        else
        {
            // Make Player ready to fight in the lobby
        }
    }

    private void ButtonIsNotSelected()
    {
        selectButtonText.color = Color.white;
        selectButtonText.text = "SELECT";
    }

    private void ButtonIsSelected()
    {
        selectButtonText.color = Color.green;
        selectButtonText.text = "SELECTED";
    }

    private void SaveCurrentSelection()
    {
        selectedCharacter = true;
        PlayerPrefs.SetInt("selectedMultiplayerPlayerCharacter", currentCharacterSelected);
    }

    private void ResetSelection()
    {
        selectedCharacter = false;
        PlayerPrefs.SetInt("selectedMultiplayerPlayerCharacter", 0);
    }

    #endregion
}