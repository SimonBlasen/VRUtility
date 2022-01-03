using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InverseKinematic
{
    public class InverseKinematic : MonoBehaviour
    {

        private List<InverseKinematicJoint> joints = new List<InverseKinematicJoint>();
        private List<InverseKinematicJointNode> nodes = new List<InverseKinematicJointNode>();

        void Start()
        {
            joints.AddRange(GetComponentsInChildren<InverseKinematicJoint>());
            nodes.AddRange(GetComponentsInChildren<InverseKinematicJointNode>());

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Init();
            }

            for (int i = 0; i < joints.Count; i++)
            {
                joints[i].Init();
            }
        }

        void Update()
        {

        }
    }

}