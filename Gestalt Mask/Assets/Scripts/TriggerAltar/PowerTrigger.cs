using Sirenix.OdinInspector;
using UnityEngine;

public class PowerTrigger : Interactable
{
    [ShowIf("IsNeedMovingPlatform")]
    [OnValueChanged("MarkUsePower")]
    public MovingPlatform BindMovingPlatform;
    protected PowerSourcePickable powerSource;

    public bool IsPowerOn => BindMovingPlatform.HasPower;

    protected virtual bool IsNeedMovingPlatform()
    {
        return true;
    }

    protected override void Interact(PlayerInteractor interactor)
    {
        
    }

    protected void StartPower()
    {
        OnPowerStarted();
        if (BindMovingPlatform)
        {
            BindMovingPlatform.ActivePower();
        }
    }

    protected virtual void OnPowerStarted()
    {
        
    }

    public void StopPower()
    {
        powerSource = null;

        if (BindMovingPlatform)
        {
            BindMovingPlatform.DisablePower();
        }

        OnPowerStopped();
    }

    protected virtual void OnPowerStopped()
    {

    }

    private void MarkUsePower()
    {
        BindMovingPlatform.NeedPower = true;
    }
}
