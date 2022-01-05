using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Characters
{
    public enum CharacterAnimOverride
    {
        SITTING, WALK
    }

    public class Character : MonoBehaviour
    {
        [SerializeField]
        private 

        protected NavMeshAgent _navAgent = null;

        protected Animator _animator = null;

        protected CharacterAnimOverride _curAnimState = CharacterAnimOverride.WALK;

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
            _navAgent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {

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
                _curAnimState = value;

                //_animator.set
            }
        }
    }

}