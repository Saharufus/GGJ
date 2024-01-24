using Code.DataClasses;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Core {

    public class PowerUp : MonoBehaviour {

        public List<PowerUpEffectData> _stats;

        public void Init(PowerUpData data) {

            _stats = data.effects;

            Destroy(gameObject, Time.time + data.durationInSeconds);
        }
    }
}