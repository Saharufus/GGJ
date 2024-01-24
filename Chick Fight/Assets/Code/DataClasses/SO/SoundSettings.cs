using Code.DataClasses;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSettings", menuName = "Data/SoundSettings")]
public class SoundSettings : ScriptableObject {

    [Header("Music")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("Sound")]
    public List<SoundEffectData> effects;
}