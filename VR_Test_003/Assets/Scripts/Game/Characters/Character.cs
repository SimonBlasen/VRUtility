using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Characters
{
    public enum CharacterAnimOverride
    {
        SITTING, WALK
    }

    public enum CharacterAnimAction
    {
        IDLE, TEXTING, DEATH, WAVING, PUNCH_LEFT, PUNCH_RIGHT, RUNNING, WALK, KICK
    }

    public class Character : MonoBehaviour
    {
        [SerializeField]
        private CharAnimControllerState[] overrideStates = null;
        [SerializeField]
        protected float navMeshReachDestinationDistance = 0.3f;


        [Space]

        [Header("Anim Settings")]
        [SerializeField]
        private float moveSpeedWalkAnim = 1f;


        protected NavMeshAgent _navAgent = null;

        protected Animator _animator = null;

        protected CharacterAnimOverride _curAnimState = CharacterAnimOverride.WALK;

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
            _navAgent = GetComponent<NavMeshAgent>();

            _navAgent.stoppingDistance = 0.1f;
        }

        protected virtual void Update()
        {
            if (_navAgent.velocity.magnitude >= moveSpeedWalkAnim)
            {
                _animator.SetBool("isWalking", true);
            }
            else
            {
                _animator.SetBool("isWalking", false);
            }
        }

        public void WalkTo(Vector3 position)
        {
            _navAgent.SetDestination(position);
        }

        public CharacterAnimOverride AnimState
        {
            get
            {
                return _curAnimState;
            }
            set
            {
                CharacterAnimOverride oldAnimState = _curAnimState;
                _curAnimState = value;

                if (oldAnimState != _curAnimState)
                {
                    _animator.runtimeAnimatorController = getAnimControllerStateByAnimOverride(_curAnimState).animatorController;
                }
            }
        }

        private CharAnimControllerState getAnimControllerStateByAnimOverride(CharacterAnimOverride animOverride)
        {
            for (int i = 0; i < overrideStates.Length; i++)
            {
                if (overrideStates[i].stateType == animOverride)
                {
                    return overrideStates[i];
                }
            }
            return null;
        }
    }



    [Serializable]
    public class CharAnimControllerState
    {
        [SerializeField]
        public CharacterAnimOverride stateType;
        [SerializeField]
        public RuntimeAnimatorController animatorController;
    }
}