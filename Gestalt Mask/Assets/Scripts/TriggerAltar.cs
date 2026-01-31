using UnityEngine;

public class TriggerAltar : Interactable
{
    public Transform PosMarker;

    protected override void Interact(PlayerInteractor interactor)
    {
        Debug.Log("interact altar");

        if (interactor.PickUp.Holding)
        {
            PickableObj dropped = interactor.PickUp.ForceDrop();

            dropped.transform.position = PosMarker.transform.position;
            dropped.transform.parent = PosMarker;
            dropped.SetRigibody(false);
        }
    }
}
