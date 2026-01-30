using Unity.VisualScripting;
using UnityEngine;
using static MaskController;

public class ChangeColor : BetterMonoBehaviour, IEventSubcriber<EvsCurseChanged>
{
    private Renderer _renderer;
    private GlobalColStorManager colStor;

    //Subscribe + unsub from CurseChange event
    protected override void OnEnable()
    {
        base.OnEnable();
        _renderer = GetComponent<Renderer>();
        EventBus.AddSubcriber<EvsCurseChanged>(this);
    }

    protected override void OnDisable()
    {
        EventBus.RemoveSubcriber<EvsCurseChanged>(this);
        base.OnDisable();
    }
    protected override void Start()
    {
        colStor = GlobalColStorManager.Instance;
    }

    //On curse change event:
    public void OnEventBusTrigger(EvsCurseChanged eventType)
    {
        if(eventType.NextCurse == ECurse.None)
        {
            _renderer.material.SetColor("_BaseColor", colStor.none_base);
        }
        else if(eventType.NextCurse == ECurse.Similarity)
        {
            _renderer.material.SetColor("_BaseColor", colStor.similarity_base);
        }
        else if (eventType.NextCurse == ECurse.Proximity)
        {
            _renderer.material.SetColor("_BaseColor", colStor.proximity_base);
        }
        else if (eventType.NextCurse == ECurse.Continuance)
        {
            _renderer.material.SetColor("_BaseColor", colStor.continuance_base);
        }
        else if (eventType.NextCurse == ECurse.Closure)
        {
            _renderer.material.SetColor("_BaseColor", colStor.closure_base);
        }
        else if (eventType.NextCurse == ECurse.Invariance)
        {
            _renderer.material.SetColor("_BaseColor", colStor.invariance_base);
        }
    }
}
