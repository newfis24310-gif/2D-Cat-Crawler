using UnityEngine;
using FMODUnity;

[CreateAssetMenu(menuName = "Audio/Music Library")]
public class MusicLibrary : ScriptableObject
{
    public EventReference round1;
    public EventReference round2;
    public EventReference winStinger;
    public EventReference loseStinger;
}