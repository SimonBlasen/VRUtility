using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractableHinge : VRInteractable
{
    [Header("Settings")]
    [SerializeField]
    private float forceMultiPos = 1f;
    [SerializeField]
    private float forceMultiRot = 1f;
    [SerializeField]
    private bool followPosition = false;
    [SerializeField]
    private bool followRotation = false;

    [Space]

    [Header("References")]
    [SerializeField]
    private Transform fixedJointPosition = null;

    private Transform grabbedControllerPivot = null;

    private Rigidbody rigidbodyFollower = null;
    private Rigidbody attachedRig = null;

    private Transform oldParent = null;
    private Vector3 oldParentPos = Vector3.zero;
    private Quaternion oldParentRot = Quaternion.identity;

    private Vector3 grabStartPosOffset = Vector3.zero;
    private Quaternion grabStartRotOffset = Quaternion.identity;

    private GameObject instFixedJoint = null;

    protected new void Start()
    {
        base.Start();

        oldParent = transform.parent;
        oldParentPos = transform.localPosition;
        oldParentRot = transform.localRotation;

        //rigidbodyFollower = transform.parent.GetComponent<RigSimpleFollow>().GetComponent<Rigidbody>();
        attachedRig = transform.parent.GetComponent<Rigidbody>();
    }


    protected new void Update()
    {
        base.Update();

        if (grabbedControllerPivot != null)
        {
            transform.position = Quaternion.Inverse(grabStartRotOffset) * grabbedControllerPivot.rotation * grabStartPosOffset + grabbedControllerPivot.position;
            transform.rotation = grabbedControllerPivot.rotation * grabStartRotOffset;
        }
    }

    public override void Interact(VRController vrController)
    {
        base.Interact(vrController);

        IsGrabbed = true;

        transform.parent = null;
        grabbedControllerPivot = vrController.VRControllerInteract.InteractPivot;

        grabStartPosOffset = transform.position - grabbedControllerPivot.position;
        grabStartRotOffset = Quaternion.Inverse(grabbedControllerPivot.rotation) * transform.rotation;

        if (instFixedJoint != null)
        {
            Destroy(instFixedJoint);
            instFixedJoint = null;
        }

        instFixedJoint = new GameObject("Fixed Joint");
        instFixedJoint.transform.position = fixedJointPosition.position;
        instFixedJoint.transform.rotation = fixedJointPosition.rotation;
        instFixedJoint.AddComponent<Rigidbody>();
        FixedJoint fixedJoint = instFixedJoint.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = attachedRig;

        RigSimpleFollow rigSimpleFollow = instFixedJoint.AddComponent<RigSimpleFollow>();
        rigSimpleFollow.FollowPosition = followPosition;
        rigSimpleFollow.FollowRotation = followRotation;
        rigSimpleFollow.Target = transform;
    }

    public override void DeInteract(VRController vrController)
    {
        base.DeInteract(vrController);

        IsGrabbed = false;

        grabbedControllerPivot = null;

        transform.parent = oldParent;

        transform.localPosition = oldParentPos;
        transform.localRotation = oldParentRot;
        //rigidbodyFollower.transform.position = transform.position;


        if (instFixedJoint != null)
        {
            Destroy(instFixedJoint);
            instFixedJoint = null;
        }

        (Vector3 velocity, Vector3 angularVelocity) = vrController.VRControllerInteract.VelocitiesAtPivot();


        attachedRig.velocity = Vector3.zero;
        attachedRig.angularVelocity = Vector3.zero;
        attachedRig.AddForceAtPosition(velocity * forceMultiPos, fixedJointPosition.position, ForceMode.VelocityChange);
        attachedRig.AddTorque(angularVelocity * forceMultiRot, ForceMode.VelocityChange);

        //rigidbodyFollower.velocity = velocity;
        //rigidbodyFollower.angularVelocity = angularVelocity;

        //Rigidbody.velocity = velocity;
        //Rigidbody.angularVelocity = angularVelocity;
    }

    public bool IsGrabbed
    {
        get; protected set;
    } = false;
}
