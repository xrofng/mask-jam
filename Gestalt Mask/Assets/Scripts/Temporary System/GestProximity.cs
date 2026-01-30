using System.Collections.Generic;
using UnityEngine;

public class GestProximity : GestaltObj
{
    public List<Interactable> GroupMember;
    public PickableObj ThisPickable;

    protected override MaskController.ECurse TargetCurse => MaskController.ECurse.Proximity;

    private List<Vector3> _memberIdToInitialLocalPos;

    protected override void Awake()
    {
        base.Awake();
        _memberIdToInitialLocalPos = new List<Vector3>();
        foreach (var member in GroupMember)
        {
            _memberIdToInitialLocalPos.Add(member.transform.localPosition);
        }
    }

    protected override void OnCurseEnabled()
    {
        base.OnCurseEnabled();
        foreach (var member in GroupMember)
        {
            member.EnableInteraction();
            member.Collider.enabled = true;
        }
        ThisPickable.DisableInteraction();
    }

    protected override void OnCurseDisabled()
    {
        base.OnCurseDisabled();
        for (int i = 0; i < GroupMember.Count; i++)
        {
            GroupMember[i].DisableInteraction();
            GroupMember[i].transform.localPosition = _memberIdToInitialLocalPos[i];
        }
        ThisPickable.EnableInteraction();
    }
}
