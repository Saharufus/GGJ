using UnityEngine;

namespace Code.DataClasses {

    [System.Serializable]
    public class SoundEffectData {
        public SoundEffectType effect;
        public AudioClip[] variations;
    }
}