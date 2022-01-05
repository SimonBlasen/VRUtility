using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerInteract : MonoBehaviour
{
    [SerializeField]
    private Transform interactPivot = null;
    [SerializeField]
    private float interactDistance = 0.15f;
    [SerializeField]
    private float triggerButtonThreshDown = 0.2f;
    [SerializeField]
    private float triggerButtonThreshUp = 0.1f;

    private List<VRInteractable> vrInteractables = new List<VRInteractable>();

    private Rigidbody interactPivotRig = null;

    private float oldTriggerButtonValue = 0f;

    // Start is called before the first frame update
    void Start()
    {
        interactPivotRig = interactPivot.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (VRController.Trigger > triggerButtonThreshDown && oldTriggerButtonValue <= triggerButtonThreshDown && HasItemInHand == false)
        {
            float closestDistance = float.MaxValue;
            int closestIndex = -1;
            for (int i = 0; i < vrInteractables.Count; i++)
            {
                if (vrInteractables[i].IsInteractable)
                {
                    float distance = Vector3.Distance(interactPivot.position, vrInteractables[i].transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestIndex = i;
                    }
                }
            }

            if (closestDistance <= interactDistance && closestIndex != -1)
            {
                vrInteractables[closestIndex].Interact(VRController);
            }
        }
        else if (VRController.Trigger < triggerButtonThreshUp && oldTriggerButtonValue >= triggerButtonThreshUp)
        {
            releaseInteractable();
        }


        oldTriggerButtonValue = VRController.Trigger;
        cachedNearestIntDistanceValid = false;
    }

    private VRController vrController = null;

    private VRController VRController
    {
        get
        {
            if (vrController == null)
            {
                vrController = GetComponent<VRController>();
            }
            return vrController;
        }
    }

    public void RegisterInteractable(VRInteractable vrInteractable)
    {
        if (vrInteractables.Contains(vrInteractable) == false)
        {
            vrInteractables.Add(vrInteractable);
        }
    }

    public void DeRegisterInteractable(VRInteractable vrInteractable)
    {
        if (vrInteractables.Contains(vrInteractable))
        {
            vrInteractables.Remove(vrInteractable);
        }
    }

    private float cachedNearestIntDistance = 0f;
    private bool cachedNearestIntDistanceValid = false;
    public float GetNearestInteractableDistance()
    {
        if (!cachedNearestIntDistanceValid)
        {
            cachedNearestIntDistanceValid = true;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < vrInteractables.Count; i++)
            {
                if (vrInteractables[i].IsInteractable)
                {
                    float distance = Vector3.Distance(interactPivot.position, vrInteractables[i].transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                    }
                }
            }
            cachedNearestIntDistance = closestDistance;
        }
        return cachedNearestIntDistance;
    }

    public bool HasItemInHand
    {
        get; protected set;
    } = false;

    public Transform ItemInHAnd
    {
        get; protected set;
    }

    public VRInteractable InteractableInHand
    {
        get; protected set;
    }

    public Transform InteractPivot
    {
        get
        {
            return interactPivot;
        }
    }

    public Rigidbody InteractPivotRigidbody
    {
        get
        {
            return interactPivotRig;
        }
    }

    public float InteractDistance
    {
        get
        {
            return interactDistance;
        }
    }

    private void releaseInteractable()
    {
        if (HasItemInHand)
        {
            HasItemInHand = false;


            InteractableInHand.DeInteract(VRController);
            /*if (InteractableInHand.Rigidbody != null)
            {
                FixedJoint fixedJoint = ItemInHAnd.GetComponent<FixedJoint>();
                DestroyImmediate(fixedJoint);

                //InteractableInHand.Rigidbody.velocity = velocityForward + vrController.VelocitySmoothed(smoothTime);
                //InteractableInHand.Rigidbody.angularVelocity = vrController.AngularVelocitySmoothed(smoothTime); 
            }*/

            ItemInHAnd = null;
            InteractableInHand = null;
        }
    }

    public (Vector3 velocity, Vector3 angularVelocity) VelocitiesAtPivot()
    {
        float smoothTime = 0.0f;
        Vector3 velocityForward = Vector3.Cross(vrController.AngularVelocity, ItemInHAnd.position - vrController.CenterOfMass);
        return (velocityForward + vrController.VelocitySmoothed(smoothTime), vrController.AngularVelocitySmoothed(smoothTime));
    }


    public void GrabInteractable(VRInteractable vrInteractableGrab)
    {
        //vrInteractableGrab.transform.position = interactPivot.position;

        /*if (vrInteractableGrab.Rigidbody != null)
        {
            FixedJoint fixedJoint = vrInteractableGrab.gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = interactPivotRig;
        }*/


        ItemInHAnd = vrInteractableGrab.transform;
        HasItemInHand = true;
        InteractableInHand = vrInteractableGrab;
        //InteractableInHand.PerformGrab(VRController);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(interactPivot.position, 0.2f);
    }
}
