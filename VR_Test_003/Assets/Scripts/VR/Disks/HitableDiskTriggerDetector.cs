using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HitableDiskTriggerDetector : MonoBehaviour
{
    public HitableDisk hitableDisk;

    private void Start()
    {
        hitableDisk = GetComponentInParent<HitableDisk>();
    }

    private void OnTriggerEnter(Collider other)
    {
        hitableDisk.OnTriggerEnter(other);
    }
}