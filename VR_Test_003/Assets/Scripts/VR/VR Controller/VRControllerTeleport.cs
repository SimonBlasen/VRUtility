using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRTeleport;

public class VRControllerTeleport : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private TeleportTarget teleportTarget = null;
    [SerializeField]
    private TeleportLines teleportLines = null;
    [SerializeField]
    private Transform teleportDirectionOut = null;
    [SerializeField]
    private float teleportOutStrength = 1f;
    [SerializeField]
    private float raycastStepLength = 1f;
    [SerializeField]
    private int raycastMaxSteps = 30;
    [SerializeField]
    private AnimationCurve angleToDistance = null;
    [SerializeField]
    private TeleportVignette teleportVignette = null;

    [Space]

    [Header("Settings")]
    [SerializeField]
    private string layerTeleportable = "Teleportable";
    [SerializeField]
    private float moveDelay = 0.1f;

    private static bool isOneTrackpadDown = false;

    private bool isTargeting = false;

    private XRRig xrRig = null;
    private Transform cameraTransform = null;
    private Transform cameraOffset = null;

    private int layermaskTeleportable = -1;

    private bool isTeleportingBlocked = false;


    // Start is called before the first frame update
    void Start()
    {
        layermaskTeleportable = LayerMask.GetMask(layerTeleportable);
        vrController = GetComponentInParent<VRController>();

        xrRig = GetComponentInParent<XRRig>();
        cameraTransform = xrRig.cameraGameObject.transform;
        cameraOffset = xrRig.cameraFloorOffsetObject.transform;

        teleportLines.StepDistance = raycastStepLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (vrController.TrackpadButtonDown && isOneTrackpadDown == false)
        {
            isTargeting = true;
            isOneTrackpadDown = true;

            teleportLines.IsVisible = true;
        }

        if (vrController.TrackpadButtonUp && isTargeting)
        {
            isTargeting = false;
            isOneTrackpadDown = false;

            teleportLines.IsVisible = false;
            teleportTarget.IsVisible = false;

            if (!teleportTarget.IsRed)
            {
                teleportPlayer();
            }
        }

        if (isTargeting)
        {
            performRaycast();
        }
    }

    private VRController vrController = null;

    private VRController VRController
    {
        get
        {
            if (vrController == null)
            {
                vrController = GetComponent<VRController>();
            }
            return vrController;
        }
    }

    private void teleportPlayer()
    {
        moveDelta = teleportTarget.Position - cameraTransform.position;
        moveDelta.y = teleportTarget.Position.y - xrRig.transform.position.y;

        isTeleportingBlocked = true;
        Invoke("delayedMoveAboutDelta", moveDelay);
        Invoke("unblockTeleporting", moveDelay + 0.01f);

        teleportVignette.Blink();
    }

    private void unblockTeleporting()
    {
        isTeleportingBlocked = false;
    }

    private Vector3 moveDelta = Vector3.zero;
    private void delayedMoveAboutDelta()
    {
        xrRig.transform.position += moveDelta;
    }

    private void performRaycast()
    {
        Vector3 prevPos = teleportDirectionOut.position;
        Vector3 dir = teleportDirectionOut.forward;

        List<Vector3> positions = new List<Vector3>();
        positions.Add(prevPos);

        teleportTarget.IsVisible = false;
        teleportTarget.IsRed = true;
        teleportLines.IsRed = true;

        for (int i = 0; i < raycastMaxSteps; i++)
        {
            Vector3 posHere = prevPos + dir.normalized * raycastStepLength;

            RaycastHit hit;
            if (Physics.Raycast(new Ray(prevPos, posHere - prevPos), out hit, raycastStepLength, layermaskTeleportable))
            {
                positions.Add(hit.point);
                teleportTarget.Position = hit.point;
                teleportTarget.Normal = hit.normal;

                teleportTarget.IsVisible = true;

                if (hit.collider.tag != "BlockTeleportation")
                {
                    teleportTarget.IsRed = false;
                    teleportLines.IsRed = false;
                }

                break;
            }
            else
            {
                positions.Add(posHere);

                float downwardsAngle = Vector3.Angle(Vector3.up, teleportDirectionOut.forward);

                float flatDistance = Vector2.Distance(new Vector2(prevPos.x, prevPos.z), new Vector2(posHere.x, posHere.z));

                Vector3 turnAxis = Vector3.Cross(teleportDirectionOut.forward, Vector3.up);
                dir = Quaternion.AngleAxis(angleToDistance.Evaluate(downwardsAngle) * flatDistance, turnAxis) * dir;

                prevPos = posHere;
            }
        }

        teleportLines.SetLinePoints(positions.ToArray());
    }
}
