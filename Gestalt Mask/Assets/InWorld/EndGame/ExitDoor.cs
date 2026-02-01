using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [Header("Door Pivots")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Sound")]
    public SimpleMMSoundPlayer scrollSfx;
    public AudioClip openClip;

    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 4f;

    private bool isOpen;

    private Quaternion leftTargetRot;
    private Quaternion rightTargetRot;
    private PickUpScript pk;

    private void Start()
    {
        pk = FindAnyObjectByType<PickUpScript>();
    }

    private void Update()
    {
        if (!isOpen) return;

        leftDoor.localRotation = Quaternion.Lerp(
            leftDoor.localRotation,
            leftTargetRot,
            Time.deltaTime * openSpeed
        );

        rightDoor.localRotation = Quaternion.Lerp(
            rightDoor.localRotation,
            rightTargetRot,
            Time.deltaTime * openSpeed
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isOpen) return;
        if (pk.Holding.name.Contains("Key") == false) return;

        OpenDoors(other.transform);
    }

    private void OpenDoors(Transform player)
    {
        isOpen = true;

        Vector3 toPlayer = player.position - transform.position;
        float side = Vector3.Dot(transform.forward, toPlayer);

        float dir = side > 0 ? 1f : -1f;

        // Doors open away from player
        leftTargetRot = Quaternion.Euler(0, -openAngle * dir, 0);
        rightTargetRot = Quaternion.Euler(0, openAngle * dir, 0);

        if (scrollSfx && openClip)
            scrollSfx.PlayClip(openClip);

        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}
