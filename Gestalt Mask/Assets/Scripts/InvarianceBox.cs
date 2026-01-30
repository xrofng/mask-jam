using UnityEngine;

public class InvarianceBox : GestaltObj
{

    [SerializeField] MeshRenderer _onEnableCurse_Mesh;
    [SerializeField] Collider _onEnableCurse_Collider;
    [SerializeField] MeshRenderer _onDisableCurse_Mesh;
    [SerializeField] Collider _onDisaCurse_Collider;
    protected override MaskController.ECurse TargetCurse => MaskController.ECurse.Invariance;


    protected override void Start()
    {
        OnCurseDisabled();
    }


    protected override void OnCurseDisabled()
    {
        _onDisableCurse_Mesh.enabled = false;
        _onEnableCurse_Collider.enabled = false;
        _onDisableCurse_Mesh.enabled = true;
        _onDisableCurse_Mesh.enabled = true;
    }

    protected override void OnCurseEnabled()
    {
        _onDisableCurse_Mesh.enabled = true;
        _onEnableCurse_Collider.enabled = true;
        _onDisableCurse_Mesh.enabled = false;
        _onDisableCurse_Mesh.enabled = false;
    }
}
