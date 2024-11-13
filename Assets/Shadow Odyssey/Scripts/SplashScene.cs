using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))] // Add audio source when attaching the script
public class SplashScene : MonoBehaviour
{
    public VideoClip _overviewVideoClip; // Overview video clip
    public GUIStyle _skipButtonStyle; // GUIStyle for skip button text

    private AudioSource _overviewAudio; // Reference to AudioSource component
    public AudioClip _overviewMusic; // The background music for the splash screen

    private VideoPlayer _videoPlayer; // Reference to the VideoPlayer component
    private RenderTexture _renderTexture; // RenderTexture to display video
    private float _overviewFadeValue; // Fading effect for video and audio
    private float _overviewFadeSpeed = 0.15f; // Speed of fading effect

    private bool _skipClicked = false; // Check if skip button was clicked

    // Enum to control fade-in and fade-out phases
    private enum OverviewStoryController
    {
        OverviewFadeIn = 0,
        OverviewFadeOut = 1
    }
    private OverviewStoryController _overviewStoryController;

    void Awake()
    {
        _overviewFadeValue = 0; // Initialize fade value to 0 (fully transparent)
    }

    void Start()
    {
        Cursor.visible = true; // Show cursor

        // Setup the AudioSource component for background music
        _overviewAudio = GetComponent<AudioSource>();
        _overviewAudio.volume = 0; // Start with volume at 0 for fading in
        _overviewAudio.clip = _overviewMusic; // Assign the background music clip
        _overviewAudio.loop = true; // Set the audio to loop
        _overviewAudio.Play(); // Start playing the music

        // Setup the VideoPlayer component for the splash video
        _videoPlayer = gameObject.AddComponent<VideoPlayer>();
        _videoPlayer.clip = _overviewVideoClip; // Assign the video clip to the video player

        // Create a RenderTexture to display the video
        _renderTexture = new RenderTexture((int)GetComponent<RectTransform>().rect.width, (int)GetComponent<RectTransform>().rect.height, 0);
        _videoPlayer.targetTexture = _renderTexture;

        // Make sure the video has audio tracks
        Debug.Log("Number of audio tracks: " + _videoPlayer.audioTrackCount);

        // Enable the audio track and set the audio output mode
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        _videoPlayer.SetTargetAudioSource(0, _overviewAudio); // Use the first audio track
        _videoPlayer.SetDirectAudioMute(0, false); // Unmute the first audio track
        _videoPlayer.SetDirectAudioVolume(0, 1.0f); // Set the audio volume for the first audio track

        // Set the initial state to fade in the video and music
        _overviewStoryController = OverviewStoryController.OverviewFadeIn;

        // Start the overview story manager coroutine
        StartCoroutine("OverviewStoryManager");
    }

    void Update()
    {
        // Nothing specific in Update for now
    }

    // Coroutine to manage the fade-in and fade-out process
    private IEnumerator OverviewStoryManager()
    {
        while (true)
        {
            switch (_overviewStoryController)
            {
                case OverviewStoryController.OverviewFadeIn:
                    OverviewFadeIn(); // Handle fading in video and music
                    break;
                case OverviewStoryController.OverviewFadeOut:
                    if (_skipClicked)
                    {
                        OverviewFadeOut(); // Handle fading out if skip is clicked
                    }
                    break;
            }
            yield return null;
        }
    }

    // Handles the fade-in of video and music
    private void OverviewFadeIn()
    {
        // Gradually increase the audio volume and fade value
        _overviewAudio.volume += _overviewFadeSpeed * Time.deltaTime;
        _overviewFadeValue += _overviewFadeSpeed * Time.deltaTime;

        if (_overviewFadeValue > 1) // Cap the fade value at 1
            _overviewFadeValue = 1;

        if (_overviewFadeValue == 1)
        {
            _videoPlayer.Play(); // Start playing the video once fully faded in
            _overviewStoryController = OverviewStoryController.OverviewFadeOut; // Switch to fade-out state
        }
    }

    // Handles the fade-out of video and music
    private void OverviewFadeOut()
    {
        // Gradually decrease the audio volume and fade value
        _overviewAudio.volume -= _overviewFadeSpeed * Time.deltaTime;
        _overviewFadeValue -= _overviewFadeSpeed * Time.deltaTime;

        if (_overviewFadeValue < 0) // Cap the fade value at 0
            _overviewFadeValue = 0;

        // Once the fade-out is complete, load the next scene
        if (_overviewFadeValue == 0)
            LoadStartScene();
    }

    // Load the start scene (or another scene) after the splash screen
    private void LoadStartScene()
    {
        SceneManager.LoadScene("OverviewStory"); // Load the next scene after splash
    }

    // Handles GUI elements (like the skip button)
    private void OnGUI()
    {
        // Apply fading effect to the GUI color (video and text fade together)
        GUI.color = new Color(1, 1, 1, _overviewFadeValue);

        // Make the whole screen act as a skip button
        if (Event.current.type == EventType.MouseDown && !_skipClicked)
        {
            _skipClicked = true; // Mark that skip was clicked
            SkipButtonClicked(); // Call skip functionality
        }
    }

    // Functionality when the skip button is clicked
    void SkipButtonClicked()
    {
        Cursor.visible = true; // Ensure cursor is visible
        LoadStartScene(); // Load the start scene immediately after skipping
    }
}
