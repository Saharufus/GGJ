using Code.DataClasses;
using UnityEngine;

namespace Code.Core {
    public class CollisionSoundTrigger : MonoBehaviour {

        [SerializeField] private LayerMask collLayers;
        [SerializeField] private SoundEffectType sound = SoundEffectType.HitBetweenCharacters;

        public void OnCollisionEnter2D(Collision2D collision) {

            if (collision.collider.IsTouchingLayers(collLayers)) {
                SoundSystem.Instance.PlaySound(sound);
            }
            
        }
    }
    
}