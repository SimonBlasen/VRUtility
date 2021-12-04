using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleTrackPieceGrabbable : VRInteractableGrab
{
    private MarbleTrackPiece mtp = null;

    protected new void Start()
    {
        base.Start();

        mtp = GetComponent<MarbleTrackPiece>();
    }

    protected new void Update()
    {
        base.Update();
    }

    public override void Interact(VRController vrController)
    {

        // Set snaps to false again
        for (int i = 0; i < mtp.ConnectedPieces.Length; i++)
        {
            if (mtp.SnapsOccupied[i])
            {
                for (int j = 0; j < mtp.ConnectedPieces[i].ConnectedPieces.Length; j++)
                {
                    if (mtp.ConnectedPieces[i].ConnectedPieces[j] == mtp)
                    {
                        mtp.ConnectedPieces[i].ConnectedPieces[j] = null;
                        mtp.ConnectedPieces[i].SnapsOccupied[j] = false;
                    }
                }
            }
        }

        GameObject instFlying = Instantiate(MarbleParts.Inst.MarblePartPrefabs[mtp.OwnPartId].prefabThrowing);
        instFlying.GetComponent<MarbleTrackPieceFlying>().OwnPartId = mtp.OwnPartId;
        instFlying.transform.position = transform.position;
        instFlying.transform.rotation = transform.rotation;

        vrController.VRControllerInteract.GrabInteractable(instFlying.GetComponent<VRInteractableGrab>());

        Destroy(gameObject);
    }
}
