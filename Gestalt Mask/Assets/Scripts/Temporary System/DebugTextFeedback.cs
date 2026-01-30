using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class DebugTextFeedback : BetterMonoBehaviour, IEventSubcriber<MaskController.EvsCurseChanged>
{
    [SerializeField] List<objectPairWithEnum<MaskController.ECurse>> frames = new List<objectPairWithEnum<MaskController.ECurse>>();
    [SerializeField] UnityEvent onOpen;
    [SerializeField] UnityEvent onClose;
    [SerializeField] float openDelay;
    [SerializeField] float closeDelay;
    bool canCancel;
    bool isOpen;
    objectPairWithEnum<MaskController.ECurse> previosOpenItem;


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

        StopAllCoroutines();

        StartCoroutine(effectRoutine(eventType));

    }

    IEnumerator effectRoutine(MaskController.EvsCurseChanged eventType)
    {
        if (isOpen == false)
        {
            onOpen?.Invoke();
            yield return new WaitForSeconds(openDelay);
        }

        isOpen = true;

        if (eventType.NextCurse == MaskController.ECurse.None)
        {
            if (previosOpenItem != null)
            {
                previosOpenItem.TriggerOnDeselect();
            }
            previosOpenItem = null;
        }
        else
        {
            if (previosOpenItem != null)
            {
                previosOpenItem.TriggerOnDeselect();
            }
            // TODO I-pun
            //previosOpenItem = frames.FirstOrDefault(i => i.Type == eventType.NextCurse);
            //previosOpenItem.TriggerOnSelect();
        }




        yield return new WaitForSeconds(closeDelay);

        onClose?.Invoke();
        isOpen = false;


    }



    public void ClearText()
    {

    }
}

[System.Serializable]
public class objectPairWithEnum<T> where T : Enum
{
    [SerializeField] T type;
    [SerializeField] UnityEvent onSelect;
    [SerializeField] UnityEvent onDeselect;

    public void TriggerOnSelect()
    {
        onSelect?.Invoke();
    }

    public void TriggerOnDeselect()
    {
        onDeselect?.Invoke();
    }

    public T Type { get => type; set => type = value; }
}

