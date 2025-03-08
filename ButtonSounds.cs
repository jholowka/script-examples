using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SoundEffect))]
public class ButtonSounds : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private ButtonSoundsSO sounds;
    private SoundEffect soundEffect;

    protected virtual void Awake()
    {
        soundEffect = GetComponent<SoundEffect>();
        if (!sounds)
        {
            Debug.LogError($"Error: Need to attach a ButtonSounds Scriptable Object to {gameObject.name}!");
        }
        else
        {
            soundEffect.DisablePlayOnAwake();
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (sounds && sounds.HoverSound)
        {
            soundEffect.PlaySound(sounds.HoverSound);
        }
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        if (sounds && sounds.SelectSound)
        {
            soundEffect.PlaySound(sounds.SelectSound);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (sounds && sounds.ActiveSound)
        {
            soundEffect.PlaySound(sounds.ActiveSound);
        }
    }
}
