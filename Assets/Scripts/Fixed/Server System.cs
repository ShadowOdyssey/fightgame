using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ServerSystem : MonoBehaviour
{
    #region Variables

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

    [Header("Loaded Data")]
    public string[] listenerInfoPlayer = new string[0];
    public string[] listenerInfoEnemy = new string[0];

    [Header("Last Data")]
    public string responsePlayerFromServer = "";
    public string responseEnemyFromServer = "";
    public string playerForward = "";
    public string enemyForward = "";
    public string playerBackward = "";
    public string enemyBackward = "";
    public string playerAttack1 = "";
    public string enemyAttack1 = "";
    public string playerAttack2 = "";
    public string enemyAttack2 = "";
    public string playerAttack3 = "";
    public string enemyAttack3 = "";
    public string playerHit = "";
    public string enemyHit = "";

    #endregion

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

                if (playerForward == "yes")
                {
                    playerMultiplayer.RegisterForwardPlayer("yes");
                }
                else
                {
                    playerMultiplayer.RegisterForwardPlayer("no");
                }
            }

            if (playerBackward != listenerInfoPlayer[1])
            {
                playerBackward = listenerInfoPlayer[1];

                if (playerBackward == "yes")
                {
                    playerMultiplayer.RegisterBackwardPlayer("yes");
                }
                else
                {
                    playerMultiplayer.RegisterBackwardPlayer("no");
                }
            }

            if (playerAttack1 != listenerInfoPlayer[2])
            {
                playerAttack1 = listenerInfoPlayer[2];

                if (playerAttack1 == "yes")
                {
                    playerMultiplayer.RegisterAttack1Player();
                }
            }

            if (playerAttack2 != listenerInfoPlayer[3])
            {
                playerAttack2 = listenerInfoPlayer[3];

                if (playerAttack2 == "yes")
                {
                    playerMultiplayer.RegisterAttack2Player();
                }
            }

            if (playerAttack3 != listenerInfoPlayer[4])
            {
                playerAttack3 = listenerInfoPlayer[4];

                if (playerAttack3 == "yes")
                {
                    playerMultiplayer.RegisterAttack3Player();
                }
            }

            if (playerHit != listenerInfoPlayer[5])
            {
                playerHit = listenerInfoPlayer[5];

                if (playerHit == "yes")
                {
                    //playerMultiplayer.RegisterAttack3Player();
                }
            }

            wasDataLoadedPlayer = false;
        }

        if (wasDataLoadedEnemy == true)
        {
            if (enemyForward != listenerInfoEnemy[0])
            {
                enemyForward = listenerInfoEnemy[0];

                if (enemyForward == "yes")
                {
                    enemyMultiplayer.RegisterForwardEnemy("yes");
                }
                else
                {
                    enemyMultiplayer.RegisterForwardEnemy("no");
                }
            }

            if (enemyBackward != listenerInfoEnemy[1])
            {
                enemyBackward = listenerInfoEnemy[1];

                if (enemyBackward == "yes")
                {
                    enemyMultiplayer.RegisterBackwardEnemy("yes");
                }
                else
                {
                    enemyMultiplayer.RegisterBackwardEnemy("no");
                }
            }

            if (enemyAttack1 != listenerInfoEnemy[2])
            {
                enemyAttack1 = listenerInfoEnemy[2];

                if (enemyAttack1 == "yes")
                {
                    enemyMultiplayer.RegisterAttack1Enemy();
                }
            }

            if (enemyAttack2 != listenerInfoEnemy[3])
            {
                enemyAttack2 = listenerInfoEnemy[3];

                if (enemyAttack2 == "yes")
                {
                    enemyMultiplayer.RegisterAttack2Enemy();
                }
            }

            if (enemyAttack3 != listenerInfoEnemy[4])
            {
                enemyAttack3 = listenerInfoEnemy[4];

                if (enemyAttack3 == "yes")
                {
                    enemyMultiplayer.RegisterAttack3Enemy();
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
