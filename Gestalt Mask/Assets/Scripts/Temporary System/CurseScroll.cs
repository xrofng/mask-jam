using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CurseScroll : BetterMonoBehaviour
{


    [SerializeField] MaskController maskController;
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
        maskController.OnActive += onActiveUi;
        maskController.OnScrollMovement += moveIndex;
        maskController.OnSetUpList += onSetUpList;


        foreach (Transform i in boxPosition)
        {
            if (i.TryGetComponent<CurseBox_Ui>(out CurseBox_Ui box))
            {
                curseBox_Uis.Add(box);
            }
        }
    }


    void onSetUpList(List<MaskController.ECurse> curses)
    {
        for (int number = 0; number < curses.Count; number++)
        {


            Sprite getSprite = eCurseToSprites.FirstOrDefault(i => i.Curse == curses[number]).Sprite;

            curseBox_Uis[number].SetUp(curses[number], number, getSprite);
            boxPos.Add(boxPosition[number].position);

        }
    }

    void moveIndex(int direction)
    {
        if (direction == 0) return;
        Debug.Log($"move all {direction}");
        foreach (CurseBox_Ui box in curseBox_Uis)
        {
            box.moveCurrentIndex(direction, eCurseToSprites.Count);
            box.UpdatePos(boxPos[box.CurrentIndexPosition]);
        }
    }

    void onActiveUi(MaskController.ECurse setCurse, bool Active)
    {

        if (Active)
        {

        }
        else
        {

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
