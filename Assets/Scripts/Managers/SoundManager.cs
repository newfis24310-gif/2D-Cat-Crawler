using UnityEngine;
using Yarn.Unity;
using FMODUnity;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SFXLibrary sfxLibrary;
    [SerializeField] private MusicLibrary musicLibrary;
    public EventInstance currentMusic, currentAmbient;

    void Start()
    {
        PlayMusic(musicLibrary.round1);
    }

    public void PlayCatEntrance()
    {
        RuntimeManager.PlayOneShot(sfxLibrary.catEntrance);
    }

    public void PlayCatRobotMouseMovement()
    {
        RuntimeManager.PlayOneShot(sfxLibrary.robotMouseMovement);
    }
    
    public void PlayBoxMovement()
    {
        RuntimeManager.PlayOneShot(sfxLibrary.boxMovement);
        Debug.Log("Played box sound effect.");
    }

    public void PlayBoxOpen()
    {
        RuntimeManager.PlayOneShot(sfxLibrary.boxOpen);
    }

    public void PlayVacantBox()
    {
        RuntimeManager.PlayOneShot(sfxLibrary.vacantBox);
    }

    public void PlayGas()
    {
        RuntimeManager.PlayOneShot(sfxLibrary.gas);
    }

    public void PlayFindFish()
    {
        RuntimeManager.PlayOneShot(sfxLibrary.findFish);
    }

    public void PlayEatFish()
    {
        RuntimeManager.PlayOneShot(sfxLibrary.eatFish);
    }
    
    public void PlayMusic2ndRound()
    {
       PlayMusic(musicLibrary.round2, 0.3f);
    }


    public void PlayMusic(EventReference musicEvent, float volume = 1f)
    {
        // Stop previous music
        StopMusic();

        // Create new instance
        currentMusic = RuntimeManager.CreateInstance(musicEvent);
        currentMusic.setVolume(volume);
        currentMusic.start();
    }

    [YarnCommand("stopmusic")]
    public void StopMusic()
    {
        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }
    }


    public void PlayAmbienceLab()
    {
       PlayAmbience(sfxLibrary.ambience, 0.3f);
    }

    public void PlayAmbience(EventReference ambientEvent, float volume = 1f)
    {
        StopAmbience();
        currentAmbient = RuntimeManager.CreateInstance(ambientEvent);
        currentAmbient.setVolume(volume);
        currentAmbient.start();
    }

    [YarnCommand("stopambience")]
    public void StopAmbience()
    {
        if (currentAmbient.isValid())
        {
            currentAmbient.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentAmbient.release();
        }
    }
}
