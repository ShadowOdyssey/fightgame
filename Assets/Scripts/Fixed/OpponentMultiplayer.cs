using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class OpponentMultiplayer : MonoBehaviour
{
    [Header("Database Setup")]
    [Tooltip("Put the URL of the PHP file Verify User in the host server")]
    public string verifyUser = "https://queensheartgames.com/shadowodyssey/verifyuser.php";
    [Tooltip("Put the URL of the PHP file Dueling User in the host server")]
    public string duelingUser = "https://queensheartgames.com/shadowodyssey/duelinguser.php";
    [Tooltip("Put the URL of the PHP file Update User in the host server")]
    public string updateUser = "https://queensheartgames.com/shadowodyssey/updateuser.php";
    [Tooltip("Put the URL of the PHP file Log Off Player in the host server")]
    public string logOffPlayer = "https://queensheartgames.com/shadowodyssey/logoffplayer.php";
    [Tooltip("Put the URL of the PHP file Listen User in the host server")]
    public string listenUser = "https://queensheartgames.com/shadowodyssey/listenuser.php";

    [Header("Script Setup")]
    public RoundManager roundSystem;
    public PlayerSystem opponentIsPlayer;
    public EnemySystem opponentIsEnemy;

    [Header("Lobby Data")]
    public int actualHost = 0;
    public int actualListener = 0;

    [Header("Listener Setup")]
    private string[] listenerInfo = new string[0];
    public float listenerTimer = 0.3f;
    public float countListen = 0f;
    public bool isEnemyPlayer = false;
    public bool canListen = false;
    public bool wasDataLoaded = false;
    public bool isCheckingWin = false;
    private string responseFromServer = "";
    private string listenerForward = "";
    private string listenerBackward = "";
    private string listenerAttack1 = "";
    private string listenerAttack2 = "";
    private string listenerAttack3 = "";
    private string senderForward = "";
    private string senderBackward = "";
    private string senderAttack1 = "";
    private string senderAttack2 = "";
    private string senderAttack3 = "";
    private string listenerHit = "";
    private string checkWin = "";

    public void Awake()
    {
        roundSystem = GameObject.Find("RoundManager").GetComponent<RoundManager>();
    }

    public void Update()
    {
        ListenOpponent();
    }

    public void LateUpdate()
    {
        CheckRoundStartToListen();
        CheckRoundOverToStopListen();
    }

    #region Database Operations

    public IEnumerator ListenUser(string urlPHP, int actualListener)
    {
        // UPDATE lobby SET status = 'offline' WHERE id = 1; - Example use of UpdateUser();

        WWWForm form = new WWWForm();
        form.AddField("actualListener", actualListener);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && responseFromServer != request.downloadHandler.text)
        {
            responseFromServer = request.downloadHandler.text;

            listenerInfo = responseFromServer.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (listenerForward != listenerInfo[0])
            {
                listenerForward = listenerInfo[0];
            }
            
            if (listenerBackward != listenerInfo[1])
            {
                listenerBackward = listenerInfo[1];
            }

            if (listenerAttack1 != listenerInfo[2])
            {
                listenerAttack1 = listenerInfo[2];
            }

            if (listenerAttack2 != listenerInfo[3])
            {
                listenerAttack2 = listenerInfo[3];
            }

            if (listenerAttack3 != listenerInfo[4])
            {
                listenerAttack3 = listenerInfo[4];
            }

            if (listenerHit != listenerInfo[5])
            {
                listenerHit = listenerInfo[5];
            }

            RegisterAllData();

            wasDataLoaded = false;
            canListen = false;
        }

        request.Dispose();
    }

    public IEnumerator VerifyUser(string urlPHP, string desiredCollumn, string requestedTable, string requestedCollumn, string desiredSearch)
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

    public IEnumerator RegisterDuel(string urlPHP, int actualPlayer)
    {
        //UPDATE lobby SET profile='0', status = 'queue', duel='0', host='0' WHERE id = ?

        WWWForm form = new WWWForm();
        form.AddField("validateRequest", actualPlayer);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            request.Dispose();
            yield break; // Close Coroutine
        }
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

        if (request.result == UnityWebRequest.Result.Success)
        {
            request.Dispose();
            yield break; // Close Coroutine
        }
    }

    public void UpdateData(string newValue, string desiredCollumn, string validateRequest)
    {
        if (gameObject.activeInHierarchy == true)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateUser(updateUser, newValue, desiredCollumn, validateRequest));
        }
    }

    #endregion

    #region Apply loaded data from Lobby

    public void SetHost(int newHost, int newListener)
    {
        if (gameObject.activeInHierarchy == true)
        {
            actualHost = newHost;
            actualListener = newListener;
            StartCoroutine(RegisterDuel(duelingUser, actualHost));
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

    #region Listening Methods

    private void ListenOpponent()
    {
        if (canListen == false)
        {
            countListen = countListen + Time.deltaTime;

            if (countListen > listenerTimer)
            {
                canListen = true;
                countListen = 0;
            }
        }

        if (canListen == true && wasDataLoaded == false)
        {
            StartCoroutine(ListenUser(listenUser, actualListener));
            wasDataLoaded = true;
        }
    }

    private void CheckRoundStartToListen()
    {
        if (roundSystem.isMultiplayer == true && roundSystem.roundStarted == true && roundSystem.roundOver == false && canListen == false)
        {
            Debug.Log("Started to listen");

            canListen = true;
        }
    }

    private void CheckRoundOverToStopListen()
    {
        if (roundSystem.isMultiplayer == true && roundSystem.roundOver == true && canListen == true)
        {
            Debug.Log("Stopped to listen");

            canListen = false;
        }
    }

    #endregion

    #region Round is over

    public void RegisterVictory()
    {
        if (isCheckingWin == false && gameObject.activeInHierarchy == true)
        {
            isCheckingWin = true;
            StartCoroutine(VerifyUser(verifyUser, "wins", "lobby", "id", actualHost.ToString()));
        }
    }

    private void UpdateWins()
    {
        if (gameObject.activeInHierarchy == true)
        {
            int newWin = int.Parse(checkWin);
            newWin = newWin + 1;
            checkWin = newWin.ToString();
            UpdateData(checkWin, "wins", actualHost.ToString());
        }
    }

    #endregion

    #region Synchronize Now

    private void RegisterAllData()
    {
        if (isEnemyPlayer == true)
        {
            RegisterForwardPlayer();
            RegisterBackwardPlayer();
            RegisterAttack1Player();
            RegisterAttack2Player();
            RegisterAttack3Player();
        }
        else
        {
            RegisterForwardEnemy();
            RegisterBackwardEnemy();
            RegisterAttack1Enemy();
            RegisterAttack2Enemy();
            RegisterAttack3Enemy();
        }
    }

    #endregion

    #region Data Sent

    public void SendStop()
    {

    }

    public void SendForward()
    {

    }

    public void SendBackward()
    {

    }

    public void SendAttack1()
    {

    }

    public void SendAttack2()
    {

    }

    public void SendAttack3()
    {

    }

    #endregion

    #region Data Received

    #region Enemy is Player, register new data

    public void RegisterForwardPlayer()
    {
        if (listenerForward == "yes")
        {
            opponentIsPlayer.MultiplayerMovesForward();
        }

        if (listenerForward == "no")
        {
            opponentIsPlayer.MultiplayerStopForward();
        }
    }

    public void RegisterBackwardPlayer()
    {
        if (listenerBackward == "yes")
        {
            opponentIsPlayer.MultiplayerMovesBackward();
        }

        if (listenerBackward == "no")
        {
            opponentIsPlayer.MultiplayerStopBackward();
        }
    }

    public void RegisterAttack1Player()
    {
        if (listenerAttack1 == "yes")
        {
            opponentIsPlayer.MultiplayerAttacked1();
        }
    }

    public void RegisterAttack2Player()
    {
        if (listenerAttack2 == "yes")
        {
            opponentIsPlayer.MultiplayerAttacked2();
        }
    }

    public void RegisterAttack3Player()
    {
        if (listenerAttack3 == "yes")
        {
            opponentIsPlayer.MultiplayerAttacked3();
        }
    }

    #endregion

    #region Enemy is Enemy, register new data

    public void RegisterForwardEnemy()
    {
        if (listenerForward == "yes")
        {
            opponentIsEnemy.MultiplayerMovesForward();
        }

        if (listenerForward == "no")
        {
            opponentIsEnemy.MultiplayerStopForward();
        }
    }

    public void RegisterBackwardEnemy()
    {
        if (listenerBackward == "yes")
        {
            opponentIsEnemy.MultiplayerMovesBackward();
        }

        if (listenerBackward == "no")
        {
            opponentIsEnemy.MultiplayerStopBackward();
        }
    }

    public void RegisterAttack1Enemy()
    {
        if (listenerAttack1 == "yes")
        {
            opponentIsEnemy.MultiplayerAttacked1();
        }
    }

    public void RegisterAttack2Enemy()
    {
        if (listenerAttack2 == "yes")
        {
            opponentIsEnemy.MultiplayerAttacked2();
        }
    }

    public void RegisterAttack3Enemy()
    {
        if (listenerAttack3 == "yes")
        {
            opponentIsEnemy.MultiplayerAttacked3();
        }
    }

    #endregion

    #endregion
}
