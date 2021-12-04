using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRController : MonoBehaviour
{
    [SerializeField]
    protected bool isLeftHand = false;
    [SerializeField]
    protected float trackpadSwipeThresh = 0.2f;

    private bool areControlsPulled = false;

    private bool triggerDown = false;

    public AnimationCurve animCurveVel;

    private VRControllerInteract vrControllerInteract = null;


    // Start is called before the first frame update
    protected void Start()
    {
        vrControllerInteract = GetComponent<VRControllerInteract>();
        if (vrControllerInteract == null)
        {
            vrControllerInteract = gameObject.AddComponent<VRControllerInteract>();
        }

        if (isLeftHand)
        {
            vrControllerL = this;
            for (int i = 0; i < vrControllerInits.Count; i++)
            {
                if (vrControllerInitsLeftHand[i])
                {
                    vrControllerInits[i].Inited(vrControllerL);
                    vrControllerInits.RemoveAt(i);
                    vrControllerInitsLeftHand.RemoveAt(i);
                    i--;
                }
            }
        }
        else
        {
            vrControllerR = this;
            for (int i = 0; i < vrControllerInits.Count; i++)
            {
                if (!vrControllerInitsLeftHand[i])
                {
                    vrControllerInits[i].Inited(vrControllerR);
                    vrControllerInits.RemoveAt(i);
                    vrControllerInitsLeftHand.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if (TrackpadTouchDown)
        {
            controllerValueTrackpadPosDown = Trackpad;
        }
        if (TrackpadTouch && controllerValueTrackpad != Vector2.zero)
        {
            controllerValueTrackpadPosNotZero = controllerValueTrackpad;
        }
    }

    private void LateUpdate()
    {
        positionBuffer[bufferIndex] = controllerPosition;
        rotationBuffer[bufferIndex] = controllerRotation;

        velocityBuffer[bufferIndex] = controllerVelocity;
        angularVelocityBuffer[bufferIndex] = controllerAngularVelocity;
        accelerationBuffer[bufferIndex] = controllerAcceleration;
        angularAccelerationBuffer[bufferIndex] = controllerAngularAcceleration;
        bufferTimeDeltas[bufferIndex] = Time.deltaTime;

        bufferIndex++;
        bufferIndex = bufferIndex % bufferTimeDeltas.Length;


        areControlsPulled = false;
    }



    public VRControllerInteract VRControllerInteract
    {
        get 
        {
            return vrControllerInteract;
        }
    }


    private Vector2 controllerValueTrackpadPosDown;
    private Vector2 controllerValueTrackpadPosNotZero;

    protected Vector2 controllerValueTrackpad;
    protected float controllerValueTrigger;
    protected float controllerValueBatteryLevel;
    protected bool controllerValueTriggerButton;
    protected bool controllerValueTriggerButtonPrev;
    protected bool controllerValueTrackpadButton;
    protected bool controllerValueTrackpadButtonPrev;
    protected bool controllerValueTrackpadTouch;
    protected bool controllerValueTrackpadTouchPrev;
    protected bool controllerValueMenuButton;
    protected bool controllerValueMenuButtonPrev;

    protected float controllerGrip;

    protected Vector3 controllerPosition;
    protected Quaternion controllerRotation;

    protected Vector3 controllerVelocity;
    protected Vector3 controllerAngularVelocity;
    protected Vector3 controllerAcceleration;
    protected Vector3 controllerAngularAcceleration;

    private int bufferIndex = 0;
    private Vector3[] positionBuffer = new Vector3[256];
    private Quaternion[] rotationBuffer = new Quaternion[256];
    private Vector3[] velocityBuffer = new Vector3[256];
    private Vector3[] angularVelocityBuffer = new Vector3[256];
    private Vector3[] accelerationBuffer = new Vector3[256];
    private Vector3[] angularAccelerationBuffer = new Vector3[256];
    private float[] bufferTimeDeltas = new float[256];

    protected Vector3 controllerCenterOfMass;

    public Vector2 Trackpad
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueTrackpad;
        }
    }

    public float Trigger
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueTrigger;
        }
    }

    public bool IsGripped
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerGrip == 1f;
        }
    }

    public bool TriggerButton
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueTriggerButton;
        }
    }

    public bool TriggerButtonDown
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueTriggerButton && controllerValueTriggerButtonPrev == false;
        }
    }

    public bool TriggerButtonUp
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueTriggerButton == false && controllerValueTriggerButtonPrev;
        }
    }

    public bool TrackpadTouch
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueTrackpadTouch;
        }
    }

    public bool TrackpadSwipeUp
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            if (TrackpadTouchUp)
            {
                Vector2 delta = controllerValueTrackpadPosNotZero - controllerValueTrackpadPosDown;

                if (delta.y >= trackpadSwipeThresh)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool TrackpadSwipeDown
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            if (TrackpadTouchUp)
            {
                Vector2 delta = controllerValueTrackpadPosNotZero - controllerValueTrackpadPosDown;

                if (delta.y <= -trackpadSwipeThresh)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool TrackpadSwipeRight
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            if (TrackpadTouchUp)
            {
                Vector2 delta = controllerValueTrackpadPosNotZero - controllerValueTrackpadPosDown;

                if (delta.x >= trackpadSwipeThresh)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool TrackpadSwipeLeft
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            if (TrackpadTouchUp)
            {
                Vector2 delta = controllerValueTrackpadPosNotZero - controllerValueTrackpadPosDown;

                if (delta.x <= -trackpadSwipeThresh)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool TrackpadTouchDown
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueTrackpadTouch && controllerValueTrackpadTouchPrev == false;
        }
    }

    public bool TrackpadTouchUp
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueTrackpadTouch == false && controllerValueTrackpadTouchPrev;
        }
    }

    public bool TrackpadButton
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueTrackpadButton;
        }
    }

    public bool TrackpadButtonDown
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueTrackpadButton && controllerValueTrackpadButtonPrev == false;
        }
    }

    public bool TrackpadButtonUp
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueTrackpadButton == false && controllerValueTrackpadButtonPrev;
        }
    }

    public bool MenuButton
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueMenuButton;
        }
    }

    public bool MenuButtonDown
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueMenuButton && controllerValueMenuButtonPrev == false;
        }
    }

    public bool MenuButtonUp
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueMenuButton == false && controllerValueMenuButtonPrev;
        }
    }

    public float BatteryLevel
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerValueBatteryLevel;
        }
    }

    public Vector3 Position
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerPosition;
        }
    }

    public Quaternion Rotation
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerRotation;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerVelocity;
        }
    }

    public Vector3 AngularVelocity
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerAngularVelocity;
        }
    }

    public Vector3 Acceleration
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerAcceleration;
        }
    }

    public Vector3 AngularAcceleration
    {
        get
        {
            if (!areControlsPulled)
            {
                pullControllerValues();
            }
            return controllerAngularAcceleration;
        }
    }

    public Vector3 VelocitySmoothed(float time)
    {
        float summedTime = Time.deltaTime;
        Vector3 avgVal = Velocity;

        int negIndex = 0;
        while (summedTime <= time && negIndex < bufferTimeDeltas.Length)
        {
            int index = bufferIndex - negIndex;
            if (index < 0)
            {
                index += bufferTimeDeltas.Length;
            }

            summedTime += bufferTimeDeltas[index];
            avgVal += velocityBuffer[index];
            negIndex++;
        }

        return avgVal / (negIndex + 1);
    }

    public Vector3 AngularVelocitySmoothed(float time)
    {
        float summedTime = Time.deltaTime;
        Vector3 avgVal = AngularVelocity;

        int negIndex = 0;
        while (summedTime <= time && negIndex < bufferTimeDeltas.Length)
        {
            int index = bufferIndex - negIndex;
            if (index < 0)
            {
                index += bufferTimeDeltas.Length;
            }

            summedTime += bufferTimeDeltas[index];
            avgVal += angularVelocityBuffer[index];
            negIndex++;
        }

        return avgVal / (negIndex + 1);
    }

    public Vector3 AngularVelocitySmoothed(int samples)
    {
        float summedTime = Time.deltaTime;
        Vector3 avgVal = AngularVelocity;

        int negIndex = 0;
        while (negIndex < Mathf.Min(bufferTimeDeltas.Length, samples - 1))
        {
            int index = bufferIndex - negIndex;
            if (index < 0)
            {
                index += bufferTimeDeltas.Length;
            }

            summedTime += bufferTimeDeltas[index];
            avgVal += angularVelocityBuffer[index];
            negIndex++;
        }

        return avgVal / (negIndex + 1);
    }

    public Vector3 AccelerationSmoothed(float time)
    {
        float summedTime = Time.deltaTime;
        Vector3 avgVal = Acceleration;

        int negIndex = 0;
        while (summedTime <= time && negIndex < bufferTimeDeltas.Length)
        {
            int index = bufferIndex - negIndex;
            if (index < 0)
            {
                index += bufferTimeDeltas.Length;
            }

            summedTime += bufferTimeDeltas[index];
            avgVal += accelerationBuffer[index];
            negIndex++;
        }

        return avgVal / (negIndex + 1);
    }

    public Vector3 AngularAccelerationSmoothed(float time)
    {
        float summedTime = Time.deltaTime;
        Vector3 avgVal = AngularAcceleration;

        int negIndex = 0;
        while (summedTime <= time && negIndex < bufferTimeDeltas.Length)
        {
            int index = bufferIndex - negIndex;
            if (index < 0)
            {
                index += bufferTimeDeltas.Length;
            }

            summedTime += bufferTimeDeltas[index];
            avgVal += angularAccelerationBuffer[index];
            negIndex++;
        }

        return avgVal / (negIndex + 1);
    }

    public void Vibrate(float amplitude, float duration)
    {
        vibrate(0, amplitude, duration);
    }

    public void VibrateShort()
    {
        vibrate(0, 0.2f, 0.4f);
    }

    public void VibrateLong()
    {
        vibrate(0, 0.5f, 1f);
    }

    public Vector3 CenterOfMass
    {
        get
        {
            return controllerCenterOfMass;
        }
    }

    public float AngularVelocityError
    {
        get
        {
            //Quaternion diff = controllerRotation * Quaternion.Inverse(rotationBuffer[bufferIndex]);
            return (controllerAngularVelocity.x);
        }
    }

    protected virtual void vibrate(uint channel, float amplitude, float duration)
    {

    }

    protected virtual void pullControllerValues()
    {
        controllerValueTriggerButtonPrev = controllerValueTriggerButton;
        controllerValueTrackpadButtonPrev = controllerValueTrackpadButton;
        controllerValueTrackpadTouchPrev = controllerValueTrackpadTouch;

        controllerValueMenuButtonPrev = controllerValueMenuButton;

        areControlsPulled = true;
    }

    public bool IsLeftHand
    {
        get
        {
            return isLeftHand;
        }
    }


    private static VRController vrControllerL = null;
    private static VRController vrControllerR = null;

    private static List<VRControllerInit> vrControllerInits = new List<VRControllerInit>();
    private static List<bool> vrControllerInitsLeftHand = new List<bool>();
    public static void RegisterInit(VRControllerInit vrControllerInit, bool leftHand)
    {
        if (leftHand && vrControllerL != null)
        {
            vrControllerInit.Inited(vrControllerL);
            return;
        }
        if (!leftHand && vrControllerR != null)
        {
            vrControllerInit.Inited(vrControllerR);
            return;
        }

        vrControllerInits.Add(vrControllerInit);
        vrControllerInitsLeftHand.Add(leftHand);
    }
}
