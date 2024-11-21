using UnityEngine;

public class OpponentMultiplayer : MonoBehaviour
{
    public int actualHost = 0;

    public void SetHost(int newHost)
    {
        actualHost = newHost;

        LoadData();
    }

    private void LoadData()
    {

    }
}
