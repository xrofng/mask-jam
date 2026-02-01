using Unity.VisualScripting;
using UnityEngine;

public class BlockDoor : MonoBehaviour
{
    [SerializeField] Collider dorCol;

    PickUpScript pickUp;

    private bool _isAutorized;

    private void Awake()
    {
        pickUp = FindAnyObjectByType<PickUpScript>();
    }

    bool PlayerIn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false) return;
        PlayerIn = true;
        Debug.Log("in");

        if (pickUp.Holding && pickUp.Holding.GetComponent<GestProximity>() == null)
        {
            pickUp.ForceDrop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false) return;
        Debug.Log("out");
        PlayerIn = false;
    }

    PickableObj previosCheckItem;
    private void Update()
    {
        //if (PlayerIn == false) return;

        //if (pickUp.Holding == null)

        //{
        //    //dorCol.enabled = false;
        //    return;
        //}

        //if (previosCheckItem == pickUp.Holding) return;

        //if (pickUp.Holding.GetComponent<GestProximity>() != null)
        //{
        //    dorCol.enabled = false;

        //    return;
        //}

        //dorCol.enabled = true;
        //pickUp.ForceDrop();
    }

}
