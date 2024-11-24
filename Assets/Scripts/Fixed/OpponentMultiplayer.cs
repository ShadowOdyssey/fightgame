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

    [Header("Listener Setup")]
    public float sendDelay = 0f;
    public float receiveDelay = 0.2f;

    [Header("Lobby Data")]
    public int actualHost = 0;
    public int actualListener = 0;
    public bool selected = false;

    #region Hidden Variables

    [Header("Listener Setup")]
    public float countListen = 0f;
    public bool isEnemyPlayer = false;
    public bool canListen = false;
    public bool wasDataLoaded = false;
    public bool isCheckingWin = false;
    public bool canApplyHit = false;
    public string responseFromServer = "";
    public string listenerForward = "";
    public string listenerBackward = "";
    public string listenerAttack1 = "";
    public string listenerAttack2 = "";
    public string listenerAttack3 = "";
    public string listenerHit = "";
    private string[] listenerInfo = new string[0];
    private int newDamage = 0;
    private string checkWin = "";

    #endregion

    #endregion

    #region Load Components

    public void Awake()
    {
        roundSystem = GameObject.Find("RoundManager").GetComponent<RoundManager>();
    }

    #endregion

    #region Real Time Operations

    public void Update()
    {
        #region Receiving Data from Server

        if (selected == true && roundSystem.roundOver == false)
        {
            countListen = countListen + Time.deltaTime;

            if (countListen > receiveDelay)
            {
                //Debug.Log("Listening opponent actions");

                StartCoroutine(ListenUser(listenUser, actualListener));
                countListen = 0f;
            }
        }

        #endregion

        #region Processing received Data from Server

        if (wasDataLoaded == true)
        {
            if (listenerForward != listenerInfo[0])
            {
                listenerForward = listenerInfo[0];

                if (isEnemyPlayer == true)
                {
                    RegisterForwardPlayer();
                }
                else
                {
                    RegisterForwardEnemy();
                }
            }

            if (listenerBackward != listenerInfo[1])
            {
                listenerBackward = listenerInfo[1];

                if (isEnemyPlayer == true)
                {
                    RegisterBackwardPlayer();
                }
                else
                {
                    RegisterBackwardEnemy();
                }
            }

            if (listenerAttack1 != listenerInfo[2])
            {
                listenerAttack1 = listenerInfo[2];

                if (isEnemyPlayer == true)
                {
                    RegisterAttack1Player();
                }
                else
                {
                    RegisterAttack1Enemy();
                }
            }

            if (listenerAttack2 != listenerInfo[3])
            {
                listenerAttack2 = listenerInfo[3];

                if (isEnemyPlayer == true)
                {
                    RegisterAttack2Player();
                }
                else
                {
                    RegisterAttack2Enemy();
                }
            }

            if (listenerAttack3 != listenerInfo[4])
            {
                listenerAttack3 = listenerInfo[4];

                if (isEnemyPlayer == true)
                {
                    RegisterAttack3Player();
                }
                else
                {
                    RegisterAttack3Enemy();
                }
            }

            if (listenerHit != listenerInfo[5])
            {
                listenerHit = listenerInfo[5];

                if (isEnemyPlayer == true)
                {
                    RegisterHitPlayer();
                }
                else
                {
                    RegisterHitEnemy();
                }
            }

            wasDataLoaded = false;
        }

        #endregion
    }

    #endregion

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

            Debug.Log("Data received from opponent: " + responseFromServer);

            wasDataLoaded = true;
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

    public IEnumerator LogOffPlayer(string urlPHP, string playerSession, string playerName)
    {
        WWWForm form = new WWWForm();
        form.AddField("validateRequest", playerSession);

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

    #endregion

    #region Apply loaded data from Lobby

    public void SetHost(int newHost, int newListener)
    {
        if (gameObject.activeInHierarchy == true)
        {
            actualHost = newHost;
            actualListener = newListener;
            selected = true;
            StartCoroutine(RegisterDuel(duelingUser, actualHost));
        }
    }

    public void SetOpponentPlayer(PlayerSystem actualEnemySystem)
    {
        //Debug.Log("Opponent is Player");

        opponentIsPlayer = actualEnemySystem;
        isEnemyPlayer = true;
    }

    public void SetOpponentEnemy(EnemySystem actualEnemySystem)
    {
        //Debug.Log("Opponent is Enemy");

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

    #region Hit Operations

    public void PlayerTakeHit(int damage)
    {
        Debug.Log("Send to server only hits in clone Player");

        UpdateData("yes", "hit", actualHost.ToString());
        Invoke(nameof(ResetHitPlayer), 0.5f);
        newDamage = damage;        
    }

    public void EnemyTakeHit(int damage)
    {
        Debug.Log("Send to server only hits in clone Enemy");

        UpdateData("yes", "hit", actualHost.ToString());
        Invoke(nameof(ResetHitEnemy), 0.5f);
        newDamage = damage;
    }

    private void ResetHitPlayer()
    {
        UpdateData("no", "hit", actualHost.ToString());
        canApplyHit = false;
    }

    private void ResetHitEnemy()
    {
        UpdateData("no", "hit", actualHost.ToString());
        canApplyHit = false;
    }

    #endregion

    #region Data Sent

    public void SendStop()
    {
        UpdateData("no", "forward", actualHost.ToString());
        UpdateData("no", "backward", actualHost.ToString());
    }

    public void SendForward()
    {
        //Debug.Log("Sending forward to server");
        UpdateData("yes", "forward", actualHost.ToString());
    }

    public void SendBackward()
    {
        //Debug.Log("Sending backward to server");
        UpdateData("yes", "backward", actualHost.ToString());
    }

    public void SendStopForward()
    {
        //Debug.Log("Sending stop forward to server");
        UpdateData("no", "forward", actualHost.ToString());
    }

    public void SendStopBackward()
    {
        //Debug.Log("Sending stop backward to server");
        UpdateData("no", "backward", actualHost.ToString());
    }

    public void SendAttack1()
    {
        //Debug.Log("Sending attack1 to server");
        UpdateData("yes", "attack1", actualHost.ToString());
        Invoke(nameof(ResetAttack1), sendDelay);
    }

    public void SendAttack2()
    {
        //Debug.Log("Sending attack2 to server");
        UpdateData("yes", "attack2", actualHost.ToString());
        Invoke(nameof(ResetAttack2), sendDelay);
    }

    public void SendAttack3()
    {
        //Debug.Log("Sending attack3 to server");
        UpdateData("yes", "attack3", actualHost.ToString());
        Invoke(nameof(ResetAttack3), sendDelay);
    }

    private void ResetAttack1()
    {
        //Debug.Log("Sending reset attack 1 to server");
        UpdateData("no", "attack1", actualHost.ToString());
    }

    private void ResetAttack2()
    {
        //Debug.Log("Sending reset attack 2 to server");
        UpdateData("no", "attack2", actualHost.ToString());
    }

    private void ResetAttack3()
    {
        //Debug.Log("Sending reset attack 3 to server");
        UpdateData("no", "attack3", actualHost.ToString());
    }

    #endregion

    #region Data Received

    #region Enemy is Player, register new data

    public void RegisterForwardPlayer()
    {
        if (listenerForward == "yes")
        {
            //Debug.Log("Opponent as Player is moving forward");
            opponentIsPlayer.MultiplayerMovesForward();
        }

        if (listenerForward == "no")
        {
            //Debug.Log("Opponent as Player stopped to move forward");
            opponentIsPlayer.MultiplayerStopForward();
        }
    }

    public void RegisterBackwardPlayer()
    {
        if (listenerBackward == "yes")
        {
            //Debug.Log("Opponent as Player is moving backward");
            opponentIsPlayer.MultiplayerMovesBackward();
        }

        if (listenerBackward == "no")
        {
            //Debug.Log("Opponent as Player stopped to move backward");
            opponentIsPlayer.MultiplayerStopBackward();
        }
    }

    public void RegisterAttack1Player()
    {
        if (listenerAttack1 == "yes")
        {
            //Debug.Log("Opponent as Player used Attack 1");
            opponentIsPlayer.MultiplayerAttacked1();
        }
    }

    public void RegisterAttack2Player()
    {
        if (listenerAttack2 == "yes")
        {
            //Debug.Log("Opponent as Player used Attack 2");
            opponentIsPlayer.MultiplayerAttacked2();
        }
    }

    public void RegisterAttack3Player()
    {
        if (listenerAttack3 == "yes")
        {
            //Debug.Log("Opponent as Player used Attack 3");
            opponentIsPlayer.MultiplayerAttacked3();
        }
    }

    public void RegisterHitPlayer()
    {
        if (listenerHit == "yes<br>" && canApplyHit == false)
        {
            Debug.Log("Opponent as Player got hit");

            opponentIsPlayer.TakeHit(newDamage);
            newDamage = 0;
            canApplyHit = true;
        }
    }

    #endregion

    #region Enemy is Enemy, register new data

    public void RegisterForwardEnemy()
    {
        if (listenerForward == "yes")
        {
            //Debug.Log("Opponent as Enemy stopped is moving forward");
            opponentIsEnemy.MultiplayerMovesForward();
        }

        if (listenerForward == "no")
        {
            //Debug.Log("Opponent as Enemy stopped to move forward");
            opponentIsEnemy.MultiplayerStopForward();
        }
    }

    public void RegisterBackwardEnemy()
    {
        if (listenerBackward == "yes")
        {
            //Debug.Log("Opponent as Enemy stopped is moving backward");
            opponentIsEnemy.MultiplayerMovesBackward();
        }

        if (listenerBackward == "no")
        {
            //Debug.Log("Opponent as Enemy stopped to move backward");
            opponentIsEnemy.MultiplayerStopBackward();
        }
    }

    public void RegisterAttack1Enemy()
    {
        if (listenerAttack1 == "yes")
        {
            //Debug.Log("Opponent as Enemy used Attack 1");
            opponentIsEnemy.MultiplayerAttacked1();
        }
    }

    public void RegisterAttack2Enemy()
    {
        if (listenerAttack2 == "yes")
        {
            //Debug.Log("Opponent as Enemy used Attack 2");
            opponentIsEnemy.MultiplayerAttacked2();
        }
    }

    public void RegisterAttack3Enemy()
    {
        if (listenerAttack3 == "yes")
        {
            //Debug.Log("Opponent as Enemy used Attack 3");
            opponentIsEnemy.MultiplayerAttacked3();
        }
    }

    public void RegisterHitEnemy()
    {
        if (listenerHit == "yes<br>" && canApplyHit == false)
        {
            Debug.Log("Opponent as Enemy got hit");
            
            opponentIsEnemy.TakeDamage(newDamage);
            newDamage = 0;
            canApplyHit = true;
        }
    }

    #endregion

    #endregion

    #region Log Off Operations

    public void LeaveFight()
    {
        if (gameObject.activeInHierarchy == true)
        {
            if (isEnemyPlayer == true)
            {
                StartCoroutine(LogOffPlayer(logOffPlayer, PlayerPrefs.GetString("playerServerID"), roundSystem.enemyNameText.text));
            }
            else
            {
                StartCoroutine(LogOffPlayer(logOffPlayer, PlayerPrefs.GetString("playerServerID"), roundSystem.playerNameText.text));
            }
        }
    }

    public void OnDisable()
    {
        if (gameObject.activeInHierarchy == true)
        {
            if (isEnemyPlayer == true)
            {
                StartCoroutine(LogOffPlayer(logOffPlayer, PlayerPrefs.GetString("playerServerID"), roundSystem.enemyNameText.text));
            }
            else
            {
                StartCoroutine(LogOffPlayer(logOffPlayer, PlayerPrefs.GetString("playerServerID"), roundSystem.playerNameText.text));
            }
        }
    }

    public void OnDestroy()
    {
        if (gameObject.activeInHierarchy == true)
        {
            if (isEnemyPlayer == true)
            {
                StartCoroutine(LogOffPlayer(logOffPlayer, PlayerPrefs.GetString("playerServerID"), roundSystem.enemyNameText.text));
            }
            else
            {
                StartCoroutine(LogOffPlayer(logOffPlayer, PlayerPrefs.GetString("playerServerID"), roundSystem.playerNameText.text));
            }
        }
    }

    #endregion

    #endregion
}
