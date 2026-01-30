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
}
