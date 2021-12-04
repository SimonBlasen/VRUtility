using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SnapTag
{
    BOTTOM, SIDE, TOP
}

public class MarbleTrackPiece : MonoBehaviour
{
    [SerializeField]
    private Transform[] snaps = null;
    [SerializeField]
    private SnapTag[] snapTags = null;


    private Vector3 snapStartPos = Vector3.zero;
    private Vector3 snapDestPos = Vector3.zero;
    private Quaternion snapStartRot = Quaternion.identity;
    private Quaternion snapDestRot = Quaternion.identity;

    private bool[] snapsOccupied = null;
    private MarbleTrackPiece[] connectedPieces = null;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool inited = false;
    public void Init()
    {
        if (!inited)
        {
            inited = true;
            snapsOccupied = new bool[snaps.Length];
            for (int i = 0; i < snapsOccupied.Length; i++)
            {
                snapsOccupied[i] = false;
            }

            connectedPieces = new MarbleTrackPiece[snaps.Length];
        }
    }

    public int OwnPartId
    {
        get; set;
    } = -1;

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

    public bool[] SnapsOccupied
    {
        get
        {
            return snapsOccupied;
        }
    }

    public MarbleTrackPiece[] ConnectedPieces
    {
        get
        {
            return connectedPieces;
        }
    }

}
