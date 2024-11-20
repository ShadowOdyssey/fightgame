using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public int actualScene = 0; // 0 = SplashScreen 1 = StartScene 2 = MainMenu 3 = Game
    public float timeToLoadScene = 10f; // Time to load next scene, adjust it based in the actual scene is using it

    private bool canLoadScene = false; // A simple boolean to trigger the load scene when needed in Fixed Update

    public void Awake() // All variables that need to be setup before scene to start should be applied in Awake()
    {
        if (actualScene == 0) // If actual scene is SplashScreen so...
        {
            canLoadScene = true; // Activate the timeToLoadScene to load next scene after intro video!
        }
    }

    private void FixedUpdate()
    {
        if (canLoadScene == true) // If canLoadScene is true, so...
        {
            timeToLoadScene = timeToLoadScene - Time.deltaTime; // Decrease the timeToLoadScene value in fixed frame rate in Fixed Update...

            if (timeToLoadScene < 0f) // If timeToLoadScene reached zero, so...
            {
                timeToLoadScene = 0f; // Make sure timeToLoadScene is totally zeroed and...

                // Check the actual scene to load the correct next scene...

                if (actualScene == 0) // If actual scene is SplashScreen, so...
                {
                    SceneManager.LoadScene(1); // Load StartScene scene, but...
                }

                if (actualScene == 1) // If actual scene is StartScene, so...
                {
                    SceneManager.LoadScene(2); // Load MainMenu scene, but...
                }

                if (actualScene == 2) // If actual scene is MainMenu, so...
                {
                    SceneManager.LoadScene(3);// Load Game scene, but...
                }

                if (actualScene == 3) // If actual scene is Game, so...
                {
                    SceneManager.LoadScene(2);// Load MainMenu scene!
                }

                canLoadScene = false;
            }
        }
    }

    public void CanLoadScene()
    {
        canLoadScene = true; // canLoadScene now is true and FixedUpdate can load a new scene when the timeToLoadScene to reachs zero value
    }
}
