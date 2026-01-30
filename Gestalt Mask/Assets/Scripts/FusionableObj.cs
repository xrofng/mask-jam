using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;

public class FusionableObj : BetterMonoBehaviour, IEventSubcriber<MaskController.EvsCurseChanged>
{
    public LayerMask FusionableLayer;
    public float FusionRadius = 1;
    public bool TryFusionOnAwake = false;

    private PickableObj _pickable;
    public PickableObj Pickable
    {
        get
        {
            if (_pickable == null)
                _pickable = GetComponent<PickableObj>();

            return _pickable;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.AddSubcriber<MaskController.EvsCurseChanged>(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.RemoveSubcriber<MaskController.EvsCurseChanged>(this);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    Collider[] _fusionBuffer = new Collider[10]; // increase if needed

    public void TryFusion(out int proximityCount)
    {
        _fusionBuffer = new Collider[10];
        proximityCount = 0;
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            FusionRadius,
            _fusionBuffer,
            FusionableLayer
        );

        if (count < 2) return;

        Vector3 sum = Vector3.zero;

        for (int i = 0; i < count; i++)
        {
            Collider col = _fusionBuffer[i];

            sum.x += col.transform.position.x;
            sum.z += col.transform.position.z;

            //// disable object
            //col.gameObject.GetComponent<PickableObj>().HideObject();
        }

        Vector3 center = new Vector3(
            sum.x / count,
            transform.position.y, // keep your Y
            sum.z / count
        );

        Debug.Log($"Fusion center: {center}");

        // Example:
        // Spawn fused object here
        //StartCoroutine( SpawnNewFuseMatterRoutine(center, count));
        GestProximity newProx = FusionController.Instance.SpawnFusionMatter(center);
        for (int i = 0; i < count; i++)
        {
            //// disable object
            FusionableObj fuse = _fusionBuffer[i].gameObject.GetComponent<FusionableObj>();
            fuse.Pickable.HideObject();
            newProx.AddMember(fuse);
            _fusionBuffer[i].transform.parent = newProx.transform;
        }
    }
    IEnumerator SpawnNewFuseMatterRoutine(Vector3 center, int count)
    {
        yield return null;
        GestProximity newProx = FusionController.Instance.SpawnFusionMatter(center);
        for (int i = 0; i < count; i++)
        {
            //// disable object
            FusionableObj fuse = _fusionBuffer[i].gameObject.GetComponent<FusionableObj>();
            fuse.Pickable.HideObject();
            newProx.AddMember(fuse);
            _fusionBuffer[i].transform.parent = newProx.transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(.6f,0,1, .3f);
        Gizmos.DrawSphere(transform.position, FusionRadius);
    }

    public void OnEventBusTrigger(MaskController.EvsCurseChanged eventType)
    {
        if (eventType.NextCurse != MaskController.ECurse.Proximity)
        {
            if (Pickable.IsEnabled == true)
            {
                TryFusion(out int proxiCount);
            }
        }
    }
}
