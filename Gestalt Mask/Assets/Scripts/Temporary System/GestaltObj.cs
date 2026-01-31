using System.Collections;
using UnityEngine;
using static MaskController;

public class GestaltObj : BetterMonoBehaviour, IEventSubcriber<EvsCurseChanged>
{
    protected virtual ECurse TargetCurse => ECurse.None;
    public ECurse changedCurse => TargetCurse;

    #region
    private MeshRenderer[] _renderers;
    private MeshRenderer[] Renderers
    {
        get
        {
            if (_renderers == null)
            {
                _renderers = GetComponentsInChildren<MeshRenderer>(true);
            }

            return _renderers;
        }
    }

    private Collider[] _colliders;
    private int subscribeDelay = 1;

    private Collider[] Colliders
    {
        get
        {
            if (_colliders == null)
            {
                _colliders = GetComponentsInChildren<Collider>(true);
            }

            return _colliders;
        }
    }
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(SubscribeRoutine(subscribeDelay));
    }

    IEnumerator SubscribeRoutine(int frameDelay)
    {
        for (int i  = 0; i < frameDelay; i++)
        {
            yield return null;
        }
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

    private void SetObjectState(bool en)
    {
        foreach (var renderer in Renderers)
            renderer.enabled = en;

        foreach (var col in Colliders)
            col.enabled = en;

        enabled = en;
    }

    public void ShowObject()
    {
        SetObjectState(true);
    }

    public void HideObject()
    {
        SetObjectState(false);
    }
}
