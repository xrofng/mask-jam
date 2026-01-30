using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : BetterMonoBehaviour, IEventSubcriber<MaskController.EvsCurseChanged>
{
    public Collider playerCollider;
    public Transform holdPos;
    public Transform CameraTransform;
    public LayerMask PickableLayer;
    public string HoldLayerName = "Holding";
    public string PickLayerName = "Pickable";
    //if you copy from below this point, you are legally required to like the video
    public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 5f; //how far the player can pickup the object from
    private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    private GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    int holdingLayer;
    int pickLayer;
    private MaskController.ECurse currMask;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.AddSubcriber<MaskController.EvsCurseChanged>(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.RemoveSubcriber<MaskController.EvsCurseChanged>(this);
    }

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    //MouseLookScript mouseLookScript;
    void Start()
    {
        holdingLayer = LayerMask.NameToLayer(HoldLayerName); //if your holdLayer is named differently make sure to change this ""
        pickLayer = LayerMask.NameToLayer(PickLayerName);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) //change E to whichever key you want to press to pick up
        {
            if (heldObj == null)
            {
                RaycastHit closestHit;
                bool found;
                DoSphereRay(out closestHit, out found);

                if (found)
                {
                    PickUpObject(closestHit.transform.gameObject);
                }
            }
            else
            {
                if(canDrop == true)
                {
                    StopClipping(); //prevents object from clipping through walls
                    DropObject();
                }
            }
        }
        if (heldObj != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos
            RotateObject();
            //if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
            //{
            //    StopClipping();
            //    ThrowObject();
            //}

        }
    }

    private void DoSphereRay(out RaycastHit closestHit, out bool found)
    {
        Ray ray = new Ray(
                            transform.position,
                            CameraTransform.forward
                        );

        float radius = 0.35f; 
        RaycastHit[] hits = Physics.SphereCastAll(
            ray,
            radius,
            pickUpRange,
            PickableLayer,
            QueryTriggerInteraction.Ignore
        );

        closestHit = default;
        float closestDistance = float.MaxValue;
        found = false;
        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;

            if (hit.collider.TryGetComponent<Interactable>(out var interactable) && interactable.IsEnabled == false)
            {
                continue;
            }

            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                closestHit = hit;
                found = true;
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = holdingLayer; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), playerCollider, true);
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), playerCollider, false);
        heldObj.layer = pickLayer; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object

        if (currMask != MaskController.ECurse.Proximity)
        {
            if (heldObj.TryGetComponent(out FusionableObj fusionable))
            {
                fusionable.TryFusion(out int proxCount);
            }
        }
        heldObj = null; //undefine game object

    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPos.transform.position;
    }
    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))//hold R key to rotate, change this to whatever key you want
        {
            canDrop = false; //make sure throwing can't occur during rotating

            //disable player being able to look around
            //mouseLookScript.verticalSensitivity = 0f;
            //mouseLookScript.lateralSensitivity = 0f;

            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            //rotate the object depending on mouse X-Y Axis
            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            //re-enable player being able to look around
            //mouseLookScript.verticalSensitivity = originalvalue;
            //mouseLookScript.lateralSensitivity = originalvalue;
            canDrop = true;
        }
    }
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), playerCollider, false);
        heldObj.layer = PickableLayer;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }

    public void OnEventBusTrigger(MaskController.EvsCurseChanged eventType)
    {
        currMask = eventType.NextCurse;
    }
}
