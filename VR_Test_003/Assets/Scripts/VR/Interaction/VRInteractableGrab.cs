using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractableGrab : VRInteractable
{
    [SerializeField]
    private MeshRenderer hoverMesh = null;
    [SerializeField]
    private Color nearObject;
    [SerializeField]
    private Color atObject;


    public delegate void InteractableGrabbedEvent(VRController vrController);
    public event InteractableGrabbedEvent InteractableGrabbed;

    public delegate void InteractableReleasedEvent(VRController vrController);
    public event InteractableReleasedEvent InteractableReleased;

    // Start is called before the first frame update
    protected new void Start()
    {
        //Material matCopy = new Material(hoverMesh.sharedMaterial);
        //hoverMesh.sharedMaterial = matCopy;

        base.Start();
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        /*
        if (!isGrabbed)
        {
            bool noControllerNear = true;
            for (int i = 0; i < vrControllers.Length; i++)
            {
                float distanceToController = Vector3.Distance(transform.position, vrControllers[i].InteractPivot.position);

                if (vrControllers[i].TriggerButtonDown && distanceToController <= interactDistance)
                {
                    isGrabbed = true;
                    grabbedController = vrControllers[i];

                    transform.position = grabbedController.InteractPivot.position;
                    FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
                    fixedJoint.connectedBody = grabbedController.InteractPivotRigidbody;
                }
                if (vrControllers[i].TriggerButton == false && distanceToController <= interactDistance * 2f)
                {
                    noControllerNear = false;
                    hoverMesh.enabled = true;

                    float colorLerp = (interactDistance * 2f - distanceToController) / interactDistance;
                    colorLerp = Mathf.Clamp(colorLerp, 0f, 1f);
                    hoverMesh.sharedMaterial.color = Color.Lerp(nearObject, atObject, colorLerp);
                }
            }

            if (noControllerNear)
            {
                hoverMesh.enabled = false;
            }
        }


        if (isGrabbed)
        {
            Debug.DrawRay(grabbedController.CenterOfMass, transform.position, Color.green);
            Debug.DrawRay(grabbedController.CenterOfMass, grabbedController.CenterOfMass + grabbedController.AngularVelocity, Color.red);
            Vector3 velocityForwardDebug = Vector3.Cross(grabbedController.AngularVelocity, transform.position - grabbedController.CenterOfMass);
            Debug.DrawRay(grabbedController.CenterOfMass, grabbedController.CenterOfMass + velocityForwardDebug, Color.blue);

            float smoothTime = 0.0f;

            if (grabbedController.TriggerButtonUp)
            {
                if (rigidbody != null)
                {
                    Vector3 velocityForward = Vector3.Cross(grabbedController.AngularVelocitySmoothed(smoothTime), transform.position - grabbedController.CenterOfMass);
                    //Debug.Log(((transform.position - grabbedController.CenterOfMass) * 10f).ToString());


                    //Debug.Log("Settings velocity");
                    FixedJoint fixedJoint = transform.GetComponent<FixedJoint>();
                    DestroyImmediate(fixedJoint);

                    //rigidbody.velocity =  grabbedController.Velocity;
                    rigidbody.velocity = velocityForward + grabbedController.VelocitySmoothed(smoothTime);

                    Vector3 correctedAngVel = grabbedController.AngularVelocitySmoothed(smoothTime);
                    float mag = correctedAngVel.magnitude;
                    //correctedAngVel = correctedAngVel.normalized * grabbedController.animCurveVel.Evaluate(mag);

                    Debug.Log(mag.ToString("n2"));

                    rigidbody.angularVelocity = correctedAngVel;
                }


                isGrabbed = false;
                grabbedController = null;
            }
        }*/
    }


    public bool IsGrabbed
    {
        get; protected set;
    } = false;

    public override void DeInteract(VRController vrController)
    {
        base.DeInteract(vrController);

        RelaseGrab(vrController);

        (Vector3 velocity, Vector3 angularVelocity) = vrController.VRControllerInteract.VelocitiesAtPivot(transform.TransformPoint(Rigidbody.centerOfMass));
        Rigidbody.velocity = velocity;
        Rigidbody.angularVelocity = angularVelocity;
    }

    public void RelaseGrab(VRController vrController)
    {
        IsGrabbed = false;

        FixedJoint fixedJoint = GetComponent<FixedJoint>();
        DestroyImmediate(fixedJoint);

        InteractableReleased?.Invoke(vrController);
    }


    public override void Interact(VRController vrController)
    {
        makeCollidersConves(Rigidbody.transform, true);
        Rigidbody.isKinematic = false;

        FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = vrController.VRControllerInteract.InteractPivotRigidbody;

        base.Interact(vrController);

        IsGrabbed = true;
        InteractableGrabbed?.Invoke(vrController);
    }

    private void makeCollidersConves(Transform parentTrans, bool makeConves)
    {
        MeshCollider[] mcs = parentTrans.GetComponentsInChildren<MeshCollider>();
        for (int i = 0; i < mcs.Length; i++)
        {
            mcs[i].convex = makeConves;
        }
    }

    /*
    public bool IsGrabbed
    {
        get
        {
            return isGrabbed;
        }
    }*/
}
