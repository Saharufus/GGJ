using Code.DataClasses;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplaySettings", menuName = "Data/GameplaySettings")]
public class GameplaySettings : ScriptableObject {

    [Header("Level Settings")]
    public LayerMask whatIsGround;
    public LayerMask whatIsCharacter;
    public LayerMask whatIsPowerUp;

    [Header("Gameplay Settings")]
    [Range(5, 120)] public int matchDurationInSeconds;

    [Header("Character Settings")]
    public CharacterData characterSettings;

    [Header("PowerUp Settings")]
    public PowerUpsData powerUpSettings;
    [Range(1, 120)] public int powerUpDurtaionInSeconds;
    [Range(0, 20)] public float powerupSpawnTime;
}