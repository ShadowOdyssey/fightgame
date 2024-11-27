using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ServerSystem : MonoBehaviour
{
    [Header("Round Setup")]
    public RoundManager roundSystem;
    public OpponentMultiplayer playerMultiplayer;
    public OpponentMultiplayer enemyMultiplayer;

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

    [Header("Listener Setup")]
    public float receiveDelay = 0.2f;

    [Header("Lobby Data")]
    public int actualPlayer = 0;
    public int actualEnemy = 0;

    [Header("Monitor")]
    public float countListen = 0f;
    public bool isEnemyPlayer = false;
    public bool canListen = false;
    public bool wasDataLoadedPlayer = false;
    public bool wasDataLoadedEnemy = false;
    public bool isCheckingWin = false;
    public bool canApplyHit = false;

    public string responsePlayerFromServer = "";
    public string responseEnemyFromServer = "";
    public string playerForward = "";
    public string enemyForward = "";

    public string[] listenerInfoPlayer = new string[0];
    public string[] listenerInfoEnemy = new string[0];

    #region Load Components

    public void Awake()
    {
        roundSystem = GameObject.Find("RoundManager").GetComponent<RoundManager>();
    }

    #endregion

    public void Update()
    {
        #region Receiving Data from Server

        if (roundSystem.roundOver == false && canListen == true)
        {
            countListen = countListen + Time.deltaTime;

            if (countListen > receiveDelay)
            {
                StartCoroutine(ListenPlayer(listenUser, actualPlayer));
                StartCoroutine(ListenEnemy(listenUser, actualEnemy));
                countListen = 0f;
            }
        }

        if (wasDataLoadedPlayer == true)
        {
            if (playerForward != listenerInfoPlayer[0])
            {
                playerForward = listenerInfoPlayer[0];

                if (listenerInfoPlayer[0] == "yes")
                {
                    playerMultiplayer.RegisterForwardPlayer("yes");
                }
                else
                {
                    playerMultiplayer.RegisterForwardPlayer("no");
                }
            }

            wasDataLoadedPlayer = false;
        }

        if (wasDataLoadedEnemy == true)
        {
            if (enemyForward != listenerInfoEnemy[0])
            {
                enemyForward = listenerInfoEnemy[0];

                if (listenerInfoEnemy[0] == "yes")
                {
                    enemyMultiplayer.RegisterForwardEnemy("yes");
                }
                else
                {
                    enemyMultiplayer.RegisterForwardEnemy("no");
                }
            }

            wasDataLoadedEnemy = false;
        }

        #endregion
    }

    #region Database Operations

    public IEnumerator ListenPlayer(string urlPHP, int actualListener)
    {
        // UPDATE lobby SET status = 'offline' WHERE id = 1; - Example use of UpdateUser();

        WWWForm form = new WWWForm();
        form.AddField("actualListener", actualListener);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && responsePlayerFromServer != request.downloadHandler.text)
        {
            responsePlayerFromServer = request.downloadHandler.text;
            listenerInfoPlayer = responsePlayerFromServer.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            wasDataLoadedPlayer = true;
        }

        request.Dispose();
    }

    public IEnumerator ListenEnemy(string urlPHP, int actualListener)
    {
        // UPDATE lobby SET status = 'offline' WHERE id = 1; - Example use of UpdateUser();

        WWWForm form = new WWWForm();
        form.AddField("actualListener", actualListener);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && responseEnemyFromServer != request.downloadHandler.text)
        {
            responseEnemyFromServer = request.downloadHandler.text;
            listenerInfoEnemy = responseEnemyFromServer.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            wasDataLoadedEnemy = true;
        }

        request.Dispose();
    }

    #endregion

    #region Register actual players

    public void RegisterOpponentPlayer(OpponentMultiplayer newPlayer)
    {
        playerMultiplayer = newPlayer;
    }

    public void RegisterOpponentEnemy(OpponentMultiplayer newEnemy)
    {
        enemyMultiplayer = newEnemy;
    }

    public void RegisterHost(int newHost)
    {
        actualPlayer = newHost;
    }

    public void RegisterDuel(int newDuel)
    {
        actualEnemy = newDuel;
        canListen = true;
    }

    #endregion
}
