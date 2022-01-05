using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractable : MonoBehaviour, VRControllerInit
{
    [SerializeField]
    protected float interactDistance = 0.1f;
    [SerializeField]
    private Collider interactCollider = null;

    protected VRController[] vrControllers = new VRController[2];

    private bool inited = false;

    // Start is called before the first frame update
    protected void Start()
    {
        init();
    }

    private void init()
    {
        if (!inited)
        {
            inited = true;

            VRController.RegisterInit(this, false);
            VRController.RegisterInit(this, true);

            Rigidbody = GetComponent<Rigidbody>();
            if (Rigidbody)
            {
                Rigidbody.maxAngularVelocity = float.MaxValue;
            }
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    public void Inited(VRController vrController)
    {
        vrController.VRControllerInteract.RegisterInteractable(this);
        if (vrController.IsLeftHand)
        {
            vrControllers[0] = vrController;
        }
        else
        {
            vrControllers[1] = vrController;
        }
    }

    public virtual void Interact(VRController vrController)
    {
        vrController.VRControllerInteract.GrabInteractable(this);
    }

    public virtual void DeInteract(VRController vrController)
    {

    }

    private void OnDestroy()
    {
        for (int i = 0; i < vrControllers.Length; i++)
        {
            if (vrControllers[i] != null)
            {
                vrControllers[i].VRControllerInteract.DeRegisterInteractable(this);
            }
        }
    }

    private Rigidbody rig = null;
    public Rigidbody Rigidbody
    {
        get
        {
            init();
            return rig;
        }
        protected set
        {
            rig = value;
        }
    }



    private bool isInteractable = true;
    public bool IsInteractable
    {
        get
        {
            return isInteractable;
        }
        set
        {
            isInteractable = value;
        }
    }

    public float DistanceTo(Vector3 worldPosition)
    {
        if (interactCollider == null)
        {
            return Vector3.Distance(transform.position, worldPosition);
        }
        else
        {
            return Vector3.Distance(interactCollider.ClosestPoint(worldPosition), worldPosition);
        }
    }
}
