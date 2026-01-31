using UnityEngine;

public class PlayerSound : BetterMonoBehaviour, IEventSubcriber<MaskController.EvsCurseChanged>
{
    public SimpleMMSoundPlayer MotionPauseSfx;
    public SimpleMMSoundPlayer SelectMask;

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

    public void OnEventBusTrigger(MaskController.EvsCurseChanged eventType)
    {
        if (eventType.NextCurse == MaskController.ECurse.Continuance)
        {
            MotionPauseSfx?.PlayClip();
        }
        else
        {
            SelectMask?.PlayClip();
        }
    }
}
