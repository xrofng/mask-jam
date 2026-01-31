using System.Collections.Generic;
using UnityEngine;

public class GestProximity : GestaltObj
{
    public List<FusionableObj> GroupMember;

    protected override MaskController.ECurse TargetCurse => MaskController.ECurse.Proximity;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnCurseEnabled()
    {
        base.OnCurseEnabled();
        foreach (FusionableObj member in GroupMember)
        {
            if(Vector3.Distance(member.transform.position, this.transform.position ) > 2)
            {
                member.transform.position = Random.insideUnitSphere + transform.position;
            }
            member.Pickable.ShowObject();
            member.transform.parent = null;
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
