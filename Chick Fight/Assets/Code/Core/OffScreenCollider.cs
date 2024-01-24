using UnityEngine;

namespace Code.Core
{
    public class OffScreenCollider : MonoBehaviour {

        private void OnTriggerEnter2D(Collider2D collision) {

            var character = collision.GetComponentInParent<CharacterController>();
            if (character != null) { 
                GameplayController.Instance.PlayerOffScreen(character);
            }
        }
    }
    
}