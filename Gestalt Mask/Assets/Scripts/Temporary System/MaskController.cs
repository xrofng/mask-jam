using UnityEngine;

public class MaskController : BetterMonoBehaviour
{
    public enum ECurse
    {
        None,
        Proximity,
        Similarity,
        Continuance,
        Closure,
        Invariance
    }

    public struct EvsCurseChanged
    {
        public ECurse NextCurse;
        public ECurse PrevCurse;

        public EvsCurseChanged(ECurse nextCurse, ECurse prevCurse)
        {
            NextCurse = nextCurse;
            PrevCurse = prevCurse;
        }
    }

    public ECurse CurrentCurse;

    public void SetCurse(ECurse curse)
    {
        // when successfully set curse
        EventBus.TriggerEvent(new EvsCurseChanged(curse, CurrentCurse));
        CurrentCurse = curse;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetCurse(ECurse.None);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            SetCurse(ECurse.Proximity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetCurse(ECurse.Similarity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetCurse(ECurse.Continuance);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetCurse(ECurse.Closure);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetCurse(ECurse.Invariance);
        }
    }
}

