using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class LobbyManager : MonoBehaviour
{
    #region Variables

    #region Components Setup

    [Header("Lobby Setup")]
    [Tooltip("Attach here Audio System object in the hierarchy")]
    public LobbyAudioSystem lobbyAudioSystem;
    [Tooltip("Prefab used to instantiate players in the Players On Lobby list")]
    public GameObject playerLobbyPrefab;
    [Tooltip("Attach here Connecting object inside UI object in the hierarchy")]
    public GameObject connectingScreen;
    [Tooltip("Adjust the time Lobby System will spend to refresh Players On Lobby list. Remember that low values cause more server calls making the server unstable. Find a moderated value to refresh Player On Lobby list!")]
    public float timeToRefreshPlayerList = 2f;

    [Header("Texts Setup")]
    [Tooltip("Attach here Server Messages object inside Character Selected object that is inside UI object in the hierarchy")]
    public TextMeshProUGUI serverMessage;
    [Tooltip("Attach here Character Name object inside Character Selected object that is inside UI object in the hierarchy")]
    public TextMeshProUGUI characterName;
    [Tooltip("Attach here Select Text object inside Select Character button that is inside Character Selected object that is inside UI object in the hierarchy")]
    public TextMeshProUGUI selectButtonText;
    [Tooltip("Attach here Connecting Text object inside Connecting object that is inside Character Selected object that is inside UI object in the hierarchy")]
    public TextMeshProUGUI connectionText;

    [Header("Attributes Values")]
    [Tooltip("Attach here Durability Value slider inside DURABILITY object that is inside Character Selected object that is inside UI object in the hierarchy")]
    public Slider durabilityValue;
    [Tooltip("Attach here Offense Value slider inside OFFENSE object that is inside Character Selected object that is inside UI object in the hierarchy")]
    public Slider offenseValue;
    [Tooltip("Attach here Control Effect Value slider inside CONTROL EFFECT object that is inside Character Selected object that is inside UI object in the hierarchy")]
    public Slider controlEffectValue;
    [Tooltip("Attach here Difficulty Value slider inside DIFFICULTY object that is inside Character Selected object that is inside UI object in the hierarchy")]
    public Slider difficultyValue;

    #endregion

    #region Database Setup

    [Header("Database Setup")]
    [Tooltip("Put the URL of the PHP file Verify User in the host server")]
    public string verifyUser = "https://queensheartgames.com/shadowodyssey/verifyuser.php";
    [Tooltip("Put the URL of the PHP file Connect User in the host server")]
    public string connectUser = "https://queensheartgames.com/shadowodyssey/connectuser.php";
    [Tooltip("Put the URL of the PHP file Register User in the host server")]
    public string registerUser = "https://queensheartgames.com/shadowodyssey/registeruser.php";
    [Tooltip("Put the URL of the PHP file Update User in the host server")]
    public string updateUser = "https://queensheartgames.com/shadowodyssey/updateuser.php";
    [Tooltip("Put the URL of the PHP file Delete User in the host server")]
    public string deleteUser = "https://queensheartgames.com/shadowodyssey/deleteuser.php";
    [Tooltip("Put the URL of the PHP file Enter Session in the host server")]
    public string enterSession = "https://queensheartgames.com/shadowodyssey/entersession.php";

    [Header("Success Response Setup")]
    [Tooltip("Setup the message when a connection is a success with the server")]
    public string success001 = "Connection success! Connected with the server! Loading Lobby..."; // Done
    [Tooltip("Setup the message when to register a new user is a success in the database")]
    public string success002 = "New account registered with sucess! Connecting..."; // Done
    [Tooltip("Setup the message when a user verification if exists in database is a success")]
    public string success003 = "User found in database, connecting with server..."; // DOne
    [Tooltip("Setup the message when a user data update in database is a success")]
    public string success004 = "User register in database changed!"; // DOne

    [Header("Error Response Setup")]
    [Tooltip("Setup the message when a connection with the server have failed")]
    public string error001 = "Connection failed! Please try again!"; // Done
    [Tooltip("Setup the message when a user verification if exists in database have failed")]
    public string error002 = "Requested user dont found in the database! Registering new user..."; // Done
    [Tooltip("Setup the message when to register a new user is a failure in the database")]
    public string error003 = "Was not possible to regirster a new user. Open a ticket to admin!"; // Done
    [Tooltip("Setup the message when a data verification if exists in database have failed")]
    public string error004 = "Requested data dont found in the database!"; // Done
    [Tooltip("Setup the message when a failed to change a user data in database")]
    public string error005 = "Was not possible to apply new changes in database!"; // Done

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
    private bool verifiedSucces = false;
    private bool connectedSucces = false;
    private bool registeredSucces = false;
    private bool joinedLobby = false;
    private bool loadedLobby = false;
    private bool notRegistered = false;
    private string actualName = "";
    private string currentSession = "";
    private string responseFromServer = "";

    #endregion

    #endregion

    #region Load Components
    
    public void Awake()
    {
        if (PlayerPrefs.GetString("playerName") != "")
        {
            actualName = PlayerPrefs.GetString("playerName");
        }

        PlayerPrefs.SetInt("selectedMultiplayerPlayerCharacter", 0);
    }

    #endregion

    #region Setup Components Loaded

    public void Start()
    {
        connectingScreen.SetActive(true); // We make sure that Connecting Screen always will appear enabled when Lobby Manager to start
        StopAllCoroutines(); // Stop all coroutines from old scenes
        LoadDefault(); // Load all default values before to apply new values in Select Character
        StartCoroutine(VerifyUser(verifyUser, "id", "lobby", "name", "'" + actualName + "'")); // Everything is ready, so lets start to connect with the server automatically
    }

    #endregion

    #region Real Time Operations

    public void Update()
    {
        #region User not registered, so try to register a new user in the database

        if (notRegistered == true)
        {
            notRegistered = false;
            StopAllCoroutines(); // After any Coroutine call, stop the last coroutine activated. It is mandatory. Always stop a Coroutine after to use it
            StartCoroutine(RegisterNewUser(registerUser, actualName));
        }

        #endregion

        #region User registration was a success in the database

        if (registeredSucces == true)
        {
            registeredSucces = false;
            StopAllCoroutines(); // After any Coroutine call, stop the last coroutine activated. It is mandatory. Always stop a Coroutine after to use it
            StartCoroutine(ConnectToServer(connectUser, actualName, "online"));
        }

        #endregion

        #region User was verified with a success, user exist in database

        if (verifiedSucces == true)
        {
            verifiedSucces = false;
            StopAllCoroutines(); // After any Coroutine call, stop the last coroutine activated. It is mandatory. Always stop a Coroutine after to use it
            StartCoroutine(ConnectToServer(connectUser, actualName, "online"));
        }

        #endregion

        #region User is connected with the server with success

        if (connectedSucces == true)
        {
            connectedSucces = false;
            StopAllCoroutines(); // After any Coroutine call, stop the last coroutine activated. It is mandatory. Always stop a Coroutine after to use it
            connectingScreen.SetActive(false);
            joinedLobby = true;
        }

        #endregion

        #region Load Players On Lobby List after user to connect with success

        if (joinedLobby == true)
        {

        }

        #endregion
    }

    #endregion

    #region Select Character Methods

    private void CheckCharacterImage()
    {
        switch (currentCharacterSelected)
        {
            case 1:
                characterSelectedImage.sprite = gabriellaImage; characterName.text = "GABRIELLA";
                durabilityValue.value = 7f;
                offenseValue.value = 8f;
                controlEffectValue.value = 6f;
                difficultyValue.value = 4f;
                break;

            case 2:
                characterSelectedImage.sprite = marcusImage; characterName.text = "MARCUS";
                durabilityValue.value = 5f;
                offenseValue.value = 9f;
                controlEffectValue.value = 4f;
                difficultyValue.value = 6f;
                break;

            case 3:
                characterSelectedImage.sprite = selenaImage; characterName.text = "SELENA";
                durabilityValue.value = 9f;
                offenseValue.value = 7f;
                controlEffectValue.value = 8f;
                difficultyValue.value = 8f;
                break;

            case 4:
                characterSelectedImage.sprite = bryanImage; characterName.text = "BRYAN";
                durabilityValue.value = 6f;
                offenseValue.value = 6f;
                controlEffectValue.value = 5f;
                difficultyValue.value = 3f;
                break;

            case 5:
                characterSelectedImage.sprite = nunImage; characterName.text = "NUN";
                durabilityValue.value = 8f;
                offenseValue.value = 6f;
                controlEffectValue.value = 7f;
                difficultyValue.value = 5f;
                break;

            case 6:
                characterSelectedImage.sprite = oliverImage; characterName.text = "OLIVER";
                durabilityValue.value = 6f;
                offenseValue.value = 9f;
                controlEffectValue.value = 5f;
                difficultyValue.value = 4f;
                break;

            case 7:
                characterSelectedImage.sprite = orionImage; characterName.text = "ORION";
                durabilityValue.value = 8f;
                offenseValue.value = 7f;
                controlEffectValue.value = 9f;
                difficultyValue.value = 5f;
                break;

            case 8:
                characterSelectedImage.sprite = ariaImage; characterName.text = "ARIA";
                durabilityValue.value = 10f;
                offenseValue.value = 10f;
                controlEffectValue.value = 10f;
                difficultyValue.value = 10f;
                break;
        }

        lobbyAudioSystem.PlayIntro(currentCharacterSelected);
    }

    public void SelectButton()
    {
        if (currentCharacterSelected != PlayerPrefs.GetInt("selectedMultiplayerPlayerCharacter"))
        {
            ShowMessage(3);
            ButtonIsSelected();
            SaveCurrentSelection();
        }
        else
        {
            if (selectedCharacter == false)
            {
                ShowMessage(1);
                ButtonIsNotSelected();
                SaveCurrentSelection();
            }

            if (selectedCharacter == true)
            {
                ShowMessage(2);
                ButtonIsNotSelected();
                ResetSelection();
            }
        }
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

    private void LoadDefault()
    {
        durabilityValue.value = 7f;
        offenseValue.value = 8f;
        controlEffectValue.value = 6f;
        difficultyValue.value = 4f;
        lobbyAudioSystem.PlayIntro(1);
    }

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

    #region Database Methods

    #region Verify if user exists in database

    public IEnumerator VerifyUser(string urlPHP, string newSelection, string requestedTable, string requestedCollumn, string desiredSearch)
    {
        /* It makes the query in MySQL using ($sql = "SELECT " . newSelection . " FROM " . requestedTable . " WHERE " . requestedCollumn . " = " . desiredSearch);
         * 
         * With VerifyData we can make any consult and to make MySQL to return a value to PHP to inform Unity by WWW class
         * 
         * In the case we start a query selecting ID in the table and return ID value if in this row contains the player name.
         * 
         * If it return a value is because player name exist in the database, but if returns NULL the player dont exist in the database, so
         * we register a new user automatically and try to make connection after to register a new user.
         * 
         * This query always will return the value we was set in newSelection, as we used ID in newSelection it returns the ID value, but
         * we can use any collumn of the table we to wish, just need to use desiredSearch to validate the returned value.
         * 
         * To validate the data the desiredSearch value need to match with the value inside requestedCollumn, if it to exist in requestedCollumn so
         * the value was validated and PHP is allowed to return the value in newSelection.
         * 
         * Just to remind, newSelection is a name of a collumn.
         * 
         * So the logic is: we want the value inside collumn ID (newSelection), but in the row of this collumn that have many collumns on it,
         * the row should contains the value of desiredSearch in the specific requestedCollumn.
         * 
         * To verify player so we want the ID, but in the row of ID need to contains player name in the collumn name, if to contains it so return to us the value in ID.
         * If not to contains it, register a new user and verify again to validate again with success.
        */


        WWWForm form = new WWWForm();
        form.AddField("desiredSelection", newSelection);
        form.AddField("currentTable", requestedTable);
        form.AddField("currentCollumn", requestedCollumn);
        form.AddField("newSearch", desiredSearch);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response from server was: " + responseFromServer);

            if (responseFromServer == "error002")
            {
                connectionText.text = error002;
                notRegistered = true;
            }

            if (responseFromServer != "error002")
            {
                // Server is returning ID from Database, lets use ID as session code to others players be able to connect with player. ID in database is using Auto Increment, so it dont generate repeated values, always unique values so we need it to diferentiate player from others. By this way we can make the others to listen and to send data to the right player. The other player have ID also, so unique values, so it makes easy to connect everybody.

                currentSession = responseFromServer;
                connectionText.text = success003;
                verifiedSucces = true;
            }
        }

        request.Dispose();
    }

    #endregion

    #region Register a new user

    public IEnumerator RegisterNewUser(string urlPHP, string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("currentName", name);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response from server was: " + responseFromServer);

            if (responseFromServer == "error003")
            {
                connectionText.text = error003;
            }

            if (responseFromServer == "success002")
            {
                connectionText.text = success002;
                registeredSucces = true;
            }
        }

        request.Dispose();
    }

    #endregion

    #region Connect user to server

    public IEnumerator ConnectToServer(string urlPHP, string name, string status)
    {
        WWWForm form = new WWWForm();
        form.AddField("currentName", name);
        form.AddField("newStatus", status);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response from server was: " + responseFromServer);

            if (responseFromServer == "error001")
            {
                connectionText.text = error001;
            }

            if (responseFromServer == "success001")
            {
                serverMessage.text = success001;
                connectedSucces = true;
            }
        }

        request.Dispose();
    }

    #endregion

    #region Update user info in database

    public IEnumerator UpdateUser(string urlPHP, string newValue, string desiredCollumn, string referenceCollumn, string validateCollumn)
    {
        // Lets use Update User to change values of a collumn to a specific player

        // UPDATE lobby SET status = 'offline' WHERE id = 1; - Example use of UpdateUser();

        // UPDATE table SET desiredCollumn = 'newValue' WHERE referenceCollumn = validateCollumn - Main structure of the query

        WWWForm form = new WWWForm();
        form.AddField("desiredCollumn", desiredCollumn);
        form.AddField("newValue", "'" + newValue + "'");
        form.AddField("referenceCollumn", referenceCollumn);
        form.AddField("validateCollumn", validateCollumn);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response from server was: " + responseFromServer);

            if (responseFromServer == "error005")
            {
                connectionText.text = error005;
            }

            if (responseFromServer == "success004")
            {

            }
        }

        request.Dispose();
    }

    #endregion

    #region Verify specific data

    public IEnumerator VerifyData(string urlPHP, string newSelection, string requestedTable, string requestedCollumn, string desiredSearch)
    {
        // Lets use Verify Data to request a specific data in database

        WWWForm form = new WWWForm();
        form.AddField("desiredSelection", newSelection);
        form.AddField("currentTable", requestedTable);
        form.AddField("currentCollumn", requestedCollumn);
        form.AddField("newSearch", desiredSearch);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response from server was: " + responseFromServer);

            if (responseFromServer == "error002")
            {
                connectionText.text = error004;
            }

            if (responseFromServer != "error002")
            {
            
            }
        }

        request.Dispose();
    }

    #endregion

    #region Request Data Methods

    public void RequestData(string desiredData, string collumnVerification, string dataValidation)
    {
        StartCoroutine(VerifyData(verifyUser, desiredData, "lobby", collumnVerification, "'" + dataValidation + "'"));
    }

    #endregion

    #endregion
}