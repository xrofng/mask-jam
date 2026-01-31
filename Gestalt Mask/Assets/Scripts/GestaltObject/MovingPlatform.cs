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
    Transform target;




    int currentIndex = 0;
    protected override MaskController.ECurse TargetCurse
        => MaskController.ECurse.Continuance;

    protected override void Awake()
    {
        if (movementPos.Count == 0) return;

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
            if (oneTrip)
            {
                target = null;
            }

            target = movementPos[currentIndex];
        }
    }




    protected override void OnCurseEnabled()
    {

        target = null;

    }

    protected override void OnCurseDisabled()
    {





        transform.position = movementPos[0].position;



        target = movementPos[1];
    }
}