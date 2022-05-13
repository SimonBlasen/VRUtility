using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCOM : MonoBehaviour
{
    [SerializeField] private Transform comTransform;
    
    private void Awake()
    {
        GetComponent<Rigidbody>().centerOfMass = transform.InverseTransformPoint(comTransform.position);
        Destroy(this);
    }
}
