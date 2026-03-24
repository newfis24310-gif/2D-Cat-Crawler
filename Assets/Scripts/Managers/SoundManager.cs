using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using FMODUnity;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    
    [SerializeField] private SFXLibrary sfxLibrary;
    [SerializeField] private MusicLibrary musicLibrary;
    private VCA musicVCA;
    private VCA sfxVCA;
    private VCA ambienceVCA;
    private VCA masterVCA;
    private EventInstance currentMusic;
    public EventInstance currentAmbient;
    public Slider masterSlider, musicSlider, sfxSlider, ambienceSlider;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Κάνουμε το SoundManager να μην καταστραφεί κατά τη φόρτωση νέας σκηνής
        } 
        else Destroy(gameObject);
    }

    void Start()
    {
        musicVCA = RuntimeManager.GetVCA("vca:/Music");
        sfxVCA = RuntimeManager.GetVCA("vca:/SFX");
        ambienceVCA = RuntimeManager.GetVCA("vca:/Ambience");
        masterVCA = RuntimeManager.GetVCA("vca:/Master");
        PlayMusic(musicLibrary.round1); // Ξεκινάμε με τη μουσική του πρώτου γύρου
    }

    public void PlayCatEntrance() => RuntimeManager.PlayOneShot(sfxLibrary.catEntrance);
    public void PlayCatRobotMouseMovement() => RuntimeManager.PlayOneShot(sfxLibrary.robotMouseMovement);
    public void PlayBoxMovement() => RuntimeManager.PlayOneShot(sfxLibrary.boxMovement);
    public void PlayBoxOpen() => RuntimeManager.PlayOneShot(sfxLibrary.boxOpen);
    public void PlayVacantBox() => RuntimeManager.PlayOneShot(sfxLibrary.vacantBox);
    public void PlayGas() => RuntimeManager.PlayOneShot(sfxLibrary.gas);
    public void PlayFindFish() => RuntimeManager.PlayOneShot(sfxLibrary.findFish);
    public void PlayEatFish() => RuntimeManager.PlayOneShot(sfxLibrary.eatFish);
    

    public void PlayMusic2ndRound() => PlayMusic(musicLibrary.round2, 0.3f);
    public void PlayAmbienceLab() => PlayAmbience(sfxLibrary.ambience, 0.3f);
   

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

    public void SetMusicVolume()
    {
        float adjusted = Mathf.Pow(Mathf.Clamp(musicSlider.value, 0.0001f, 1f), 2.2f);   //BEST METHOD
        Debug.Log($"Slider: {musicSlider.value} → Adjusted Volume: {adjusted}");
        musicVCA.setVolume(adjusted);
    }

    public void SetSFXVolume()
    {
        float adjusted = Mathf.Pow(Mathf.Clamp(sfxSlider.value, 0.0001f, 1f), 2.2f);
        sfxVCA.setVolume(adjusted);
    }
    public void SetAmbienceVolume()
    {
        float adjusted = Mathf.Pow(Mathf.Clamp(ambienceSlider.value, 0.0001f, 1f), 2.2f);
        ambienceVCA.setVolume(adjusted);
    }

    public void SetMasterVolume()
    {
        float adjusted = Mathf.Pow(Mathf.Clamp(masterSlider.value, 0.0001f, 1f), 2.2f);
        masterVCA.setVolume(adjusted);
    }
}
