using UnityEngine;

namespace Code.DataClasses {

    [System.Serializable]
    public class CharacterData {
        [Min(1)] public float jumpForce;
        [Min(1)] public float rotationSpeed;
        [Min(0)] public float jumpXFriction;
    }
}