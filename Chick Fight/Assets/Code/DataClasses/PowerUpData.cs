using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpData {
    [Min(1)] public int durationInSeconds = 1;
    public List<PowerUpEffectData> effects;
    [Min(0)] public float sound;
}