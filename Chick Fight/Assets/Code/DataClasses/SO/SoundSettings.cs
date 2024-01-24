using Code.DataClasses;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSettings", menuName = "Data/SoundSettings")]
public class SoundSettings : ScriptableObject {

    [Header("Music")]
    public LayerMask menuMusic;
    public LayerMask gameMusic;

    [Header("Sound Effects")]
    public SoundEffectType powerUpSettings;
}