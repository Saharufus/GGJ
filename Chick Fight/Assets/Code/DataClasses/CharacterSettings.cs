using UnityEngine;

[System.Serializable]
public class CharacterSettings {
    [Min(1)] public float jumpForce;
    [Min(1)] public float rotationSpeed;
    [Min(0)] public float maxRotationDegree;
    [Min(0)] public float jumpXFriction;
}