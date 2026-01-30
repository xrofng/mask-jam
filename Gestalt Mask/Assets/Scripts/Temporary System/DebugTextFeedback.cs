using TMPro;
using UnityEngine;

public class DebugTextFeedback : BetterMonoBehaviour, IEventSubcriber<DebugMaskSwitcher.EvsCurseChanged>
{
    private TextMeshProUGUI _text;
    private TextMeshProUGUI Text
    {
        get
        {
            if (_text == null)
                _text = GetComponent<TextMeshProUGUI>();

            return _text;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBusRegister.EventBusSubcribe(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBusRegister.EventBusUnscribe(this);
    }

    public void OnEventBusTrigger(DebugMaskSwitcher.EvsCurseChanged eventType)
    {
        Text.text = "Equip mask " + eventType.NextCurse;
        Invoke(nameof(ClearText), 1);
    }

    public void ClearText()
    {
        Text.text = "";
    }
}
