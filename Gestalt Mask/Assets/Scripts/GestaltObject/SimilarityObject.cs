using UnityEngine;

public class SimilarityObject : GestaltObj
{
    [SerializeField] Color RealColor;
    [SerializeField] MeshRenderer MeshRenderer;
    protected override MaskController.ECurse TargetCurse => MaskController.ECurse.Similarity;

    private Color _initialColor;

    protected override void Start()
    {
        base.Start();
        _initialColor = MeshRenderer.material.GetColor("_BaseColor");
    }

    protected override void OnCurseEnabled()
    {
        base.OnCurseEnabled();
        MeshRenderer.material.SetColor("_BaseColor", RealColor);
    }

    protected override void OnCurseDisabled()
    {
        base.OnCurseDisabled();
        MeshRenderer.material.SetColor("_BaseColor", _initialColor);
    }
}
