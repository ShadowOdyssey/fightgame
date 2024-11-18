using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    #region Audio Mixer Setup

    [Header("Audio Mixer")]
    public AudioMixer generalAudio;

    [Header("Audio Players")]
    public AudioSource gameMusic;
    public AudioSource playerAudio;
    public AudioSource enemyAudio;

    [Header("AudioMixer Setup")]
    public float minMusicVolume = -20f;
    public float maxMusicVolume = -15f;
    public float minPlayerVolume = -10f;
    public float maxPlayerVolume = 0f;
    public float minEnemyVolume = -10f;
    public float maxEnemyVolume = 0f;

    #endregion

    #region UI Sound Setup

    [Header("UI Sound")]
    public AudioClip uiButtonClicked;
    public AudioClip uiButtonCooldown;
    public AudioClip uiButtonConfirm;
    public AudioClip uiButtonCancel;
    public AudioClip uiNotificationFromServer;

    #endregion

    #region Music Setup

    [Header("Music Sound")]
    public AudioClip mainMusic;
    public AudioClip arena1Music;
    public AudioClip arena2Music;
    public AudioClip arena3Music;
    public AudioClip arena4Music;

    #endregion

    #region Round Setup

    [Header("Round Sound")]
    public AudioClip roundSound1;
    public AudioClip roundSound2;
    public AudioClip roundSound3;
    public AudioClip roundSoundWon;
    public AudioClip roundSoundDefeat;

    #endregion

    #region Characters Setup

    #region Intro Sound

    [Header("Intro Sounds")]
    public AudioClip gabriellaIntro;
    public AudioClip marcusIntro;
    public AudioClip selenaIntro;
    public AudioClip bryanIntro;
    public AudioClip nunIntro;
    public AudioClip oliverIntro;
    public AudioClip orionIntro;
    public AudioClip ariaIntro;

    #endregion

    #region Attack Sound

    [Header("Attack 1 Sounds")]
    public AudioClip gabriellaAttack1;
    public AudioClip marcusAttack1;
    public AudioClip selenaAttack1;
    public AudioClip bryanAttack1;
    public AudioClip nunAttack1;
    public AudioClip oliverAttack1;
    public AudioClip orionAttack1;
    public AudioClip ariaAttack1;


    [Header("Attack 2 Sounds")]
    public AudioClip gabriellaAttack2;
    public AudioClip marcusAttack2;
    public AudioClip selenaAttack2;
    public AudioClip bryanAttack2;
    public AudioClip nunAttack2;
    public AudioClip oliverAttack2;
    public AudioClip orionAttack2;
    public AudioClip ariaAttack2;

    [Header("Attack 3 Sounds")]
    public AudioClip gabriellaAttack3;
    public AudioClip marcusAttack3;
    public AudioClip selenaAttack3;
    public AudioClip bryanAttack3;
    public AudioClip nunAttack3;
    public AudioClip oliverAttack3;
    public AudioClip orionAttack3;
    public AudioClip ariaAttack3;

    #endregion

    #region Hit Sound

    [Header("Hit Sounds")]
    public AudioClip gabriellaHit;
    public AudioClip marcusHit;
    public AudioClip selenaHit;
    public AudioClip bryanHit;
    public AudioClip nunHit;
    public AudioClip oliverHit;
    public AudioClip orionHit;
    public AudioClip ariaHit;

    #endregion

    #region Block Sound

    [Header("Block Sounds")]
    public AudioClip gabriellaBlock;
    public AudioClip marcusBlock;
    public AudioClip selenaBlock;
    public AudioClip bryanBlock;
    public AudioClip nunBlock;
    public AudioClip oliverBlock;
    public AudioClip orionBlock;
    public AudioClip ariaBlock;

    #endregion

    #region Defeat Sound

    [Header("Defeat Sounds")]
    public AudioClip gabriellaDefeat;
    public AudioClip marcusDefeat;
    public AudioClip selenaDefeat;
    public AudioClip bryanDefeat;
    public AudioClip nunDefeat;
    public AudioClip oliverDefeat;
    public AudioClip orionDefeat;
    public AudioClip ariaDefeat;

    #endregion

    #region Victory Sound

    [Header("Victory Sounds")]
    public AudioClip gabriellaVictory;
    public AudioClip marcusVictory;
    public AudioClip selenaVictory;
    public AudioClip bryanVictory;
    public AudioClip nunVictory;
    public AudioClip oliverVictory;
    public AudioClip orionVictory;
    public AudioClip ariaVictory;

    #endregion

    #endregion

    #region Hidden Variables

    [Header("MONITOR - DONT CHANGE ANY VALUE HERE")]
    private int actualSourceIndex = 0;
    private bool checkIncreaseMusic = false;
    private bool checkDecreaseMusic = false;
    private bool checkIncreasePlayer = false;
    private bool checkDecreasePlayer = false;
    private bool checkIncreaseEnemy = false;
    private bool checkDecreaseEnemy = false;
    private bool adjustedMusicVolume = false;
    private bool increaseMusicVolume = false;
    private bool decreaseMusicVolume = false;
    private bool adjustedPlayerVolume = false;
    private bool increasePlayerVolume = false;
    private bool decreasePlayerVolume = false;
    private bool adjustedEnemyVolume = false;
    private bool increaseEnemyVolume = false;
    private bool decreaseEnemyVolume = false;
    private float musicVolumeInteraction = 0f;
    private float playerVolumeInteraction = 0f;
    private float enemyVolumeInteraction = 0f;

    #endregion

    #region Loading Components

    private void Start()
    {
        checkDecreaseMusic = true; // As scene starts with player and enemy intro, automatically reduce audio when starts
    }

    #endregion

    #region RealTime Processing

    private void Update()
    {
        #region Music Audio Methods

        CheckForIncreaseMusicVolume();
        CheckForDecreaseMusicVolume();
        IncreaseMusicVolume();
        DecreaseMusicVolume();

        #endregion

        #region Player Audio Methods

        CheckForIncreasePlayerVolume();
        CheckForDecreasePlayerVolume();
        IncreasePlayerVolume();
        DecreasePlayerVolume();

        #endregion

        #region Enemy Audio Methods

        CheckForIncreaseEnemyVolume();
        CheckForDecreaseEnemyVolume();
        IncreaseEnemyVolume();
        DecreaseEnemyVolume();

        #endregion
    }

    #endregion

    #region Play Music

    public void PlayMusic(int musicIndex)
    {
        if (gameMusic.loop == false)
        {
            gameMusic.loop = true;
        }

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

    #endregion

    #region Play Attack

    public void PlayIntro(int sourceIndex, int introIndex)
    {
        actualSourceIndex = sourceIndex;
        generalAudio.SetFloat("MusicVolume", -20f);
        CheckNoLoop();

        switch (introIndex)
        {
            case 1: if (sourceIndex == 1) { playerAudio.clip = gabriellaIntro; } if (sourceIndex == 2) { enemyAudio.clip = gabriellaIntro; } break;
            case 2: if (sourceIndex == 1) { playerAudio.clip = marcusIntro; } if (sourceIndex == 2) { enemyAudio.clip = marcusIntro; } break;
            case 3: if (sourceIndex == 1) { playerAudio.clip = selenaIntro; } if (sourceIndex == 2) { enemyAudio.clip = selenaIntro; } break;
            case 4: if (sourceIndex == 1) { playerAudio.clip = bryanIntro; } if (sourceIndex == 2) { enemyAudio.clip = bryanIntro; } break;
            case 5: if (sourceIndex == 1) { playerAudio.clip = nunIntro; } if (sourceIndex == 2) { enemyAudio.clip = nunIntro; } break;
            case 6: if (sourceIndex == 1) { playerAudio.clip = oliverIntro; } if (sourceIndex == 2) { enemyAudio.clip = oliverIntro; } break;
            case 7: if (sourceIndex == 1) { playerAudio.clip = orionIntro; } if (sourceIndex == 2) { enemyAudio.clip = orionIntro; } break;
            case 8: if (sourceIndex == 1) { playerAudio.clip = ariaIntro; } if (sourceIndex == 2) { enemyAudio.clip = ariaIntro; } break;
        }

        checkIncreaseMusic = true;
        PlayAudio();
    }

    public void PlayAttack1(int sourceIndex, int introIndex)
    {
        actualSourceIndex = sourceIndex;
        CheckNoLoop();

        switch (introIndex)
        {
            case 1: if (sourceIndex == 1) { playerAudio.clip = gabriellaAttack1; } if (sourceIndex == 2) { enemyAudio.clip = gabriellaAttack1; } break;
            case 2: if (sourceIndex == 1) { playerAudio.clip = marcusAttack1; } if (sourceIndex == 2) { enemyAudio.clip = marcusAttack1; } break;
            case 3: if (sourceIndex == 1) { playerAudio.clip = selenaAttack1; } if (sourceIndex == 2) { enemyAudio.clip = selenaAttack1; } break;
            case 4: if (sourceIndex == 1) { playerAudio.clip = bryanAttack1; } if (sourceIndex == 2) { enemyAudio.clip = bryanAttack1; } break;
            case 5: if (sourceIndex == 1) { playerAudio.clip = nunAttack1; } if (sourceIndex == 2) { enemyAudio.clip = nunAttack1; } break;
            case 6: if (sourceIndex == 1) { playerAudio.clip = oliverAttack1; } if (sourceIndex == 2) { enemyAudio.clip = oliverAttack1; } break;
            case 7: if (sourceIndex == 1) { playerAudio.clip = orionAttack1; } if (sourceIndex == 2) { enemyAudio.clip = orionAttack1; } break;
            case 8: if (sourceIndex == 1) { playerAudio.clip = ariaAttack1; } if (sourceIndex == 2) { enemyAudio.clip = ariaAttack1; } break;
        }

        PlayAudio();
    }

    public void PlayAttack2(int sourceIndex, int introIndex)
    {
        actualSourceIndex = sourceIndex;
        CheckNoLoop();

        switch (introIndex)
        {
            case 1: if (sourceIndex == 1) { playerAudio.clip = gabriellaAttack2; } if (sourceIndex == 2) { enemyAudio.clip = gabriellaAttack2; } break;
            case 2: if (sourceIndex == 1) { playerAudio.clip = marcusAttack2; } if (sourceIndex == 2) { enemyAudio.clip = marcusAttack2; } break;
            case 3: if (sourceIndex == 1) { playerAudio.clip = selenaAttack2; } if (sourceIndex == 2) { enemyAudio.clip = selenaAttack2; } break;
            case 4: if (sourceIndex == 1) { playerAudio.clip = bryanAttack2; } if (sourceIndex == 2) { enemyAudio.clip = bryanAttack2; } break;
            case 5: if (sourceIndex == 1) { playerAudio.clip = nunAttack2; } if (sourceIndex == 2) { enemyAudio.clip = nunAttack2; } break;
            case 6: if (sourceIndex == 1) { playerAudio.clip = oliverAttack2; } if (sourceIndex == 2) { enemyAudio.clip = oliverAttack2; } break;
            case 7: if (sourceIndex == 1) { playerAudio.clip = orionAttack2; } if (sourceIndex == 2) { enemyAudio.clip = orionAttack2; } break;
            case 8: if (sourceIndex == 1) { playerAudio.clip = ariaAttack2; } if (sourceIndex == 2) { enemyAudio.clip = ariaAttack2; } break;
        }

        PlayAudio();
    }

    public void PlayAttack3(int sourceIndex, int introIndex)
    {
        actualSourceIndex = sourceIndex;
        CheckNoLoop();

        switch (introIndex)
        {
            case 1: if (sourceIndex == 1) { playerAudio.clip = gabriellaAttack3; } if (sourceIndex == 2) { enemyAudio.clip = gabriellaAttack3; } break;
            case 2: if (sourceIndex == 1) { playerAudio.clip = marcusAttack3; } if (sourceIndex == 2) { enemyAudio.clip = marcusAttack3; } break;
            case 3: if (sourceIndex == 1) { playerAudio.clip = selenaAttack3; } if (sourceIndex == 2) { enemyAudio.clip = selenaAttack3; } break;
            case 4: if (sourceIndex == 1) { playerAudio.clip = bryanAttack3; } if (sourceIndex == 2) { enemyAudio.clip = bryanAttack3; } break;
            case 5: if (sourceIndex == 1) { playerAudio.clip = nunAttack3; } if (sourceIndex == 2) { enemyAudio.clip = nunAttack3; } break;
            case 6: if (sourceIndex == 1) { playerAudio.clip = oliverAttack3; } if (sourceIndex == 2) { enemyAudio.clip = oliverAttack3; } break;
            case 7: if (sourceIndex == 1) { playerAudio.clip = orionAttack3; } if (sourceIndex == 2) { enemyAudio.clip = orionAttack3; } break;
            case 8: if (sourceIndex == 1) { playerAudio.clip = ariaAttack3; } if (sourceIndex == 2) { enemyAudio.clip = ariaAttack3; } break;
        }

        PlayAudio();
    }

    #endregion

    #region Play Victory

    public void PlayVictory(int sourceIndex, int introIndex)
    {
        actualSourceIndex = sourceIndex;
        CheckNoLoop();

        switch (introIndex)
        {
            case 1: if (sourceIndex == 1) { playerAudio.clip = gabriellaVictory; } if (sourceIndex == 2) { enemyAudio.clip = gabriellaVictory; } break;
            case 2: if (sourceIndex == 1) { playerAudio.clip = marcusVictory; } if (sourceIndex == 2) { enemyAudio.clip = marcusVictory; } break;
            case 3: if (sourceIndex == 1) { playerAudio.clip = selenaVictory; } if (sourceIndex == 2) { enemyAudio.clip = selenaVictory; } break;
            case 4: if (sourceIndex == 1) { playerAudio.clip = bryanVictory; } if (sourceIndex == 2) { enemyAudio.clip = bryanVictory; } break;
            case 5: if (sourceIndex == 1) { playerAudio.clip = nunVictory; } if (sourceIndex == 2) { enemyAudio.clip = nunVictory; } break;
            case 6: if (sourceIndex == 1) { playerAudio.clip = oliverVictory; } if (sourceIndex == 2) { enemyAudio.clip = oliverVictory; } break;
            case 7: if (sourceIndex == 1) { playerAudio.clip = orionVictory; } if (sourceIndex == 2) { enemyAudio.clip = orionVictory; } break;
            case 8: if (sourceIndex == 1) { playerAudio.clip = ariaVictory; } if (sourceIndex == 2) { enemyAudio.clip = ariaVictory; } break;
        }

        PlayAudio();
    }

    #endregion

    #region Music Volume - Apply Increase Decrease

    private void IncreaseMusicVolume()
    {
        if (adjustedMusicVolume == false)
        {
            musicVolumeInteraction = minMusicVolume;
            adjustedMusicVolume = true;
        }

        if (increaseMusicVolume == true)
        {
            musicVolumeInteraction = musicVolumeInteraction + Time.deltaTime;
            generalAudio.SetFloat("MusicVolume", musicVolumeInteraction);

            if (musicVolumeInteraction > maxMusicVolume)
            {
                increaseMusicVolume = false;
                generalAudio.SetFloat("MusicVolume", maxMusicVolume);
                musicVolumeInteraction = 0f;
                adjustedMusicVolume = false;
            }
        }
    }

    private void DecreaseMusicVolume()
    {
        if (adjustedMusicVolume == false)
        {
            musicVolumeInteraction = maxMusicVolume;
            adjustedMusicVolume = true;
        }

        if (decreaseMusicVolume == true)
        {
            musicVolumeInteraction = musicVolumeInteraction - Time.deltaTime;
            generalAudio.SetFloat("MusicVolume", musicVolumeInteraction);

            if (musicVolumeInteraction < minMusicVolume)
            {
                decreaseMusicVolume = false;
                generalAudio.SetFloat("MusicVolume", minMusicVolume);
                musicVolumeInteraction = 0f;
                adjustedMusicVolume = false;
            }
        }
    }

    #endregion

    #region Player Volume - Apply Increase Decrease

    private void IncreasePlayerVolume()
    {
        if (adjustedPlayerVolume == false)
        {
            playerVolumeInteraction = minPlayerVolume;
            adjustedPlayerVolume = true;
        }

        if (increasePlayerVolume == true)
        {
            playerVolumeInteraction = playerVolumeInteraction + Time.deltaTime;
            generalAudio.SetFloat("PlayerVolume", playerVolumeInteraction);

            if (playerVolumeInteraction > maxPlayerVolume)
            {
                increaseMusicVolume = false;
                generalAudio.SetFloat("PlayerVolume", maxPlayerVolume);
                playerVolumeInteraction = 0f;
                adjustedPlayerVolume = false;
            }
        }
    }

    private void DecreasePlayerVolume()
    {
        if (adjustedPlayerVolume == false)
        {
            playerVolumeInteraction = maxPlayerVolume;
            adjustedPlayerVolume = true;
        }

        if (decreasePlayerVolume == true)
        {
            playerVolumeInteraction = playerVolumeInteraction - Time.deltaTime;
            generalAudio.SetFloat("PlayerVolume", playerVolumeInteraction);

            if (playerVolumeInteraction < minPlayerVolume)
            {
                decreasePlayerVolume = false;
                generalAudio.SetFloat("PlayerVolume", minPlayerVolume);
                playerVolumeInteraction = 0f;
                adjustedPlayerVolume = false;
            }
        }
    }

    #endregion

    #region Enemy Volume - Apply Increase Decrease

    private void IncreaseEnemyVolume()
    {
        if (adjustedEnemyVolume == false)
        {
            enemyVolumeInteraction = minEnemyVolume;
            adjustedEnemyVolume = true;
        }

        if (increaseEnemyVolume == true)
        {
            enemyVolumeInteraction = enemyVolumeInteraction + Time.deltaTime;
            generalAudio.SetFloat("EnemyVolume", enemyVolumeInteraction);

            if (enemyVolumeInteraction > maxEnemyVolume)
            {
                increaseEnemyVolume = false;
                generalAudio.SetFloat("EnemyVolume", maxEnemyVolume);
                enemyVolumeInteraction = 0f;
                adjustedEnemyVolume = false;
            }
        }
    }

    private void DecreaseEnemyVolume()
    {
        if (adjustedEnemyVolume == false)
        {
            enemyVolumeInteraction = maxEnemyVolume;
            adjustedEnemyVolume = true;
        }

        if (decreaseEnemyVolume == true)
        {
            enemyVolumeInteraction = enemyVolumeInteraction - Time.deltaTime;
            generalAudio.SetFloat("EnemyVolume", enemyVolumeInteraction);

            if (enemyVolumeInteraction < minEnemyVolume)
            {
                decreaseEnemyVolume = false;
                generalAudio.SetFloat("EnemyVolume", minEnemyVolume);
                enemyVolumeInteraction = 0f;
                adjustedEnemyVolume = false;
            }
        }
    }

    #endregion

    #region Loop Check

    private void CheckNoLoop()
    {
        if (actualSourceIndex == 1 && playerAudio.loop == true)
        {
            playerAudio.loop = false;
        }
        
        if (actualSourceIndex == 2 && enemyAudio.loop == true)
        {
            enemyAudio.loop = false;
        }
    }

    #endregion

    #region Play Sound

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

    #endregion

    #region Music Volume - Check For Increase Decrease

    private void CheckForIncreaseMusicVolume()
    {
        if (checkIncreaseMusic == true && playerAudio.isPlaying == false || checkIncreaseMusic == true && enemyAudio.isPlaying == false)
        {
            increaseMusicVolume = true;
            checkIncreaseMusic = false;
        }
    }

    private void CheckForDecreaseMusicVolume()
    {
        if (checkDecreaseMusic == true && playerAudio.isPlaying == false || checkDecreaseMusic == true && enemyAudio.isPlaying == false)
        {
            decreaseMusicVolume = true;
            checkDecreaseMusic = false;
        }
    }

    #endregion

    #region Player Volume - Check For Increase Decrease

    private void CheckForIncreasePlayerVolume()
    {
        if (checkIncreasePlayer == true && playerAudio.isPlaying == false)
        {
            increasePlayerVolume = true;
            checkIncreasePlayer = false;
        }
    }

    private void CheckForDecreasePlayerVolume()
    {
        if (checkDecreasePlayer == true && playerAudio.isPlaying == false)
        {
            decreaseMusicVolume = true;
            checkDecreasePlayer = false;
        }
    }

    #endregion

    #region Enemy Volume - Check For Increase Decrease

    private void CheckForIncreaseEnemyVolume()
    {
        if (checkIncreaseEnemy == true && enemyAudio.isPlaying == false)
        {
            increaseEnemyVolume = true;
            checkIncreaseEnemy = false;
        }
    }

    private void CheckForDecreaseEnemyVolume()
    {
        if (checkDecreaseEnemy == true && enemyAudio.isPlaying == false)
        {
            decreaseEnemyVolume = true;
            checkDecreaseEnemy = false;
        }
    }

    #endregion
}