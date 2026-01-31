using UnityEngine;
using UnityEngine.Events;
public class ClosureBox : GestaltObj
{
    [SerializeField] UnityEvent onCurseDisable, onCurseEnable;
    protected override MaskController.ECurse TargetCurse => MaskController.ECurse.Closure;


    protected override void Awake()
    {
        OnCurseDisabled();
    }


    protected override void OnCurseDisabled()
    {
        onCurseDisable?.Invoke();
    }

    protected override void OnCurseEnabled()
    {
        onCurseEnable?.Invoke();
    }
}
