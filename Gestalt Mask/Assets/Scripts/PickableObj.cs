using UnityEngine;

public class PickableObj : Interactable
{
    private float positionSmoothTime = 0.05f;
    private float rotationSmoothSpeed = 20f;

    private bool _held;
    private Transform _holdPoint;
    private Vector3 _velocity;
    private Transform _originalParent;

    public bool IsHeld => _held;

    protected override void Awake()
    {
        base.Awake();

        _originalParent = transform.parent;
    }

    private void LateUpdate()
    {
        if (!_held) return;

        FollowHoldPoint();
    }

    protected override void Interact(PlayerInteractor interactor)
    {
        if (_held)
        {
            Drop();
            return;
        }

        Pick(interactor);
    }

    private void Pick(PlayerInteractor interactor)
    {
        if (_held) return;

        _holdPoint = interactor.HoldPoint;
        _held = true;

        transform.SetParent(null); // prevents parent scale issues

        if (Collider != null)
            Collider.enabled = false; // avoids camera clipping
    }

    private void Drop()
    {
        _held = false;
        _holdPoint = null;

        transform.SetParent(_originalParent);

        if (Collider != null)
            Collider.enabled = true;
    }

    private void FollowHoldPoint()
    {
        // Smooth position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            _holdPoint.position,
            ref _velocity,
            positionSmoothTime
        );

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            _holdPoint.rotation,
            rotationSmoothSpeed * Time.deltaTime
        );
    }
}
