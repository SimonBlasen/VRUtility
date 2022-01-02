using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigSimpleFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target = null;
    [SerializeField]
    private bool followPosition = false;
    [SerializeField]
    private bool followRotation = false;

    private Rigidbody rig = null;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followPosition)
        {
            rig.MovePosition(target.position);
        }
        if (followRotation)
        {
            rig.MoveRotation(target.rotation);
        }
    }
}
