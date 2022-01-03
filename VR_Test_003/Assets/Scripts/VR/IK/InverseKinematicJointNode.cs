using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InverseKinematic
{
    public class InverseKinematicJointNode : MonoBehaviour
    {
        [Header("References")]
        private Transform moveTransform = null;

        void Start()
        {

        }

        void Update()
        {

        }

        public void Init()
        {
            transform.position = moveTransform.position;
            transform.rotation = moveTransform.rotation;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

}