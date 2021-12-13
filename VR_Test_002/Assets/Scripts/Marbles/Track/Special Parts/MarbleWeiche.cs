using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleWeiche : MonoBehaviour
{
    [SerializeField]
    private MarbleTrigger frontTrigger = null;
    [SerializeField]
    private MarbleTrigger leftTrigger = null;
    [SerializeField]
    private MarbleTrigger rightTrigger = null;
    [SerializeField]
    private Transform turnerTransform = null;



    private bool turnsLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        frontTrigger.MarbleEnter += FrontTrigger_MarbleEnter;
        leftTrigger.MarbleEnter += LeftTrigger_MarbleEnter;
        rightTrigger.MarbleEnter += RightTrigger_MarbleEnter;

        refreshTurner();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RightTrigger_MarbleEnter(Transform marbleTransform)
    {
        turnsLeft = false;
        refreshTurner();
    }

    private void LeftTrigger_MarbleEnter(Transform marbleTransform)
    {
        turnsLeft = true;
        refreshTurner();
    }

    private void FrontTrigger_MarbleEnter(Transform marbleTransform)
    {
        turnsLeft = !turnsLeft;
        refreshTurner();
    }

    private void refreshTurner()
    {
        if (turnsLeft)
        {
            turnerTransform.localRotation = Quaternion.identity;
        }
        else
        {
            turnerTransform.localRotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }
}
