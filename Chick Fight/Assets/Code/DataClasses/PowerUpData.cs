using System.Collections.Generic;
using UnityEngine;

namespace Code.DataClasses {

    [System.Serializable]
    public class PowerUpData {
        [Min(1)] public int durationInSeconds = 1;
        public List<PowerUpEffectData> effects;
        public SoundEffectType[] sound;
    }
}