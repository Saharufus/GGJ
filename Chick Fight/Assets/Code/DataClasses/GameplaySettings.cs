using UnityEngine;

[CreateAssetMenu(fileName = "GameplaySettings", menuName = "Data/GameplaySettings")]
public class GameplaySettings : ScriptableObject {

    [Header("Level Settings")]
    public LayerMask whatIsGround;
    public LayerMask whatIsCharacter;
    public LayerMask whatIsPowerUp;

    [Header("Gameplay Settings")]
    [Range(5, 120)] public int matchDurationInSeconds;
    [Range(1, 120)] public int powerUpDurtaionInSeconds;

    [Header("Character Settings")]
    public CharacterSettings characterSettings;
}