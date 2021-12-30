using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRTeleport
{
    public class TeleportTarget : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private MeshRenderer meshRenderer = null;
        [SerializeField]
        private Material materialRed = null;

        private Material materialNormal = null;

        // Start is called before the first frame update
        void Start()
        {
            materialNormal = new Material(meshRenderer.sharedMaterial);
            materialRed = new Material(materialRed);
            meshRenderer.sharedMaterial = materialNormal;

            meshRenderer.enabled = isVisible;

            transform.parent = null;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private Vector3 curPosition = Vector3.zero;
        public Vector3 Position
        {
            get
            {
                return curPosition;
            }
            set
            {
                curPosition = value;

                transform.position = curPosition;
            }
        }

        private Vector3 curNormal = Vector3.up;
        public Vector3 Normal
        {
            get
            {
                return curNormal;
            }
            set
            {
                curNormal = value;

                transform.up = curNormal;
            }
        }

        private bool isVisible = false;
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;

                meshRenderer.enabled = isVisible;
            }
        }


        private bool isRed = false;
        public bool IsRed
        {
            get
            {
                return isRed;
            }
            set
            {
                bool wasRed = isRed;
                isRed = value;

                if (wasRed != isRed)
                {
                    meshRenderer.sharedMaterial = isRed ? materialRed : materialNormal;
                }
            }
        }
    }
}