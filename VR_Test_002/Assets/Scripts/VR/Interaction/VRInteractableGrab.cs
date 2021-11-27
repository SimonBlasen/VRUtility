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

        Material matCopy = new Material(hoverMesh.sharedMaterial);
        hoverMesh.sharedMaterial = matCopy;

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
            //transform.position = grabbedController.InteractPivot.position;
            //transform.rotation = grabbedController.InteractPivot.rotation;

            if (grabbedController.TriggerButtonUp)
            {


                if (rigidbody != null)
                {

                    Debug.Log("Settings velocity");
                    FixedJoint fixedJoint = transform.GetComponent<FixedJoint>();
                    DestroyImmediate(fixedJoint);

                    rigidbody.velocity =  grabbedController.Velocity;
                    rigidbody.angularVelocity = grabbedController.AngularVelocity;
                }


                isGrabbed = false;
                grabbedController = null;
            }
        }
    }
}
