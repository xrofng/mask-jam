using UnityEngine;
using UnityEngine.Events;

public class InvarianceBox : GestaltObj
{

    [SerializeField] UnityEvent onDisable;

    [SerializeField] UnityEvent onEnable;


    protected override MaskController.ECurse TargetCurse => MaskController.ECurse.Invariance;





    protected override void OnCurseDisabled()
    {

        onDisable?.Invoke();
    }

    protected override void OnCurseEnabled()
    {
        onEnable?.Invoke();
    }
}
