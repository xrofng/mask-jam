using UnityEngine;

public class PlayerInteractor : BetterMonoBehaviour
{
    [SerializeField] private float ScanDistance = 3f;
    public float interactRange = 5f; //how far the player can pickup the object from
    public LayerMask InteractLayer;

    [Header("Obj Ref")]
    public Transform CameraTransform;

    private Interactable _current;
    private RaycastHit _currentHit;
    private bool _isFound;
    public Interactable FacingInteractable => _current;

    private PickUpScript _pickUp;
    private bool _isPrevFound;

    public PickUpScript PickUp
    {
        get
        {
            if (_pickUp == null)
                _pickUp = GetComponent<PickUpScript>();

            return _pickUp;
        }
    }

    protected override void Update()
    {
        base.Update();
        DoSphereRay(out _current, out _currentHit, out _isFound);
        // event to update ui
        if (_isPrevFound != _isFound)
        {
            if (_isFound)
            {
                EventBus.TriggerEvent(new Crosshair.EvsPlayerAim(Crosshair.SelectionState.Highlighted, _current));
            }
            else
            {
                EventBus.TriggerEvent(new Crosshair.EvsPlayerAim(Crosshair.SelectionState.Normal, null));
            }
        }
        _isPrevFound = _isFound;


        if (Input.GetKeyDown(KeyCode.Mouse0) && _isFound)
        {
            EventBus.TriggerEvent(new Crosshair.EvsPlayerAim(Crosshair.SelectionState.Pressed, _current));
            _current.TryInteract(this);
        }
    }

    public void DoSphereRay(out Interactable current, out RaycastHit closestHit, out bool found)
    {
        current = null;
        Ray ray = new Ray(
                            transform.position,
                            CameraTransform.forward
                        );

        float radius = 0.35f;
        RaycastHit[] hits = Physics.SphereCastAll(
            ray,
            radius,
            interactRange,
            InteractLayer,
            QueryTriggerInteraction.Ignore
        );

        closestHit = default;
        float closestDistance = float.MaxValue;
        found = false;
        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;

            if (hit.collider.TryGetComponent<Interactable>(out var interactable) && interactable.IsEnabled == false)
            {
                continue;
            }

            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                closestHit = hit;
                found = true;
                current = interactable;
            }
        }
    }
}
