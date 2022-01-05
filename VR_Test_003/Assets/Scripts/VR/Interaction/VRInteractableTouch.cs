using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractableTouch : MonoBehaviour
{
    public delegate void TriggerTouchEvent(VRController vrController);
    public event TriggerTouchEvent TriggerTouch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerInteract(VRController vrController)
    {
        TriggerTouch?.Invoke(vrController);
    }
}
