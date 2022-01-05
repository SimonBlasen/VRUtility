using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followTransform = null;
    [SerializeField]
    private bool followPosition = true;
    [SerializeField]
    private bool followRotation = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followPosition)
        {
            transform.position = followTransform.position;
        }
        if (followRotation)
        {
            transform.rotation = followTransform.rotation;
        }
    }
}
