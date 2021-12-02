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

        private VRController vrController = null;

        private bool menuOpened = false;
        private bool isTrackpadTouched = false;
        private float lastTrackpadX = 0f;

        // Start is called before the first frame update
        void Start()
        {
            vrController = GetComponentInParent<VRController>();
            linesScaleup.Scale = Vector3.zero;

            objectsParentAnim.LocalPosition = objectsParentPosInvis.localPosition;
            //objectsParentAnim.Scale = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            if (vrController.TrackpadButtonDown)
            {
                MenuOpened = !MenuOpened;
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