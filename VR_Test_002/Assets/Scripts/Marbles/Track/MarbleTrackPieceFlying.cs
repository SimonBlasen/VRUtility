using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MarbleTrackPieceFlying : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve animCurveSnap = null;
    [SerializeField]
    private float snapTime = 0.3f;
    [SerializeField]
    private Transform[] snaps = null;
    [SerializeField]
    private SnapTag[] snapTags = null;
    [SerializeField]
    private float snapDistance = 0.04f;
    [SerializeField]
    private VRInteractableGrab vrInteractableGrab = null;

    private bool isSnapping = false;
    private float snapS = 0f;

    private float threshAngle = 45f;

    private MeshCollider meshCollider = null;

    private Vector3 snapStartPos = Vector3.zero;
    private Vector3 snapDestPos = Vector3.zero;
    private Quaternion snapStartRot = Quaternion.identity;
    private Quaternion snapDestRot = Quaternion.identity;

    private Rigidbody selfRig = null;

    private float stillFor = 0f;
    private Vector3 oldPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        meshCollider = GetComponentInChildren<MeshCollider>();
        selfRig = GetComponent<Rigidbody>();

        vrInteractableGrab.InteractableReleased += VrInteractableGrab_InteractableReleased;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSnapping)
        {
            snapS += Time.deltaTime / snapTime;
            snapS = Mathf.Clamp(snapS, 0f, 1f);



            transform.position = Vector3.Lerp(snapStartPos, snapDestPos, animCurveSnap.Evaluate(snapS));
            transform.rotation = Quaternion.Lerp(snapStartRot, snapDestRot, animCurveSnap.Evaluate(snapS));

            if (snapS >= 1f)
            {
                finishSnapping();
            }
        }

        while (gosToDestroy.Count > 0)
        {
            Destroy(gosToDestroy[0]);
            gosToDestroy.RemoveAt(0);
        }

        for (int i = 0; i < triggerRemoveAfter.Count; i++)
        {
            triggerRemoveAfter[i] -= Time.deltaTime;

            if (triggerRemoveAfter[i] <= 0f)
            {
                triggeredPieces.RemoveAt(i);
                triggerRemoveAfter.RemoveAt(i);
                i--;
            }
        }

        if (!isSnapping)
        {
            if (oldPos != transform.position)
            {
                stillFor = 0f;
                oldPos = transform.position;
            }
            else
            {
                stillFor += Time.deltaTime;

                if (stillFor >= 1.5f)
                {
                    finishSnapping(false);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        MarbleTrackPiece mtp = collision.transform.GetComponentInParent<MarbleTrackPiece>();
        if (mtp != null && !isSnapping && !vrInteractableGrab.IsGrabbed)
        {
            float distance;
            (int index0, int index1) = findClosestSnaps(this, mtp, out distance);

            if (distance <= snapDistance)
            {
                Debug.Log("Snapping...");
                snapTo(mtp, index0, index1);
            }
        }
    }

    public List<MarbleTrackPiece> triggeredPieces = new List<MarbleTrackPiece>();
    public List<float> triggerRemoveAfter = new List<float>();

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("MarbleRunning"))
        {

            MarbleTrackPiece mtp = other.transform.GetComponentInParent<MarbleTrackPiece>();
            if (mtp != null)
            {
                //Debug.Log("Enter: " + other.gameObject.name);

                if (triggeredPieces.Contains(mtp) == false)
                {
                    triggeredPieces.Add(mtp);
                    triggerRemoveAfter.Add(1f);
                }
                for (int i = 0; i < triggeredPieces.Count; i++)
                {
                    if (triggeredPieces[i] == mtp)
                    {
                        triggerRemoveAfter[i] = 1f;
                        break;
                    }
                }

            }
        }
    }
    
    /*

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("MarbleRunning"))
        {
            MarbleTrackPiece mtp = other.transform.GetComponentInParent<MarbleTrackPiece>();
            if (mtp != null)
            {
                Debug.Log("Leave: " + other.gameObject.name);
                triggeredPieces.Remove(mtp);
            }
        }
    }*/

    private void VrInteractableGrab_InteractableReleased(VRController vrController)
    {
        Debug.Log("Triggers to consider: " + triggeredPieces.Count.ToString());
        float minSnapDistance = float.MaxValue;
        MarbleTrackPiece closestPiece = null;
        int closestIndex0 = -1;
        int closestIndex1 = -1;
        for (int i = 0; i < triggeredPieces.Count; i++)
        {
            MarbleTrackPiece mtp = triggeredPieces[i];
            if (mtp != null && !isSnapping)
            {
                float distance;
                (int index0, int index1) = findClosestSnaps(this, mtp, out distance);

                if (distance <= snapDistance)
                {
                    if (distance < minSnapDistance)
                    {
                        minSnapDistance = distance;
                        closestPiece = mtp;
                        closestIndex0 = index0;
                        closestIndex1 = index1;
                    }
                }
            }
        }

        if (closestPiece != null)
        {
            Debug.Log("Snapping trigger...");
            snapTo(closestPiece, closestIndex0, closestIndex1);
        }
    }

    private void finishSnapping(bool attachToOtherSnap = true)
    {
        Debug.Log("Snapped DONE");
        isSnapping = false;

        GameObject instStaticTrackObj = Instantiate(MarbleParts.Inst.MarblePartPrefabs[OwnPartId].prefabNonConvex);
        instStaticTrackObj.transform.position = transform.position;
        instStaticTrackObj.transform.rotation = transform.rotation;

        MarbleTrackPiece mtp = instStaticTrackObj.GetComponent<MarbleTrackPiece>();

        mtp.Init();
        mtp.OwnPartId = OwnPartId;

        if (attachToOtherSnap)
        {
            mtp.SnapsOccupied[snapToOwnIndex] = true;
            mtp.ConnectedPieces[snapToOwnIndex] = mtpToSnap;

            mtpToSnap.SnapsOccupied[snapToOtherIndex] = true;
            mtpToSnap.ConnectedPieces[snapToOtherIndex] = mtp;

            checkOtherSnapAttachements(mtp);
        }



        Destroy(gameObject);
    }

    private void checkOtherSnapAttachements(MarbleTrackPiece mtp)
    {
        for (int i = 0; i < mtp.Snaps.Length; i++)
        {
            if (!mtp.SnapsOccupied[i])
            {
                RaycastHit hit;
                if (Physics.Raycast(new Ray(mtp.Snaps[i].position + mtp.Snaps[i].forward * -0.005f, mtp.Snaps[i].forward), out hit, 0.01f, LayerMask.GetMask("MarbleRunning")))
                {
                    MarbleTrackPiece mtpHit = hit.transform.GetComponentInParent<MarbleTrackPiece>();

                    if (mtpHit != null)
                    {
                        for (int j = 0; j < mtpHit.Snaps.Length; j++)
                        {
                            if (Vector3.Distance(mtpHit.Snaps[j].position, mtp.Snaps[i].position) <= 0.001f)
                            {
                                mtpHit.SnapsOccupied[j] = true;
                                mtpHit.ConnectedPieces[j] = mtp;
                                mtp.SnapsOccupied[i] = true;
                                mtp.ConnectedPieces[i] = mtpHit;
                                Debug.Log("Attached additional snaps");
                            }
                        }
                    }
                }
            }
        }
    }

    public int OwnPartId
    {
        get; set;
    } = -1;

    private List<GameObject> gosToDestroy = new List<GameObject>();

    private MarbleTrackPiece mtpToSnap = null;
    private int snapToOwnIndex = -1;
    private int snapToOtherIndex = -1;
    private void snapTo(MarbleTrackPiece otherPiece, int ownIndex, int otherIndex)
    {
        isSnapping = true;
        snapS = 0f;
        snapStartPos = transform.position;
        snapStartRot = transform.rotation;

        SnapTag tagSelf = snapTags[ownIndex];
        SnapTag tagOther = otherPiece.SnapTags[otherIndex];

        if (tagSelf == SnapTag.SIDE && tagOther == SnapTag.SIDE)
        {
            GameObject copyThis = new GameObject("Temp 0");
            GameObject copyThisSnap = new GameObject("Temp 1");

            GameObject tempMove = new GameObject("Temp 2");

            copyThis.transform.position = transform.position;
            copyThis.transform.rotation = transform.rotation;
            copyThisSnap.transform.position = snaps[ownIndex].position;
            copyThisSnap.transform.rotation = snaps[ownIndex].rotation;
            copyThisSnap.transform.parent = copyThis.transform;

            tempMove.transform.position = copyThisSnap.transform.position;
            tempMove.transform.rotation = copyThisSnap.transform.rotation;
            copyThis.transform.parent = tempMove.transform;

            tempMove.transform.position = otherPiece.Snaps[otherIndex].position;
            tempMove.transform.rotation = otherPiece.Snaps[otherIndex].rotation;
            tempMove.transform.Rotate(0f, 180f, 0f);

            snapDestPos = copyThis.transform.position;
            snapDestRot = copyThis.transform.rotation;


            gosToDestroy.Add(copyThis);
            gosToDestroy.Add(tempMove);
            gosToDestroy.Add(copyThisSnap);

            mtpToSnap = otherPiece;
            snapToOwnIndex = ownIndex;
            snapToOtherIndex = otherIndex;
        }
        else
        {
            float closestQuatAngle = float.MaxValue;
            Quaternion closestQuat = Quaternion.identity;

            for (int rot = 0; rot < 4; rot++)
            {
                GameObject copyThis = new GameObject("Temp 0");
                GameObject copyThisSnap = new GameObject("Temp 1");

                GameObject tempMove = new GameObject("Temp 2");

                copyThis.transform.position = transform.position;
                copyThis.transform.rotation = transform.rotation;
                copyThisSnap.transform.position = snaps[ownIndex].position;
                copyThisSnap.transform.rotation = snaps[ownIndex].rotation;
                copyThisSnap.transform.parent = copyThis.transform;

                tempMove.transform.position = copyThisSnap.transform.position;
                tempMove.transform.rotation = copyThisSnap.transform.rotation;
                copyThis.transform.parent = tempMove.transform;

                tempMove.transform.position = otherPiece.Snaps[otherIndex].position;
                tempMove.transform.rotation = otherPiece.Snaps[otherIndex].rotation;
                tempMove.transform.Rotate(0f, 180f, 0f);
                tempMove.transform.Rotate(0f, 0f, 90f * rot);

                snapDestPos = copyThis.transform.position;
                snapDestRot = copyThis.transform.rotation;


                gosToDestroy.Add(copyThis);
                gosToDestroy.Add(tempMove);
                gosToDestroy.Add(copyThisSnap);

                float angleHere = Quaternion.Angle(snapDestRot, transform.rotation);
                if (angleHere < closestQuatAngle)
                {
                    closestQuatAngle = angleHere;
                    closestQuat = snapDestRot;
                }


                mtpToSnap = otherPiece;
                snapToOwnIndex = ownIndex;
                snapToOtherIndex = otherIndex;
            }

            snapDestRot = closestQuat;
        }

    }

    public Transform[] Snaps
    {
        get
        {
            return snaps;
        }
    }

    public SnapTag[] SnapTags
    {
        get
        {
            return snapTags;
        }
    }

    private (int, int) findClosestSnaps(MarbleTrackPieceFlying piece0, MarbleTrackPiece piece1, out float distance)
    {
        int index0 = -1;
        int index1 = -1;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < piece0.snaps.Length; i++)
        {
            for (int j = 0; j < piece1.Snaps.Length; j++)
            {
                float distanceHere = Vector3.Distance(piece0.snaps[i].position, piece1.Snaps[j].position);

                float angle = Vector3.Angle(piece0.snaps[i].forward, piece1.Snaps[j].forward);

                if (((piece0.snapTags[i] == SnapTag.SIDE && piece1.SnapTags[j] == SnapTag.SIDE)
                        || (piece0.snapTags[i] == SnapTag.BOTTOM && piece1.SnapTags[j] == SnapTag.TOP)
                        || (piece0.snapTags[i] == SnapTag.TOP && piece1.SnapTags[j] == SnapTag.BOTTOM))
                    &&

                    distanceHere < closestDistance && (180f - angle) <= threshAngle
                    
                    &&
                    
                    !piece1.SnapsOccupied[j])
                {
                    Debug.Log(piece1.gameObject.name + ": " + j.ToString());
                    Debug.Log("Self index: " + i.ToString());

                    closestDistance = distanceHere;
                    index0 = i;
                    index1 = j;
                }
            }
        }

        distance = closestDistance;
        return (index0, index1);
    }
}
