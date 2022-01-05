using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.InteractObjects
{
    public class BedroomPillow : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Transform[] pillowTargets = null;
        [SerializeField]
        private float targetAngleThresh = 15f;
        [SerializeField]
        private float targetPosThresh = 0.2f;
        [SerializeField]
        private VRInteractableGrab vrInteractableGrab = null;
        [SerializeField]
        private Rigidbody pillowRigidbody = null;

        private float _checkPosFor = 0f;
        private Transform _pillowTransform = null;

        // Start is called before the first frame update
        void Start()
        {
            _pillowTransform = pillowRigidbody.transform;
            vrInteractableGrab.InteractableReleased += VrInteractableGrab_InteractableReleased;
        }

        private void VrInteractableGrab_InteractableReleased(VRController vrController)
        {
            _checkPosFor = 2f;
        }

        // Update is called once per frame
        void Update()
        {
            if (_checkPosFor > 0f)
            {
                _checkPosFor -= Time.deltaTime;
                if (pillowRigidbody.velocity.magnitude > 0.01f || pillowRigidbody.angularVelocity.magnitude > 0.01f)
                {
                    _checkPosFor = 2f;
                }

                bool isCloseEnough = false;
                for (int i = 0; i < pillowTargets.Length; i++)
                {
                    if (Vector3.Distance(pillowTargets[i].position, _pillowTransform.position) <= targetPosThresh
                        && Quaternion.Angle(pillowTargets[i].rotation, _pillowTransform.rotation) <= targetAngleThresh)
                    {
                        isCloseEnough = true;
                        break;
                    }
                }

                LaysGood = isCloseEnough;
            }
        }

        public bool LaysGood
        {
            get; protected set;
        } = false;
    }

}