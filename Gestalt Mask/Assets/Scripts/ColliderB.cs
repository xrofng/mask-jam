using UnityEngine;
using StarterAssets;

public class ColliderB : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] float disableTime = 0.1f;

    bool isActive;
    bool teleporting;

    float reenableTime;
    FirstPersonController player;

    public void SetActive()
    {
        isActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (!other.CompareTag("Player")) return;

        player = other.GetComponent<FirstPersonController>();
        if (player == null) return;

        teleporting = true;
        player.EnableMove = false;

        // Teleport safely
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.transform.position = destination.position;
        cc.enabled = true;

        reenableTime = Time.time + disableTime;
    }

    private void Update()
    {
        if (!teleporting) return;

        if (Time.time >= reenableTime)
        {
            player.EnableMove = true;
            teleporting = false;
            player = null;
        }
    }
}
