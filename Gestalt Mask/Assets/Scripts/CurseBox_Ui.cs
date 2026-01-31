using UnityEngine;
using UnityEngine.UI;

public class CurseBox_Ui : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image backImage;
    MaskController.ECurse boxCurseType;


    RectTransform rect;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }



    int currentIndexPosition = -1;
    public MaskController.ECurse BoxCurseType { get => boxCurseType; set => boxCurseType = value; }
    public int CurrentIndexPosition { get => currentIndexPosition; set => currentIndexPosition = value; }

    public void moveCurrentIndex(int Direction, int maxRange)
    {
        int futureDirection = currentIndexPosition - Direction;

        if (futureDirection < 0)
        {
            currentIndexPosition = maxRange - 1;
        }
        else if (futureDirection >= maxRange)
        {
            currentIndexPosition = 0;
        }
        else
        {
            currentIndexPosition = futureDirection;

        }
    }

    public void UpdatePos(Vector3 newPosition)
    {
        rect.transform.position = newPosition;
    }


    public void SetUp(MaskController.ECurse boxCurseType, int index, Sprite sprite)
    {
        currentIndexPosition = index;
        image.sprite = sprite;
        this.BoxCurseType = boxCurseType;
    }

    public void UpdateState(bool lockState)
    {
        if (lockState == true)
        {

        }
        else
        {

        }
    }


    public void ActiveEffect()
    {
        image.enabled = true;
        backImage.enabled = true;
        image.color = Color.white;
    }

    public void DisActiveEffect()
    {
        image.enabled = true;
        image.color = Color.black;
        backImage.enabled = false;

    }



}
