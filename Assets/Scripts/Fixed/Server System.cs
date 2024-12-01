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
    public int actualPlayerHealth = 100;
    public int actualEnemyHealth = 100;
    public int decimalIndexA1 = 0;
    public int decimalIndexA2 = 0;
    public int decimalIndexB1 = 0;
    public int decimalIndexB2 = 0;
    public int realDistanceA = 0;
    public int realDistanceA1 = 0;
    public int realDistanceA2 = 0;
    public int realDistanceB = 0;
    public int realDistanceB1 = 0;
    public int realDistanceB2 = 0;

    [Header("Monitor")]
    public float countListen = 0f;
    public bool isEnemyPlayer = false;
    public bool canListen = false;
    public bool wasDataLoadedPlayer = false;
    public bool wasDataLoadedEnemy = false;
    public bool wasPlayerDamaged = false;
    public bool wasEnemyDamaged = false;
    public bool canParseA = false;
    public bool canParseB = false;
    public bool isParsedA = false;
    public bool isParsedB = false;
    public bool canParseHealthA = false;
    public bool canParseHealthB = false;

    [Header("Loaded Data")]
    public string[] listenerInfoPlayer = new string[0];
    public string[] listenerInfoEnemy = new string[0];

    [Header("Last Data")]
    public int damageParseA = 0;
    public int damageParseB = 0;
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
    public string playerHealth = "";
    public string enemyHealth = "";
    public string playerZPosition = "";
    public string enemyZPosition = "";
    public string playerDamage = "";
    public string enemyDamage = "";

    #endregion

    #region Load Components

    public void Awake()
    {
        roundSystem = GameObject.Find("RoundManager").GetComponent<RoundManager>();
    }

    #endregion

    #region Real Time Operation

    public void Update()
    {
        if (roundSystem.isMultiplayer == true)
        {
            #region Receiving Data from Server

            if (roundSystem.roundOver == false && canListen == true)
            {
                countListen = countListen + Time.deltaTime;

                if (countListen > receiveDelay)
                {
                    ListenPlayers();
                    countListen = 0f;
                }
            }

            #endregion

            #region Setup new Data from Server

            if (wasDataLoadedPlayer == true)
            {
                if (playerHealth != listenerInfoPlayer[5])
                {
                    playerHealth = listenerInfoPlayer[5];
                    canParseHealthA = true;
                }

                if (playerDamage != listenerInfoPlayer[7])
                {
                    playerDamage = listenerInfoPlayer[7];
                }

                if (playerZPosition != listenerInfoPlayer[6])
                {
                    playerZPosition = listenerInfoPlayer[6];

                    if (playerZPosition != "0")
                    {
                        canParseA = true;
                    }
                }

                if (playerHit != listenerInfoPlayer[8])
                {
                    playerHit = listenerInfoPlayer[8];
                    wasEnemyDamaged = true;
                }

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

                wasDataLoadedPlayer = false;
            }

            if (wasDataLoadedEnemy == true)
            {
                if (enemyHealth != listenerInfoEnemy[5])
                {
                    enemyHealth = listenerInfoEnemy[5];
                    canParseHealthB = true;
                }

                if (enemyDamage != listenerInfoEnemy[7])
                {
                    enemyDamage = listenerInfoEnemy[7];
                }

                if (enemyZPosition != listenerInfoEnemy[6])
                {
                    enemyZPosition = listenerInfoEnemy[6];
                    canParseB = true;
                }

                if (enemyHit != listenerInfoEnemy[8])
                {
                    enemyHit = listenerInfoEnemy[8];
                    wasPlayerDamaged = true;
                }

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

            #region Parse Distances

            if (canParseA == true)
            {
                canParseA = false;

                if (int.TryParse(playerDamage, out int newDamageA))
                {
                    damageParseA = newDamageA;
                }

                if (playerZPosition.Contains("."))
                {
                    decimalIndexA1 = playerZPosition.IndexOf('.');
                    playerZPosition = playerZPosition.Substring(0, decimalIndexA1);
                }

                if (playerZPosition.Contains(","))
                {
                    decimalIndexA1 = playerZPosition.IndexOf(',');
                    playerZPosition = playerZPosition.Substring(0, decimalIndexA1);
                }

                if (enemyZPosition.Contains("."))
                {
                    decimalIndexB1 = enemyZPosition.IndexOf('.');
                    enemyZPosition = enemyZPosition.Substring(0, decimalIndexB1);
                }

                if (enemyZPosition.Contains(","))
                {
                    decimalIndexB1 = enemyZPosition.IndexOf(',');
                    enemyZPosition = enemyZPosition.Substring(0, decimalIndexB1);
                }

                if (int.TryParse(playerZPosition, out int newDistanceA1))
                {
                    realDistanceA1 = newDistanceA1;
                }

                if (int.TryParse(enemyZPosition, out int newDistanceA2))
                {
                    realDistanceA2 = newDistanceA2;
                }

                if (realDistanceA1 > realDistanceA2)
                {
                    realDistanceA = realDistanceA1 - realDistanceA2;
                }
                else
                {
                    realDistanceA = realDistanceA2 - realDistanceA1;
                }

                isParsedA = true;
            }

            if (canParseB == true)
            {
                canParseB = false;

                if (int.TryParse(enemyDamage, out int newDamageB))
                {
                    damageParseB = newDamageB;
                }

                if (playerZPosition.Contains("."))
                {
                    decimalIndexA2 = playerZPosition.IndexOf('.');
                    playerZPosition = playerZPosition.Substring(0, decimalIndexA2);
                }

                if (playerZPosition.Contains(","))
                {
                    decimalIndexA2 = playerZPosition.IndexOf(',');
                    playerZPosition = playerZPosition.Substring(0, decimalIndexA2);
                }

                if (enemyZPosition.Contains("."))
                {
                    decimalIndexB2 = enemyZPosition.IndexOf('.');
                    enemyZPosition = enemyZPosition.Substring(0, decimalIndexB2);
                }

                if (enemyZPosition.Contains(","))
                {
                    decimalIndexB2 = enemyZPosition.IndexOf(',');
                    enemyZPosition = enemyZPosition.Substring(0, decimalIndexB2);
                }

                if (int.TryParse(playerZPosition, out int newDistanceB1))
                {
                    realDistanceB1 = newDistanceB1;
                }

                if (int.TryParse(enemyZPosition, out int newDistanceB2))
                {
                    realDistanceB2 = newDistanceB2;
                }

                if (realDistanceB1 > realDistanceB2)
                {
                    realDistanceB = realDistanceB1 - realDistanceB2;
                }
                else
                {
                    realDistanceB = realDistanceB2 - realDistanceB1;
                }

                isParsedB = true;
            }

            #endregion

            #region Check for damage

            if (wasPlayerDamaged == true && isParsedA == true)
            {
                wasPlayerDamaged = false;
                CheckForPlayerDamage();
            }

            if (wasEnemyDamaged == true && isParsedB == true)
            {
                wasEnemyDamaged = false;
                CheckForEnemyDamage();
            }

            #endregion

            #region Health Update

            if (int.TryParse(playerHealth, out int newHealthA) && canParseHealthA == true)
            {
                actualPlayerHealth = newHealthA;

                roundSystem.UpdatePlayerHealth(actualPlayerHealth);
                canParseHealthA = false;
            }

            if (int.TryParse(enemyHealth, out int newHealthB) && canParseHealthB == true)
            {
                actualEnemyHealth = newHealthB;

                roundSystem.UpdateEnemyHealth(actualEnemyHealth);
                canParseHealthB = false;
            }

            #endregion
        }
    }

    #endregion

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

    public void ListenPlayers()
    {
        StartCoroutine(ListenPlayer(listenUser, actualPlayer));
        StartCoroutine(ListenEnemy(listenUser, actualEnemy));
    }

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

    #region Hit Operations

    private void CheckForPlayerDamage()
    {
        if (enemyHit == "yes<br>")
        {
            if (realDistanceA <= roundSystem.enemySystem.attackRange)
            {
                Debug.Log("Enemy dealed damage to Player");

                playerMultiplayer.RegisterPlayerTakesDamage(damageParseB);
            }

            damageParseB = 0;
            realDistanceA = 0;
            realDistanceA1 = 0;
            realDistanceA2 = 0;
            isParsedA = false;
        }
    }

    private void CheckForEnemyDamage()
    {
        if (playerHit == "yes<br>")
        {
            if (realDistanceB <= roundSystem.playerSystem.attackRange)
            {
                Debug.Log("Player dealed damage to Enemy");

                enemyMultiplayer.RegisterEnemyTakesDamage(damageParseA);
            }

            damageParseA = 0;
            realDistanceB = 0;
            realDistanceB1 = 0;
            realDistanceB2 = 0;
            isParsedB = false;
        }
    }

    #endregion

    #region Arena Operations

    public void LoadArena()
    {
        if (playerMultiplayer != null)
        {
            Debug.Log("Loading Arena as Player");

            playerMultiplayer.LoadCurrentArena();
        }

        if (enemyMultiplayer != null)
        {
            Debug.Log("Loading Arena as Enemy");

            enemyMultiplayer.LoadCurrentArena();
        }
    }

    #endregion
}
