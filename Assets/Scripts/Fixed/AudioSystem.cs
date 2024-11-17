using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer generalAudio;

    [Header("Audio Players")]
    public AudioSource gameMusic;
    public AudioSource playerAudio;
    public AudioSource enemyAudio;

    [Header("Music Sound")]
    public AudioClip mainMusic;
    public AudioClip arena1Music;
    public AudioClip arena2Music;
    public AudioClip arena3Music;
    public AudioClip arena4Music;

    [Header("Intro Sounds")]
    public AudioClip gabriellaIntro;
    public AudioClip marcusIntro;

    [Header("Victory Sounds")]
    public AudioClip gabriellaVictory;
    public AudioClip marcusVictory;

    private int actualSourceIndex = 0;
    private bool checkIntro = false;
    private bool increaseVolume = false;
    private float volumeInteraction = -20f;

    private void Update()
    {
        if (checkIntro == true && playerAudio.isPlaying == false || checkIntro == true && enemyAudio.isPlaying == false)
        {
            increaseVolume = true;
            checkIntro = false;
        }

        if (increaseVolume == true)
        {
            volumeInteraction = volumeInteraction + Time.deltaTime;

            generalAudio.SetFloat("MusicVolume", volumeInteraction);

            if (volumeInteraction > -15f)
            {
                increaseVolume = false;
                generalAudio.SetFloat("MusicVolume", -15f);
                volumeInteraction = 0f;
            }
        }
    }

    public void PlayMusic(int musicIndex)
    {
        gameMusic.loop = true;

        switch (musicIndex)
        {
            case 1: gameMusic.clip = mainMusic; break;
            case 2: gameMusic.clip = arena1Music; break;
            case 3: gameMusic.clip = arena2Music; break;
            case 4: gameMusic.clip = arena3Music; break;
            case 5: gameMusic.clip = arena4Music; break;
        }

        gameMusic.Play();
    }

    public void PlayIntro(int sourceIndex, int introIndex)
    {
        actualSourceIndex = sourceIndex;

        generalAudio.SetFloat("MusicVolume", -20f);

        CheckNoLoop();

        switch (introIndex)
        {
            case 1: if (sourceIndex == 1) { playerAudio.clip = gabriellaIntro; } if (sourceIndex == 2) { enemyAudio.clip = gabriellaIntro; } break;
            case 2: if (sourceIndex == 1) { playerAudio.clip = marcusIntro; } if (sourceIndex == 2) { enemyAudio.clip = marcusIntro; } break;
        }

        checkIntro = true;

        PlayAudio();
    }

    public void PlayVictory(int sourceIndex, int introIndex)
    {
        actualSourceIndex = sourceIndex;

        CheckNoLoop();

        switch (introIndex)
        {
            case 1: if (sourceIndex == 1) { playerAudio.clip = gabriellaVictory; } if (sourceIndex == 2) { enemyAudio.clip = gabriellaVictory; } break;
            case 2: if (sourceIndex == 1) { playerAudio.clip = marcusVictory; } if (sourceIndex == 2) { enemyAudio.clip = marcusVictory; } break;
        }
    }

    private void CheckNoLoop()
    {
        if (actualSourceIndex == 1)
        {
            playerAudio.loop = false;
        }
        else
        {
            enemyAudio.loop = false;
        }
    }

    private void PlayAudio()
    {
        if (actualSourceIndex == 1)
        {
            playerAudio.Play();
        }
        else
        {
            enemyAudio.Play();
        }
    }
}
