using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRTeleport
{
    public class TeleportLines : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private LineRenderer lineRenderer = null;

        [Space]

        [Header("Settings")]
        [SerializeField]
        private float dashSpeed = 1f;
        [SerializeField]
        private float dashesRepeatDistance = 1f;
        [SerializeField]
        private Material materialRed = null;





        private Material lineMat = null;
        private Material lineMatRed = null;
        private float dashS = 0f;

        // Start is called before the first frame update
        void Start()
        {
            lineRenderer.enabled = isVisible;

            lineMatRed = new Material(materialRed);
            lineMat = new Material(lineRenderer.sharedMaterial);
            lineRenderer.sharedMaterial = lineMat;
            lineRenderer.transform.parent = null;
            lineRenderer.transform.position = Vector3.zero;
            lineRenderer.transform.rotation = Quaternion.identity;
            lineRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            dashS += dashSpeed * Time.deltaTime;
            if (dashS >= 1f)
            {
                dashS -= 1f;
            }

            if (IsVisible)
            {
                float lerpS = dashS / StepDistance;

                if (lineRenderer.positionCount != curPoints.Length - 1)
                {
                    lineRenderer.positionCount = curPoints.Length - 1;
                }

                for (int i = 0; i < lineRenderer.positionCount; i++)
                {
                    int indexHere = (int)lerpS;
                    float lerpSHere = lerpS - indexHere;

                    if (i + 1 + indexHere < curPoints.Length)
                    {
                        lineRenderer.SetPosition(i, Vector3.Lerp(curPoints[i + indexHere], curPoints[i + 1 + indexHere], lerpSHere));
                    }
                    else
                    {
                        lineRenderer.SetPosition(i, Vector3.Lerp(curPoints[curPoints.Length - 1], curPoints[curPoints.Length - 1], 0f));
                    }
                }
            }
        }


        private Vector3[] curPoints = new Vector3[0];
        public void SetLinePoints(Vector3[] points)
        {
            curPoints = points;
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

                lineRenderer.enabled = isVisible;
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
                    lineRenderer.sharedMaterial = isRed ? lineMatRed : lineMat;
                }
            }
        }

        public float StepDistance
        {
            get; set;
        } = 1f;
    }

}