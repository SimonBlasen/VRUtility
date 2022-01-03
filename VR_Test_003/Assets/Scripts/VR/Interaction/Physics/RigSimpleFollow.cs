using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigSimpleFollow : MonoBehaviour
{
    public Transform Target
    { get; set; } = null;

    public bool FollowPosition
    { get; set; } = false;
    public bool FollowRotation
    { get; set; } = false;

    private Rigidbody rig = null;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FollowPosition)
        {
            rig.MovePosition(Target.position);
        }
        if (FollowRotation)
        {
            rig.MoveRotation(Target.rotation);
        }
    }
}
