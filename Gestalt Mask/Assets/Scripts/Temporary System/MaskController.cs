using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaskController : BetterMonoBehaviour
{

    public event Action<List<EcurseMaskState>> OnSetUpList;
    public event Action<ECurse> OnUnLock;
    public event Action<int> OnScrollMovement;
    public event Action<ECurse, bool> OnActive;
    [SerializeField] float scrollThreshold = 0.1f;


    [SerializeField]
    List<EcurseMaskState> eCuseList = new List<EcurseMaskState>()
    {
   new EcurseMaskState(ECurse.Proximity , false),
      new EcurseMaskState(ECurse.Similarity , false),
            new EcurseMaskState(ECurse.Continuance , true),
                        new EcurseMaskState(ECurse.Closure , false),
                        new EcurseMaskState(ECurse.Invariance , false),

    };
    int currentIndex = 2;
    int maxIndex => eCuseList.Count;









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
        Invoke(nameof(SetCurseInitial), float.MinValue);


        OnSetUpList?.Invoke(eCuseList);
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

        handleScrollMovement();
        activeHandle();
        handleUnlockKey();
    }

    void handleUnlockKey()
    {
        foreach (var curse in eCuseList)
        {
            bool beforeKeyState = curse.IsUnLock;
            curse.unlockKeyPress();
            if (beforeKeyState == false && curse.IsUnLock)
            {
                OnUnLock?.Invoke(curse.CurseMask);
            }

        }
    }

    void handleScrollMovement()
    {
        float scroll = Input.mouseScrollDelta.y;

        // Ignore tiny scroll noise
        if (Mathf.Abs(scroll) < scrollThreshold)
            return;

        int dir = (int)Mathf.Sign(scroll);

        int futureIndex = currentIndex + dir;

        if (futureIndex >= maxIndex)
            currentIndex = 0;
        else if (futureIndex < 0)
            currentIndex = maxIndex - 1;
        else
            currentIndex = futureIndex;

        Debug.Log($"Scroll dir: {dir}");
        OnScrollMovement?.Invoke(dir);
    }

    void activeHandle()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            setUpNewCurse();
        }

    }

    void setUpNewCurse()
    {

        bool isSameCurse = (CurrentCurse == eCuseList[currentIndex].CurseMask);

        if (eCuseList.FirstOrDefault(i => i.CurseMask == eCuseList[currentIndex].CurseMask).IsUnLock == false) return;

        // onDisable
        SetCurse(ECurse.None);

        if (isSameCurse)
        {
            OnActive?.Invoke(eCuseList[currentIndex].CurseMask, false);
            return;
        }


        // on Enable
        SetCurse(eCuseList[currentIndex].CurseMask);
        OnActive?.Invoke(CurrentCurse, true);



    }
}
[Serializable]
public class EcurseMaskState
{
    [SerializeField] KeyCode UnLockKeyCode;
    [SerializeField] MaskController.ECurse curseMask;
    [SerializeField] bool isUnLock;

    public EcurseMaskState(MaskController.ECurse curseMask, bool isUnLock)
    {
        this.CurseMask = curseMask;
        this.IsUnLock = isUnLock;
    }

    public MaskController.ECurse CurseMask { get => curseMask; set => curseMask = value; }
    public bool IsUnLock { get => isUnLock; set => isUnLock = value; }

    public void unlockKeyPress()
    {
        if (IsUnLock) return;

        if (Input.GetKeyDown(UnLockKeyCode))
        {
            IsUnLock = true;
        }
    }

}
