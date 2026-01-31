using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class AltarGroup : PowerTrigger
{
    public List<TriggerAltar> RequiredTrigger = new List<TriggerAltar>();

    public void UpdatePower()
    {
        Debug.Log(RequiredTrigger);
        foreach (TriggerAltar trigger in RequiredTrigger)
        {
            if (trigger.IsPowerOn == false)
            {
                if (BindMovingPlatform.HasPower)
                {
                    StopPower();
                }
                return;
            }
        }
        StartPower();
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
    //override 
}
