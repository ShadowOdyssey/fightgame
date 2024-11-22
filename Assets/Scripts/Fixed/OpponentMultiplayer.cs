using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class OpponentMultiplayer : MonoBehaviour
{
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
    [Tooltip("Put the URL of the PHP file Verify Duel in the host server")]
    public string verifyDuel = "https://queensheartgames.com/shadowodyssey/verifyduel.php";
    [Tooltip("Put the URL of the PHP file Log Off Player in the host server")]
    public string logOffPlayer = "https://queensheartgames.com/shadowodyssey/logoffplayer.php";

    [Header("Lobby Data")]
    public int actualHost = 0;

    private string responseFromServer = "";

    public IEnumerator UpdateUser(string urlPHP, string newValue, string desiredCollumn, string validateRequest)
    {
        // Lets use Update User to change values of a collumn to a specific player

        // UPDATE lobby SET status = 'offline' WHERE id = 1; - Example use of UpdateUser();

        // UPDATE table SET desiredCollumn = 'newValue' WHERE id = validateRequest - Main structure of the query

        WWWForm form = new WWWForm();
        form.AddField("desiredCollumn", desiredCollumn);
        form.AddField("newValue", newValue);
        form.AddField("validateRequest", validateRequest);

        UnityWebRequest request = UnityWebRequest.Post(urlPHP, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            responseFromServer = request.downloadHandler.text;

            //Debug.Log("Response from server was: " + responseFromServer);

            if (responseFromServer == "error005")
            {
                yield break; // Exit the coroutine if there's an error
            }
        }

        request.Dispose();
    }

    public void UpdateData(string newValue, string desiredCollumn, string validateRequest)
    {
        if (gameObject.activeInHierarchy == true)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateUser(updateUser, newValue, desiredCollumn, validateRequest));
        }
    }

    public void SetHost(int newHost)
    {
        actualHost = newHost;

        LoadData();
    }

    private void LoadData()
    {

    }
}
