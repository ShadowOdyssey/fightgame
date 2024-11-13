using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SinglePlayer : MonoBehaviour
{
    public Texture2D background; // Background image for the whole scene
    public Texture2D[] characterImages; // Array to hold character images
    public Texture2D buttonLeftImage; // Texture for the left button
    public Texture2D buttonRightImage; // Texture for the right button
    public Texture2D buttonBackImage; // Texture for the back button
    public Texture2D buttonSelectImage; // Texture for the select button
    public Texture2D buttonPlayImage; // Texture for the play button
    public Texture2D[] imageComics; // Array to hold image comics for the selected character
    public Texture2D closeButtonImage; // Texture for the close button

    public VideoClip videoClip; // Public reference to the VideoClip

    private int currentIndex = 0; // Index to keep track of the current image
    private bool showComic = false; // Flag to show/hide the comic panel
    private int currentComicIndex = 0; // Index to keep track of the current comic image
    private bool isPlayingVideo = false; // Flag to check if the video is playing

    private VideoPlayer videoPlayer; // Video player component

    void Start()
    {
        // Find the VideoPlayer component in the scene
        videoPlayer = FindObjectOfType<VideoPlayer>();
        if (videoPlayer != null)
        {
            // Assign the video clip to the video player
            videoPlayer.clip = videoClip;

            // Disable the video player UI initially
            videoPlayer.targetCameraAlpha = 0;
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    void OnGUI()
    {
        float buttonWidth = 200f; // Define button width for select and back buttons
        float buttonHeight = 200f; // Define button height for select and back buttons
        float smallButtonWidth = 50f; // Define button width for left and right buttons
        float smallButtonHeight = 50f; // Define button height for left and right buttons

        // Draw background image
        if (background != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background, ScaleMode.StretchToFill);
        }

        // Draw panel for character selection if not showing comic and not playing video
        if (!showComic && !isPlayingVideo)
        {
            DrawCharacterPanel(buttonWidth, buttonHeight);

            // Draw left button
            if (buttonLeftImage != null && GUI.Button(new Rect((Screen.width - smallButtonWidth) / 2 - 400, (Screen.height - smallButtonHeight) / 2, smallButtonWidth, smallButtonHeight), buttonLeftImage, GUIStyle.none))
            {
                ShowPreviousImage();
            }

            // Draw right button
            if (buttonRightImage != null && GUI.Button(new Rect((Screen.width - smallButtonWidth) / 2 + 400, (Screen.height - smallButtonHeight) / 2, smallButtonWidth, smallButtonHeight), buttonRightImage, GUIStyle.none))
            {
                ShowNextImage();
            }

            // Draw back, select, and play buttons
            DrawActionButtons(buttonWidth, buttonHeight);
        }
        else if (showComic)
        {
            // Draw the comic panel if showComic is true
            DrawComicPanel(buttonWidth, buttonHeight);
        }
    }

    void DrawCharacterPanel(float buttonWidth, float buttonHeight)
    {
        float panelWidth = Screen.width * 0.8f;
        float panelHeight = Screen.height * 0.8f;
        float panelX = (Screen.width - panelWidth) / 2;
        float panelY = (Screen.height - panelHeight) / 2;

        // Draw current character image
        if (characterImages.Length > 0)
        {
            // Increase image size for character
            float imageWidth = panelWidth * 0.9f;
            float imageHeight = panelHeight * 0.9f;
            float imageX = panelX + (panelWidth - imageWidth) / 2;
            float imageY = panelY + (panelHeight - imageHeight) / 2;

            GUI.DrawTexture(new Rect(imageX, imageY, imageWidth, imageHeight), characterImages[currentIndex], ScaleMode.ScaleToFit);
        }
    }

    void DrawActionButtons(float buttonWidth, float buttonHeight)
    {
        float panelWidth = Screen.width * 0.8f;
        float panelHeight = Screen.height * 0.8f;
        float panelX = (Screen.width - panelWidth) / 2;
        float panelY = (Screen.height - panelHeight) / 2;

        float buttonSpacing = 50f; // Increased spacing between the buttons
        float buttonY = panelY + panelHeight - 150f; // Adjusted position above the panel

        float buttonWidthAdjusted = 200f; // Adjusted button width
        float buttonHeightAdjusted = 200f; // Adjusted button height

        // Draw back button
        if (buttonBackImage != null && GUI.Button(new Rect(panelX + (panelWidth / 2) - buttonWidthAdjusted - buttonSpacing, buttonY, buttonWidthAdjusted, buttonHeightAdjusted), buttonBackImage, GUIStyle.none))
        {
            BackButtonClicked();
        }

        // Draw select button
        if (buttonSelectImage != null && GUI.Button(new Rect(panelX + (panelWidth / 2) + buttonSpacing, buttonY, buttonWidthAdjusted, buttonHeightAdjusted), buttonSelectImage, GUIStyle.none))
        {
            SelectButtonClicked();
        }

        // Draw play button below the select and back buttons
        float playButtonWidth = 150f; // Smaller width for the play button
        float playButtonHeight = 150f; // Smaller height for the play button
        float playButtonY = buttonY + buttonHeightAdjusted + buttonSpacing - 70f; // Adjusted position above slightly
        if (buttonPlayImage != null && GUI.Button(new Rect(panelX + (panelWidth / 2) - playButtonWidth / 2, playButtonY, playButtonWidth, playButtonHeight), buttonPlayImage, GUIStyle.none))
        {
            PlayButtonClicked();
        }
    }

    void DrawComicPanel(float buttonWidth, float buttonHeight)
    {
        float panelWidth = Screen.width * 0.8f;
        float panelHeight = Screen.height * 0.8f;
        float panelX = (Screen.width - panelWidth) / 2;
        float panelY = (Screen.height - panelHeight) / 2;

        // Draw the panel for the comics
        GUI.Box(new Rect(panelX, panelY, panelWidth, panelHeight), "");

        // Draw current comic image
        if (imageComics.Length > 0)
        {
            float imageWidth = panelWidth;
            float imageHeight = panelHeight * 0.9f; // Adjust height for the image
            float imageX = panelX;
            float imageY = panelY + (panelHeight - imageHeight) / 2;

            GUI.DrawTexture(new Rect(imageX, imageY, imageWidth, imageHeight), imageComics[currentComicIndex], ScaleMode.ScaleToFit);

            // Calculate position for the close button
            float closeButtonWidth = 50f; // Smaller width for the close button
            float closeButtonHeight = 50f; // Smaller height for the close button
            float closeButtonX = panelX + panelWidth - closeButtonWidth;
            float closeButtonY = panelY;

            // Close comic button
            if (closeButtonImage != null && GUI.Button(new Rect(closeButtonX, closeButtonY, closeButtonWidth, closeButtonHeight), closeButtonImage, GUIStyle.none))
            {
                showComic = false; // Close the comic panel
            }
        }
    }

    void ShowPreviousImage()
    {
        if (characterImages.Length == 0) return;

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = characterImages.Length - 1;
        }
    }

    void ShowNextImage()
    {
        if (characterImages.Length == 0) return;

        currentIndex++;
        if (currentIndex >= characterImages.Length)
        {
            currentIndex = 0;
        }
    }

    void BackButtonClicked()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    void SelectButtonClicked()
    {
        // Show the comic panel
        showComic = true;
        currentComicIndex = 0; // Reset comic index to the first image
    }

    void PlayButtonClicked()
    {
        // Start playing the video
        if (videoPlayer != null && videoClip != null)
        {
            isPlayingVideo = true;
            videoPlayer.targetCameraAlpha = 1; // Make the video player visible
            videoPlayer.Play();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Load the fighting scene after the video finishes
        SceneManager.LoadScene("FightingSingle");
    }
}
