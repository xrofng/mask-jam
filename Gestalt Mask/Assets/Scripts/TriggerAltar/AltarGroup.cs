using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class AltarGroup : PowerTrigger
{
    public List<TriggerAltar> RequiredTrigger = new List<TriggerAltar>();

    public void UpdatePower()
    {
        int count = 0;
        foreach (TriggerAltar trigger in RequiredTrigger)
        {
            if (trigger.IsPowerOn == false)
            {
                if (BindMovingPlatform.HasPower)
                {
                    StopPower();
                }
                Debug.Log(name + " has " + count + " trigger power on");
                return;
            }
            count += 1;
        }
        StartPower();
    }

    protected override void OnPowerStarted()
    {
        base.OnPowerStarted();
        foreach (TriggerAltar trigger in RequiredTrigger)
        {
            trigger.UpdateSignifier();
        }
    }

    protected override void OnPowerStopped()
    {
        base.OnPowerStopped();
        foreach (TriggerAltar trigger in RequiredTrigger)
        {
            trigger.UpdateSignifier();
        }
    }

    [Button("Bind child trigger")]
    public void AutoBind()
    {
        RequiredTrigger.Clear();
        foreach (TriggerAltar triggerAltar  in GetComponentsInChildren<TriggerAltar>())
        {
            triggerAltar.Mode = TriggerAltar.EMode.SubTrigger;
            triggerAltar.BindAltarGroup = this;
            RequiredTrigger.Add(triggerAltar);
        }
    }
}
