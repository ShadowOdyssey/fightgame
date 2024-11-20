using UnityEngine;

public class LobbyAudioSystem : MonoBehaviour
{
    [Header("Lobby Audio Setup")]
    public AudioSource lobbyAudio;
    public AudioSource lobbyMusicAudio;

    [Header("Lobby Music Sounds")]
    public AudioClip lobbyMusic;

    [Header("Select Sounds")]
    public AudioClip selectHero;

    [Header("Intro Sounds")]
    public AudioClip gabriellaIntro;
    public AudioClip marcusIntro;
    public AudioClip selenaIntro;
    public AudioClip bryanIntro;
    public AudioClip nunIntro;
    public AudioClip oliverIntro;
    public AudioClip orionIntro;
    public AudioClip ariaIntro;

    public void Start()
    {
        PlayLobbyMusic();
    }

    public void PlayIntro(int introIndex)
    {
        lobbyAudio.loop = false;

        switch (introIndex)
        {
            case 1: lobbyAudio.clip = gabriellaIntro; break;
            case 2: lobbyAudio.clip = marcusIntro; break;
            case 3: lobbyAudio.clip = selenaIntro; break;
            case 4: lobbyAudio.clip = bryanIntro; break;
            case 5: lobbyAudio.clip = nunIntro; break;
            case 6: lobbyAudio.clip = oliverIntro; break;
            case 7: lobbyAudio.clip = orionIntro; break;
            case 8: lobbyAudio.clip = ariaIntro; break;
        }

        lobbyAudio.Play();
    }

    public void PlayLobbyMusic()
    {
        lobbyMusicAudio.loop = true;
        lobbyMusicAudio.clip = lobbyMusic;
        lobbyMusicAudio.Play();
    }

    public void PlaySelectHero()
    {
        lobbyAudio.loop = false;
        lobbyAudio.clip = selectHero;
        lobbyAudio.Play();
    }
}
