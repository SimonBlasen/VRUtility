using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class ViveController : VRController
{
    private float trackpadMaxPos = 0.01761f;
    private float triggerAngle = -22.446f;
    private float menuButtonDistance = -0.0021f;
    private Transform triggerPivot = null;
    private Transform trackpadPivot = null;
    private Transform menuButtonPivot = null;
    private ViveControllerTrackpadIndic trackpadIndic = null;
    private GameObject triggerButtonIndic = null;
    private TextMeshPro textMeshBatteryLevel = null;

    private Transform comTransform = null;


    private InputDevice targetDevice;
    private bool deviceValid = false;

    private InputDeviceCharacteristics inputDeviceChar = InputDeviceCharacteristics.Controller;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        if (isLeftHand)
        {
            inputDeviceChar = InputDeviceCharacteristics.Left;
        }
        else
        {
            inputDeviceChar = InputDeviceCharacteristics.Right;
        }

        getDevice();

        init();
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        if (!deviceValid)
        {
            getDevice();
        }

        if (deviceValid)
        {
            updateTrackpad();
            updateTrigger();
            updateMenuButton();
            updateTrackpadButton();
            updateTriggerButton();
            updateBatteryLevel();
            updateCenterOfMAss();
        }
    }

    protected override void vibrate(uint channel, float amplitude, float duration)
    {
        base.vibrate(channel, amplitude, duration);

        targetDevice.SendHapticImpulse(channel, amplitude, duration);
    }

    protected override void pullControllerValues()
    {
        base.pullControllerValues();

        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out controllerValueTrackpad);
        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out controllerValueTrigger);
        targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out controllerValueTriggerButton);
        targetDevice.TryGetFeatureValue(CommonUsages.menuButton, out controllerValueMenuButton);
        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out controllerValueTrackpadButton);
        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out controllerValueTrackpadTouch);
        targetDevice.TryGetFeatureValue(CommonUsages.batteryLevel, out controllerValueBatteryLevel);

        targetDevice.TryGetFeatureValue(CommonUsages.devicePosition, out controllerPosition);
        targetDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out controllerRotation);

        targetDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out controllerVelocity);
        Vector3 tempAngVel;
        targetDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out tempAngVel);
        controllerAngularVelocity = new Vector3(-tempAngVel.x, tempAngVel.y, -tempAngVel.z);
        targetDevice.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerAcceleration);
        targetDevice.TryGetFeatureValue(CommonUsages.deviceAngularAcceleration, out controllerAngularAcceleration);

        targetDevice.TryGetFeatureValue(CommonUsages.grip, out controllerGrip);
    }

    private void updateTrackpad()
    {
        trackpadPivot.localPosition = new Vector3(Trackpad.x, Trackpad.y, 0f) * trackpadMaxPos;
    }

    private void updateTrigger()
    {
        triggerPivot.localRotation = Quaternion.Euler(Trigger * triggerAngle, 0f, 0f);
    }

    private void updateMenuButton()
    {
        menuButtonPivot.localPosition = MenuButton ? new Vector3(0f, menuButtonDistance, 0f) : Vector3.zero;
    }

    private void updateTrackpadButton()
    {
        trackpadIndic.Glowing = TrackpadButton;
    }

    private void updateTriggerButton()
    {
        triggerButtonIndic.SetActive(TriggerButton);
    }

    private void updateBatteryLevel()
    {
        /*textMeshBatteryLevel.text = AngularVelocity.x.ToString("n3") + "\n"
                                     + AngularVelocity.y.ToString("n3") + "\n"
                                     + AngularVelocity.z.ToString("n3");*/

        //textMeshBatteryLevel.text = BatteryLevel.ToString("n5");
    }

    private void updateCenterOfMAss()
    {
        controllerCenterOfMass = comTransform.position;
    }

    private void getDevice()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputDeviceChar, inputDevices);

        if (inputDevices.Count > 0)
        {
            targetDevice = inputDevices[0];
            deviceValid = true;
        }
    }

    private void init()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "Trigger pivot")
            {
                triggerPivot = children[i];
            }
            else if (children[i].name == "Trackpad pivot")
            {
                trackpadPivot = children[i];
            }
            else if (children[i].name == "Menu Button pivot")
            {
                menuButtonPivot = children[i];
            }
            else if (children[i].name == "Trackpad Indicator")
            {
                trackpadIndic = children[i].GetComponent<ViveControllerTrackpadIndic>();
            }
            else if (children[i].name == "FlatTriggerLight")
            {
                triggerButtonIndic = children[i].gameObject;
            }
            else if (children[i].name == "Text Battery Level")
            {
                textMeshBatteryLevel = children[i].GetComponent<TextMeshPro>();
            }
            else if (children[i].name == "COM")
            {
                comTransform = children[i];
            }
        }
    }
}
