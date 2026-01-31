using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : BetterMonoBehaviour, IEventSubcriber<Crosshair.EvsPlayerAim>
{
    public Image CrosshairImage;
    public Color NormalColor = Color.white;
    public Color HighlightedColor = Color.white;
    public Color PressedColor = Color.white;

    public Color _currColor;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBusRegister.EventBusSubcribe(this);
        Cursor.visible = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBusRegister.EventBusUnscribe(this);
    }

    public void OnEventBusTrigger(EvsPlayerAim eventType)
    {
        
        if (eventType.State == SelectionState.Pressed)
        {

            StartCoroutine(ColorBlinkRoutine());
        }
        else
        {
            _currColor = eventType.Interactable ? HighlightedColor : NormalColor;
            CrosshairImage.color = _currColor;
        }
    }

    IEnumerator ColorBlinkRoutine(float blinkDura =.5f)
    {
        CrosshairImage.color = PressedColor;
        yield return new WaitForSeconds(blinkDura);
        CrosshairImage.color = _currColor;
    }

    public enum SelectionState
    {
        Normal,
        Highlighted,
        Pressed,
        Selected,
        Disabled,
    }

    public struct EvsPlayerAim
    {
        public SelectionState State;
        public Interactable Interactable;

        public EvsPlayerAim(SelectionState state, Interactable interactable)
        {
            State = state;
            Interactable = interactable;
        }
    }
}
