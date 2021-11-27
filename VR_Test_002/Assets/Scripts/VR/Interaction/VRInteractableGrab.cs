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
    [SerializeField]
    private Transform graphParentX = null;
    [SerializeField]
    private Transform graphParentY = null;
    [SerializeField]
    private Transform graphParentZ = null;

    public bool refresh = false;

    private VRController[] vrControllers = null;

    private bool isGrabbed = false;
    private VRController grabbedController = null;

    private Rigidbody rigidbody = null;

    private float counter = 0f;

    // Start is called before the first frame update
    protected new void Start()
    {
        graphParentX = GameObject.Find("Graph X").transform;
        graphParentY = GameObject.Find("Graph Y").transform;
        graphParentZ = GameObject.Find("Graph Z").transform;

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
            Debug.DrawRay(grabbedController.CenterOfMass, transform.position, Color.green);
            Debug.DrawRay(grabbedController.CenterOfMass, grabbedController.CenterOfMass + grabbedController.AngularVelocity, Color.red);
            Vector3 velocityForwardDebug = Vector3.Cross(grabbedController.AngularVelocity, transform.position - grabbedController.CenterOfMass);
            Debug.DrawRay(grabbedController.CenterOfMass, grabbedController.CenterOfMass + velocityForwardDebug, Color.blue);

            float smoothTime = 0.0f;

            graphParentX.position = grabbedController.CenterOfMass;
            graphParentX.forward = transform.position - grabbedController.CenterOfMass;
            graphParentY.position = grabbedController.CenterOfMass;
            graphParentY.forward = grabbedController.AngularVelocitySmoothed(smoothTime);
            graphParentZ.position = grabbedController.CenterOfMass;
            graphParentZ.forward = velocityForwardDebug;

            graphParentX.localScale = new Vector3(1f, 1f, Vector3.Distance(grabbedController.CenterOfMass, transform.position));
            graphParentY.localScale = new Vector3(1f, 1f, grabbedController.AngularVelocitySmoothed(smoothTime).magnitude);
            graphParentZ.localScale = new Vector3(1f, 1f, velocityForwardDebug.magnitude);


            //GraphManager.Graph.Plot("Vel X", grabbedController.Velocity.x, Color.green, new GraphManager.Matrix4x4Wrapper(graphParentX.position, graphParentX.rotation, graphParentX.localScale));
            //GraphManager.Graph.Plot("Vel Y", grabbedController.Velocity.y, Color.green, new GraphManager.Matrix4x4Wrapper(graphParentY.position, graphParentY.rotation, graphParentY.localScale));
            //GraphManager.Graph.Plot("Vel Z", grabbedController.Velocity.z, Color.green, new GraphManager.Matrix4x4Wrapper(graphParentZ.position, graphParentZ.rotation, graphParentZ.localScale));
            //GraphManager.Graph.Plot("Vel X", grabbedController.Velocity.x, Color.green, new Rect(new Vector2(10f, 10f), new Vector2(500f, 100f)));
            //GraphManager.Graph.Plot("Vel Y", grabbedController.Velocity.y, Color.green, new Rect(new Vector2(10f, 120f), new Vector2(500f, 100f)));
            //GraphManager.Graph.Plot("Vel Z", grabbedController.Velocity.z, Color.green, new Rect(new Vector2(10f, 230f), new Vector2(500f, 100f)));

            //transform.position = grabbedController.InteractPivot.position;
            //transform.rotation = grabbedController.InteractPivot.rotation;

            if (grabbedController.TriggerButtonUp)
            {


                if (rigidbody != null)
                {
                    Vector3 velocityForward = Vector3.Cross(grabbedController.AngularVelocitySmoothed(smoothTime), transform.position - grabbedController.CenterOfMass);
                    Debug.Log(((transform.position - grabbedController.CenterOfMass) * 10f).ToString());


                    Debug.Log("Settings velocity");
                    FixedJoint fixedJoint = transform.GetComponent<FixedJoint>();
                    DestroyImmediate(fixedJoint);

                    //rigidbody.velocity =  grabbedController.Velocity;
                    rigidbody.velocity = velocityForward + grabbedController.VelocitySmoothed(smoothTime);
                    rigidbody.angularVelocity = grabbedController.AngularVelocitySmoothed(smoothTime);
                }


                isGrabbed = false;
                grabbedController = null;
            }
        }
    }

    public bool IsGrabbed
    {
        get
        {
            return isGrabbed;
        }
    }
}
