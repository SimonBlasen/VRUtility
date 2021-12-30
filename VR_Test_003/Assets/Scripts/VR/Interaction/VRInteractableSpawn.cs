using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractableSpawn : VRInteractable
{
    public GameObject prefab = null;
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
                if (vrControllers[i].TriggerButtonDown)
                {
                    GameObject inst = Instantiate(prefab);
                    isGrabbed = true;
                    grabbedController = vrControllers[i];
                }
            }
        }


        if (isGrabbed)
        {
            transform.position = grabbedController.VRControllerInteract.InteractPivot.position;
            transform.rotation = grabbedController.VRControllerInteract.InteractPivot.rotation;

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
