using Sirenix.OdinInspector;
using UnityEngine;

public class PowerTrigger : Interactable
{
    [ShowIf("IsNeedMovingPlatform")]
    [OnValueChanged("MarkUsePower")]
    public MovingPlatform[] BindMovingPlatform;
    protected PowerSourcePickable powerSource;
    private bool _isPowerOn;

    public bool IsPlatformMove => BindMovingPlatform[0].HasPower;
    public bool IsPowerOn => _isPowerOn;

    protected virtual bool IsNeedMovingPlatform()
    {
        return true;
    }

    protected override void Interact(PlayerInteractor interactor)
    {
        
    }

    protected void StartPower()
    {
        _isPowerOn = true;
        OnPowerStarted();
        if (BindMovingPlatform.Length > 0)
        {
            foreach (var platform in BindMovingPlatform)
            {
                platform.ActivePower();
            }
        }
    }

    protected virtual void OnPowerStarted()
    {
        
    }

    public void StopPower()
    {
        _isPowerOn = false;
        powerSource = null;

        if (BindMovingPlatform.Length > 0)
        {
            foreach (var platform in BindMovingPlatform)
            {
                platform.DisablePower();
            }
        }

        OnPowerStopped();
    }

    protected virtual void OnPowerStopped()
    {

    }

    private void MarkUsePower()
    {
        if (BindMovingPlatform.Length > 0)
        {
            foreach (var platform in BindMovingPlatform)
            {
                platform.NeedPower = true;
            }
        }
    }
}
