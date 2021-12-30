using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractableHinge : VRInteractable
{
    private Transform grabbedControllerPivot = null;

    private Rigidbody rigidbodyFollower = null;

    protected new void Start()
    {
        base.Start();

        rigidbodyFollower = transform.parent.GetComponent<RigSimpleFollow>().GetComponent<Rigidbody>();
    }


    protected new void Update()
    {
        base.Update();

        if (grabbedControllerPivot != null)
        {
            transform.position = grabbedControllerPivot.position;
        }
    }

    public override void Interact(VRController vrController)
    {
        base.Interact(vrController);

        grabbedControllerPivot = vrController.VRControllerInteract.InteractPivot;
    }

    public override void DeInteract(VRController vrController)
    {
        base.DeInteract(vrController);

        grabbedControllerPivot = null;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rigidbodyFollower.transform.position = transform.position;


        (Vector3 velocity, Vector3 angularVelocity) = vrController.VRControllerInteract.VelocitiesAtPivot();

        rigidbodyFollower.velocity = velocity;
        rigidbodyFollower.angularVelocity = angularVelocity;

        //Rigidbody.velocity = velocity;
        //Rigidbody.angularVelocity = angularVelocity;
    }
}
