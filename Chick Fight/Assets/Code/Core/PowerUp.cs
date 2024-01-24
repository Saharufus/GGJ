using Code.DataClasses;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Core {

    public class PowerUp : MonoBehaviour {

        private List<PowerUpEffectData> _stats;

        public void Init(List<PowerUpEffectData> stats) {

            _stats = stats;
        }

        public void End() { 
            
            gameObject.SetActive(false);
        }
    }
}