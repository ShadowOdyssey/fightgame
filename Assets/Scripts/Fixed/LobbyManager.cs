using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    /* *** REMEMBER *** 
     * 
     * METHODS TO FINISH:
     * 
     * VERIFY DATA
     * FIGHT
     * INPUT SEND/RECEIVE
     * 
     * */

    #region Variables

    #region Components Setup

    [Header("Lobby Setup")]
    [Tooltip("Attach here Audio System object in the hierarchy")]
    public LobbyAudioSystem lobbyAudioSystem;
    [Tooltip("Attach here Duel object inside UI object in the hierarchy")]
    public DuelSystem duelSystem;
    [Tooltip("Prefab used to instantiate players in the Players On Lobby list")]
    public GameObject playerLobbyPrefab;
    [Tooltip("Attach here Connecting object inside UI object in the hierarchy")]
    public GameObject connectingScreen;
    [Tooltip("Attach here Duel object inside UI object in the hierarchy")]
    public GameObject duelScreen;
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
    [Tooltip("Attach here Ready Text object inside Ready button that is inside Lobby object that is inside UI object in the hierarchy")]
    public TextMeshProUGUI readyButtonText;

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

    #region PHP URLs

    [Header("Database Setup")]
    [Tooltip("Put the URL of the PHP file Verify User in the host server")]
    public string verifyUser = "https://queensheartgames.com/shadowodyssey/verifyuser.php";
    [Tooltip("Put the URL of the PHP file Connect User in the host server")]
    public string connectUser = "https://queensheartgames.com/shadowodyssey/connectuser.php";
    [Tooltip("Put the URL of the PHP file Register User in the host server")]
    public string registerUser = "https://queensheartgames.com/shadowodyssey/registeruser.php";
    [Tooltip("Put the URL of the PHP file Update User in the host server")]
    public string updateUser = "https://queensheartgames.com/shadowodyssey/updateuser.php";
    [Tooltip("Put the URL of the PHP file Update Player List in the host server")]
    public string updatePlayerList = "https://queensheartgames.com/shadowodyssey/updateplayerlist.php";
    [Tooltip("Put the URL of the PHP file Verify Offline in the host server")]
    public string verifyOffline = "https://queensheartgames.com/shadowodyssey/verifyoffline.php";
    [Tooltip("Put the URL of the PHP file Log Off Player in the host server")]
    public string logOffPlayer = "https://queensheartgames.com/shadowodyssey/logoffplayer.php";

    #endregion

    #region Success Response From Database

    [Header("Success Response Setup")]
    [Tooltip("Setup the message when a connection is a success with the server")]
    public string success001 = "Connection success! Connected with the server! Loading Lobby...";
    [Tooltip("Setup the message when to register a new user is a success in the database")]
    public string success002 = "New account registered with sucess! Connecting...";
    [Tooltip("Setup the message when a user verification if exists in database is a success")]
    public string success003 = "User found in database, connecting with server...";
    [Tooltip("Setup the message when a user data update in database is a success")]
    public string success004 = "User register in database changed!";
    [Tooltip("Setup the message when Player On Lobby list was loaded successfuly")]
    public string success005 = "Players On Lobby list refreshed with success!";
    [Tooltip("Setup the message when some player has logged off")]
    public string success006 = " has logged off!";

    #endregion

    #region Error Response From Database

    [Header("Error Response Setup")]
    [Tooltip("Setup the message when a connection with the server have failed")]
    public string error001 = "Connection to database failed! Please try again!";
    [Tooltip("Setup the message when a user verification if exists in database have failed")]
    public string error002 = "Requested user dont found in the database! Registering new user...";
    [Tooltip("Setup the message when to register a new user is a failure in the database")]
    public string error003 = "Was not possible to regirster a new user. Open a ticket to admin!";
    [Tooltip("Setup the message when a data verification if exists in database have failed")]
    public string error004 = "Requested data dont found in the database!";
    [Tooltip("Setup the message when a failed to change a user data in database")]
    public string error005 = "Was not possible to apply new changes in database!";
    [Tooltip("Setup the message when to update Players On Lobby list has failed to load")]
    public string error006 = "Was not possible to generate a new list, try again!";
    [Tooltip("Setup the message when to request Offline Players list has failed to load")]
    public string error007 = "Was not possible to find offline players, try again!";

    #endregion

    #endregion

    #region Player List Setup

    [Header("Player List Setup")]
    public Transform spawnPlayerArea;

    #endregion

    #region Image Setup

    [Header("Image Displayer")]
    public Image characterSelectedImage;

    [Header("Player List Setup")]
    public Sprite noImage;

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

    [Header("Parse Variables")]
    private string[] playersList = new string[0];
    private string[] playerInfo = new string[0];
    private string id = "";
    private string playerName = "";
    private string profile = "";
    private string wins = "";
    private string ready = "";
    private string status = "";

    [Header("Monitor")]
    private int currentCharacterSelected = 1;
    private float actualRefreshTime = 0f;
    private bool selectedCharacter = false;
    private bool verifiedSucces = false;
    private bool connectedSucces = false;
    private bool registeredSucces = false;
    private bool joinedLobby = false;
    private bool loadedLobby = false;
    private bool notRegistered = false;
    private bool isReady = false;
    private string actualName = "";
    private string currentSession = "";
    private string currentHost = "";
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
            StartCoroutine(VerifyUser(verifyUser, "id", "lobby", "name", "'" + actualName + "'")); // Everything is ready, so lets start to connect with the server automatically
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
            joinedLobby = false;
            RefreshList();
        }

        #endregion

        #region Refresh a new list of players

        if (loadedLobby == true)
        {
            actualRefreshTime = actualRefreshTime + Time.deltaTime;

            if (actualRefreshTime > timeToRefreshPlayerList)
            {
                actualRefreshTime = 0f;
                RefreshList();
            }
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
                ButtonIsSelected();
                SaveCurrentSelection();
            }

            if (selectedCharacter == true)
            {
                if (isReady == true)
                {
                    ReadyButton();
                }

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

    public void ReadyButton()
    {
        if (selectedCharacter == false)
        {
            ShowMessage(4);
            lobbyAudioSystem.PlaySelectHero();
        }
        else
        {
            if (isReady == false)
            {
                readyButtonText.color = Color.green;
                readyButtonText.text = "FIGHT!";
                isReady = true;
                UpdateData("yes", "ready", int.Parse(currentSession));
            }
            else
            {
                readyButtonText.color = Color.white;
                readyButtonText.text = "READY?";
                isReady = false;
                UpdateData("no", "ready", int.Parse(currentSession));
            }
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
        UpdateData(currentCharacterSelected.ToString(), "profile", int.Parse(currentSession));
    }

    private void ResetSelection()
    {
        selectedCharacter = false;
        PlayerPrefs.SetInt("selectedMultiplayerPlayerCharacter", 0);
        UpdateData(0.ToString(), "profile", int.Parse(currentSession));
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
                yield break; // Exit the coroutine if there's an error
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
                yield break; // Exit the coroutine if there's an error
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
                yield break; // Exit the coroutine if there's an error
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

    public IEnumerator UpdateUser(string urlPHP, string newValue, string desiredCollumn, int validateRequest)
    {
        // Lets use Update User to change values of a collumn to a specific player

        // UPDATE lobby SET status = 'offline' WHERE id = 1; - Example use of UpdateUser();

        // UPDATE table SET desiredCollumn = 'newValue' WHERE id = validateRequest - Main structure of the query

        WWWForm form = new WWWForm();
        form.AddField("desiredCollumn", desiredCollumn);

        if (desiredCollumn == "profile")
        {
            form.AddField("newValue", int.Parse(newValue));
        }
        else
        {
            form.AddField("newValue", newValue);
        }

        form.AddField("validateRequest", validateRequest);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response from server was: " + responseFromServer);

            if (responseFromServer == "error005")
            {
                connectionText.text = error005;
                yield break; // Exit the coroutine if there's an error
            }

            if (responseFromServer == "success004")
            {

            }
        }

        request.Dispose();
    }

    #endregion

    #region Update player list

    public IEnumerator UpdatePlayerList()
    {
        //SELECT * FROM lobby WHERE status = 'online';

        WWWForm form = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(updatePlayerList, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response from server was: " + responseFromServer);

            if (responseFromServer == "error006")
            {
                serverMessage.text = error006;
                loadedLobby = false;
                yield break; // Exit the coroutine if there's an error
            }

            if (responseFromServer != "error006")
            {
                playersList = responseFromServer.Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string playerString in playersList)
                {
                    playerInfo = playerString.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if (playerInfo[0] != currentSession)
                    {
                        id = playerInfo[0];
                        playerName = playerInfo[1];
                        profile = playerInfo[2];
                        wins = playerInfo[3];
                        ready = playerInfo[4];
                        status = playerInfo[5];

                        GameObject actualPlayer = GameObject.Find(id + playerName + "(Clone)");

                        if (actualPlayer == null)
                        {
                            playerLobbyPrefab.name = id + playerName;

                            Instantiate(playerLobbyPrefab, spawnPlayerArea);

                            GameObject newPlayer = GameObject.Find(id + playerName + "(Clone)");

                            newPlayer.GetComponent<LobbyPlayerSystem>().UpdateSession(int.Parse(id));
                            newPlayer.GetComponent<LobbyPlayerSystem>().UpdateProfile(int.Parse(profile));
                            newPlayer.GetComponent<LobbyPlayerSystem>().UpdateWins(int.Parse(wins));
                            newPlayer.GetComponent<LobbyPlayerSystem>().UpdateName(playerName);
                            newPlayer.GetComponent<LobbyPlayerSystem>().UpdateReady(ready);
                            newPlayer.GetComponent<LobbyPlayerSystem>().UpdateStatus(status);

                            playerLobbyPrefab.name = "LobbyPlayers";
                        }
                        else
                        {
                            actualPlayer.GetComponent<LobbyPlayerSystem>().UpdateSession(int.Parse(id));
                            actualPlayer.GetComponent<LobbyPlayerSystem>().UpdateProfile(int.Parse(profile));
                            actualPlayer.GetComponent<LobbyPlayerSystem>().UpdateWins(int.Parse(wins));
                            actualPlayer.GetComponent<LobbyPlayerSystem>().UpdateName(playerName);
                            actualPlayer.GetComponent<LobbyPlayerSystem>().UpdateReady(ready);
                            actualPlayer.GetComponent<LobbyPlayerSystem>().UpdateStatus(status);
                        }
                    }
                }

                playerInfo = new string[0];

                id = "";
                playerName = "";
                profile = "";
                wins = "";
                ready = "";
                status = "";

                if (loadedLobby == false)
                {
                    loadedLobby = true;
                }

                serverMessage.text = success005;
            }
        }

        request.Dispose();
    }

    #endregion

    #region Find offline players

    public IEnumerator FindOfflinePlayers()
    {
        //$sql = "SELECT id, name FROM lobby WHERE status = 'offline'";

        WWWForm form = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(verifyOffline, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response from server was: " + responseFromServer);

            if (responseFromServer == "error007")
            {
                serverMessage.text = error007;
                yield break; // Exit the coroutine if there's an error
            }

            if (responseFromServer != "error006")
            {
                playersList = responseFromServer.Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string playerString in playersList)
                {
                    playerInfo = playerString.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if (playerInfo[0] != currentSession)
                    {
                        id = playerInfo[0];
                        playerName = playerInfo[1];

                        GameObject actualPlayer = GameObject.Find(id + playerName + "(Clone)");

                        if (actualPlayer != null)
                        {
                            actualPlayer.GetComponent<LobbyPlayerSystem>().PlayerIsOffline();
                        }
                    }
                }
            }
        }

        request.Dispose();
    }

    #endregion

    #region Log off a player that left the lobby

    public IEnumerator LogOffPlayer(string urlPHP, int playerSession, string playerName)
    {
        WWWForm form = new WWWForm();
        form.AddField("validateRequest", playerSession);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response from server was: " + responseFromServer);

            if (responseFromServer == "success006")
            {
                serverMessage.text = "Player " + playerName + success006;
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
                yield break; // Exit the coroutine if there's an error
            }

            if (responseFromServer != "error002")
            {
            
            }
        }

        request.Dispose();
    }

    #endregion

    #region Player List Methods

    public void RefreshList()
    {
        StopAllCoroutines();
        StartCoroutine(FindOfflinePlayers());
        StartCoroutine(UpdatePlayerList());
    }

    #endregion

    #region Data Methods

    public void UpdateData(string newValue, string desiredCollumn, int validateRequest)
    {
        if (gameObject.activeInHierarchy == true)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateUser(updateUser, newValue, desiredCollumn, validateRequest));
        }
    }

    public void RequestData(string desiredData, string collumnVerification, string dataValidation)
    {
        if (gameObject.activeInHierarchy == true)
        {
            StopAllCoroutines();
            StartCoroutine(VerifyData(verifyUser, desiredData, "lobby", collumnVerification, "'" + dataValidation + "'"));
        }
    }

    #endregion

    #region Duel Methods

    public void UpdateDuelPlayer(string opponentName, int opponentSession, int opponentProfile)
    {
        duelScreen.SetActive(true);

        duelSystem.UpdateSessions(int.Parse(currentSession), opponentSession);
        duelSystem.UpdateNames(actualName, opponentName);
        duelSystem.LoadVersusImages(currentCharacterSelected, opponentProfile);

        if (currentHost == currentSession)
        {
            duelSystem.OpenDuel(1);
        }
        else 
        {
            duelSystem.OpenDuel(2);
        }
    }

    public void DuelAccepted(int playerDuel, int opponentDuel)
    {
        PlayerPrefs.SetInt("multiplayerPlayer", playerDuel);
        PlayerPrefs.SetInt("multiplayerOpponent", opponentDuel);
    }

    public void DuelDeclined(int playerDuel, int opponentDuel)
    {
        PlayerPrefs.SetInt("multiplayerPlayer", 0);
        PlayerPrefs.SetInt("multiplayerOpponent", 0);
    }

    #endregion

    #region Register Offline Methods

    public void LeaveLobby() // Used by Leave Lobby Button
    {
        loadedLobby = false;
        RemovePlayer(int.Parse(currentSession), actualName);
        UpdateData("offline", "status", int.Parse(currentSession));
        connectingScreen.SetActive(true);
        connectionText.text = "Leaving the lobby! Please wait...";
        Invoke(nameof(ReturnToMenu), 5f); // Delay MainMenu load to give enough time to register the Offline data in database before to leave Arcade Mode scene
    }

    public void RemovePlayer(int playerSession, string playerName)
    {
        //Debug.Log("Player " + playerName + " is offline!");
        StartCoroutine(LogOffPlayer(logOffPlayer, playerSession, playerName));
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnDestroy()
    {
        if (gameObject.activeInHierarchy == true)
        {
            loadedLobby = false;
            RemovePlayer(int.Parse(currentSession), actualName); // For some reason Arcade Mode scene was destroyed, so inform player is offline
        }
    }

    public void OnApplicationQuit()
    {
        if (gameObject.activeInHierarchy == true)
        {
            loadedLobby = false;
            RemovePlayer(int.Parse(currentSession), actualName); // Player closed the game, inform database that player is offline
        }
    }

    #endregion

    #endregion
}