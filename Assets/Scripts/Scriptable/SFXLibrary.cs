using UnityEngine;
using FMODUnity;

[CreateAssetMenu(menuName = "Audio/SFX Library")]
public class SFXLibrary : ScriptableObject
{
    public EventReference boxMovement;
    public EventReference boxOpen;
    public EventReference vacantBox;
    public EventReference catEntrance;
    public EventReference eatFish;
    public EventReference findFish;
    public EventReference robotMouseMovement;
    public EventReference gas;
    public EventReference ambience;
}