using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float ScanDistance = 3f;

    [Header("Obj Ref")]
    [SerializeField] private Camera cam;
    public Transform HoldPoint;

    private Interactable _current;

    private void Update()
    {
        Scan();

        if (Input.GetKeyDown(KeyCode.E) && _current != null)
            _current.TryInteract(this);
    }

    private void Scan()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit[] hits = Physics.RaycastAll(ray, ScanDistance);

        // VERY IMPORTANT → sort by distance
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        _current = null;

        foreach (var hit in hits)
        {
            if (!hit.collider.TryGetComponent(out Interactable interactable))
                continue;

            if (!interactable.IsEnabled)
                continue;

            _current = interactable;
            break;
        }
    }
}
