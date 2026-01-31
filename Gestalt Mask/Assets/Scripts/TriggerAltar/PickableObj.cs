using System;
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

    public void InvokeOnPick()
    {
        OnPickedUp();
    }

    protected virtual void OnPickedUp()
    {
        
    }

    protected override void Interact(PlayerInteractor interactor)
    {
        // no need implementation
        // let PickUpScript to their job
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
