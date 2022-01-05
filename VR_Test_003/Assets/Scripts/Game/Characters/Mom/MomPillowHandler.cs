using Game.InteractObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Characters
{

    public enum MomPillowState
    {
        IDLE, 
    }

    public class MomPillowHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Door bedroomDoor = null;
        [SerializeField]
        private BedroomPillow pillow = null;
        [SerializeField]
        private Transform playerTransform = null;
        [SerializeField]
        private Transform playerInHallwayMin = null;
        [SerializeField]
        private Transform playerInHallwayMax = null;

        [Space]

        [SerializeField]
        private AudioClip clipInitialGoodPillow = null;
        [SerializeField]
        private AudioClip clipTidyPillowPlease = null;

        private MomPillowState state = MomPillowState.IDLE;

        // Start is called before the first frame update
        void Start()
        {
            bedroomDoor.DoorOpenClose += BedroomDoor_DoorOpenClose;
        }

        private void BedroomDoor_DoorOpenClose(bool opened, float force)
        {
            if (opened && state == MomPillowState.IDLE)
            {
                // Lays good initially
                if (pillow.LaysGood)
                {
                    AudioManager.Play(clipInitialGoodPillow, 1f, true, transform.position);
                }
                else
                {
                    AudioManager.Play(clipTidyPillowPlease, 1f, true, transform.position);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected bool isPlayerInHallway
        {
            get
            {
                return playerTransform.position.x >= playerInHallwayMin.position.x
                    && playerTransform.position.y >= playerInHallwayMin.position.y
                    && playerTransform.position.z >= playerInHallwayMin.position.z

                    && playerTransform.position.x <= playerInHallwayMax.position.x
                    && playerTransform.position.y <= playerInHallwayMax.position.y
                    && playerTransform.position.z <= playerInHallwayMax.position.z;
            }
        }
    }

}