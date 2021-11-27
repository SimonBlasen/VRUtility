using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRController : MonoBehaviour
{
    [SerializeField]
    private Transform interactPivot = null;



    private bool areControlsPulled = false;

    private bool triggerDown = false;

    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {

    }

    private void LateUpdate()
    {
        areControlsPulled = false;
    }

    public Transform InteractPivot
    {
        get
        {
            return interactPivot;
        }
    }


    protected Vector2 controllerValueTrackpad;
    protected float controllerValueTrigger;
    protected float controllerValueBatteryLevel;
    protected bool controllerValueTriggerButton;
    protected bool controllerValueTriggerButtonPrev;
    protected bool controllerValueTrackpadButton;
    protected bool controllerValueTrackpadTouch;
    protected bool controllerValueMenuButton;

    protected float controllerGrip;

    protected Vector3 controllerPosition;
    protected Quaternion controllerRotation;

    protected Vector3 controllerVelocity;
    protected Vector3 controllerAngularVelocity;
    protected Vector3 controllerAcceleration;
    protected Vector3 controllerAngularAcceleration;

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

    protected virtual void vibrate(uint channel, float amplitude, float duration)
    {

    }

    protected virtual void pullControllerValues()
    {
        controllerValueTriggerButtonPrev = controllerValueTriggerButton;

        areControlsPulled = true;
    }
}
