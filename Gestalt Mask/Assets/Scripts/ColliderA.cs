using UnityEngine;

public class ColliderA : MonoBehaviour
{
    [SerializeField] ColliderB colB;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false) return;

        colB.SetActive();


    }
}
