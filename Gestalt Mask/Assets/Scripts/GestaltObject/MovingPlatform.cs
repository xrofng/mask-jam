using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : GestaltObj
{
    [Header("Movement Settings")]
    [SerializeField] List<Transform> movementPos = new List<Transform>();
    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField] float speed = 5f;
    [SerializeField] bool lockY = true;
    [SerializeField] bool oneTrip = false;
    [SerializeField] bool needPower;
    public bool NeedPower { set { needPower = value; } get { return needPower; } }

    bool currentPower;
    public bool HasPower => currentPower;
    Transform target;

    bool usedOneTrip = false;   

    int currentIndex = 0;
    protected override MaskController.ECurse TargetCurse
        => MaskController.ECurse.Continuance;


    public void ActivePower()
    {
        currentPower = true;
    }

    public void DisablePower()
    {
        currentPower = false;
    }
    StarterAssets.FirstPersonController playerControl;

    protected override void Awake()
    {
        if (movementPos.Count == 0) return;

        playerControl = FindAnyObjectByType<StarterAssets.FirstPersonController>();
        transform.position = movementPos[0].position;
        currentIndex = 1 % movementPos.Count;
        target = movementPos[currentIndex];
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    protected override void Update()
    {
        if (currentPower == false && needPower)
        {
            return;
        }
        if (!target) return;

        Vector3 targetPos = target.position + offset;
        Vector3 currentPos = transform.position;




        if (lockY)
        {
            targetPos.y = currentPos.y;
        }

        Vector3 direction = (targetPos - currentPos).normalized;

        // Move platform
        transform.position = Vector3.MoveTowards(
            currentPos,
            targetPos,
            speed * Time.deltaTime
        );



        // Distance check (ignore Y if locked)
        Vector3 a = currentPos;
        Vector3 b = target.position;

        if (lockY)
        {
            a.y = 0;
            b.y = 0;
        }

        if (Vector3.Distance(a, b) < 0.125f)
        {
            currentIndex++;

            if (currentIndex >= movementPos.Count)
                currentIndex = 0;
            if (oneTrip && usedOneTrip == false)
            {
                
                target = null;
                usedOneTrip = true;
                return;
                
            }

            target = movementPos[currentIndex];
        }
    }


    bool isPlayerIn = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false) return;
        isPlayerIn = true;

    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerIn = false;
    }

    protected override void OnCurseEnabled()
    {

        target = null;

    }

    protected override void OnCurseDisabled()
    {
        usedOneTrip = false;
        if (isPlayerIn)
        {
            playerControl.EnableMove = false;
            transform.position = movementPos[0].position;
            playerControl.transform.position = movementPos[0].position;
            playerControl.EnableMove = true;
            target = movementPos[1];
        }
        else
        {
            transform.position = movementPos[0].position;
            target = movementPos[1];
        }
    }
}