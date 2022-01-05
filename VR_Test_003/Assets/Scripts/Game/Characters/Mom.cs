using Game.InteractObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Characters
{
    public class Mom : Character
    {
        [Header("References")]
        [SerializeField]
        private Door bedroomDoor = null;
        [SerializeField]
        private BedroomPillow pillow = null;



        // Start is called before the first frame update
        protected new void Start()
        {
            base.Start();

            bedroomDoor.DoorOpenClose += BedroomDoor_DoorOpenClose;
        }

        private void BedroomDoor_DoorOpenClose(bool opened, float force)
        {

        }

        // Update is called once per frame
        protected new void Update()
        {
            base.Update();
        }
    }
}