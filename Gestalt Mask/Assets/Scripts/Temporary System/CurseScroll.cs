using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurseScroll : BetterMonoBehaviour
{
    [SerializeField] float disableTime = 3;
    [SerializeField] MaskController maskController;
    [SerializeField] CanvasGroup CanvasGroup;
    [SerializeField]
    List<eCurseToSprite> eCurseToSprites = new List<eCurseToSprite>()
    {
      new eCurseToSprite(MaskController.ECurse.Continuance,null),
        new eCurseToSprite(MaskController.ECurse.Proximity,null),
       new eCurseToSprite(MaskController.ECurse.Closure,null),
     new eCurseToSprite(MaskController.ECurse.Similarity,null),
      new eCurseToSprite(MaskController.ECurse.Invariance,null),
    };


    [SerializeField] List<RectTransform> boxPosition = new List<RectTransform>() { null, null, null, null, null };
    List<Vector3> boxPos = new List<Vector3>();

    List<CurseBox_Ui> curseBox_Uis = new List<CurseBox_Ui>();
    int activeECurseIndex = -1;




    protected override void Awake()
    {
        if (maskController == null)
        {
            maskController = FindAnyObjectByType<MaskController>();
        }


        maskController.OnActive += onActiveUi;
        maskController.OnScrollMovement += moveIndex;
        maskController.OnSetUpList += onSetUpList;
        maskController.OnUnLock += UnLock;

        foreach (Transform i in boxPosition)
        {
            if (i.TryGetComponent<CurseBox_Ui>(out CurseBox_Ui box))
            {
                curseBox_Uis.Add(box);
            }
        }
    }






    void onSetUpList(List<EcurseMaskState> curses)
    {
        for (int number = 0; number < curses.Count; number++)
        {


            Sprite getSprite = eCurseToSprites.FirstOrDefault(i => i.Curse == curses[number].CurseMask).Sprite;

            curseBox_Uis[number].SetUp(curses[number].CurseMask, number, getSprite);
            boxPos.Add(boxPosition[number].position);
            curseBox_Uis[number].DisActiveEffect();
        }
        CanvasGroup.alpha = 0;
    }

    void moveIndex(int direction)
    {
        StopAllCoroutines();
        StartCoroutine(routine());
        if (direction == 0) return;
        Debug.Log($"move all {direction}");
        foreach (CurseBox_Ui box in curseBox_Uis)
        {
            box.moveCurrentIndex(direction, eCurseToSprites.Count);
            box.UpdatePos(boxPos[box.CurrentIndexPosition]);
        }
    }

    void UnLock(MaskController.ECurse triggerUnlokcCurse)
    {
        StopAllCoroutines();
        StartCoroutine(routine());
        curseBox_Uis.FirstOrDefault(i => i.BoxCurseType == triggerUnlokcCurse).UpdateState(true);
    }


    IEnumerator routine()
    {
        CanvasGroup.alpha = 1;
        yield return new WaitForSeconds(disableTime);
        CanvasGroup.alpha = 0;
    }

    MaskController.ECurse previosActiveCurse = MaskController.ECurse.None;
    void onActiveUi(MaskController.ECurse setCurse, bool Active)
    {
        StopAllCoroutines();
        StartCoroutine(routine());
        Debug.Log($"{setCurse} : {Active}");
        if (Active)
        {
            if (previosActiveCurse != MaskController.ECurse.None && Active)
            {
                curseBox_Uis.FirstOrDefault(i => i.BoxCurseType == previosActiveCurse).DisActiveEffect();
                previosActiveCurse = MaskController.ECurse.None;
            }


            curseBox_Uis.FirstOrDefault(i => i.BoxCurseType == setCurse)?.ActiveEffect();
            previosActiveCurse = setCurse;
        }
        else
        {

            curseBox_Uis.FirstOrDefault(i => i.BoxCurseType == setCurse).DisActiveEffect();

        }

    }
}

[System.Serializable]
public class eCurseToSprite
{
    [SerializeField] MaskController.ECurse curse;
    [SerializeField] Sprite sprite;
    public eCurseToSprite(MaskController.ECurse curse, Sprite sprite)
    {
        this.Curse = curse;
        this.Sprite = null;
    }

    public MaskController.ECurse Curse { get => curse; set => curse = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
}
