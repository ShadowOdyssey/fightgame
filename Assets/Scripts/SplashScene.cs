using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))] // Add audio source when attaching the script
public class SplashScene : MonoBehaviour
{
    public VideoClip _overviewVideoClip; // Overview video clip
    public GUIStyle _skipButtonStyle; // GUIStyle for skip button text

    private AudioSource _overviewAudio;
    public AudioClip _overviewMusic;

    private VideoPlayer _videoPlayer;
    private RenderTexture _renderTexture;
    private float _overviewFadeValue;
    private float _overviewFadeSpeed = 0.15f;

    private bool _skipClicked = false;

    private enum OverviewStoryController
    {
        OverviewFadeIn = 0,
        OverviewFadeOut = 1
    }
    private OverviewStoryController _overviewStoryController;

    void Awake()
    {
        _overviewFadeValue = 0;
    }

    void Start()
    {
        Cursor.visible = true;

        _overviewAudio = GetComponent<AudioSource>();
        _overviewAudio.volume = 0;
        _overviewAudio.clip = _overviewMusic;
        _overviewAudio.loop = true;
        _overviewAudio.Play();

        _videoPlayer = gameObject.AddComponent<VideoPlayer>();

        // Assign the video clip to the video player
        _videoPlayer.clip = _overviewVideoClip;

        // Create a RenderTexture to display the video
        _renderTexture = new RenderTexture((int)GetComponent<RectTransform>().rect.width, (int)GetComponent<RectTransform>().rect.height, 0);
        _videoPlayer.targetTexture = _renderTexture;

        _overviewStoryController = OverviewStoryController.OverviewFadeIn;

        StartCoroutine("OverviewStoryManager");
    }

    void Update()
    {
    }

    private IEnumerator OverviewStoryManager()
    {
        while (true)
        {
            switch (_overviewStoryController)
            {
                case OverviewStoryController.OverviewFadeIn:
                    OverviewFadeIn();
                    break;
                case OverviewStoryController.OverviewFadeOut:
                    if (_skipClicked)
                    {
                        OverviewFadeOut();
                    }
                    break;
            }
            yield return null;
        }
    }

    private void OverviewFadeIn()
    {
        _overviewAudio.volume += _overviewFadeSpeed * Time.deltaTime;
        _overviewFadeValue += _overviewFadeSpeed * Time.deltaTime;

        if (_overviewFadeValue > 1)
            _overviewFadeValue = 1;

        if (_overviewFadeValue == 1)
        {
            _videoPlayer.Play(); // Start playing the video
            _overviewStoryController = OverviewStoryController.OverviewFadeOut;
        }
    }

    private void OverviewFadeOut()
    {
        _overviewAudio.volume -= _overviewFadeSpeed * Time.deltaTime;
        _overviewFadeValue -= _overviewFadeSpeed * Time.deltaTime;

        if (_overviewFadeValue < 0)
            _overviewFadeValue = 0;

        // If fade-out is complete, don't hide the skip button
        if (_overviewFadeValue == 0)
            LoadStartScene(); // Load the StartScene after fade-out
    }

    private void LoadStartScene()
    {
        SceneManager.LoadScene("OverviewStory"); // Load the StartScene
    }

    private void OnGUI()
    {
        GUI.color = new Color(1, 1, 1, _overviewFadeValue);

        // Whole screen acts as skip button
        if (Event.current.type == EventType.MouseDown && !_skipClicked)
        {
            _skipClicked = true;
            SkipButtonClicked();
        }
    }

    void SkipButtonClicked()
    {
        Cursor.visible = true; // Ensure cursor is visible
        // Skip button clicked, load the StartScene
        LoadStartScene();
    }
}
