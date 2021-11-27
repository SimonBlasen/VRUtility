using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRController : MonoBehaviour
{
    [SerializeField]
    private InputDeviceCharacteristics inputDeviceChar = InputDeviceCharacteristics.Controller;

    [Space]

    [SerializeField]
    private Transform triggerTrans;
    [SerializeField]
    private Transform triggerButtonTrans;
    [SerializeField]
    private Transform primButtonTrans;
    [SerializeField]
    private Transform trackPadTrans;


    private bool inputValid = false;

    private InputDevice targetDevice;

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputDeviceChar, inputDevices);

        if (inputDevices.Count > 0)
        {
            targetDevice = inputDevices[0];
            inputValid = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputValid)
        {
            Start();
        }
        else
        {
            float triggerVal = 0f;
            Vector2 trackPad = Vector2.zero;
            bool primaryButtonVal = false;
            bool triggerButton = false;
            targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonVal);
            targetDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerVal);
            targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButton);
            targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out trackPad);

            triggerTrans.localScale = new Vector3(1f, 1f, triggerVal);
            triggerButtonTrans.gameObject.SetActive(triggerButton);
            primButtonTrans.gameObject.SetActive(primaryButtonVal);
            trackPadTrans.localPosition = trackPad;
        }

    }
}
