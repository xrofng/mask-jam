using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : BetterMonoBehaviour
{
    public event Action<List<ECurse>> OnSetUpList;
    public event Action<int> OnScrollMovement;
    public event Action<ECurse, bool> OnActive;
    [SerializeField] float scrollThreshold = 0.1f;


    [SerializeField] List<ECurse> eCuseList = new List<ECurse>();
    int currentIndex;
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
        if (eCuseList.Count == 0)
        {
            setUpList();
        }

        OnSetUpList?.Invoke(eCuseList);
    }
    [Button("set up List")]
    void setUpList()
    {
        foreach (ECurse value in Enum.GetValues(typeof(ECurse)))
        {
            if (value == ECurse.None) continue;
            eCuseList.Add(value);
        }
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
        OnScrollMovement.Invoke(dir);
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

        bool isSameCurse = (CurrentCurse == eCuseList[currentIndex]);
        // onDisable
        SetCurse(ECurse.None);
        if (isSameCurse)
        {
            OnActive?.Invoke(CurrentCurse, false);
            return;
        }


        // on Enable
        SetCurse(eCuseList[currentIndex]);
        OnActive?.Invoke(CurrentCurse, true);



    }
}

