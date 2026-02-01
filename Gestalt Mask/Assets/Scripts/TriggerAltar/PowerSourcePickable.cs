using System;
using UnityEngine;

public class PowerSourcePickable : PickableObj
{
    TriggerAltar TriggerAltar;

    public string Key;

    private FusionableObj fusionableObj;

    protected override void OnPickedUp()
    {
        base.OnPickedUp();
        if (TriggerAltar != null)
        {
            TriggerAltar?.StopPower();
            SetObjectFreeze(false);
        }
        if (TryGetComponent(out fusionableObj))
        {
            fusionableObj.RemoveIntercpetor(this);
        }
    }

    public void GivePower(TriggerAltar triggerAltar)
    {
        TriggerAltar = triggerAltar;
        if (TryGetComponent(out fusionableObj))
        {
            fusionableObj.AddIntercpetor(this);
        }
        SetObjectFreeze(true);
    }

    public void SetObjectFreeze(bool isFreeze)
    {
        //SetColliderActive(isFreeze == false);
        SetRigibody(isFreeze == false);
    }
}
