using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TeleportAreaActivator : MonoBehaviour
    {
        [SerializeField]
        private Transform[] areasToActivate = null;

        private bool _isActivated = false;

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        protected void activateAreas()
        {
            if (!_isActivated)
            {
                Debug.Log("Activated some areas");

                _isActivated = true;

                for (int i = 0; i < areasToActivate.Length; i++)
                {
                    areasToActivate[i].tag = "Untagged";
                }
            }
        }


        protected bool _IsActivated
        {
            get
            {
                return _isActivated;
            }
        }
    }

}