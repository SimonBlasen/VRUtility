using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractableHinge : VRInteractable
{
    [SerializeField]
    private float forceMulti = 1f;

    private Transform grabbedControllerPivot = null;

    private Rigidbody rigidbodyFollower = null;
    private Rigidbody attachedRig = null;

    private Transform oldParent = null;
    private Vector3 oldParentPos = Vector3.zero;
    private Quaternion oldParentRot = Quaternion.identity;

    private Vector3 grabStartPosOffset = Vector3.zero;
    private Quaternion grabStartRotOffset = Quaternion.identity;

    protected new void Start()
    {
        base.Start();

        oldParent = transform.parent;
        oldParentPos = transform.localPosition;
        oldParentRot = transform.localRotation;

        rigidbodyFollower = transform.parent.GetComponent<RigSimpleFollow>().GetComponent<Rigidbody>();
        attachedRig = rigidbodyFollower.GetComponent<FixedJoint>().connectedBody;
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

        transform.parent = null;
        grabbedControllerPivot = vrController.VRControllerInteract.InteractPivot;

        grabStartPosOffset = transform.position - grabbedControllerPivot.position;
        grabStartRotOffset = Quaternion.Inverse(grabbedControllerPivot.rotation) * transform.rotation;
    }

    public override void DeInteract(VRController vrController)
    {
        base.DeInteract(vrController);

        grabbedControllerPivot = null;

        transform.parent = oldParent;

        transform.localPosition = oldParentPos;
        transform.localRotation = oldParentRot;
        //rigidbodyFollower.transform.position = transform.position;


        (Vector3 velocity, Vector3 angularVelocity) = vrController.VRControllerInteract.VelocitiesAtPivot();


        attachedRig.AddForceAtPosition(velocity * forceMulti, rigidbodyFollower.position, ForceMode.VelocityChange);

        //rigidbodyFollower.velocity = velocity;
        //rigidbodyFollower.angularVelocity = angularVelocity;

        //Rigidbody.velocity = velocity;
        //Rigidbody.angularVelocity = angularVelocity;
    }
}
