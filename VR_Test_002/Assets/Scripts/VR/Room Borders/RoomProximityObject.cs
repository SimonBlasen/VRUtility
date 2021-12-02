using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProximityObject : MonoBehaviour, VRControllerInit
{
    [SerializeField]
    private float minDistance = 0.1f;
    [SerializeField]
    private float beginDistance = 0.2f;

    private Collider selfCollider = null;

    private VRController[] vrControllers = new VRController[2];

    private Material selfMaterial = null;
    private MeshRenderer meshRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        selfCollider = GetComponent<Collider>();

        VRController.RegisterInit(this, false);
        VRController.RegisterInit(this, true);

        meshRenderer = GetComponent<MeshRenderer>();
        selfMaterial = new Material(meshRenderer.sharedMaterial);
        meshRenderer.sharedMaterial = selfMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        float closestDistance = float.MaxValue;
        for (int i = 0; i < vrControllers.Length; i++)
        {
            if (vrControllers[i] != null)
            {
                Vector3 closest = selfCollider.ClosestPointOnBounds(vrControllers[i].Position);
                float distance = Vector3.Distance(closest, vrControllers[i].Position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                }
            }
        }

        float inverseLerp = Mathf.InverseLerp(minDistance, beginDistance, closestDistance);

        selfMaterial.color = new Color(selfMaterial.color.r, selfMaterial.color.g, selfMaterial.color.b, 1f - inverseLerp);

        meshRenderer.enabled = inverseLerp <= beginDistance;
        
    }


    public void Inited(VRController vrController)
    {
        vrControllers[vrController.IsLeftHand ? 0 : 1] = vrController;
    }
}
