using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractableGrab : VRInteractable
{
    public bool refresh = false;

    private VRController[] vrControllers = null;

    private bool isGrabbed = false;
    private VRController grabbedController = null;

    private Rigidbody rigidbody = null;

    private float counter = 0f;

    // Start is called before the first frame update
    protected new void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        vrControllers = FindObjectsOfType<VRController>();

        base.Start();
    }

    // Update is called once per frame
    protected new void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 1f)
        {
            counter = 0f;
            refresh = false;

            vrControllers = FindObjectsOfType<VRController>();

        }

        base.Update();

        if (!isGrabbed)
        {
            for (int i = 0; i < vrControllers.Length; i++)
            {
                if (vrControllers[i].TriggerButtonDown && Vector3.Distance(transform.position, vrControllers[i].InteractPivot.position) <= interactDistance)
                {
                    isGrabbed = true;
                    grabbedController = vrControllers[i];
                }
            }
        }


        if (isGrabbed)
        {
            transform.position = grabbedController.InteractPivot.position;
            transform.rotation = grabbedController.InteractPivot.rotation;

            if (grabbedController.TriggerButtonUp)
            {
                if (rigidbody != null)
                {
                    rigidbody.velocity = grabbedController.Velocity;
                    rigidbody.angularVelocity = grabbedController.AngularVelocity;
                }


                isGrabbed = false;
                grabbedController = null;
            }
        }
    }
}
