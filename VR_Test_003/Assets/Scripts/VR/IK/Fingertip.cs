using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InverseKinematic
{
    public class Fingertip : MonoBehaviour
    {
        [SerializeField]
        private Transform posOpened = null;
        [SerializeField]
        private Transform posClosed = null;
        [SerializeField]
        private AnimationCurve curveY = null;

        [SerializeField]
        private float _lerpS = 0f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.Lerp(posClosed.position, posOpened.position, _lerpS) + posClosed.up * curveY.Evaluate(_lerpS);
        }

        public float Closed
        {
            get
            {
                return _lerpS;
            }
            set
            {
                _lerpS = value;
            }
        }
    }

}