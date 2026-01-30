using UnityEngine;
using static MaskController;

public class GestaltObj : BetterMonoBehaviour, IEventSubcriber<EvsCurseChanged>
{
    protected virtual ECurse TargetCurse => ECurse.None;
    public ECurse changedCurse => TargetCurse;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.AddSubcriber<EvsCurseChanged>(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.RemoveSubcriber<EvsCurseChanged>(this);
    }

    public void OnEventBusTrigger(EvsCurseChanged e)
    {
        var target = TargetCurse;

        if (target == ECurse.None)
            return;

        // ⭐ Curse Enabled
        if (e.NextCurse == target)
        {
            OnCurseEnabled();
            return;
        }

        // ⭐ Curse Disabled
        if (e.PrevCurse == target && e.NextCurse != target)
        {
            OnCurseDisabled();
        }
    }
    
    /// <summary>
    /// Called when the target curse becomes active.
    /// </summary>
    protected virtual void OnCurseEnabled()
    {
        Debug.Log($"{name} reacted to {TargetCurse}");
    }

    /// <summary>
    /// Called when the target curse is no longer active.
    /// </summary>
    protected virtual void OnCurseDisabled()
    {
        Debug.Log($"{name} stopped reacting to {TargetCurse}");
    }
}
