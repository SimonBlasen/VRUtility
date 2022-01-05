using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TeleportAreaActivatorTransform : TeleportAreaActivator
    {
        [SerializeField]
        private Transform checkTransform = null;
        [SerializeField]
        private Transform targetTransform = null;
        [SerializeField]
        private float angleThresh = 0f;
        [SerializeField]
        private float positionThresh = 0f;


        private Vector3 _oldPos = Vector3.zero;
        private Quaternion _oldRot = Quaternion.identity;

        protected new void Start()
        {
            base.Start();
        }


        protected new void Update()
        {
            base.Update();

            if (!_IsActivated)
            {
                if (checkTransform.position != _oldPos || checkTransform.rotation != _oldRot)
                {
                    _oldPos = checkTransform.position;
                    _oldRot = checkTransform.rotation;

                    if (Vector3.Distance(checkTransform.position, targetTransform.position) <= positionThresh
                        || Quaternion.Angle(checkTransform.rotation, targetTransform.rotation) <= angleThresh)
                    {
                        activateAreas();
                    }
                }
            }

        }
    }

}