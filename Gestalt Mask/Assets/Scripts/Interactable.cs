using UnityEngine;

public abstract class Interactable : BetterMonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private string prompt = "Interact";
    [SerializeField] private bool isEnabled = true;

    private Collider _col;

    public Collider Collider => _col ? _col : _col = GetComponent<Collider>();

    public virtual string Prompt => prompt;
    public bool IsEnabled => isEnabled;
    public bool HideOnInitial = false;

    #region
    private MeshRenderer[] _renderers;
    private MeshRenderer[] Renderers
    {
        get
        {
            if (_renderers == null)
            {
                _renderers = GetComponentsInChildren<MeshRenderer>(true);
            }

            return _renderers;
        }
    }

    private Collider[] _colliders;
    private Collider[] Colliders
    {
        get
        {
            if (_colliders == null)
            {
                _colliders = GetComponentsInChildren<Collider>(true);
            }

            return _colliders;
        }
    }

    private Rigidbody[] _rigidbodies;
    private Rigidbody[] Rigidbodies
    {
        get
        {
            if (_rigidbodies == null)
            {
                _rigidbodies = GetComponentsInChildren<Rigidbody>(true);
            }

            return _rigidbodies;
        }
    }
    #endregion

    public void EnableInteraction() => isEnabled = true;
    public void DisableInteraction() => isEnabled = false;

    public void TryInteract(PlayerInteractor interactor)
    {
        if (!isEnabled)
        {
            OnDisabled(interactor);
            return;
        }

        if (!CanInteract(interactor))
            return;

        Interact(interactor);
    }

    protected virtual bool CanInteract(PlayerInteractor interactor) => true;
    protected virtual void OnDisabled(PlayerInteractor interactor) { }
    protected abstract void Interact(PlayerInteractor interactor);

    protected override void Awake()
    {
        base.Awake();
        if (HideOnInitial)
        {
            HideObject();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Collider col = _col != null ? _col : GetComponent<Collider>();
        if (col == null) return;

        Bounds bounds = col.bounds;

        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    private void SetObjectState(bool enabled)
    {
        foreach (var renderer in Renderers)
            renderer.enabled = enabled;

        SetColliderActive(enabled);
        SetRigibody(enabled);

        isEnabled = enabled;
    }

    public void SetRigibody(bool enabled)
    {
        foreach (var rb in Rigidbodies)
        {
            rb.useGravity = enabled;

            if (!enabled)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    public void SetColliderActive(bool enabled)
    {
        foreach (var col in Colliders)
            col.enabled = enabled;
    }

    public void ShowObject()
    {
        SetObjectState(true);
    }

    public void HideObject()
    {
        SetObjectState(false);
    }
}
