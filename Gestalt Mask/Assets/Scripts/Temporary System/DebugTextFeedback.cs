using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class DebugTextFeedback : BetterMonoBehaviour, IEventSubcriber<MaskController.EvsCurseChanged>
{
    [SerializeField] List<objectPairWithEnum<MaskController.ECurse>> frames = new List<objectPairWithEnum<MaskController.ECurse>>();
    [SerializeField] MoreMountains.Feedbacks.MMF_Player onOpen;
    [SerializeField] MoreMountains.Feedbacks.MMF_Player onClose;
    [SerializeField] float openDelay;
    [SerializeField] float closeDelay;
    [SerializeField] float deselectDelay;
    bool canCancel;
    bool isOpen;
    bool isFinishEffect;
    objectPairWithEnum<MaskController.ECurse> previosOpenItem;
    Coroutine coroutine;

    objectPairWithEnum<MaskController.ECurse> desActivePlayEffect;
    objectPairWithEnum<MaskController.ECurse> activePlayEffect;

    protected override void OnEnable()
    {


        base.OnEnable();
        EventBusRegister.EventBusSubcribe(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBusRegister.EventBusUnscribe(this);
    }



    public void OnEventBusTrigger(MaskController.EvsCurseChanged eventType)
    {
        Debug.Log("Active bus");

        if (previosOpenItem != null && eventType.NextCurse == previosOpenItem.Type) return;


        if (coroutine != null)
        {
            if (desActivePlayEffect != null)
            {
                desActivePlayEffect.OnDeselect.StopFeedbacks();
                desActivePlayEffect.TriggerToNormalScale();
                //reset the normal scale
            }

            if (activePlayEffect != null)
            {
                activePlayEffect.OnSelect.StopFeedbacks();
                activePlayEffect.TriggerToNormalScale();
                //reset To Normal
            }

            StopAllCoroutines();
        }


        coroutine = StartCoroutine(effectRoutine(eventType));

    }

    IEnumerator effectRoutine(MaskController.EvsCurseChanged eventType)
    {
        if (isOpen == false)
        {
            onOpen.PlayFeedbacks();
            yield return new WaitForSeconds(openDelay);
        }

        isOpen = true;

        if (eventType.NextCurse == MaskController.ECurse.None)
        {
            if (previosOpenItem != null)
            {

                previosOpenItem.TriggerOnDeselect();
                desActivePlayEffect = previosOpenItem;
            }


            previosOpenItem = null;
        }
        else
        {
            if (previosOpenItem != null)
            {
                previosOpenItem.TriggerOnDeselect();
                desActivePlayEffect = previosOpenItem;
                yield return new WaitForSeconds(deselectDelay);
            }
            // TODO I-pun
            previosOpenItem = frames.FirstOrDefault(i => i.Type == eventType.NextCurse);
            previosOpenItem.TriggerOnSelect();
            activePlayEffect = previosOpenItem;

        }




        yield return new WaitForSeconds(closeDelay);

        onClose.PlayFeedbacks();
        isOpen = false;
        coroutine = null;
    }



    public void ClearText()
    {

    }
}

[System.Serializable]
public class objectPairWithEnum<T> where T : Enum
{
    [SerializeField] RectTransform mainFreme;
    [SerializeField] T type;
    [SerializeField] MoreMountains.Feedbacks.MMF_Player onSelect;
    [SerializeField] MoreMountains.Feedbacks.MMF_Player onDeselect;

    public void TriggerToNormalScale()
    {
        mainFreme.transform.localScale = Vector3.one;
    }

    public void TriggerOnSelect()
    {
        OnSelect.PlayFeedbacks();
    }

    public void TriggerOnDeselect()
    {
        OnDeselect.PlayFeedbacks();
    }

    public T Type { get => type; set => type = value; }
    public MMF_Player OnSelect { get => onSelect; set => onSelect = value; }
    public MMF_Player OnDeselect { get => onDeselect; set => onDeselect = value; }
}

