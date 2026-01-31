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

    public ECurse InitialCurse;
    public ECurse CurrentCurse;

    protected override void Start()
    {
        base.Start();
        Invoke(nameof(SetCurseInitial), float.MinValue);
    }

    private void SetCurseInitial()
    {
        SetCurse(InitialCurse);
    }

    public void SetCurse(ECurse curse)
    {
        // when successfully set curse
        EventBus.TriggerEvent(new EvsCurseChanged(curse, CurrentCurse));
        CurrentCurse = curse;
    }

    protected override void Update()
    {
        base.Update();


    }
}

