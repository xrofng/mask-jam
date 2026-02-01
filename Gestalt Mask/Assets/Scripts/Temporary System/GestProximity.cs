using System.Collections.Generic;
using UnityEngine;

public class GestProximity : GestaltObj
{
    public List<FusionableObj> GroupMember;

    protected override MaskController.ECurse TargetCurse => MaskController.ECurse.Proximity;

    public SimpleMMSoundPlayer OnDeFusionSFX;

    public SimpleMMSoundPlayer OnSpawnSFX;

    protected override void Awake()
    {
        base.Awake();
        OnSpawnSFX?.PlayClip();
    }

    protected override void OnCurseEnabled()
    {
        base.OnCurseEnabled();
        foreach (FusionableObj member in GroupMember)
        {
            Vector3 rand = Random.insideUnitSphere;
            rand.y = Mathf.Abs(rand.y);
            member.transform.position = rand + transform.position + Vector3.one;
            member.Pickable.ShowObject();
            member.transform.parent = null;
            OnDeFusionSFX?.PlayClip();
        }
        GroupMember.Clear();
        Invoke(nameof(HideThisObj), float.MinValue);
    }

    private void HideThisObj()
    {
        HideObject();
        Destroy(this.gameObject);
    }

    protected override void OnCurseDisabled()
    {
        base.OnCurseDisabled();
        
    }

    public void AddMember(FusionableObj newMember)
    {
        GroupMember.Add(newMember);
    }
}
