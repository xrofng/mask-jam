using UnityEngine;

public class ColliderB : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] float disableTime = 0.1f;
    bool isActive;


    public void SetActive()
    {
        isActive = true;
    }


    StarterAssets.FirstPersonController players;
    private void OnTriggerEnter(Collider other)
    {
        if (isActive == false) return;
        if (other.gameObject.CompareTag("Player") == false) return;
        Debug.Log($"found Player");
        players = FindAnyObjectByType<StarterAssets.FirstPersonController>();

    }

    float currentTime;

    private void Update()
    {
        if (players != null && players.EnableMove)
        {
            currentTime = Time.time + disableTime;
            players.EnableMove = false;
            players.transform.position = destination.position;
            return;
        }


        if (players != null && players.EnableMove == false)
        {
            if (currentTime > Time.time) return;
            if (players.transform.position != destination.position) return;

            players.EnableMove = true;
            players = null;
        }
    }



}
