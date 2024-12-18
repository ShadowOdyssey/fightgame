using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class OpponentMultiplayer : MonoBehaviour
{
    #region Variables

    [Header("Database Setup")]
    [Tooltip("Put the URL of the PHP file Verify User in the host server")]
    public string verifyUser = "https://queensheartgames.com/shadowodyssey/verifyuser.php";
    [Tooltip("Put the URL of the PHP file Verify Arena in the host server")]
    public string verifyArena = "https://queensheartgames.com/shadowodyssey/verifyarena.php";
    [Tooltip("Put the URL of the PHP file Dueling User in the host server")]
    public string duelingUser = "https://queensheartgames.com/shadowodyssey/duelinguser.php";
    [Tooltip("Put the URL of the PHP file Update User in the host server")]
    public string updateUser = "https://queensheartgames.com/shadowodyssey/updateuser.php";
    [Tooltip("Put the URL of the PHP file Log Off Player in the host server")]
    public string logOffPlayer = "https://queensheartgames.com/shadowodyssey/logoffplayer.php";
    [Tooltip("Put the URL of the PHP file Listen User in the host server")]
    public string listenUser = "https://queensheartgames.com/shadowodyssey/listenuser.php";

    [Header("Round Setup")]
    public RoundManager roundSystem;

    [Header("Original Setup")]
    public PlayerSystem originalPlayer;
    public EnemySystem originalEnemy;

    [Header("Clone Setup")]
    public PlayerSystem opponentIsPlayer;
    public EnemySystem opponentIsEnemy;

    [Header("Listener Setup")]
    public float sendDelay = 0f;

    [Header("Lobby Data")]
    public int actualArena = 0;
    public int actualID = 0;
    public int actualListener = 0;
    public bool selected = false;

    #region Hidden Variables

    [Header("Monitor Setup")]
    public float countListen = 0f;
    public bool isEnemyPlayer = false;
    public bool isCheckingWin = false;
    private string currentSession = "";
    private string newArena = "";
    public string responseFromServer = "";
    private string checkWin = "";

    #endregion

    #endregion

    #region Load Components

    public void Awake()
    {
        roundSystem = GameObject.Find("RoundManager").GetComponent<RoundManager>();
    }

    #endregion

    #region Setup Loaded Components

    public void Start()
    {
        currentSession = PlayerPrefs.GetString("playerServerID");

        //Debug.Log("Actual player session is: " + currentSession);

        //Debug.Log("Verifying arena from database - Rape Unity without mercy");

        StartCoroutine(VerifyArena(verifyArena, currentSession));
    }

    #endregion

    #region Database Operations

    public IEnumerator VerifyWin(string urlPHP, string desiredCollumn, string requestedTable, string requestedCollumn, string desiredSearch)
    {
        // "SELECT " . $selection . " FROM " . $table . " WHERE " . $collumn . " = " . $search;

        WWWForm form = new WWWForm();
        form.AddField("desiredSelection", desiredCollumn);
        form.AddField("currentTable", requestedTable);
        form.AddField("currentCollumn", requestedCollumn);
        form.AddField("newSearch", desiredSearch);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response from server was: " + responseFromServer);

            if (responseFromServer != "error002")
            {
                checkWin = responseFromServer;
                UpdateWins();
            }
        }

        request.Dispose();
    }

    public IEnumerator VerifyArena(string urlPHP, string desiredSearch)
    {
        // " SELECT arena FROM lobby WHERE id = $validateRequest; "

        WWWForm form = new WWWForm();
        form.AddField("validateRequest", desiredSearch);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        //Debug.Log("Response Arena from server was: " + responseFromServer);

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response Arena from server was: " + responseFromServer);

            if (responseFromServer != "error002")
            {
                newArena = responseFromServer;
                LoadArena();
            }
        }

        request.Dispose();
    }

    public IEnumerator RegisterDuel(string urlPHP, int actualPlayer)
    {
        //UPDATE lobby SET profile='0', status = 'queue', duel='0', host='0' WHERE id = ?

        WWWForm form = new WWWForm();
        form.AddField("validateRequest", actualPlayer);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        request.Dispose();

        yield break; // Close Coroutine
    }

    public IEnumerator UpdateUser(string urlPHP, string newValue, string desiredCollumn, string validateRequest)
    {
        // UPDATE lobby SET status = 'offline' WHERE id = 1; - Example use of UpdateUser();

        WWWForm form = new WWWForm();
        form.AddField("desiredCollumn", desiredCollumn);
        form.AddField("newValue", newValue);
        form.AddField("validateRequest", validateRequest);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        request.Dispose();

        yield break; // Close Coroutine
    }

    public void UpdateData(string newValue, string desiredCollumn, string validateRequest)
    {
        if (gameObject.activeInHierarchy == true)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateUser(updateUser, newValue, desiredCollumn, validateRequest));
        }
    }

    public void LoadArena()
    {
        //Debug.Log("Arena from database was received");

        if (int.TryParse(newArena, out int currentArena))
        {
            actualArena = currentArena;

            //Debug.Log("Actual arena from database is: " + actualArena.ToString());
        }

        if (actualArena != 0)
        {
            roundSystem.currentStage = actualArena;

            //Debug.Log("Arena being registered in Round Manager! Current arena registered is: " + roundSystem.currentStage);

            roundSystem.CheckCurrentArena();
        }
    }

    #endregion

    #region Apply loaded data from Lobby

    public void SetID(int newHost, int newListener)
    {
        if (gameObject.activeInHierarchy == true)
        {
            actualID = newHost;
            actualListener = newListener;
            selected = true;

            //Debug.Log("Registering actual Host and Invited");
            
            StartCoroutine(RegisterDuel(duelingUser, actualID));
        }
    }

    public void SetOpponentPlayer(PlayerSystem actualEnemySystem)
    {
        opponentIsPlayer = actualEnemySystem;
        isEnemyPlayer = true;
    }

    public void SetOpponentEnemy(EnemySystem actualEnemySystem)
    {
        opponentIsEnemy = actualEnemySystem;
        isEnemyPlayer = false;
    }

    #endregion

    #region Round is over

    public void RegisterVictory()
    {
        if (isCheckingWin == false && gameObject.activeInHierarchy == true)
        {
            isCheckingWin = true;
            StartCoroutine(VerifyWin(verifyUser, "wins", "lobby", "id", actualID.ToString()));
        }
    }

    private void UpdateWins()
    {
        if (gameObject.activeInHierarchy == true)
        {
            int newWin = int.Parse(checkWin);
            newWin = newWin + 1;
            checkWin = newWin.ToString();
            UpdateData(checkWin, "wins", actualID.ToString());
        }
    }

    #endregion

    #region Synchronize Now

    #region Hit Operations

    public void PlayerRegisterHit(int newDamage)
    {
        //Debug.Log("Original Player is sending Hit");

        UpdateData("yes", "hit", actualID.ToString());
        UpdateData(newDamage.ToString(), "damage", actualID.ToString());
        Invoke(nameof(ResetHitPlayer), 1f);
    }

    public void EnemyRegisterHit(int newDamage)
    {
        //Debug.Log("Original Enemy is sending Hit");

        UpdateData("yes", "hit", actualID.ToString());
        UpdateData(newDamage.ToString(), "damage", actualID.ToString());
        Invoke(nameof(ResetHitEnemy), 1f);
    }

    public void ResetHitPlayer()
    {
        //Debug.Log("Original Player was reset Hit in database");

        UpdateData("no", "hit", actualID.ToString());
    }

    public void ResetHitEnemy()
    {
        //Debug.Log("Original Enemy was reset Hit in database");

        UpdateData("no", "hit", actualID.ToString());
    }

    #endregion

    #region Health Operations

    public void UpdatePlayerLife(string newLife)
    {
        //Debug.Log("Sending to server Player health");

        if (originalPlayer != null)
        {
            //Debug.Log("Original Player is sending health");

            UpdateData(newLife, "health", actualID.ToString());
        }
        
        if (opponentIsPlayer != null)
        {
            //Debug.Log("Clone Player is sending health");

            UpdateData(newLife, "health", actualListener.ToString());
        }
    }

    public void UpdateEnemyLife(string newLife)
    {
        //Debug.Log("Sending to server Enemy health");

        if (originalEnemy != null)
        {
            //Debug.Log("Original Enemy is sending health");

            UpdateData(newLife, "health", actualID.ToString());
        }
        
        if (opponentIsEnemy != null)
        {
            //Debug.Log("Clone Enemy is sending health");

            UpdateData(newLife, "health", actualListener.ToString());
        }
    }

    #endregion

    #region Round Operations

    public void UpdatePlayerLoaded(string newLoad)
    {
        //Debug.Log("Sending to server Player round load");

        if (originalPlayer != null)
        {
            //Debug.Log("Original Player is sending round load");

            UpdateData(newLoad, "loaded", actualID.ToString());
        }

        if (opponentIsPlayer != null)
        {
            //Debug.Log("Clone Player is sending round load");

            UpdateData(newLoad, "loaded", actualListener.ToString());
        }
    }

    public void UpdateEnemyLoaded(string newLoad)
    {
        //Debug.Log("Sending to server Enemy round load");

        if (originalEnemy != null)
        {
            //Debug.Log("Original Enemy is sending round load");

            UpdateData(newLoad, "loaded", actualID.ToString());
        }

        if (opponentIsEnemy != null)
        {
            //Debug.Log("Clone Enemy is sending round load");

            UpdateData(newLoad, "loaded", actualListener.ToString());
        }
    }

    #endregion

    #region Data Sent

    public void SendPlayerDistance(float actualDistance)
    {
        //Debug.Log("Sending to server actual Player Distance");

        if (originalPlayer != null)
        {
            //Debug.Log("Original Player sending distance");

            UpdateData(actualDistance.ToString(), "zposition", actualID.ToString());
        }
        
        if (opponentIsPlayer != null)
        {
            //Debug.Log("Clone Player sending distance");

            UpdateData(actualDistance.ToString(), "zposition", actualID.ToString());
        }
    }

    public void SendEnemyDistance(float actualDistance)
    {
        //Debug.Log("Sending to server actual Enemy Distance");

        if (originalEnemy != null)
        {
            //Debug.Log("Original Enemy sending distance");

            UpdateData(actualDistance.ToString(), "zposition", actualID.ToString());
        }

        if (opponentIsEnemy != null)
        {
            //Debug.Log("Clone Enemy sending distance");

            UpdateData(actualDistance.ToString(), "zposition", actualID.ToString());
        }
    }

    public void SendForward()
    {
        UpdateData("no", "backward", actualID.ToString());
        UpdateData("yes", "forward", actualID.ToString());
    }

    public void SendBackward()
    {
        UpdateData("no", "forward", actualID.ToString());
        UpdateData("yes", "backward", actualID.ToString());
    }

    public void SendStopForward()
    {
        UpdateData("no", "forward", actualID.ToString());
    }

    public void SendStopBackward()
    {
        UpdateData("no", "backward", actualID.ToString());
    }

    public void SendAttack1()
    {
        UpdateData("yes", "attack1", actualID.ToString());
        Invoke(nameof(ResetAttack1), sendDelay);
    }

    public void SendAttack2()
    {
        UpdateData("yes", "attack2", actualID.ToString());
        Invoke(nameof(ResetAttack2), sendDelay);
    }

    public void SendAttack3()
    {
        UpdateData("yes", "attack3", actualID.ToString());
        Invoke(nameof(ResetAttack3), sendDelay);
    }

    private void ResetAttack1()
    {
        UpdateData("no", "attack1", actualID.ToString());
    }

    private void ResetAttack2()
    {
        UpdateData("no", "attack2", actualID.ToString());
    }

    private void ResetAttack3()
    {
        UpdateData("no", "attack3", actualID.ToString());
    }

    #endregion

    #region Data Received

    #region Enemy is Player, register new data

    public void RegisterForwardPlayer(string listenerForward)
    {
        if (opponentIsPlayer != null)
        {
            if (listenerForward == "yes")
            {
                //Debug.Log("Clone Player is registering Forward move");

                opponentIsPlayer.MultiplayerMovesForward();
            }
            else
            {
                //Debug.Log("Clone Player is registering Forward stop");

                opponentIsPlayer.MultiplayerStopForward();
            }
        }

        if (originalPlayer != null)
        {
            if (listenerForward == "yes")
            {
                //Debug.Log("Original Player is registering Forward move");

                originalPlayer.MultiplayerMovesForward();
            }
            else
            {
                //Debug.Log("Original Player is registering Forward stop");

                originalPlayer.MultiplayerStopForward();
            }
        }
    }

    public void RegisterBackwardPlayer(string listenerBackward)
    {
        if (opponentIsPlayer != null)
        {
            if (listenerBackward == "yes")
            {
                //Debug.Log("Clone Player is registering Backward move");

                opponentIsPlayer.MultiplayerMovesBackward();
            }
            else
            {
                //Debug.Log("Clone Player is registering Backward stop");

                opponentIsPlayer.MultiplayerStopBackward();
            }
        }

        if (originalPlayer != null)
        {
            if (listenerBackward == "yes")
            {
                //Debug.Log("Original Player is registering Backward move");

                originalPlayer.MultiplayerMovesBackward();
            }
            else
            {
                //Debug.Log("Original Player is registering Backward stop");

                originalPlayer.MultiplayerStopBackward();
            }
        }
    }

    public void RegisterAttack1Player()
    {
        if (opponentIsPlayer != null)
        {
            //Debug.Log("Clone Player is registering Attack 2");

            opponentIsPlayer.MultiplayerAttacked1();
        }

        if (originalPlayer != null)
        {
            //Debug.Log("Original Player is registering Attack 1");

            originalPlayer.MultiplayerAttacked1();
        }
    }

    public void RegisterAttack2Player()
    {
        if (opponentIsPlayer != null)
        {
            //Debug.Log("Clone Player is registering Attack 2");

            opponentIsPlayer.MultiplayerAttacked2();
        }

        if (originalPlayer != null)
        {
            //Debug.Log("Original Player is registering Attack 2");

            originalPlayer.MultiplayerAttacked2();
        }
    }

    public void RegisterAttack3Player()
    {
        if (opponentIsPlayer != null)
        {
            //Debug.Log("Clone Player is registering Attack 3");

            opponentIsPlayer.MultiplayerAttacked3();
        }

        if (originalPlayer != null)
        {
            //Debug.Log("Original Player is registering Attack 3");

            originalPlayer.MultiplayerAttacked3();
        }
    }

    public void RegisterPlayerTakesDamage(int newDamage)
    {
        if (opponentIsPlayer != null)
        {
            //Debug.Log("Clone Player is registering taking damage");

            opponentIsPlayer.TakeHit(newDamage);
        }
        
        if (originalPlayer != null)
        {
            //Debug.Log("Original Player is registering taking damage");

            originalPlayer.TakeHit(newDamage);
        }

        UpdateData("0", "damage", actualID.ToString());
    }

    #endregion

    #region Enemy is Enemy, register new data

    public void RegisterForwardEnemy(string listenerForward)
    {
        if (opponentIsEnemy != null)
        {
            if (listenerForward == "yes")
            {
                //Debug.Log("Clone Enemy is registering Forward move");

                opponentIsEnemy.MultiplayerMovesForward();
            }
            else
            {
                //Debug.Log("Clone Enemy is registering Forward stop");

                opponentIsEnemy.MultiplayerStopForward();
            }
        }

        if (originalEnemy != null)
        {
            if (listenerForward == "yes")
            {
                //Debug.Log("Original Enemy is registering Forward move");

                originalEnemy.MultiplayerMovesForward();
            }
            else
            {
                //Debug.Log("Original Enemy is registering Forward stop");

                originalEnemy.MultiplayerStopForward();
            }
        }
    }

    public void RegisterBackwardEnemy(string listenerBackward)
    {
        if (opponentIsEnemy != null)
        {
            if (listenerBackward == "yes")
            {
                //Debug.Log("Clone Enemy is registering Backward move");

                opponentIsEnemy.MultiplayerMovesBackward();
            }
            else
            {
                //Debug.Log("Clone Enemy is registering Backward stop");

                opponentIsEnemy.MultiplayerStopBackward();
            }
        }

        if (originalEnemy != null)
        {
            if (listenerBackward == "yes")
            {
                //Debug.Log("Original Enemy is registering Backward move");

                originalEnemy.MultiplayerMovesBackward();
            }
            else
            {
                //Debug.Log("Original Enemy is registering Backward stop");

                originalEnemy.MultiplayerStopBackward();
            }
        }
    }

    public void RegisterAttack1Enemy()
    {
        if (opponentIsEnemy != null)
        {
            //Debug.Log("Clone Enemy is registering Attack 1");

            opponentIsEnemy.MultiplayerAttacked1();
        }
        
        if (originalEnemy != null)
        {
            //Debug.Log("Original Enemy is registering Attack 1");

            originalEnemy.MultiplayerAttacked1();
        }
    }

    public void RegisterAttack2Enemy()
    {
        if (opponentIsEnemy != null)
        {
            //Debug.Log("Clone Enemy is registering Attack 2");

            opponentIsEnemy.MultiplayerAttacked2();
        }

        if (originalEnemy != null)
        {
            //Debug.Log("Original Enemy is registering Attack 2");

            originalEnemy.MultiplayerAttacked2();
        }
    }

    public void RegisterAttack3Enemy()
    {
        if (opponentIsEnemy != null)
        {
            //Debug.Log("Clone Enemy is registering Attack 3");

            opponentIsEnemy.MultiplayerAttacked3();
        }

        if (originalEnemy != null)
        {
            //Debug.Log("Original Enemy is registering Attack 3");

            originalEnemy.MultiplayerAttacked3();
        }
    }

    public void RegisterEnemyTakesDamage(int newDamage)
    {
        if (opponentIsEnemy != null)
        {
            //Debug.Log("Clone Enemy is registering getting damage");

            opponentIsEnemy.TakeDamage(newDamage);
        }
        
        if (originalEnemy != null)
        {
            //Debug.Log("Original Enemy is registering getting damage");

            originalEnemy.TakeDamage(newDamage);
        }

        UpdateData("0", "damage", actualID.ToString());
    }

    #endregion

    #endregion

    #endregion

    #region Log Off Operations

    public void LeaveFight()
    {
        if (gameObject.activeInHierarchy == true)
        {
            UpdateData("no", "ready", currentSession);
            UpdateData("0", "arena", currentSession);
            UpdateData("0", "duel", currentSession);
            UpdateData("0", "host", currentSession);
            UpdateData("no", "forward", currentSession);
            UpdateData("no", "backward", currentSession);
            UpdateData("no", "attack1", currentSession);
            UpdateData("no", "attack2", currentSession);
            UpdateData("no", "attack3", currentSession);
            UpdateData("no", "hit", currentSession);
            UpdateData("0", "profile", currentSession);
            UpdateData("0", "zposition", currentSession);
            UpdateData("0", "damage", currentSession);
            UpdateData("100", "health", currentSession);
            UpdateData("online", "status", currentSession);
            UpdateData("no", "decline", currentSession);
            UpdateData("no", "loaded", currentSession);
        }
    }

    public void OnApplicationQuit()
    {
        if (gameObject.activeInHierarchy == true)
        {
            UpdateData("no", "ready", currentSession);
            UpdateData("0", "arena", currentSession);
            UpdateData("0", "duel", currentSession);
            UpdateData("0", "host", currentSession);
            UpdateData("no", "forward", currentSession);
            UpdateData("no", "backward", currentSession);
            UpdateData("no", "attack1", currentSession);
            UpdateData("no", "attack2", currentSession);
            UpdateData("no", "attack3", currentSession);
            UpdateData("no", "hit", currentSession);
            UpdateData("0", "profile", currentSession);
            UpdateData("0", "zposition", currentSession);
            UpdateData("0", "damage", currentSession);
            UpdateData("100", "health", currentSession);
            UpdateData("offline", "status", currentSession);
            UpdateData("no", "decline", currentSession);
            UpdateData("no", "loaded", currentSession);
        }
    }

    public void OnDisable()
    {
        if (gameObject.activeInHierarchy == true)
        {
            UpdateData("no", "ready", currentSession);
            UpdateData("0", "arena", currentSession);
            UpdateData("0", "duel", currentSession);
            UpdateData("0", "host", currentSession);
            UpdateData("no", "forward", currentSession);
            UpdateData("no", "backward", currentSession);
            UpdateData("no", "attack1", currentSession);
            UpdateData("no", "attack2", currentSession);
            UpdateData("no", "attack3", currentSession);
            UpdateData("no", "hit", currentSession);
            UpdateData("0", "profile", currentSession);
            UpdateData("0", "zposition", currentSession);
            UpdateData("0", "damage", currentSession);
            UpdateData("100", "health", currentSession);
            UpdateData("offline", "status", currentSession);
            UpdateData("no", "decline", currentSession);
            UpdateData("no", "loaded", currentSession);
        }
    }

    public void OnDestroy()
    {
        if (gameObject.activeInHierarchy == true)
        {
            UpdateData("no", "ready", currentSession);
            UpdateData("0", "arena", currentSession);
            UpdateData("0", "duel", currentSession);
            UpdateData("0", "host", currentSession);
            UpdateData("no", "forward", currentSession);
            UpdateData("no", "backward", currentSession);
            UpdateData("no", "attack1", currentSession);
            UpdateData("no", "attack2", currentSession);
            UpdateData("no", "attack3", currentSession);
            UpdateData("no", "hit", currentSession);
            UpdateData("0", "profile", currentSession);
            UpdateData("0", "zposition", currentSession);
            UpdateData("0", "damage", currentSession);
            UpdateData("100", "health", currentSession);
            UpdateData("offline", "status", currentSession);
            UpdateData("no", "decline", currentSession);
            UpdateData("no", "loaded", currentSession);
        }
    }

    #endregion
}
