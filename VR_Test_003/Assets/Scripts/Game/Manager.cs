using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum GameState
    {
        IN_BEDROOM, 
    }

    public class Manager : MonoBehaviour
    {
        private GameState _state = GameState.IN_BEDROOM;


        public delegate void StateChangeEvent(GameState oldState, GameState newState);
        public event StateChangeEvent StateChanged;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameState GameState
        {
            get
            {
                return _state;
            }
        }

        public void GoToState(GameState newState)
        {
            GameState oldState = _state;
            _state = newState;
            StateChanged?.Invoke(oldState, _state);
        }

        private static Manager _inst = null;
        public static Manager Inst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = FindObjectOfType<Manager>();
                }

                return _inst;
            }
        }
    }

}