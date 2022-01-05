using Game.InteractObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Characters
{
    public enum MomState
    {
        HALWAY_WALK, 
    }

    public class Mom : Character
    {
        [Header("References")]
        [SerializeField]
        private Door bedroomDoor = null;
        [SerializeField]
        private BedroomPillow pillow = null;

        [Space]

        [Header("Hallway")]
        [SerializeField]
        private Transform[] hallwayRandomWalkPos = null;


        private MomState _curMomState = MomState.HALWAY_WALK;

        // Start is called before the first frame update
        protected new void Start()
        {
            base.Start();

            bedroomDoor.DoorOpenClose += BedroomDoor_DoorOpenClose;

            walkRandomHallway();
            AnimState = CharacterAnimOverride.WALK;

        }

        private void BedroomDoor_DoorOpenClose(bool opened, float force)
        {

        }

        // Update is called once per frame
        protected new void Update()
        {
            base.Update();

            if (_curMomState == MomState.HALWAY_WALK)
            {
                if (_navAgent.velocity.magnitude <= 0.1f 
                    && Vector2.Distance(new Vector2(_navAgent.destination.x, _navAgent.destination.z), new Vector2(transform.position.x, transform.position.z)) <= navMeshReachDestinationDistance)
                {
                    walkRandomHallway();
                }
            }
        }

        private void walkRandomHallway()
        {
            _navAgent.SetDestination(hallwayRandomWalkPos[Random.Range(0, hallwayRandomWalkPos.Length)].position);
        }
    }
}