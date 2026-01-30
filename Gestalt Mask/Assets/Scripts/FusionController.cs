using UnityEngine;

public class FusionController : MonoBehaviour
{
    public static FusionController Instance { get; private set; }

    public GestProximity FusionMatterPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public GestProximity SpawnFusionMatter(Vector3 pos)
    {
        return Instantiate(FusionMatterPrefab, pos, Quaternion.identity);
    }
}
