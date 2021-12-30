using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigSimpleFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target = null;

    private Rigidbody rig = null;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.MovePosition(target.position);
    }
}
