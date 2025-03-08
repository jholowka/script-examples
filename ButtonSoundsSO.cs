using UnityEngine;

[CreateAssetMenu(fileName = "ButtonSounds", menuName = "Data/Sounds/Button Sounds")]
public class ButtonSoundsSO : ScriptableObject
{
    [field: SerializeField] public AudioClip HoverSound { get; private set; }
    [field: SerializeField] public AudioClip ActiveSound { get; private set; }
    [field: SerializeField] public AudioClip SelectSound { get; private set; }

}
