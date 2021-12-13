using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleTrigger : MonoBehaviour
{

    public delegate void MarbleEnterEvent(Transform marbleTransform);
    public event MarbleEnterEvent MarbleEnter;


    public delegate void MarbleStayEvent(Transform marbleTransform);
    public event MarbleStayEvent MarbleStay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        MarbleEnter?.Invoke(other.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        MarbleStay?.Invoke(other.transform);
    }
}
