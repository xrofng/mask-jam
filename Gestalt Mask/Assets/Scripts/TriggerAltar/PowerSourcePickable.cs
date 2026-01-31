using System;
using UnityEngine;

public class PowerSourcePickable : PickableObj
{
    TriggerAltar TriggerAltar;

    public string Key;

    protected override void OnPickedUp()
    {
        base.OnPickedUp();
        TriggerAltar?.StopPower();
    }

    public void BindToTrigger(TriggerAltar triggerAltar)
    {
        TriggerAltar = triggerAltar;
    }
}
