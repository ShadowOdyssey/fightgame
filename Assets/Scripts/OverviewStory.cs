using UnityEngine;
using UnityEngine.SceneManagement;

public class OverviewStory : MonoBehaviour
{
    public Texture2D overviewImage; // Overview image texture
    public GUIStyle skipButtonStyle; // GUIStyle for skip button text

    private bool skipClicked = false;

    void Start()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !skipClicked)
        {
            skipClicked = true;
            LoadStartScene();
        }
    }

    private void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), overviewImage);

        if (!skipClicked && GUI.Button(new Rect(Screen.width - 100, 20, 80, 30), "Skip", skipButtonStyle))
        {
            skipClicked = true;
            LoadStartScene();
        }
    }
}
