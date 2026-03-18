using UnityEngine;
using Yarn.Unity;
using FMODUnity;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SFXLibrary sfxLibrary;
    [SerializeField] private MusicLibrary musicLibrary;
    private EventInstance currentMusic;

    void Start()
    {
        PlayMusic(musicLibrary.round1);
    }

    public void PlayBoxOpen()
    {
        RuntimeManager.PlayOneShot(sfxLibrary.boxOpen);
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
}
