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
        TriggerAltar?.StopPower();
        if (TryGetComponent(out fusionableObj))
        {
            fusionableObj.RemoveIntercpetor(this);
        }
    }

    public void BindToTrigger(TriggerAltar triggerAltar)
    {
        TriggerAltar = triggerAltar;
        if (TryGetComponent(out fusionableObj))
        {
            fusionableObj.AddIntercpetor(this);
        }
    }
}
