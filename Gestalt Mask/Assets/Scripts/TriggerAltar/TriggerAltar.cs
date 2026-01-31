using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerAltar : PowerTrigger
{
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

    public Transform PosMarker;


    protected override void Interact(PlayerInteractor interactor)
    {
        if (interactor.PickUp.Holding && powerSource == null)
        {
            powerSource = interactor.PickUp.ForceDrop() as PowerSourcePickable;

            if (powerSource != null)
            {
                StartPower();
            }
        }
    }

    protected override void OnPowerStarted()
    {
        base.OnPowerStarted();
        powerSource.transform.position = PosMarker.transform.position;
        powerSource.transform.parent = PosMarker;
        powerSource.SetRigibody(false);
        powerSource.BindToTrigger(this);

        if (BindAltarGroup)
        {
            BindAltarGroup.UpdatePower();
        }
    }

    protected override void OnPowerStopped()
    {
        base.OnPowerStopped();
        if (BindAltarGroup)
        {
            BindAltarGroup.UpdatePower();
        }
    }

    protected override bool IsNeedMovingPlatform()
    {
        return ModeIsNormal;
    }
}
