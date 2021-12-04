using SappAnims;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Marbles
{
    public class MarbleController : MonoBehaviour
    {
        [SerializeField]
        private SappAnim linesScaleup = null;
        [SerializeField]
        private SappAnim objectsParentAnim = null;
        [SerializeField]
        private Transform objectsParentPosVis = null;
        [SerializeField]
        private Transform objectsParentPosInvis = null;
        [SerializeField]
        private MarbleObjectsSelector marbleObjectsSelector = null;
        [SerializeField]
        private Transform objectInHandParent = null;

        private VRController vrController = null;

        private GameObject instPartInHand = null;

        private bool menuOpened = false;
        private bool isTrackpadTouched = false;
        private float lastTrackpadX = 0f;

        private SappAnim inHandAnimScale = null;

        private IngameMenu ingameMenu = null;

        // Start is called before the first frame update
        void Start()
        {
            inHandAnimScale = objectInHandParent.GetComponent<SappAnim>();
            vrController = GetComponentInParent<VRController>();
            ingameMenu = vrController.GetComponentInChildren<IngameMenu>();
            linesScaleup.Scale = Vector3.zero;

            objectsParentAnim.LocalPosition = objectsParentPosInvis.localPosition;
            marbleObjectsSelector.IsPreviewVisible = true;
            //objectsParentAnim.Scale = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            if (!HasPartInHand && vrController.TrackpadButtonDown && (ingameMenu == null || ingameMenu.MenuOpened == false))
            {
                MenuOpened = !MenuOpened;
            }

            if (!MenuOpened)
            {
                if (vrController.TrackpadSwipeLeft)
                {
                    marbleObjectsSelector.RotateSelectedObject(true);
                }
                else if (vrController.TrackpadSwipeRight)
                {
                    marbleObjectsSelector.RotateSelectedObject(false);
                }
            }


            if (vrController.TrackpadTouch && isTrackpadTouched == false)
            {
                isTrackpadTouched = true;
                lastTrackpadX = vrController.Trackpad.x;
            }

            if (vrController.TrackpadTouch == false && isTrackpadTouched)
            {
                isTrackpadTouched = false;
            }



            if (vrController.TrackpadTouch && MenuOpened)
            {
                float xDelta = vrController.Trackpad.x - lastTrackpadX;
                marbleObjectsSelector.MoveDelta(xDelta / Time.deltaTime);


                lastTrackpadX = vrController.Trackpad.x;
            }


            if (MenuOpened)
            {
                marbleObjectsSelector.HoldDownTrackpad = vrController.TrackpadTouch;
            }

            if (!MenuOpened && vrController.TriggerButtonDown && !HasPartInHand && vrController.VRControllerInteract.GetNearestInteractableDistance() > vrController.VRControllerInteract.InteractDistance)
            {
                instPartInHand = Instantiate(MarbleParts.Inst.MarblePartPrefabs[marbleObjectsSelector.IndexInFront].prefabThrowing, objectInHandParent);
                instPartInHand.GetComponent<MarbleTrackPieceFlying>().OwnPartId = marbleObjectsSelector.IndexInFront;
                instPartInHand.transform.localPosition = Vector3.zero;
                objectInHandParent.localRotation = Quaternion.Euler(0f, marbleObjectsSelector.SelectedPartRotation, 0f);
                objectInHandParent.localScale = Vector3.zero;

                inHandAnimScale.transform.localScale = Vector3.zero;
                inHandAnimScale.Scale = Vector3.zero;
                inHandAnimScale.Scale = new Vector3(1f, 1f, 1f);

                instPartInHand.GetComponent<VRInteractable>().Interact(vrController);

                marbleObjectsSelector.IsPreviewVisible = false;
            }
            else if (!MenuOpened && HasPartInHand && vrController.TriggerButtonUp)
            {
                //instPartInHand.AddComponent<Rigidbody>();
                instPartInHand.transform.parent = null;
                instPartInHand = null;
                marbleObjectsSelector.IsPreviewVisible = true;
            }
        }

        public bool HasPartInHand
        {
            get
            {
                return instPartInHand != null;
            }
        }


        public bool MenuOpened
        {
            get
            {
                return menuOpened;
            }
            set
            {
                bool wasMenuOpened = menuOpened;
                menuOpened = value;

                if (wasMenuOpened != menuOpened)
                {
                    linesScaleup.Scale = menuOpened ? new Vector3(1f, 1f, 1f) : new Vector3(0f, 0f, 1f);

                    objectsParentAnim.LocalPosition = menuOpened ? objectsParentPosVis.localPosition : objectsParentPosInvis.localPosition;
                    //objectsParentAnim.Scale = menuOpened ? new Vector3(1f, 1f, 1f) : Vector3.zero;

                    if (menuOpened)
                    {
                        marbleObjectsSelector.Show();
                    }
                    else
                    {
                        marbleObjectsSelector.Hide();
                    }
                }
            }
        }
    }

}