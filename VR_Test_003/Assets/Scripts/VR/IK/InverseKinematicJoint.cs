using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InverseKinematic
{
    public class InverseKinematicJoint : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private InverseKinematicJointNode node0 = null;
        [SerializeField]
        private InverseKinematicJointNode node1 = null;

        [Space]

        [Header("Settings")]
        [SerializeField]
        private Vector3 scaleForces = new Vector3(1f, 1f, 1f);
        [SerializeField]
        private Vector3 rotateForces = new Vector3(1f, 1f, 1f);

        void Start()
        {

        }

        void Update()
        {

        }

        public void Init()
        {
            //transform.position
        }
    }

}