using System;
using System.Collections.Generic;
using UnityEngine;

public class GestProximity : GestaltObj
{
    public List<GameObject> GroupMember;

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
            member.gameObject.SetActive(true);
            member.transform.parent = null;
        }
        Invoke(nameof(HideThisObj), float.MinValue);
    }

    private void HideThisObj()
    {
        this.gameObject.SetActive(false);
    }

    protected override void OnCurseDisabled()
    {
        base.OnCurseDisabled();
        for (int i = 0; i < GroupMember.Count; i++)
        {
            GroupMember[i].transform.localPosition = _memberIdToInitialLocalPos[i];
        }
    }

    public void AddMember(GameObject newMember)
    {
        GroupMember.Add(newMember);
    }
}
