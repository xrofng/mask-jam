using UnityEngine;
using UnityEngine.Events;
public class ClosureBox : GestaltObj
{
    [SerializeField] UnityEvent onEnable, onDisable;
    protected override MaskController.ECurse TargetCurse => MaskController.ECurse.Closure;


    protected override void Awake()
    {
        OnCurseDisabled();
    }


    protected override void OnCurseDisabled()
    {
        onEnable?.Invoke();
    }

    protected override void OnCurseEnabled()
    {
        onDisable?.Invoke();
    }
}
