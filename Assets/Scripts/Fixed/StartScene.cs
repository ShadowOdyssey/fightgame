using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using JetBrains.Annotations;

public class StartScene : MonoBehaviour
{
    #region Variables

    [Header("Sound Setup")]
    public AudioSource currentAudio; // Added the AudioSource component here to make Sound On and Sound Off option to work
    public Image soundButtonImage;
    public Sprite soundImageOn;
    public Sprite soundImageOff;
    public bool soundOn = true;

    [Header("Loading Setup")]
    public GameObject systemObjects;
    public GameObject loadingObject;

    [Header("Text Setup")]
    public TextMeshProUGUI welcomeMessage;
    public TextMeshProUGUI sceneMessage;

    [Header("Input Field Setup")]
    public TMP_InputField nameInput;

    [Header("Buttons Setup")]
    public Button loginButton;

    #endregion

    #region Loading Components

    public void Awake()
    {
        systemObjects.SetActive(true);
        loadingObject.SetActive(false);
    }

    #endregion

    #region Check Persisted Data

    public void Start()
    {
        //Debug.Log(PlayerPrefs.GetString("playerName"));

        CheckIfPlayerLoggedInBefore();
    }

    #endregion

    #region Scene Operations

    private void CheckIfPlayerLoggedInBefore()
    {
        if (PlayerPrefs.GetString("playerName") != "")
        {
            LoadPersistedValues();
        }
        else
        {
            LoadDefaultValues();
        }
    }

    private void LoadPersistedValues()
    {
        welcomeMessage.text = "Welcome Back " + PlayerPrefs.GetString("playerName") + "!";
        sceneMessage.text = "You can login now or to change your name";
        nameInput.text = PlayerPrefs.GetString("playerName");
        ActivateLoginButton(); // Allow player to click Login button to play the game
    }

    private void LoadDefaultValues()
    {
        DisableLoginButton(); // Dont allow player to click Login button till player to put an acceptable name
        welcomeMessage.text = "Welcome to Shadow Odyssey!";
        sceneMessage.text = "Please enter your name!";
        nameInput.text = "";
    }

    #endregion

    #region Input Field Messages

    private void NameDontHaveEnoughLength() // Display a message to player that name is not acceptable yet
    {
        if (nameInput.text != "" && nameInput.text.Length < 3 && nameInput.text != "")
        {
            if (sceneMessage.text != "Your name should contain more than 3 characters")
            {
                sceneMessage.text = "Your name should contain more than 3 characters";
            }

            DisableLoginButton(); // Dont allow player to click Login button till player to put an acceptable name
        }
    }

    private void InformPlayerIsAllowedToLogin() // Display a message to player that name is acceptable now
    {
        if (sceneMessage.text != "You can login now or to change your name")
        {
            sceneMessage.text = "You can login now or to change your name";
            ActivateLoginButton(); // Allow player to click in Login button
        }
    }

    #endregion

    #region Input Field Operations

    private void NameHaveNowEnoughLength()// Input Field checked that name have enough length to login
    {
        if (nameInput.text != "" && nameInput.text.Length > 3)
        {
            SaveNameToPersistData(); // Save name to load in the other scenes on each new character player to type
            InformPlayerIsAllowedToLogin(); // Inform player that now is allowed to login
        }
    }

    private void SaveNameToPersistData()
    {
        if (PlayerPrefs.GetString("playerName") != nameInput.text)
        {
            PlayerPrefs.SetString("playerName", nameInput.text);
        }
    }

    public void CheckNameField() // Name Input Field is checking if name typed is allowed to login or not
    {
        NameDontHaveEnoughLength();
        NameHaveNowEnoughLength();
    }

    #endregion

    #region Buttons Operations

    #region Reset all Playerprefs variables

    public void ResetAllData()
    {
        PlayerPrefs.SetString("playerName", ""); // Reset player name

        PlayerPrefs.SetString("playerUnlockedNewCharacter", ""); // Reset the value to make it disponible to next fight
        PlayerPrefs.SetInt("enemyCharacter", 0); // Reset the value to make it disponible to next fight
        PlayerPrefs.SetString("currentProgress", ""); // Reset the value to make it disponible to next fight
        PlayerPrefs.SetString("playerFinishedGame", ""); // Reset the value to make it disponible to next fight

        PlayerPrefs.SetInt("playerCharacterSelected", 0); // Reset Player selection
        PlayerPrefs.SetInt("enemyCharacterSelected", 0); // Reset Enemy selection
        PlayerPrefs.SetInt("stageSelected", 0); // Reset Arena selection

        PlayerPrefs.SetString("isMultiplayerActivated", "no"); // Reset multiplayer status
        PlayerPrefs.SetString("isTraining", "no"); // Reset training status
        PlayerPrefs.SetString("tutorialComplete", ""); // Reset tutorial status

        PlayerPrefs.SetInt("selectedMultiplayerPlayerCharacter", 0); // Reset Player Multiplayer selection
        PlayerPrefs.SetString("playerServerID", ""); // Reset actual player session

        PlayerPrefs.SetString("whoWasTheHost", ""); // Reset multiplayer host value
        PlayerPrefs.SetInt("multiplayerPlayer", 0); // Reset multiplayer Player ID value
        PlayerPrefs.SetInt("multiplayerOpponent", 0); // Reset multiplayer Enemy ID value
        PlayerPrefs.SetInt("multiplayerPlayerProfile", 0); // Reset multiplayer Player Profile value
        PlayerPrefs.SetString("multiplayerOpponentName", ""); // Reset multiplayer Enemy Name value
        PlayerPrefs.SetString("multiplayerOpponentProfile", ""); // Reset multiplayer Enemy Profile value
    }

    #endregion

    #region Button Options

    public void RepairTool()
    {
        ResetAllData();

        welcomeMessage.text = "Welcome to Shadow Odyssey!";
        sceneMessage.text = "Game repaired! Type your name again to login!";
        nameInput.text = "";
        DisableLoginButton();
    }

    public void SoundOption()
    {
        if (soundOn == true)
        {
            currentAudio.Pause();
            soundButtonImage.sprite = soundImageOff;
            soundOn = false;
        }
        else
        {
            currentAudio.Play();
            soundButtonImage.sprite = soundImageOn;
            soundOn = true;
        }
    }

    #endregion

    #region Button Actions

    public void StartButtonClicked()
    {
        // Load the next scene after clicking the Start button
        systemObjects.SetActive(false);
        loadingObject.SetActive(true);
        SceneManager.LoadScene("MainMenu");
    }

    private void DisableLoginButton()
    {
        loginButton.interactable = false;
    }

    private void ActivateLoginButton()
    {
        loginButton.interactable = true;
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

    #endregion

    #endregion
}
