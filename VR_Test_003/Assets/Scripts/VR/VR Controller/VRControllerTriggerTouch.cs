using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerTriggerTouch : MonoBehaviour
{
    private VRController _vrController = null;

    // Start is called before the first frame update
    void Start()
    {
        _vrController = transform.parent.GetComponent<VRController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        VRInteractableTouch vrInteractableTouch = other.GetComponent<VRInteractableTouch>();
        if (vrInteractableTouch != null)
        {
            vrInteractableTouch.TriggerInteract(_vrController);
        }
    }
}
