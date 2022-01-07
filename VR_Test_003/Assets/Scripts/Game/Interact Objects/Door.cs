using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.InteractObjects
{
    public class Door : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Transform doorTransform = null;
        [SerializeField]
        private VRInteractableHinge interactableHinge = null;
        [SerializeField]
        private Transform nearClosedTransform = null;
        [SerializeField]
        private Transform audioClipPosition = null;
        [SerializeField]
        private VRInteractableTouch doorKnockInteract = null;

        [Space]

        [Header("Settings")]
        [SerializeField]
        private float nearClosedAngleThresh = 5f;
        [SerializeField]
        private float snapP = 1f;
        [SerializeField]
        private float snapD = 1f;
        [SerializeField]
        private Vector3 soundAngleThresh = Vector3.zero;
        [SerializeField]
        private AudioClip clipDoorOpen = null;
        [SerializeField]
        private AudioClip clipDoorClose = null;
        [SerializeField]
        private AudioClip clipDoorKnock = null;
        [SerializeField]
        private AnimationCurve doorSoundVolumeCurve = null;
        [SerializeField]
        private AnimationCurve doorKnockSoundVolumeCurve = null;

        private bool _wasDoorGrabbed = false;
        private HingeJoint _hingeJoint = null;
        private Rigidbody _doorRig = null;

        private float _lastDoorError = 0f;
        private Vector3 _lastAngles = Vector3.zero;

        public delegate void DoorOpenCloseEvent(bool opened, float force);
        public event DoorOpenCloseEvent DoorOpenClose;

        // Start is called before the first frame update
        void Start()
        {
            _doorRig = doorTransform.GetComponent<Rigidbody>();
            _hingeJoint = doorTransform.GetComponent<HingeJoint>();

            doorKnockInteract.TriggerTouch += DoorKnockInteract_TriggerTouch;
        }

        private void DoorKnockInteract_TriggerTouch(VRController vrController)
        {
            float volume = doorKnockSoundVolumeCurve.Evaluate(vrController.VRControllerInteract.VelocitiesAtPivot().velocity.magnitude);
            //volume *= vrController.Trigger;

            if (vrController.Trigger >= 0.5f)
            {
                AudioManager.Play(clipDoorKnock, volume, true, vrController.VRControllerInteract.InteractPivot.position);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (_wasDoorGrabbed != interactableHinge.IsGrabbed)
            {
                _wasDoorGrabbed = interactableHinge.IsGrabbed;

                if (!interactableHinge.IsGrabbed)
                {
                    if (Quaternion.Angle(nearClosedTransform.rotation, doorTransform.rotation) <= nearClosedAngleThresh)
                    {
                        _hingeJoint.useMotor = true;
                    }
                }
                else
                {
                    _hingeJoint.useMotor = false;
                }
            }

            if (_hingeJoint.useMotor)
            {
                float error = Quaternion.Angle(nearClosedTransform.rotation, doorTransform.rotation);

                float u = error * snapP + (error - _lastDoorError) * snapD;

                JointMotor jointMotor = _hingeJoint.motor;
                jointMotor.targetVelocity = u * Time.deltaTime;
                _hingeJoint.motor = jointMotor;

                _lastDoorError = error;

            }

            if (_lastAngles != doorTransform.localRotation.eulerAngles)
            {
                int signNow = 0;
                float signX = Mathf.Sign(doorTransform.localRotation.eulerAngles.x - soundAngleThresh.x);
                float signY = Mathf.Sign(doorTransform.localRotation.eulerAngles.y - soundAngleThresh.y);
                float signZ = Mathf.Sign(doorTransform.localRotation.eulerAngles.z - soundAngleThresh.z);
                if (signX != Mathf.Sign(_lastAngles.x - soundAngleThresh.x))
                {
                    signNow = (int)signX;
                }
                if (signY != Mathf.Sign(_lastAngles.y - soundAngleThresh.y))
                {
                    signNow = (int)signY;
                }
                if (signZ != Mathf.Sign(_lastAngles.z - soundAngleThresh.z))
                {
                    signNow = (int)signZ;
                }
                if (signNow != 0)
                {
                    float curAngularVel = (doorTransform.localRotation.eulerAngles - _lastAngles).magnitude;
                    Debug.Log("Door ang vel: " + curAngularVel.ToString("n2"));

                    // TODO Play sound
                    if (signNow == 1)
                    {
                        IsDoorOpen = true;
                        DoorOpenClose?.Invoke(true, curAngularVel);
                        AudioManager.Play(clipDoorOpen, doorSoundVolumeCurve.Evaluate(curAngularVel), true, audioClipPosition.position);
                    }
                    else
                    {
                        IsDoorOpen = false;
                        DoorOpenClose?.Invoke(false, curAngularVel);
                        AudioManager.Play(clipDoorClose, doorSoundVolumeCurve.Evaluate(curAngularVel), true, audioClipPosition.position);
                    }

                    Debug.Log("Door sound");
                }

                _lastAngles = doorTransform.localRotation.eulerAngles;
            }
        }

        public bool IsDoorOpen
        {
            get; protected set;
        } = false;
    }

}