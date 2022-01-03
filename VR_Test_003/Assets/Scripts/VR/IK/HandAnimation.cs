using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InverseKinematic
{
    public class HandAnimation : MonoBehaviour, VRControllerInit
    {
        [SerializeField]
        private Fingertip[] fingertips;
        [SerializeField]
        private float scaleTriggerFactorLow = 0.1f;
        [SerializeField]
        private float scaleTriggerFactorHigh = 0.8f;

        protected VRController[] _vrControllers = new VRController[2];

        private VRController _vrController = null;

        public void Inited(VRController vrController)
        {
            if (vrController.IsLeftHand)
            {
                _vrControllers[0] = vrController;
            }
            else
            {
                _vrControllers[1] = vrController;
            }

            if (_vrControllers[0] != null && _vrControllers[1] != null)
            {
                VRController selfController = transform.parent.GetComponentInChildren<VRController>();
                _vrController = selfController;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            VRController.RegisterInit(this, false);
            VRController.RegisterInit(this, true);
        }

        // Update is called once per frame
        void Update()
        {
            if (_vrController != null)
            {
                for (int i = 0; i < fingertips.Length; i++)
                {
                    float triggerVal = _vrController.Trigger - scaleTriggerFactorLow;
                    triggerVal = Mathf.Clamp(triggerVal, 0f, 1f);
                    triggerVal /= (1f - scaleTriggerFactorLow);
                    triggerVal /= (scaleTriggerFactorHigh);
                    triggerVal = Mathf.Clamp(triggerVal, 0f, 1f);
                    fingertips[i].Closed = (1f - triggerVal);
                }
            }
        }
    }

}