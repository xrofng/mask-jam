using System.Collections;
using System.Collections.Generic;
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

    List<object> _fuseInterceptor = new List<object>();
    public bool CantFuse => _fuseInterceptor.Count > 0;

    public void AddIntercpetor(object obj)
    {
        _fuseInterceptor.Add(obj);
    }

    public void RemoveIntercpetor(object obj)
    {
        if (_fuseInterceptor.Contains(obj))
        {
            _fuseInterceptor.Remove(obj);
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

    Collider[] _fusionBuffer = new Collider[2]; // increase if needed

    public void TryFusion(out int proximityCount)
    {
        if (CantFuse)
        {
            proximityCount = 0;
            return;
        }

        int fusionMaterialLimit = 2;
        _fusionBuffer = new Collider[10];
        proximityCount = 0;
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            FusionRadius,
            _fusionBuffer,
            FusionableLayer
        );

        if (count < fusionMaterialLimit) return;

        List<FusionableObj> fuseMaterial = new List<FusionableObj>();
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            Collider col = _fusionBuffer[i];
            if (col && col.TryGetComponent(out FusionableObj fusionableObj))
            {
                sum.x += col.transform.position.x;
                sum.z += col.transform.position.z;
                fuseMaterial.Add(fusionableObj);
            }
            if (fuseMaterial.Count >= 2)
            {
                break;
            }
            
            //// disable object
            //col.gameObject.GetComponent<PickableObj>().HideObject();
        }

        if (fuseMaterial.Count < 2)
        {
            return;
        }

        Vector3 center = new Vector3(
            sum.x / fuseMaterial.Count,
            transform.position.y + 1, // keep your Y
            sum.z / fuseMaterial.Count
        );

        Debug.Log($"Fusion center: {center}");

        // Example:
        // Spawn fused object here
        //StartCoroutine( SpawnNewFuseMatterRoutine(center, count));
        GestProximity newProx = FusionController.Instance.SpawnFusionMatter(center);
        for (int i = 0; i < fuseMaterial.Count; i++)
        {
            //// disable object
            FusionableObj fuse = fuseMaterial[i];
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
