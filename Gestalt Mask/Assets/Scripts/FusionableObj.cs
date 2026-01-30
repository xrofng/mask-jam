using UnityEngine;

public class FusionableObj : BetterMonoBehaviour
{
    public LayerMask FusionableLayer;
    public float FusionRadius = 1;
    public bool TryFusionOnAwake = false;

    protected override void Awake()
    {
        base.Awake();
    }

    static readonly Collider[] _fusionBuffer = new Collider[32]; // increase if needed

    public void TryFusion()
    {
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

            // disable object
            col.gameObject.SetActive(false);
        }

        Vector3 center = new Vector3(
            sum.x / count,
            transform.position.y, // keep your Y
            sum.z / count
        );

        Debug.Log($"Fusion center: {center}");

        // Example:
        // Spawn fused object here
        GestProximity newProx =  FusionController.Instance.SpawnFusionMatter(center);
        for (int i = 0; i < count; i++)
        {
            newProx.AddMember(_fusionBuffer[i].gameObject);
            _fusionBuffer[i].transform.parent = newProx.transform;
        }
        //Instantiate(fusedPrefab, center, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(.6f,0,1, .3f);
        Gizmos.DrawSphere(transform.position, FusionRadius);
    }

}
