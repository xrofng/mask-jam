using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class TriggerAltar : PowerTrigger
{
    [Header("Setting")]
    public Color powerOnColor;

    public enum EMode
    {
        NormalTrigger,
        SubTrigger,
    }
    public EMode Mode;

    [ShowIf("ModeIsSub")]
    public AltarGroup BindAltarGroup;
    private bool ModeIsNormal => Mode == EMode.NormalTrigger;
    private bool ModeIsSub => Mode == EMode.SubTrigger;

    public bool SpecifyKey;
    [ShowIf("SpecifyKey")]
    public string Key;
    public Transform PosMarker;

    [Header("Obj Ref")]
    public MeshRenderer[] DetectorMesh;
    private Color _iColor;

    protected override void Awake()
    {
        base.Awake();
        _iColor = DetectorMesh[0].material.GetColor("_BaseColor");
    }

    protected override void Interact(PlayerInteractor interactor)
    {
        if (interactor.PickUp.Holding && powerSource == null)
        {
            powerSource = interactor.PickUp.ForceDrop() as PowerSourcePickable;

            if (powerSource != null)
            {
                if (SpecifyKey)
                {
                    if (Key == powerSource.Key)
                    {
                        StartPower();
                    }
                }
                else
                {
                    StartPower();
                }
                OnPowerPlaced();
            }
        }
    }

    private void OnPowerPlaced()
    {
        powerSource.transform.position = PosMarker.transform.position;
        powerSource.transform.parent = PosMarker;
        powerSource.SetRigibody(false);
        powerSource.BindToTrigger(this);

        if (BindAltarGroup)
        {
            BindAltarGroup.UpdatePower();
        }
        else
        {
            UpdateSignifier();
        }

    }

    public void UpdateSignifier()
    {
        foreach (var m in DetectorMesh)
        {
            m.material.SetColor("_BaseColor", IsPowerOn ? powerOnColor : _iColor);
        }
    }

    protected override void OnPowerStarted()
    {
        base.OnPowerStarted();
        
    }

    protected override void OnPowerStopped()
    {
        base.OnPowerStopped();
        if (BindAltarGroup)
        {
            BindAltarGroup.UpdatePower();
        }
        UpdateSignifier();
    }

    protected override bool IsNeedMovingPlatform()
    {
        return ModeIsNormal;
    }
}
